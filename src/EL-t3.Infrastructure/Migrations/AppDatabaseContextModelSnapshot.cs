﻿// <auto-generated />
using System;
using EL_t3.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EL_t3.Infrastructure.Migrations
{
    [DbContext(typeof(AppDatabaseContext))]
    partial class AppDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EL_t3.Domain.Entities.Club", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("code");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CrestUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("crest_url");

                    b.Property<bool>("IsNba")
                        .HasColumnType("boolean")
                        .HasColumnName("is_nba");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id")
                        .HasName("pk_clubs");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasDatabaseName("ix_clubs_code");

                    b.HasIndex("CrestUrl")
                        .IsUnique()
                        .HasDatabaseName("ix_clubs_crest_url");

                    b.ToTable("clubs", "euroleague-grids");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.Grid", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_grids");

                    b.ToTable("grids", "euroleague-grids");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.GridItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("GridItemType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("grid_item_type");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_grid_items");

                    b.ToTable("grid_items", "euroleague-grids");

                    b.HasDiscriminator<string>("GridItemType").HasValue("GridItem");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("birth_date");

                    b.Property<string>("Country")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("country");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id")
                        .HasName("pk_players");

                    b.HasIndex("FirstName", "LastName")
                        .HasDatabaseName("ix_players_first_name_last_name");

                    b.HasIndex("FirstName", "LastName", "BirthDate")
                        .IsUnique()
                        .HasDatabaseName("ix_players_first_name_last_name_birth_date");

                    b.ToTable("players", "euroleague-grids");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.PlayerSeason", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ClubId")
                        .HasColumnType("bigint")
                        .HasColumnName("club_id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("end_date");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint")
                        .HasColumnName("player_id");

                    b.Property<int>("Season")
                        .HasColumnType("integer")
                        .HasColumnName("season");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id")
                        .HasName("pk_player_seasons");

                    b.HasIndex("PlayerId")
                        .HasDatabaseName("ix_player_seasons_player_id");

                    b.HasIndex("ClubId", "PlayerId", "Season")
                        .IsUnique()
                        .HasDatabaseName("ix_player_seasons_club_id_player_id_season");

                    b.ToTable("player_seasons", "euroleague-grids");
                });

            modelBuilder.Entity("grid-members-x", b =>
                {
                    b.Property<long>("grid_item_id")
                        .HasColumnType("bigint")
                        .HasColumnName("grid_item_id");

                    b.Property<long>("grid_id")
                        .HasColumnType("bigint")
                        .HasColumnName("grid_id");

                    b.HasKey("grid_item_id", "grid_id")
                        .HasName("pk_grid_members_x");

                    b.HasIndex("grid_id")
                        .HasDatabaseName("ix_grid_members_x_grid_id");

                    b.ToTable("grid_members_x", "euroleague-grids");
                });

            modelBuilder.Entity("grid-members-y", b =>
                {
                    b.Property<long>("grid_item_id")
                        .HasColumnType("bigint")
                        .HasColumnName("grid_item_id");

                    b.Property<long>("grid_id")
                        .HasColumnType("bigint")
                        .HasColumnName("grid_id");

                    b.HasKey("grid_item_id", "grid_id")
                        .HasName("pk_grid_members_y");

                    b.HasIndex("grid_id")
                        .HasDatabaseName("ix_grid_members_y_grid_id");

                    b.ToTable("grid_members_y", "euroleague-grids");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.ClubGridItem", b =>
                {
                    b.HasBaseType("EL_t3.Domain.Entities.GridItem");

                    b.Property<long>("ClubId")
                        .HasColumnType("bigint")
                        .HasColumnName("club_id");

                    b.HasIndex("ClubId")
                        .HasDatabaseName("ix_grid_items_club_id");

                    b.ToTable("grid_items", "euroleague-grids");

                    b.HasDiscriminator().HasValue("Club");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.CountryGridItem", b =>
                {
                    b.HasBaseType("EL_t3.Domain.Entities.GridItem");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("country");

                    b.ToTable("grid_items", "euroleague-grids");

                    b.HasDiscriminator().HasValue("Country");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.TeammateGridItem", b =>
                {
                    b.HasBaseType("EL_t3.Domain.Entities.GridItem");

                    b.Property<long>("TeammateId")
                        .HasColumnType("bigint")
                        .HasColumnName("teammate_id");

                    b.HasIndex("TeammateId")
                        .HasDatabaseName("ix_grid_items_teammate_id");

                    b.ToTable("grid_items", "euroleague-grids");

                    b.HasDiscriminator().HasValue("Teammate");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.PlayerSeason", b =>
                {
                    b.HasOne("EL_t3.Domain.Entities.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_player_seasons_clubs_club_id");

                    b.HasOne("EL_t3.Domain.Entities.Player", "Player")
                        .WithMany("SeasonsPlayed")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_player_seasons_players_player_id");

                    b.Navigation("Club");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("grid-members-x", b =>
                {
                    b.HasOne("EL_t3.Domain.Entities.Grid", null)
                        .WithMany()
                        .HasForeignKey("grid_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_grid_members_x_grids_grid_id");

                    b.HasOne("EL_t3.Domain.Entities.GridItem", null)
                        .WithMany()
                        .HasForeignKey("grid_item_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_grid_members_x_grid_items_grid_item_id");
                });

            modelBuilder.Entity("grid-members-y", b =>
                {
                    b.HasOne("EL_t3.Domain.Entities.Grid", null)
                        .WithMany()
                        .HasForeignKey("grid_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_grid_members_y_grids_grid_id");

                    b.HasOne("EL_t3.Domain.Entities.GridItem", null)
                        .WithMany()
                        .HasForeignKey("grid_item_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_grid_members_y_grid_items_grid_item_id");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.ClubGridItem", b =>
                {
                    b.HasOne("EL_t3.Domain.Entities.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired()
                        .HasConstraintName("fk_grid_items_clubs_club_id");

                    b.Navigation("Club");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.TeammateGridItem", b =>
                {
                    b.HasOne("EL_t3.Domain.Entities.Player", "Teammate")
                        .WithMany()
                        .HasForeignKey("TeammateId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired()
                        .HasConstraintName("fk_grid_items_players_teammate_id");

                    b.Navigation("Teammate");
                });

            modelBuilder.Entity("EL_t3.Domain.Entities.Player", b =>
                {
                    b.Navigation("SeasonsPlayed");
                });
#pragma warning restore 612, 618
        }
    }
}
