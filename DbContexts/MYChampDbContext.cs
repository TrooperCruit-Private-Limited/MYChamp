using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MYChamp.Models;
using MYChamp.Pages.PaySlip;

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
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<SalaryDispatch> SalaryDispatches { get; set; }
        public DbSet<AttendenceModel> Attendence { get; set; }
        public DbSet<LeaveDetail> LeaveDetails { get; set; }


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
            Description = "Users having this responsibility will have a provision to map the CFO or an equivalent top role employee to the Organization"
        },
        new Responsibility
        {
            Id = 2,
            ResponsibilityName = "Administrator",
            Description = "User Account Unlocking, Password reset with email verification bypass, User Mapping with Responsibilities"
        },
        new Responsibility
        {
            Id = 3,
            ResponsibilityName = "Accountant",
            Description = "Invoice Generation, Receipt Tracking, Payslip Generation, Income Tracking, Expense Tracking"
        },
        new Responsibility
        {
            Id = 4,
            ResponsibilityName = "Human Resource",
            Description = "Job Posting, Job Seekers, Screening Interaction, Recording Interview, Scheduling Interview, Report Review, Offer Letter, Generation Job Confirmation, Position Management, Leave Management, Performance Management, Package Appraisal Management, Resignation Management"
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
            Description = "Tutorial Management, Training Management, Scholer Management Certifications"
        }
    );

            modelBuilder.Entity<MenuItem>().HasData(
                   new MenuItem { Id = 1, Name = "Super Admin Route", Route = "/SuperAdminRoute", ResponsibilityId = 1 },
                   new MenuItem { Id = 2, Name = "User Account Unlocking", Route = "/UnlockAccount", ResponsibilityId = 2 },
                   new MenuItem { Id = 3, Name = "Password Reset", Route = "/ResetPassword", ResponsibilityId = 2 },
                   new MenuItem { Id = 4, Name = "Invoice Generation", Route = "/GenerateInvoice", ResponsibilityId = 3 },
                   new MenuItem { Id = 5, Name = "Receipt Tracking", Route = "/TrackReceipt", ResponsibilityId = 3 },
                   new MenuItem { Id = 6, Name = "Job Posting", Route = "/PostJob", ResponsibilityId = 4 },
                   new MenuItem { Id = 7, Name = "Job Seekers", Route = "/JobSeekers", ResponsibilityId = 4 },
                   new MenuItem { Id = 8, Name = "Project Management", Route = "/ProjectManagement", ResponsibilityId = 5 },
                   new MenuItem { Id = 9, Name = "Tutorial Management", Route = "/TutorialManagement", ResponsibilityId = 6 },
                   new MenuItem { Id = 10, Name = "Training Management", Route = "/TrainingManagement", ResponsibilityId = 6 },
                   new MenuItem { Id = 11, Name = "PaySlip Generation", Route = "/PaySlip", ResponsibilityId = 3 },
                   new MenuItem { Id = 12, Name = "Credit Salary Page", Route = "/CreditSalaryPage", ResponsibilityId = 3 },
                   new MenuItem { Id = 13, Name = "Salary PaySlip Records", Route = "/SalarySlip", ResponsibilityId = 3 }

               );
        }
    }
}
