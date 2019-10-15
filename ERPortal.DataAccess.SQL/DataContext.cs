using ERPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.DataAccess.SQL
{
    public class DataContext: DbContext
    {
        public DataContext():
            base("name=DefaultConnection"){
            //Remove in Production
            Database.SetInitializer<DataContext>(new DropCreateDatabaseIfModelChanges<DataContext>());

            // Disable initializer in Production
            // Database.SetInitializer<DataContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ERApplication>().HasOptional(a => a.ScreeningReport);
            modelBuilder.Entity<ERApplication>().Property(p => p.DGHComments).HasColumnType("nvarchar(max)");
            modelBuilder.Entity<ERApplication>().Property(p => p.DGHCommentsForPilot).HasColumnType("nvarchar(max)");


            modelBuilder.Entity<ERCMemberComments>().HasRequired(c => c.LinkedApplication);
            modelBuilder.Entity<ERCMemberComments>().Property(p => p.Remarks).HasColumnType("nvarchar(max)");
        }

        public DbSet<ERApplication> ERApplications { get; set; }
        public DbSet<ERCMemberComments> ERCMemberComments { get; set; }
        public DbSet<ERScreeningDetails> ERScreeningDetails { get; set; }
        public DbSet<ERScreeningInstitute> ERScreeningInstitutes { get; set; }
        public DbSet<FieldType> FieldTypes { get; set; }
        //public DbSet<Operator> Operators { get; set; }
        public DbSet<UserAccounts> UserAccounts { get; set; }

    }
}
