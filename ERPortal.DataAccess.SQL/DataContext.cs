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
            modelBuilder.Entity<ERApplication>().HasRequired(a => a.OperatorName).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<ERApplication>().HasRequired(a => a.FieldTypes).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>().Property(c => c.Text).HasColumnType("nvarchar(max)");
            modelBuilder.Entity<Comment>().HasRequired(c => c.LinkedUser);
            modelBuilder.Entity<Comment>().HasRequired(c => c.LinkedApplication);

            modelBuilder.Entity<UserAccount>().Property(u => u.EmailID).IsRequired();
            modelBuilder.Entity<UserAccount>().HasOptional(u => u.OperatorName).WithMany().WillCascadeOnDelete(true);

            modelBuilder.Entity<Notification>().HasRequired(n => n.UserID);
            modelBuilder.Entity<Notification>().HasOptional(n => n.LinkedERApplication).WithMany().WillCascadeOnDelete(true); ;

        }

        public DbSet<ERApplication> ERApplications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ERScreeningDetail> ERScreeningDetails { get; set; }
        public DbSet<ERScreeningInstitute> ERScreeningInstitutes { get; set; }
        public DbSet<FieldType> FieldTypes { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Notification> Notifications { get; set; }

    }
}
