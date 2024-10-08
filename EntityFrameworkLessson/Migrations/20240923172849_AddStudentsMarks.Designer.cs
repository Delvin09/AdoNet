﻿// <auto-generated />
using System;
using EntityFrameworkLessson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EntityFrameworkLessson.Migrations
{
    [DbContext(typeof(UniverContext))]
    [Migration("20240923172849_AddStudentsMarks")]
    partial class AddStudentsMarks
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EntityFrameworkLessson.Facultet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Facultets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Tech fac"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Social fac"
                        });
                });

            modelBuilder.Entity("EntityFrameworkLessson.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("FacId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FacId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("EntityFrameworkLessson.Mark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExcamMark")
                        .HasColumnType("int");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.HasIndex("StudentId");

                    b.ToTable("Marks");
                });

            modelBuilder.Entity("EntityFrameworkLessson.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Passport")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Person", t =>
                        {
                            t.HasCheckConstraint("check_age", "Age BETWEEN 16 AND 70");
                        });

                    b.HasDiscriminator().HasValue("Person");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("EntityFrameworkLessson.Student", b =>
                {
                    b.HasBaseType("EntityFrameworkLessson.Person");

                    b.Property<int?>("FacultetId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasIndex("FacultetId");

                    b.ToTable(t =>
                        {
                            t.HasCheckConstraint("check_age", "Age BETWEEN 16 AND 70");
                        });

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("EntityFrameworkLessson.Teacher", b =>
                {
                    b.HasBaseType("EntityFrameworkLessson.Person");

                    b.ToTable(t =>
                        {
                            t.HasCheckConstraint("check_age", "Age BETWEEN 16 AND 70");
                        });

                    b.HasDiscriminator().HasValue("Teacher");
                });

            modelBuilder.Entity("EntityFrameworkLessson.Lesson", b =>
                {
                    b.HasOne("EntityFrameworkLessson.Facultet", "Fac")
                        .WithMany("Lessons")
                        .HasForeignKey("FacId");

                    b.HasOne("EntityFrameworkLessson.Student", null)
                        .WithMany("Lessons")
                        .HasForeignKey("StudentId");

                    b.HasOne("EntityFrameworkLessson.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId");

                    b.Navigation("Fac");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("EntityFrameworkLessson.Mark", b =>
                {
                    b.HasOne("EntityFrameworkLessson.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EntityFrameworkLessson.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("EntityFrameworkLessson.Student", b =>
                {
                    b.HasOne("EntityFrameworkLessson.Facultet", "Facultet")
                        .WithMany("Students")
                        .HasForeignKey("FacultetId");

                    b.Navigation("Facultet");
                });

            modelBuilder.Entity("EntityFrameworkLessson.Facultet", b =>
                {
                    b.Navigation("Lessons");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("EntityFrameworkLessson.Student", b =>
                {
                    b.Navigation("Lessons");
                });
#pragma warning restore 612, 618
        }
    }
}
