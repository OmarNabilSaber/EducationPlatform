using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineEducationPlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class addAssignmentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmission_StudentId",
                table: "AssignmentSubmission",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentSubmission_AspNetUsers_StudentId",
                table: "AssignmentSubmission",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentSubmission_Assignments_AssignmentId",
                table: "AssignmentSubmission",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "AssignmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentSubmission_AspNetUsers_StudentId",
                table: "AssignmentSubmission");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentSubmission_Assignments_AssignmentId",
                table: "AssignmentSubmission");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentSubmission_StudentId",
                table: "AssignmentSubmission");
        }
    }
}
