using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Runtime.CompilerServices;

namespace CMS.MentApi.Untility.DatabaseExt
{
    /// <summary>
    /// to init sqlsugar 
    /// </summary>
    public static class InitSqlSugarExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <exception cref="Exception"></exception>
        public static void IniSqlSugar(this WebApplicationBuilder builder)
        {

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

            if (connection == null) { throw new Exception("A connection string must be defined first."); }
            ConnectionConfig connectionConfig = new ConnectionConfig()
            {
                ConnectionString = connection,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            };

            builder.Services.AddScoped<ISqlSugarClient>(s =>
            {
                SqlSugarClient client = new SqlSugarClient(connectionConfig);
                client.Aop.OnLogExecuting = (s, p) =>
                {
                    Console.WriteLine($"OnLogExecuting:输出Sql语句:{s} || 参数为：{string.Join(",", p.Select(p => p.Value))}");
                };
                client.Aop.OnExecutingChangeSql = (s, p) =>
                {
                    Console.WriteLine($"OnLogExecuting:输出Sql语句:{s} || 参数为：{string.Join(",", p.Select(p => p.Value))}");
                    return new KeyValuePair<string, SugarParameter[]>(s, p);
                };
                client.Aop.OnLogExecuted = (s, p) =>
                {
                    Console.WriteLine($"OnLogExecuted:输出Sql语句:{s} || 参数为：{string.Join(",", p.Select(p => p.Value))}");
                };
                client.Aop.OnError = e =>
                {
                    Console.WriteLine($"OnError:Sql语句执行异常:{e.Message}");
                };

                {
                    //可以配置对于数据库操作的过滤器--SqlSugar特有的
                }
                return client;
            });

        }
    }
}
