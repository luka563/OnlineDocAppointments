﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StomatoloskaOrdinacija.Models;

#nullable disable

namespace StomatoloskaOrdinacija.Migrations
{
    [DbContext(typeof(zubarContext))]
    [Migration("20230512081504_v21")]
    partial class v21
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Blog", b =>
                {
                    b.Property<int>("BlogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogID"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextHTML")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BlogID");

                    b.ToTable("blogs");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Comment", b =>
                {
                    b.Property<int>("CommentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentID"));

                    b.Property<string>("CommentContent")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("CommentFromID")
                        .HasColumnType("int");

                    b.Property<int>("CommentToID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("CommentID");

                    b.HasIndex("CommentFromID");

                    b.HasIndex("CommentToID");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Intervention", b =>
                {
                    b.Property<int>("InterventionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InterventionID"));

                    b.Property<int>("InterventionTimeMinutes")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InterventionID");

                    b.ToTable("Intervention");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Rating", b =>
                {
                    b.Property<int>("RatingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RatingID"));

                    b.Property<int>("FromWhoID")
                        .HasColumnType("int");

                    b.Property<float>("Rate")
                        .HasColumnType("real");

                    b.Property<int>("ToWhoID")
                        .HasColumnType("int");

                    b.HasKey("RatingID");

                    b.HasIndex("FromWhoID");

                    b.HasIndex("ToWhoID");

                    b.ToTable("ratings");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Term", b =>
                {
                    b.Property<int>("TermID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TermID"));

                    b.Property<DateTime>("TermEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TermStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("WorkingDayID")
                        .HasColumnType("int");

                    b.HasKey("TermID");

                    b.HasIndex("UserID");

                    b.HasIndex("WorkingDayID");

                    b.ToTable("Term");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.TermInt", b =>
                {
                    b.Property<int>("TermIntID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TermIntID"));

                    b.Property<int>("InterventionID")
                        .HasColumnType("int");

                    b.Property<int>("TermID")
                        .HasColumnType("int");

                    b.HasKey("TermIntID");

                    b.HasIndex("InterventionID");

                    b.HasIndex("TermID");

                    b.ToTable("TermInt");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.US", b =>
                {
                    b.Property<int>("USID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("USID"));

                    b.Property<string>("specialization")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("USID");

                    b.ToTable("uss");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<Guid>("ActivationCode")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SpecializationUSID")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.HasIndex("SpecializationUSID");

                    b.ToTable("users");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.WorkingDay", b =>
                {
                    b.Property<int>("WorkingDayID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WorkingDayID"));

                    b.Property<DateTime>("Day")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorUserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("WorkingDayID");

                    b.HasIndex("DoctorUserID");

                    b.ToTable("WorkingDay");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Comment", b =>
                {
                    b.HasOne("StomatoloskaOrdinacija.Models.User", "CommentFrom")
                        .WithMany("CommentsFrom")
                        .HasForeignKey("CommentFromID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StomatoloskaOrdinacija.Models.User", "CommentTo")
                        .WithMany("CommentsTo")
                        .HasForeignKey("CommentToID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CommentFrom");

                    b.Navigation("CommentTo");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Rating", b =>
                {
                    b.HasOne("StomatoloskaOrdinacija.Models.User", "FromWho")
                        .WithMany("RatingsFrom")
                        .HasForeignKey("FromWhoID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StomatoloskaOrdinacija.Models.User", "ToWho")
                        .WithMany("RatingsTo")
                        .HasForeignKey("ToWhoID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FromWho");

                    b.Navigation("ToWho");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Term", b =>
                {
                    b.HasOne("StomatoloskaOrdinacija.Models.User", "User")
                        .WithMany("Terms")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StomatoloskaOrdinacija.Models.WorkingDay", "WorkingDay")
                        .WithMany("Terms")
                        .HasForeignKey("WorkingDayID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("WorkingDay");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.TermInt", b =>
                {
                    b.HasOne("StomatoloskaOrdinacija.Models.Intervention", "Intervention")
                        .WithMany("TermInt")
                        .HasForeignKey("InterventionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StomatoloskaOrdinacija.Models.Term", "Term")
                        .WithMany("TermInt")
                        .HasForeignKey("TermID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Intervention");

                    b.Navigation("Term");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.User", b =>
                {
                    b.HasOne("StomatoloskaOrdinacija.Models.US", "Specialization")
                        .WithMany("User")
                        .HasForeignKey("SpecializationUSID");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.WorkingDay", b =>
                {
                    b.HasOne("StomatoloskaOrdinacija.Models.User", "Doctor")
                        .WithMany("WorkingDays")
                        .HasForeignKey("DoctorUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Intervention", b =>
                {
                    b.Navigation("TermInt");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.Term", b =>
                {
                    b.Navigation("TermInt");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.US", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.User", b =>
                {
                    b.Navigation("CommentsFrom");

                    b.Navigation("CommentsTo");

                    b.Navigation("RatingsFrom");

                    b.Navigation("RatingsTo");

                    b.Navigation("Terms");

                    b.Navigation("WorkingDays");
                });

            modelBuilder.Entity("StomatoloskaOrdinacija.Models.WorkingDay", b =>
                {
                    b.Navigation("Terms");
                });
#pragma warning restore 612, 618
        }
    }
}
