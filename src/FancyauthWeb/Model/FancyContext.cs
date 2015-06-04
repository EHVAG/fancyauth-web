using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Fancyauth.Model;
using Fancyauth.Model.MusiG;

namespace Fancyauth
{
    public class MyConfiguration : DbConfiguration
    {
        public MyConfiguration()
        {
            SetDefaultConnectionFactory(new Npgsql.NpgsqlConnectionFactory());
            SetProviderFactory("Npgsql", Npgsql.NpgsqlFactory.Instance);
            SetProviderServices("Npgsql", Npgsql.NpgsqlServices.Instance);
        }
    }

    [DbConfigurationType(typeof(MyConfiguration))]
    public class FancyContext : FancyContextBase
    {
        public FancyContext()
            : base("Port=5433;Encoding=UTF-8;Server=192.168.56.1;Database=fancyauth;UserId=noah;Password=47abd8c6a71e7cf7e35871f90fa937c46a0975028d4d4347a6a4df43dcf4efbc;Preload Reader=true;")
        {
#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s, "SQL");
#endif
        }

        public static async Task<FancyContext> Connect()
        {
            var context = new FancyContext();
            await context.Database.Connection.OpenAsync();
            return context;
        }
    }
}

