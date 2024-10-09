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


        }
    }
}
