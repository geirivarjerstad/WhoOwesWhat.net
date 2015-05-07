using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoOwesWhat.DataProvider.Entity;

namespace WhoOwesWhat.DataProvider
{


    public interface IWhoOwesWhatContext : IDisposable, IObjectContextAdapter
    {
        DbSet<UserCredential> UserCredentials { get; set; }
        DbSet<Person> Persons { get; set; }
        int SaveChanges();
        void ResetDatabase();
    }

    public class WhoOwesWhatContext : DbContext, IWhoOwesWhatContext
    {
        // ReSharper disable UnusedMember.Local
        //http://stackoverflow.com/questions/14695163/cant-find-system-data-entity-sqlserver-sqlproviderservices
        private static Type _hack = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        // ReSharper restore UnusedMember.Local

        public WhoOwesWhatContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WhoOwesWhatContext, MigrationConfigurations>());

        }

        public DbSet<Person> Persons { get; set; }
        public void ResetDatabase()
        {
            Database.Delete();
        }

        public DbSet<UserCredential> UserCredentials { get; set; }

        //public DbSet<Handleliste> Feedlist { get; set; }
        //public DbSet<SharedHandleliste> SharedFeedList { get; set; }
        //public DbSet<Commodity> Commodities { get; set; }
        //public DbSet<Error> Errors { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<UserCredential>()
        //    //    .HasRequired(c => c.SharedHandleliste)
        //    //    .WithMany()
        //    //    .WillCascadeOnDelete(false);
        //}

        //public DbSet<Handleliste> Feedlist    
        //{
        //    get
        //    {
        //        return Set<Handleliste>();
        //    }
        //}

        //public DbSet<Commodity> Commodities
        //{
        //    get
        //    {
        //        return Set<Commodity>();
        //    }
        //}


        //public DbSet<TestMigration> TestMigrations
        //{
        //    get
        //    {
        //        return Set<TestMigration>();
        //    }
        //}

    }


}
