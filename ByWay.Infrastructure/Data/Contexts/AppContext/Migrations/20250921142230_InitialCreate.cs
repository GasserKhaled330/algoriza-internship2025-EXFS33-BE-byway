using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByWay.Infrastructure.Data.Contexts.AppContext.Migrations
{
  /// <inheritdoc />
  internal partial class InitialCreate : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Categories",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
            ImagePath = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Categories", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Instructors",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
            Rate = table.Column<double>(type: "float", nullable: false),
            JobTitle = table.Column<int>(type: "int", nullable: false),
            ImagePath = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Instructors", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Courses",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
            Certification = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
            Cost = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
            Level = table.Column<int>(type: "int", nullable: false),
            Rate = table.Column<double>(type: "float", nullable: false),
            TotalHours = table.Column<int>(type: "int", nullable: false),
            InstructorId = table.Column<int>(type: "int", nullable: false),
            CategoryId = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Courses", x => x.Id);
            table.ForeignKey(
                      name: "FK_Courses_Categories_CategoryId",
                      column: x => x.CategoryId,
                      principalTable: "Categories",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_Courses_Instructors_InstructorId",
                      column: x => x.InstructorId,
                      principalTable: "Instructors",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "Contents",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            LecturesCount = table.Column<int>(type: "int", nullable: false),
            DurationInHours = table.Column<int>(type: "int", nullable: false),
            CourseId = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Contents", x => x.Id);
            table.ForeignKey(
                      name: "FK_Contents_Courses_CourseId",
                      column: x => x.CourseId,
                      principalTable: "Courses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Contents_CourseId",
          table: "Contents",
          column: "CourseId");

      migrationBuilder.CreateIndex(
          name: "IX_Courses_CategoryId",
          table: "Courses",
          column: "CategoryId");

      migrationBuilder.CreateIndex(
          name: "IX_Courses_InstructorId",
          table: "Courses",
          column: "InstructorId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Contents");

      migrationBuilder.DropTable(
          name: "Courses");

      migrationBuilder.DropTable(
          name: "Categories");

      migrationBuilder.DropTable(
          name: "Instructors");
    }
  }
}
