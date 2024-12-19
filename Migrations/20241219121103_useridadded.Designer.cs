﻿// <auto-generated />
using System;
using System.Collections.Generic;
using MYChamp.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MYChamp.Migrations
{
    [DbContext(typeof(MYChampDbContext))]
    [Migration("20241219121103_useridadded")]
    partial class useridadded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MYChamp.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("MYChamp.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("author");

                    b.Property<string>("AuthorPosition")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("authorposition");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category");

                    b.Property<byte[]>("CoverImageData")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("coverimagedata");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean")
                        .HasColumnName("isarchived");

                    b.Property<string>("MediaLinks")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("medialinks");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision")
                        .HasColumnName("rating");

                    b.Property<string>("Relevancy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("relevancy");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("useremail");

                    b.HasKey("Id");

                    b.ToTable("article", (string)null);
                });

            modelBuilder.Entity("MYChamp.Models.AttendenceModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LeaveType")
                        .HasColumnType("integer");

                    b.Property<int>("MarkedAttendence")
                        .HasColumnType("integer");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Attendence");
                });

            modelBuilder.Entity("MYChamp.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PositionId")
                        .HasColumnType("integer");

                    b.Property<int>("RemainingLeaves")
                        .HasColumnType("integer");

                    b.Property<int?>("ReportingManagerId")
                        .HasColumnType("integer");

                    b.Property<string>("ReportingManagerName")
                        .HasColumnType("text")
                        .HasColumnName("reportingmanagername");

                    b.Property<int>("Salary")
                        .HasColumnType("integer");

                    b.Property<List<int>>("SelectedResponsibilityIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("MYChamp.Models.ForcefulLogout", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("forceful_logout_by")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ipAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("logoutTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.ToTable("forcefulLogouts");
                });

            modelBuilder.Entity("MYChamp.Models.LeaveApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateRequested")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfDays")
                        .HasColumnType("integer");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ManagerId");

                    b.ToTable("LeaveApplications");
                });

            modelBuilder.Entity("MYChamp.Models.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ResponsibilityId")
                        .HasColumnType("integer");

                    b.Property<string>("Route")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ResponsibilityId");

                    b.ToTable("MenuItem");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Super Admin Route",
                            ResponsibilityId = 1,
                            Route = "/SuperAdminRoute"
                        },
                        new
                        {
                            Id = 2,
                            Name = "User Account Unlocking",
                            ResponsibilityId = 2,
                            Route = "/UnlockAccount"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Password Reset",
                            ResponsibilityId = 2,
                            Route = "/ResetPassword"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Invoice Generation",
                            ResponsibilityId = 3,
                            Route = "/GenerateInvoice"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Receipt Tracking",
                            ResponsibilityId = 3,
                            Route = "/TrackReceipt"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Job Posting",
                            ResponsibilityId = 4,
                            Route = "/PostJob"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Job Seekers",
                            ResponsibilityId = 4,
                            Route = "/JobSeekers"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Project Management",
                            ResponsibilityId = 5,
                            Route = "/ProjectManagement"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Tutorial Management",
                            ResponsibilityId = 6,
                            Route = "/TutorialManagement"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Training Management",
                            ResponsibilityId = 6,
                            Route = "/TrainingManagement"
                        },
                        new
                        {
                            Id = 11,
                            Name = "PaySlip Generation",
                            ResponsibilityId = 3,
                            Route = "/PaySlip"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Credit Salary Page",
                            ResponsibilityId = 3,
                            Route = "/CreditSalaryPage"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Salary PaySlip Records",
                            ResponsibilityId = 3,
                            Route = "/SalarySlip"
                        });
                });

            modelBuilder.Entity("MYChamp.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AllottedLeaves")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActiveP")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("MYChamp.Models.RegisterModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("companyname");

                    b.Property<string>("EmailId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("emailid");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstname");

                    b.Property<bool>("IsEmployee")
                        .HasColumnType("boolean")
                        .HasColumnName("isemployee");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("jobtitle");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastname");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("middlename");

                    b.Property<int>("Otp")
                        .HasColumnType("integer")
                        .HasColumnName("otp");

                    b.Property<DateTime>("OtpValiditity")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("otpvalidity");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phonenumber");

                    b.Property<bool>("active")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("passwordvalidity")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("registerModel");
                });

            modelBuilder.Entity("MYChamp.Models.Responsibility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ResponsibilityName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Responsibilities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Users having this responsibility will have a provision to map the CFO or an equivalent top role employee to the Organization",
                            ResponsibilityName = "Super Administrator"
                        },
                        new
                        {
                            Id = 2,
                            Description = "User Account Unlocking, Password reset with email verification bypass, User Mapping with Responsibilities",
                            ResponsibilityName = "Administrator"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Invoice Generation, Receipt Tracking, Payslip Generation, Income Tracking, Expense Tracking",
                            ResponsibilityName = "Accountant"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Job Posting, Job Seekers, Screening Interaction, Recording Interview, Scheduling Interview, Report Review, Offer Letter, Generation Job Confirmation, Position Management, Leave Management, Performance Management, Package Appraisal Management, Resignation Management",
                            ResponsibilityName = "Human Resource"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Project Management",
                            ResponsibilityName = "Scrum Master"
                        },
                        new
                        {
                            Id = 6,
                            Description = "Tutorial Management, Training Management, Scholer Management Certifications",
                            ResponsibilityName = "Tutor"
                        });
                });

            modelBuilder.Entity("MYChamp.Models.SalaryDispatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DispatchDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("MonthlySalary")
                        .HasColumnType("numeric");

                    b.Property<decimal>("NetSalary")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("SalaryDispatches");
                });

            modelBuilder.Entity("MYChamp.Models.SessionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ipaddress");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isactive");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("logintime");

                    b.Property<DateTime>("LogoutTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("logouttime");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sessionid");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<bool>("forcefully_logout")
                        .HasColumnType("boolean");

                    b.Property<string>("forcefully_logout_by")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("logoutType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("logouttype");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("sessionModel");
                });

            modelBuilder.Entity("MYChamp.Models.VisitUsInformationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Active")
                        .HasColumnType("integer");

                    b.Property<string>("Icon")
                        .HasColumnType("text");

                    b.Property<string>("ImageType")
                        .HasColumnType("text");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("VisitUsInformation");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = 1,
                            Icon = "Icon1",
                            ImageType = "Type1",
                            Link = "Link1",
                            Name = "saikumar"
                        },
                        new
                        {
                            Id = 2,
                            Active = 1,
                            Icon = "Another Icon",
                            ImageType = "Another Type",
                            Link = "Another Link",
                            Name = "Another Name"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MYChamp.Models.Employee", b =>
                {
                    b.HasOne("MYChamp.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Position");
                });

            modelBuilder.Entity("MYChamp.Models.LeaveApplication", b =>
                {
                    b.HasOne("MYChamp.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MYChamp.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("MYChamp.Models.MenuItem", b =>
                {
                    b.HasOne("MYChamp.Models.Responsibility", "Responsibility")
                        .WithMany("MenuItems")
                        .HasForeignKey("ResponsibilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Responsibility");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MYChamp.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MYChamp.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MYChamp.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MYChamp.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MYChamp.Models.Responsibility", b =>
                {
                    b.Navigation("MenuItems");
                });
#pragma warning restore 612, 618
        }
    }
}
