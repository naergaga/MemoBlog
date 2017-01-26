using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemoBlog.Data.Migrations
{
    public partial class UserImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Emoticons",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Emoticons",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Emoticons",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Emoticons",
                nullable: true);
        }
    }
}
