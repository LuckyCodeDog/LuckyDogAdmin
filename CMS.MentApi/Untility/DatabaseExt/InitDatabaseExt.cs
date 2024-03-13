using CMS.Models.Entity;
using SqlSugar;
using SqlSugar.Extensions;
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
                foreach (Type type in typeList)
                {
                    if (type.IsDefined(typeof(MenuOrButtonAttribute), true))
                    {
                        MenuOrButtonAttribute menuOrButtonAttribute = type.GetCustomAttribute<MenuOrButtonAttribute>();

                        Sys_Menu menu = new Sys_Menu()
                        {
                            Id = Guid.NewGuid(),
                            ParentId = default,
                            MenuText = menuOrButtonAttribute.GetDescription(),
                            MenuType = (int)menuOrButtonAttribute.GetMenuType(),
                        };
                        menuList.Add(menu);
                        foreach (var action in type.GetMethods())
                        {
                            if (action.IsDefined(typeof(MenuOrButtonAttribute), true))
                            {
                                Sys_Menu button = new Sys_Menu()
                                {
                                    Id = Guid.NewGuid(),
                                    ParentId = menu.Id,
                                    MenuText = action.GetCustomAttribute<MenuOrButtonAttribute>().GetDescription(),
                                    MenuType = (int)action.GetCustomAttribute<MenuOrButtonAttribute>().GetMenuType(),
                                    ControllerName = type.Name.ToLower().Replace("Controller", ""),
                                    ActionName = action.Name.ToLower(),
                                    FullName = $"{type.Name}_{action.Name}",
                                };
                                menuList.Add(button);
                            }
                        }
                        //inset  menu and buttons info into data base
                        sqlSugarClient.Insertable(menuList).ExecuteCommand();
                    }
                }
            }


        }
    }
}
