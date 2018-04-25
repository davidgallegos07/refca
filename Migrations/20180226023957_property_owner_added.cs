using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace refca.Migrations
{
    public partial class property_owner_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Thesis",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Research",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Presentations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Magazines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Chapterbooks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Thesis");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Research");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Presentations");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Magazines");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Chapterbooks");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Articles");
        }
    }
}
