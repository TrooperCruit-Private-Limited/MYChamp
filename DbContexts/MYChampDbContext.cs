using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MYChamp.Models;

namespace MYChamp.DbContexts
{
    public class MYChampDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<VisitUsInformationModel> VisitUsInformation { get; set; }
        //public DbSet<Session_model> Session_Model{ get; set; }
        public DbSet<SessionModel> sessionModel { get; set; }
        public DbSet<RegisterModel> registerModel { get; set; }
        public DbSet<ForcefulLogout> forcefulLogouts { get; set; }
        public DbSet<Article> Article { get; set; }

        public DbSet<Position> Positions { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Responsibility> Responsibilities { get; set; }
        public MYChampDbContext(DbContextOptions<MYChampDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>().ToTable("article");

            modelBuilder.Entity<VisitUsInformationModel>().HasData(
                new VisitUsInformationModel
                {
                    Id = 1,
                    Name = "saikumar",
                    Icon = "Icon1",
                    ImageType = "Type1",
                    Link = "Link1",
                    Active = 1
                },
                new VisitUsInformationModel
                {
                    Id = 2,
                    Name = "Another Name",
                    Icon = "Another Icon",
                    ImageType = "Another Type",
                    Link = "Another Link",
                    Active = 1
                }
            );
            modelBuilder.Entity<SessionModel>()
            .Property(b => b.LoginTime)
            .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<SessionModel>()
                .Property(b => b.LogoutTime)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Responsibility>().HasData(
        new Responsibility
        {
            Id = 1,
            ResponsibilityName = "Super Administrator",
            Description = "Users having this responsibility will have a provision to map the CFO or an equivalent top role employee to the Organization."
        },
        new Responsibility
        {
            Id = 2,
            ResponsibilityName = "Administrator",
            Description = "User Account Unlocking.Password reset with email verification bypass. User Mapping with Responsibilities"
        },
        new Responsibility
        {
            Id = 3,
            ResponsibilityName = "Accountant",
            Description = "Invoice Generation Receipt Tracking Payslip Generation Income Tracking Expense Tracking"
        },
        new Responsibility
        {
            Id = 4,
            ResponsibilityName = "Human Resource",
            Description = "Job Posting Job Seekers Screening Interaction Recording Interview Scheduling Interview Report Review Offer Letter Generation Job Confirmation Position Management Leave Management Performance Management Package Appraisal Management Resignation Management"
        },
        new Responsibility
        {
            Id = 5,
            ResponsibilityName = "Scrum Master",
            Description = "Project Management"
        },
        new Responsibility
        {
            Id = 6,
            ResponsibilityName = "Tutor",
            Description = "Tutorial Management Training Management Scholer Management Certifications"
        }
    );


        }
    }
}
