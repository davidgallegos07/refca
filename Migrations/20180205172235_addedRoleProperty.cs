using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace refca.Migrations
{
    public partial class addedRoleProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "TeacherTheses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "TeacherResearch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "TeacherPresentations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "TeacherMagazines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "TeacherChapterbooks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "TeacherBooks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "TeacherArticles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeacherTheses");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeacherResearch");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeacherPresentations");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeacherMagazines");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeacherChapterbooks");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeacherBooks");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "TeacherArticles");
        }
    }
}
