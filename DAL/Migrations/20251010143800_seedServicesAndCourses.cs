using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class seedServicesAndCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "Comprehensive marketing services including digital campaigns, social media management, and promotional strategies.", "Marketing", 15 },
                    { 2, "Brand identity creation, logo design, and strategic brand positioning services.", "Brand Development", 15 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "Name", "ServiceId" },
                values: new object[,]
                {
                    { 1, "Learn the essential skills for online marketing including SEO, SEM, and PPC.", "Digital Marketing Fundamentals", 1 },
                    { 2, "Develop effective strategies to grow businesses on Facebook, Instagram, and LinkedIn.", "Social Media Marketing", 1 },
                    { 3, "Learn to design and execute email campaigns that convert leads into customers.", "Email Marketing Strategies", 1 },
                    { 4, "Master content creation and optimization techniques to drive organic growth.", "Content Marketing & SEO", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
