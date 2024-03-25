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
                            MenuText = menuOrButtonAttribute.GetMenuName(),
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

                                FunctionAttribute btnAtrribute = action.GetCustomAttribute<FunctionAttribute>();
                                Sys_Button button = new Sys_Button
                                {
                                    Id = Guid.NewGuid(),
                                    ParentId = menu.Id,
                                    BtnText = btnAtrribute.GetMenuName(),
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
                //inset  menu and buttons info into data base
                sqlSugarClient.Insertable(menuList).ExecuteCommand();
                sqlSugarClient.Insertable(buttonList).ExecuteCommand();

            }


        }
    }
}
