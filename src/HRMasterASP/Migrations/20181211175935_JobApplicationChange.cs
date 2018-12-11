using Microsoft.EntityFrameworkCore.Migrations;

namespace HRMasterASP.Migrations
{
    public partial class JobApplicationChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CvUrl",
                table: "JobApplications",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CvUrl",
                table: "JobApplications",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
