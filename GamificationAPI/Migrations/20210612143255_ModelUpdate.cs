using Microsoft.EntityFrameworkCore.Migrations;

namespace GamificationAPI.Migrations
{
    public partial class ModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badge_Application_ApplicationId",
                table: "Badge");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Application_ApplicationId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Player_PlayerId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Application_ApplicationId",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Rule_Application_ApplicationId",
                table: "Rule");

            migrationBuilder.DropForeignKey(
                name: "FK_Rule_Badge_BadgeId",
                table: "Rule");

            migrationBuilder.DropIndex(
                name: "IX_Rule_ApplicationId",
                table: "Rule");

            migrationBuilder.DropIndex(
                name: "IX_Rule_BadgeId",
                table: "Rule");

            migrationBuilder.DropIndex(
                name: "IX_Player_ApplicationId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Event_ApplicationId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_PlayerId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Badge_ApplicationId",
                table: "Badge");

            migrationBuilder.AlterColumn<int>(
                name: "BadgeId",
                table: "Rule",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "Rule",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ApplicationId1",
                table: "Rule",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BadgeId1",
                table: "Rule",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "Player",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ApplicationId1",
                table: "Player",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Event",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "Event",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ApplicationId1",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PlayerId1",
                table: "Event",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "Badge",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ApplicationId1",
                table: "Badge",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rule_ApplicationId1",
                table: "Rule",
                column: "ApplicationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_BadgeId1",
                table: "Rule",
                column: "BadgeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Player_ApplicationId1",
                table: "Player",
                column: "ApplicationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ApplicationId1",
                table: "Event",
                column: "ApplicationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Event_PlayerId1",
                table: "Event",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Badge_ApplicationId1",
                table: "Badge",
                column: "ApplicationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Badge_Application_ApplicationId1",
                table: "Badge",
                column: "ApplicationId1",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Application_ApplicationId1",
                table: "Event",
                column: "ApplicationId1",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Player_PlayerId1",
                table: "Event",
                column: "PlayerId1",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Application_ApplicationId1",
                table: "Player",
                column: "ApplicationId1",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rule_Application_ApplicationId1",
                table: "Rule",
                column: "ApplicationId1",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rule_Badge_BadgeId1",
                table: "Rule",
                column: "BadgeId1",
                principalTable: "Badge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badge_Application_ApplicationId1",
                table: "Badge");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Application_ApplicationId1",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Player_PlayerId1",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Application_ApplicationId1",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Rule_Application_ApplicationId1",
                table: "Rule");

            migrationBuilder.DropForeignKey(
                name: "FK_Rule_Badge_BadgeId1",
                table: "Rule");

            migrationBuilder.DropIndex(
                name: "IX_Rule_ApplicationId1",
                table: "Rule");

            migrationBuilder.DropIndex(
                name: "IX_Rule_BadgeId1",
                table: "Rule");

            migrationBuilder.DropIndex(
                name: "IX_Player_ApplicationId1",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Event_ApplicationId1",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_PlayerId1",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Badge_ApplicationId1",
                table: "Badge");

            migrationBuilder.DropColumn(
                name: "ApplicationId1",
                table: "Rule");

            migrationBuilder.DropColumn(
                name: "BadgeId1",
                table: "Rule");

            migrationBuilder.DropColumn(
                name: "ApplicationId1",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "ApplicationId1",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "ApplicationId1",
                table: "Badge");

            migrationBuilder.AlterColumn<long>(
                name: "BadgeId",
                table: "Rule",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApplicationId",
                table: "Rule",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApplicationId",
                table: "Player",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PlayerId",
                table: "Event",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApplicationId",
                table: "Event",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApplicationId",
                table: "Badge",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rule_ApplicationId",
                table: "Rule",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rule_BadgeId",
                table: "Rule",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_ApplicationId",
                table: "Player",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_ApplicationId",
                table: "Event",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_PlayerId",
                table: "Event",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Badge_ApplicationId",
                table: "Badge",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Badge_Application_ApplicationId",
                table: "Badge",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Application_ApplicationId",
                table: "Event",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Player_PlayerId",
                table: "Event",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Application_ApplicationId",
                table: "Player",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rule_Application_ApplicationId",
                table: "Rule",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rule_Badge_BadgeId",
                table: "Rule",
                column: "BadgeId",
                principalTable: "Badge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
