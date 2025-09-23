using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class opportunityAndDeptSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "Name" },
                values: new object[,]
                {
                    { 1, "Camera & Video Editing" },
                    { 2, "Marketing" },
                    { 3, "Graphic Design" },
                    { 4, "Operation" },
                    { 5, "PR" },
                    { 6, "Sales" },
                    { 7, "Web Development" },
                    { 8, "HR" },
                    { 9, "Marketing Manager" }
                });

            migrationBuilder.InsertData(
                table: "Opportunities",
                columns: new[] { "OpportunityId", "ApplicationEndDate", "ApplicationStartDate", "DepartmentId", "Status", "Title", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "open", "Video Editing Internship", "internship", 4 },
                    { 2, new DateTime(2025, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "open", "Marketing Coordinator Job Offer", "job offer", 4 },
                    { 3, new DateTime(2025, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "open", "Graphic Design Internship", "internship", 4 },
                    { 4, new DateTime(2025, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "open", "Operations Assistant Job Offer", "job offer", 4 },
                    { 5, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "open", "PR Internship", "internship", 4 },
                    { 6, new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "open", "Sales Executive Job Offer", "job offer", 4 },
                    { 7, new DateTime(2025, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "open", "Web Development Internship", "internship", 4 },
                    { 8, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "open", "HR Specialist Job Offer", "job offer", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Opportunities",
                keyColumn: "OpportunityId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Opportunities",
                keyColumn: "OpportunityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Opportunities",
                keyColumn: "OpportunityId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Opportunities",
                keyColumn: "OpportunityId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Opportunities",
                keyColumn: "OpportunityId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Opportunities",
                keyColumn: "OpportunityId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Opportunities",
                keyColumn: "OpportunityId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Opportunities",
                keyColumn: "OpportunityId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 8);
        }
    }
}
