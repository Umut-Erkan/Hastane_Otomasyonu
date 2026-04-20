using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApiProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlotDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Appointm__3214EC07039A92A5", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audit Logs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Userıd = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BrowserInfo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Time = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit Logs", x => x.LogID);
                });

            migrationBuilder.CreateTable(
                name: "Doktor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    İsim = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Soyisim = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Alan = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Tc = table.Column<long>(type: "bigint", nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Eposta = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    AccessToken = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    RefreshToken = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    RefreshTokenEndDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doktor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hasta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tc = table.Column<long>(type: "bigint", nullable: false),
                    İsim = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Soyisim = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Eposta = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    AccessToken = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    RefreshToken = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    RefreshTokenEndDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hasta_1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HospitalReceptionist",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tc = table.Column<long>(type: "bigint", nullable: false),
                    İsim = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Soyisim = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Eposta = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Alan = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AccessToken = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    RefreshToken = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false),
                    RefreshTokenEndDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalReceptionist", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Ilac",
                columns: table => new
                {
                    IlacID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ilac_Name = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false),
                    Kullanım_Alanı = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilac", x => x.IlacID);
                });

            migrationBuilder.CreateTable(
                name: "Appointment_To_Doktor",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoktorFK = table.Column<int>(type: "int", nullable: false),
                    AppointmentFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment_To_Doktor", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Appointment_To_Doktor_Appointment",
                        column: x => x.AppointmentFK,
                        principalTable: "Appointment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointment_To_Doktor_Doktor",
                        column: x => x.DoktorFK,
                        principalTable: "Doktor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OnlineRandevu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HastaŞikayet = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Saat = table.Column<TimeOnly>(type: "time", nullable: false),
                    Tarih = table.Column<DateOnly>(type: "date", nullable: false),
                    DoktorName = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    DoktorSurname = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    HastaName = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    HastaSurname = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    DoktorID = table.Column<int>(type: "int", nullable: false),
                    HastaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineRandevu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineRandevu_Doktor",
                        column: x => x.DoktorID,
                        principalTable: "Doktor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnlineRandevu_Hasta1",
                        column: x => x.HastaID,
                        principalTable: "Hasta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tedavi",
                columns: table => new
                {
                    TedaviID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoktorID = table.Column<int>(type: "int", nullable: false),
                    Tanı = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: false),
                    HastaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tedavi", x => x.TedaviID);
                    table.ForeignKey(
                        name: "FK_Tedavi_Doktor",
                        column: x => x.DoktorID,
                        principalTable: "Doktor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tedavi_Hasta",
                        column: x => x.HastaID,
                        principalTable: "Hasta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Kayıt",
                columns: table => new
                {
                    KayıtId = table.Column<int>(type: "int", nullable: false),
                    RandevuFK = table.Column<int>(type: "int", nullable: true),
                    YönlendirmeFişi = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kayıt", x => x.KayıtId);
                    table.ForeignKey(
                        name: "FK_Kayıt_OnlineRandevu",
                        column: x => x.RandevuFK,
                        principalTable: "OnlineRandevu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recete",
                columns: table => new
                {
                    ReceteID = table.Column<int>(type: "int", nullable: false),
                    Kullanım = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: false),
                    Gecerlilik_Tarihi = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recete", x => x.ReceteID);
                    table.ForeignKey(
                        name: "FK_Recete_Tedavi",
                        column: x => x.ReceteID,
                        principalTable: "Tedavi",
                        principalColumn: "TedaviID");
                });

            migrationBuilder.CreateTable(
                name: "Ilca_To_Recete",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceteFK = table.Column<int>(type: "int", nullable: false),
                    IlcaFK = table.Column<int>(type: "int", nullable: false),
                    Adet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilca_To_Recete", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ilca_To_Recete_Ilac",
                        column: x => x.IlcaFK,
                        principalTable: "Ilac",
                        principalColumn: "IlacID");
                    table.ForeignKey(
                        name: "FK_Ilca_To_Recete_Recete",
                        column: x => x.ReceteFK,
                        principalTable: "Recete",
                        principalColumn: "ReceteID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_To_Doktor_AppointmentFK",
                table: "Appointment_To_Doktor",
                column: "AppointmentFK");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_To_Doktor_DoktorFK",
                table: "Appointment_To_Doktor",
                column: "DoktorFK");

            migrationBuilder.CreateIndex(
                name: "IX_Ilca_To_Recete_IlcaFK",
                table: "Ilca_To_Recete",
                column: "IlcaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Ilca_To_Recete_ReceteFK",
                table: "Ilca_To_Recete",
                column: "ReceteFK");

            migrationBuilder.CreateIndex(
                name: "IX_Kayıt_RandevuFK",
                table: "Kayıt",
                column: "RandevuFK");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineRandevu_DoktorID",
                table: "OnlineRandevu",
                column: "DoktorID");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineRandevu_HastaID",
                table: "OnlineRandevu",
                column: "HastaID");

            migrationBuilder.CreateIndex(
                name: "IX_Tedavi_DoktorID",
                table: "Tedavi",
                column: "DoktorID");

            migrationBuilder.CreateIndex(
                name: "IX_Tedavi_HastaID",
                table: "Tedavi",
                column: "HastaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment_To_Doktor");

            migrationBuilder.DropTable(
                name: "Audit Logs");

            migrationBuilder.DropTable(
                name: "HospitalReceptionist");

            migrationBuilder.DropTable(
                name: "Ilca_To_Recete");

            migrationBuilder.DropTable(
                name: "Kayıt");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "Ilac");

            migrationBuilder.DropTable(
                name: "Recete");

            migrationBuilder.DropTable(
                name: "OnlineRandevu");

            migrationBuilder.DropTable(
                name: "Tedavi");

            migrationBuilder.DropTable(
                name: "Doktor");

            migrationBuilder.DropTable(
                name: "Hasta");
        }
    }
}
