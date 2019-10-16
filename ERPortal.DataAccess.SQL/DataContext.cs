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
            modelBuilder.Entity<ERApplication>().HasOptional(a => a.DGHComments);
            modelBuilder.Entity<ERApplication>().HasOptional(a => a.ERCComments);
            modelBuilder.Entity<ERApplication>().HasRequired(a => a.OperatorName);

            modelBuilder.Entity<Comments>().Property(c => c.Remarks).HasColumnType("nvarchar(max)");
            modelBuilder.Entity<Comments>().HasRequired(c => c.LinkedUser);

            modelBuilder.Entity<UserAccounts>().Property(u => u.EmailID).IsRequired();
            modelBuilder.Entity<UserAccounts>().HasOptional(u => u.OperatorName);

        }

        public DbSet<ERApplication> ERApplications { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<ERScreeningDetails> ERScreeningDetails { get; set; }
        public DbSet<ERScreeningInstitute> ERScreeningInstitutes { get; set; }
        public DbSet<FieldType> FieldTypes { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<UserAccounts> UserAccounts { get; set; }

    }
}
