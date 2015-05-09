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
    public class FancyContext : DbContext
    {
        public FancyContext()
            : base("Port=5433;Encoding=UTF-8;Server=192.168.56.1;Database=fancyauth;UserId=noah;Password=47abd8c6a71e7cf7e35871f90fa937c46a0975028d4d4347a6a4df43dcf4efbc;Preload Reader=true;")
        {
#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s, "SQL");
#endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LogEntry>()
                .Map<LogEntry.Connected>(x => x.Requires("Discriminator").HasValue((int)LogEntry.Discriminator.Connected))
                .Map<LogEntry.Disconnected>(x => x.Requires("Discriminator").HasValue((int)LogEntry.Discriminator.Disconnected))
                .Map<LogEntry.ChatMessage>(x => x.Requires("Discriminator").HasValue((int)LogEntry.Discriminator.ChatMessage));
        }

        public static async Task<FancyContext> Connect()
        {
            var context = new FancyContext();
            await context.Database.Connection.OpenAsync();
            return context;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Invite> Invites { get; set; }
        public virtual DbSet<GuestAssociation> GuestAssociations { get; set; }
        public virtual DbSet<OfflineNotification> OfflineNotifications { get; set; }
        public virtual DbSet<LogEntry> Logs { get; set; }

        #region MusiG
        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Interpret> Interprets { get; set; }
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<SongRating> SongRatings { get; set; }
        public virtual DbSet<SongSuggestion> SongSuggestions { get; set; }
        #endregion
    }
}

