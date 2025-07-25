using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineEducationPlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class fixingClassAssignmentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Classes_SubjectId",
                table: "Assignments");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ClassId",
                table: "Assignments",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Classes_ClassId",
                table: "Assignments",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Classes_ClassId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_ClassId",
                table: "Assignments");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Classes_SubjectId",
                table: "Assignments",
                column: "SubjectId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
