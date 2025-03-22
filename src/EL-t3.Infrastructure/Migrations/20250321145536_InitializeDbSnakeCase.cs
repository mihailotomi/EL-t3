using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EL_t3.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDbSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "euroleague-grids");

            migrationBuilder.CreateTable(
                name: "clubs",
                schema: "euroleague-grids",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    crest_url = table.Column<string>(type: "text", nullable: false),
                    is_nba = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clubs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grids",
                schema: "euroleague-grids",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grids", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                schema: "euroleague-grids",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    country = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_players", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grid_items",
                schema: "euroleague-grids",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grid_item_type = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    club_id = table.Column<long>(type: "bigint", nullable: true),
                    country = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    teammate_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grid_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_grid_items_clubs_club_id",
                        column: x => x.club_id,
                        principalSchema: "euroleague-grids",
                        principalTable: "clubs",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_grid_items_players_teammate_id",
                        column: x => x.teammate_id,
                        principalSchema: "euroleague-grids",
                        principalTable: "players",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "player_seasons",
                schema: "euroleague-grids",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    club_id = table.Column<long>(type: "bigint", nullable: false),
                    player_id = table.Column<long>(type: "bigint", nullable: false),
                    season = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_seasons", x => x.id);
                    table.ForeignKey(
                        name: "fk_player_seasons_clubs_club_id",
                        column: x => x.club_id,
                        principalSchema: "euroleague-grids",
                        principalTable: "clubs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_player_seasons_players_player_id",
                        column: x => x.player_id,
                        principalSchema: "euroleague-grids",
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grid_members_x",
                schema: "euroleague-grids",
                columns: table => new
                {
                    grid_item_id = table.Column<long>(type: "bigint", nullable: false),
                    grid_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grid_members_x", x => new { x.grid_item_id, x.grid_id });
                    table.ForeignKey(
                        name: "fk_grid_members_x_grid_items_grid_item_id",
                        column: x => x.grid_item_id,
                        principalSchema: "euroleague-grids",
                        principalTable: "grid_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_grid_members_x_grids_grid_id",
                        column: x => x.grid_id,
                        principalSchema: "euroleague-grids",
                        principalTable: "grids",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grid_members_y",
                schema: "euroleague-grids",
                columns: table => new
                {
                    grid_item_id = table.Column<long>(type: "bigint", nullable: false),
                    grid_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grid_members_y", x => new { x.grid_item_id, x.grid_id });
                    table.ForeignKey(
                        name: "fk_grid_members_y_grid_items_grid_item_id",
                        column: x => x.grid_item_id,
                        principalSchema: "euroleague-grids",
                        principalTable: "grid_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_grid_members_y_grids_grid_id",
                        column: x => x.grid_id,
                        principalSchema: "euroleague-grids",
                        principalTable: "grids",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_clubs_code",
                schema: "euroleague-grids",
                table: "clubs",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clubs_crest_url",
                schema: "euroleague-grids",
                table: "clubs",
                column: "crest_url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_grid_items_club_id",
                schema: "euroleague-grids",
                table: "grid_items",
                column: "club_id");

            migrationBuilder.CreateIndex(
                name: "ix_grid_items_teammate_id",
                schema: "euroleague-grids",
                table: "grid_items",
                column: "teammate_id");

            migrationBuilder.CreateIndex(
                name: "ix_grid_members_x_grid_id",
                schema: "euroleague-grids",
                table: "grid_members_x",
                column: "grid_id");

            migrationBuilder.CreateIndex(
                name: "ix_grid_members_y_grid_id",
                schema: "euroleague-grids",
                table: "grid_members_y",
                column: "grid_id");

            migrationBuilder.CreateIndex(
                name: "ix_player_seasons_club_id_player_id_season",
                schema: "euroleague-grids",
                table: "player_seasons",
                columns: new[] { "club_id", "player_id", "season" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_player_seasons_player_id",
                schema: "euroleague-grids",
                table: "player_seasons",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_players_first_name_last_name",
                schema: "euroleague-grids",
                table: "players",
                columns: new[] { "first_name", "last_name" });

            migrationBuilder.CreateIndex(
                name: "ix_players_first_name_last_name_birth_date",
                schema: "euroleague-grids",
                table: "players",
                columns: new[] { "first_name", "last_name", "birth_date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grid_members_x",
                schema: "euroleague-grids");

            migrationBuilder.DropTable(
                name: "grid_members_y",
                schema: "euroleague-grids");

            migrationBuilder.DropTable(
                name: "player_seasons",
                schema: "euroleague-grids");

            migrationBuilder.DropTable(
                name: "grid_items",
                schema: "euroleague-grids");

            migrationBuilder.DropTable(
                name: "grids",
                schema: "euroleague-grids");

            migrationBuilder.DropTable(
                name: "clubs",
                schema: "euroleague-grids");

            migrationBuilder.DropTable(
                name: "players",
                schema: "euroleague-grids");
        }
    }
}
