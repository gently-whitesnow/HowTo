﻿// <auto-generated />
using System;
using HowTo.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HowTo.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("HowTo.Entities.Article.ArticleDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CourseDtoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CourseDtoId");

                    b.ToTable("ArticleContext");
                });

            modelBuilder.Entity("HowTo.Entities.Contributor.ContributorEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ContributorEntityContext");
                });

            modelBuilder.Entity("HowTo.Entities.Course.CourseDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("CourseContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.CheckList.CheckListDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClausesJsonStringArray")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CheckListContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.CheckList.LastCheckListDto", b =>
                {
                    b.Property<int>("InteractiveId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CheckedClausesJsonBoolArray")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("InteractiveId", "CourseId", "ArticleId", "UserId");

                    b.ToTable("LastCheckListContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.ChoiceOfAnswers.ChoiceOfAnswerDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AnswersJsonBoolArray")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("QuestionsJsonStringArray")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ChoiceOfAnswerContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.ChoiceOfAnswers.LastChoiceOfAnswerDto", b =>
                {
                    b.Property<int>("InteractiveId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AnswersJsonBoolArray")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SuccessAnswersJsonBoolArray")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("InteractiveId", "CourseId", "ArticleId", "UserId");

                    b.ToTable("LastChoiceOfAnswerContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.ChoiceOfAnswers.LogChoiceOfAnswerDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AnswersJsonBoolArray")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("InteractiveId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("LogDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("SuccessAnswersJsonBoolArray")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LogChoiceOfAnswerContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.ProgramWriting.LastProgramWritingDto", b =>
                {
                    b.Property<int>("InteractiveId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Output")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Success")
                        .HasColumnType("INTEGER");

                    b.HasKey("InteractiveId", "CourseId", "ArticleId", "UserId");

                    b.ToTable("LastProgramWritingContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.ProgramWriting.LogProgramWritingDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("InteractiveId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("LogDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Success")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LogProgramWritingContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.ProgramWriting.ProgramWritingDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ProgramWritingContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.WritingOfAnswer.LastWritingOfAnswerDto", b =>
                {
                    b.Property<int>("InteractiveId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Success")
                        .HasColumnType("INTEGER");

                    b.HasKey("InteractiveId", "CourseId", "ArticleId", "UserId");

                    b.ToTable("LastWritingOfAnswerContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.WritingOfAnswer.LogWritingOfAnswerDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("InteractiveId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("LogDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Success")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LogWritingOfAnswerContext");
                });

            modelBuilder.Entity("HowTo.Entities.Interactive.WritingOfAnswer.WritingOfAnswerDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WritingOfAnswerContext");
                });

            modelBuilder.Entity("HowTo.Entities.UserInfo.UserUniqueInfoDto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("LastReadCourseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("UserUniqueInfoContext");
                });

            modelBuilder.Entity("HowTo.Entities.ViewedEntity.ViewedEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("UserUniqueInfoDtoId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserUniqueInfoDtoId");

                    b.ToTable("UserViewEntityContext");
                });

            modelBuilder.Entity("HowTo.Entities.Views.UserGuid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ViewDtoArticleId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ViewDtoCourseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ViewDtoCourseId", "ViewDtoArticleId");

                    b.ToTable("UserGuid");
                });

            modelBuilder.Entity("HowTo.Entities.Views.ViewDto", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CourseId", "ArticleId");

                    b.ToTable("ViewContext");
                });

            modelBuilder.Entity("HowTo.Entities.Article.ArticleDto", b =>
                {
                    b.HasOne("HowTo.Entities.Contributor.ContributorEntity", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HowTo.Entities.Course.CourseDto", null)
                        .WithMany("Articles")
                        .HasForeignKey("CourseDtoId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("HowTo.Entities.Course.CourseDto", b =>
                {
                    b.HasOne("HowTo.Entities.Contributor.ContributorEntity", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("HowTo.Entities.ViewedEntity.ViewedEntity", b =>
                {
                    b.HasOne("HowTo.Entities.UserInfo.UserUniqueInfoDto", null)
                        .WithMany("ApprovedViewArticleIds")
                        .HasForeignKey("UserUniqueInfoDtoId");
                });

            modelBuilder.Entity("HowTo.Entities.Views.UserGuid", b =>
                {
                    b.HasOne("HowTo.Entities.Views.ViewDto", null)
                        .WithMany("Viewers")
                        .HasForeignKey("ViewDtoCourseId", "ViewDtoArticleId");
                });

            modelBuilder.Entity("HowTo.Entities.Course.CourseDto", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("HowTo.Entities.UserInfo.UserUniqueInfoDto", b =>
                {
                    b.Navigation("ApprovedViewArticleIds");
                });

            modelBuilder.Entity("HowTo.Entities.Views.ViewDto", b =>
                {
                    b.Navigation("Viewers");
                });
#pragma warning restore 612, 618
        }
    }
}
