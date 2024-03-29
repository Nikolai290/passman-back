﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using passman_back.Infrastructure.Data.DbContexts;

#nullable disable

namespace passman_back.Infrastructure.Data.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20220415091517_inviteCodes")]
    partial class inviteCodes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DirectoryPasscard", b =>
                {
                    b.Property<long>("ParentsId")
                        .HasColumnType("bigint")
                        .HasColumnName("parents_id");

                    b.Property<long>("PasscardsId")
                        .HasColumnType("bigint")
                        .HasColumnName("passcards_id");

                    b.HasKey("ParentsId", "PasscardsId")
                        .HasName("pk_directory_passcards_relations");

                    b.HasIndex("PasscardsId")
                        .HasDatabaseName("ix_directory_passcards_relations_passcards_id");

                    b.ToTable("directory_passcards_relations", (string)null);
                });

            modelBuilder.Entity("InviteCodeUserGroup", b =>
                {
                    b.Property<long>("InviteCodesId")
                        .HasColumnType("bigint")
                        .HasColumnName("invite_codes_id");

                    b.Property<long>("UserGroupsId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_groups_id");

                    b.HasKey("InviteCodesId", "UserGroupsId")
                        .HasName("pk_invite_code_user_group");

                    b.HasIndex("UserGroupsId")
                        .HasDatabaseName("ix_invite_code_user_group_user_groups_id");

                    b.ToTable("invite_code_user_group", (string)null);
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.Directory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint")
                        .HasColumnName("parent_id");

                    b.HasKey("Id")
                        .HasName("pk_directories");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("ix_directories_parent_id");

                    b.ToTable("directories", (string)null);
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.InviteCode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("AliveBefore")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("alive_before");

                    b.Property<string>("Code")
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<bool>("IsStopped")
                        .HasColumnType("boolean")
                        .HasColumnName("is_stopped");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_invite_codes");

                    b.ToTable("invite_codes", (string)null);
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.Passcard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<string>("Login")
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Url")
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_passcards");

                    b.ToTable("passcards", (string)null);
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.RestorePasswordCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AliveBefore")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("alive_before");

                    b.Property<string>("RestoreCode")
                        .HasColumnType("text")
                        .HasColumnName("restore_code");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_restore_password_codes");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_restore_password_codes_user_id");

                    b.ToTable("restore_password_codes", (string)null);
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_blocked");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("Nickname")
                        .HasColumnType("text")
                        .HasColumnName("nickname");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("PatronymicName")
                        .HasColumnType("text")
                        .HasColumnName("patronymic_name");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.UserGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_user_groups");

                    b.ToTable("user_groups", (string)null);
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.UserGroupDirectoryRelation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("DirectoryId")
                        .HasColumnType("bigint")
                        .HasColumnName("directory_id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<int>("Permission")
                        .HasColumnType("integer")
                        .HasColumnName("permission");

                    b.Property<long?>("UserGroupId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_group_id");

                    b.HasKey("Id")
                        .HasName("pk_user_group_directory_relations");

                    b.HasIndex("DirectoryId")
                        .HasDatabaseName("ix_user_group_directory_relations_directory_id");

                    b.HasIndex("UserGroupId")
                        .HasDatabaseName("ix_user_group_directory_relations_user_group_id");

                    b.ToTable("user_group_directory_relations", (string)null);
                });

            modelBuilder.Entity("UserUserGroup", b =>
                {
                    b.Property<long>("UserGroupsId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_groups_id");

                    b.Property<long>("UsersId")
                        .HasColumnType("bigint")
                        .HasColumnName("users_id");

                    b.HasKey("UserGroupsId", "UsersId")
                        .HasName("pk_user_user_group");

                    b.HasIndex("UsersId")
                        .HasDatabaseName("ix_user_user_group_users_id");

                    b.ToTable("user_user_group", (string)null);
                });

            modelBuilder.Entity("DirectoryPasscard", b =>
                {
                    b.HasOne("passman_back.Domain.Core.DbEntities.Directory", null)
                        .WithMany()
                        .HasForeignKey("ParentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_directory_passcards_relations_directories_parents_id");

                    b.HasOne("passman_back.Domain.Core.DbEntities.Passcard", null)
                        .WithMany()
                        .HasForeignKey("PasscardsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_directory_passcards_relations_passcards_passcards_id");
                });

            modelBuilder.Entity("InviteCodeUserGroup", b =>
                {
                    b.HasOne("passman_back.Domain.Core.DbEntities.InviteCode", null)
                        .WithMany()
                        .HasForeignKey("InviteCodesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_invite_code_user_group_invite_codes_invite_codes_id");

                    b.HasOne("passman_back.Domain.Core.DbEntities.UserGroup", null)
                        .WithMany()
                        .HasForeignKey("UserGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_invite_code_user_group_user_groups_user_groups_id");
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.Directory", b =>
                {
                    b.HasOne("passman_back.Domain.Core.DbEntities.Directory", "Parent")
                        .WithMany("Childrens")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("fk_directories_directories_parent_id");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.RestorePasswordCode", b =>
                {
                    b.HasOne("passman_back.Domain.Core.DbEntities.User", "User")
                        .WithOne("RestorePasswordCode")
                        .HasForeignKey("passman_back.Domain.Core.DbEntities.RestorePasswordCode", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_restore_password_codes_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.UserGroupDirectoryRelation", b =>
                {
                    b.HasOne("passman_back.Domain.Core.DbEntities.Directory", "Directory")
                        .WithMany("Relations")
                        .HasForeignKey("DirectoryId")
                        .HasConstraintName("fk_user_group_directory_relations_directories_directory_id");

                    b.HasOne("passman_back.Domain.Core.DbEntities.UserGroup", "UserGroup")
                        .WithMany("Relations")
                        .HasForeignKey("UserGroupId")
                        .HasConstraintName("fk_user_group_directory_relations_user_groups_user_group_id");

                    b.Navigation("Directory");

                    b.Navigation("UserGroup");
                });

            modelBuilder.Entity("UserUserGroup", b =>
                {
                    b.HasOne("passman_back.Domain.Core.DbEntities.UserGroup", null)
                        .WithMany()
                        .HasForeignKey("UserGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_user_group_user_groups_user_groups_id");

                    b.HasOne("passman_back.Domain.Core.DbEntities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_user_group_users_users_id");
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.Directory", b =>
                {
                    b.Navigation("Childrens");

                    b.Navigation("Relations");
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.User", b =>
                {
                    b.Navigation("RestorePasswordCode");
                });

            modelBuilder.Entity("passman_back.Domain.Core.DbEntities.UserGroup", b =>
                {
                    b.Navigation("Relations");
                });
#pragma warning restore 612, 618
        }
    }
}
