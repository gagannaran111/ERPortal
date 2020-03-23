using ERPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        public DataContext() :
            base("name=DefaultConnection")
        {
            //Remove in Production
            // Database.SetInitializer<DataContext>(new DropCreateDatabaseIfModelChanges<DataContext>());

            // Disable initializer in Production
            Database.SetInitializer<DataContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ERApplication>().HasOptional(a => a.ERScreeningDetail);
            modelBuilder.Entity<ERApplication>().HasOptional(a => a.ERPilot);
            modelBuilder.Entity<ERApplication>().HasOptional(a => a.UHCProductionMethod);
            modelBuilder.Entity<ERApplication>().HasRequired(a => a.Organisation).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<ERApplication>().HasRequired(a => a.FieldType).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<Comment>().Property(c => c.Text).HasColumnType("nvarchar(max)");
            modelBuilder.Entity<Comment>().HasRequired(c => c.UserAccount);
            modelBuilder.Entity<Comment>().HasRequired(c => c.ERApplication);

            modelBuilder.Entity<UserAccount>().Property(u => u.EmailID).IsRequired();
            modelBuilder.Entity<UserAccount>().HasRequired(u => u.Organisation).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Notification>().HasRequired(n => n.UserAccount);
            modelBuilder.Entity<Notification>().HasOptional(n => n.ERApplication).WithMany().WillCascadeOnDelete(true); ;

            modelBuilder.Entity<ForwardApplication>().HasRequired(f => f.ERApplication);

            //modelBuilder.Entity<QueryDetails>().HasRequired(q => q.ERApplication).WithMany().WillCascadeOnDelete(false); ;
            //modelBuilder.Entity<QueryDetails>().HasRequired(q => q.UserAccount);

            //modelBuilder.Entity<QueryMaster>().HasRequired(q => q.QueryRef);

            // modelBuilder.Entity<AuditTrails>().HasRequired(at => at.Status).WithMany().WillCascadeOnDelete(false); ;
            // modelBuilder.Entity<AuditTrail>().HasRequired(at => at.QueryDetails);

            //  modelBuilder.Entity<ERAppActiveUsers>().HasRequired(ea => ea.ERApplication);
            modelBuilder.Entity<ERAppActiveUsers>().HasRequired(ea => ea.UserAccount);
        }

        public DbSet<ERApplication> ERApplications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ERScreeningDetail> ERScreeningDetails { get; set; }
        public DbSet<ERScreeningInstitute> ERScreeningInstitutes { get; set; }
        public DbSet<FieldType> FieldTypes { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<ForwardApplication> ForwardApplications { get; set; }
        public DbSet<AuditTrails> AuditTrails { get; set; }
        public DbSet<ERAppActiveUsers> ERAppActiveUsers { get; set; }
        public DbSet<QueryDetails> QueryDetails { get; set; }
        public DbSet<QueryMaster> QueryMasters { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<StatusMaster> StatusMasters { get; set; }
        public DbSet<QueryUser> QueryUsers { get; set; }
        public DbSet<DepartmentType> Departments { get; set; }
        public DbSet<ERTechniques> ERTechniques { get; set; }
        public DbSet<ERPilot> ERPilots{ get; set; }


    }
}
