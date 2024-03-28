using CMS.Common.UserStateEnum;
using CMS.Models.Entity;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CMS.MentApi.Untility.DatabaseExt
{
    /// <summary>
    /// to init data base
    /// </summary>
    public static class InitDatabaseExt
    {

        public static void InitDb(this WebApplicationBuilder builder)
        {
            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

            if (connection == null) { throw new Exception("A connection string must be defined first."); }
            SqlSugarClient sqlSugarClient = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connection,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
            });
            using (sqlSugarClient)
            {
                //init db
                sqlSugarClient.DbMaintenance.CreateDatabase();
                //code fiirst and load entiied by reflection
                Type[] types = Assembly.Load("CMS.Models").GetTypes().Where(t => t.Namespace.Contains("Entity")).Where(t => !t.FullName.Contains("Base")).ToArray();
                sqlSugarClient.CodeFirst.InitTables(types);
                //target : write menu and button into database
                // get controllers types  by refelction  
                // loop typs[]  get MenuOrButton attributes 
                // tell if  those are  menu or button with extra info  provided by attribute 
                // save info of types in to a new menu object or button object
                // make the menu id to be the button parent id  and form 
                List<Type> typeList = Assembly.Load("CMS.MentApi").GetTypes().Where(t => t.Namespace.Contains("Controllers")).ToList();
                List<Sys_Menu> menuList = new List<Sys_Menu>();
                List<Sys_Button> buttonList = new List<Sys_Button>();
                foreach (Type type in typeList)
                {
                    if (type.IsDefined(typeof(FunctionAttribute), true))
                    {
                        FunctionAttribute menuOrButtonAttribute = type.GetCustomAttribute<FunctionAttribute>();

                        Sys_Menu menu = new Sys_Menu()
                        {
                            Id = Guid.NewGuid(),
                            ParentId = default,
                            IsLeafNode = true,
                            MenuText = menuOrButtonAttribute?.GetMenuName(),
                            MenuType = (int)menuOrButtonAttribute.GetMenuType(),
                            VueFilePath = menuOrButtonAttribute.GetVueFilePath(),
                            Icon = "Home",
                            WebUrl = menuOrButtonAttribute.GetWebUrlName(),
                            ControllerName =   type.Name.ToLower().Replace("controller", ""),
                        };
                        menuList.Add(menu);
                        foreach (var action in type.GetMethods())
                        {
                            if (action.IsDefined(typeof(FunctionAttribute), true))
                            {

                                FunctionAttribute? btnAtrribute = action.GetCustomAttribute<FunctionAttribute>();
                                Sys_Button button = new Sys_Button
                                {
                                    Id = Guid.NewGuid(),
                                    ParentId = menu.Id,
                                    BtnText = btnAtrribute?.GetMenuName(),
                                    ControllerName = type.Name.ToLower().Replace("controller", ""),
                                    BtnValue = Guid.NewGuid().ToString(),
                                    ActionName = action.Name.ToLower(),
                                    Icon = "Home",
                                    FullName = $"{type.Name}_{action.Name}",
                                };
                                buttonList.Add(button);
                            }
                        }
                    }

                   
                }

                #region create super admin 
                // insert  admin role in role table 
                IConfigurationSection superAdminConfig = builder.Configuration.GetSection("SuperAdmin");
                /*           "userId": "1",
               "name": "SuperAdmin",
               "email": "123@123.com",
               "password": "123456"*/
                try
                {
                    int superAdminId = int.Parse(superAdminConfig["UserId"]);
                    string superAdminName = superAdminConfig["name"].ToString();
                    string superAdminEmail = superAdminConfig["email"].ToString();
                    string superAdminPassword = superAdminConfig["password"].ToString();
                    Sys_Role adminRole = new Sys_Role() { RoleId = superAdminId, RoleName = superAdminName, CreateTime = DateTime.Now, Status = (int)ActiveStateEnum.Active };
                    sqlSugarClient.Insertable<Sys_Role>(adminRole).ExecuteCommand();
                    Sys_User adminUser = new Sys_User()
                    {
                        UserId = superAdminId,
                        Name = superAdminName,
                        Status = (int)ActiveStateEnum.Active,
                        CreateTime = DateTime.Now,
                        Password = superAdminPassword,
                        Email = superAdminEmail,
                        Phone = "5560550",
                        Mobile = "5560550",
                        Sex = 1,
                        Address="Lincoln",
                        ImageUrl= ""
                    };
                    sqlSugarClient.Insertable<Sys_User>(adminUser).ExecuteCommand();

                    foreach (var menu in menuList)
                    {
                        sqlSugarClient.Insertable<Sys_RoleMenuMap>(new Sys_RoleMenuMap()
                        {
                            RoleId = adminRole.RoleId,
                            MenuId = menu.Id,
                        }).ExecuteCommand();
                    }

                    foreach (var btn in buttonList)
                    {
                        sqlSugarClient.Insertable<Sys_RoleBtnMap>(new Sys_RoleBtnMap()
                        {
                            RoleId = adminRole.RoleId,
                            BtnId = btn.Id,
                        }).ExecuteCommand();
                    }

                    sqlSugarClient.Insertable<Sys_UserRoleMap>(new Sys_UserRoleMap()
                    {
                        UserId = adminUser.UserId,
                        RoleId = adminRole.RoleId,
                    }).ExecuteCommand();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed to parse SuperAdmin configuration.", ex);

                }


             
                #endregion
                //inset  menu and buttons info into data base
                sqlSugarClient.Insertable(menuList).ExecuteCommand();
                sqlSugarClient.Insertable(buttonList).ExecuteCommand();

            }


        }
    }
}
