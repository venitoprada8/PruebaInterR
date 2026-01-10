using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentRegistration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegistrationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FirstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Credits = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    TeacherId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StudentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    InscriptionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "maria.gonzalez@university.edu", "María", "González" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "carlos.rodriguez@university.edu", "Carlos", "Rodríguez" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "ana.martinez@university.edu", "Ana", "Martínez" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "pedro.lopez@university.edu", "Pedro", "López" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "laura.fernandez@university.edu", "Laura", "Fernández" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Code", "Credits", "Name", "TeacherId" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), "MAT101", 3, "Matemáticas Avanzadas", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("a1111111-1111-1111-1111-111111111112"), "FIS101", 3, "Física General", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("a2222222-2222-2222-2222-222222222221"), "PROG101", 3, "Programación I", new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), "BD101", 3, "Bases de Datos", new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("a3333333-3333-3333-3333-333333333331"), "HIST101", 3, "Historia Universal", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("a3333333-3333-3333-3333-333333333332"), "LIT101", 3, "Literatura Contemporánea", new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("a4444444-4444-4444-4444-444444444441"), "QUIM101", 3, "Química Orgánica", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("a4444444-4444-4444-4444-444444444442"), "BIO101", 3, "Biología Molecular", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("a5555555-5555-5555-5555-555555555551"), "ING101", 3, "Inglés Técnico", new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("a5555555-5555-5555-5555-555555555552"), "ADMIN101", 3, "Administración de Empresas", new Guid("55555555-5555-5555-5555-555555555555") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_StudentId_SubjectId",
                table: "Inscriptions",
                columns: new[] { "StudentId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_SubjectId",
                table: "Inscriptions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Code",
                table: "Subjects",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TeacherId",
                table: "Subjects",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Email",
                table: "Teachers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscriptions");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
