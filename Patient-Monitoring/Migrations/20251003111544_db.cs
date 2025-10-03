using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient_Monitoring.Migrations
{
    /// <inheritdoc />
    public partial class db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointment_Alerts",
                columns: table => new
                {
                    AlertID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppointmentID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment_Alerts", x => x.AlertID);
                });

            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    DiseaseId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiseaseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.DiseaseId);
                });

            migrationBuilder.CreateTable(
                name: "Doctor_Details",
                columns: table => new
                {
                    DoctorID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor_Details", x => x.DoctorID);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Details",
                columns: table => new
                {
                    PatientID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmergencyContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Details", x => x.PatientID);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Doctor_Mapper",
                columns: table => new
                {
                    PatientID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DoctorID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Doctor_Mapper", x => x.PatientID);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Medications",
                columns: table => new
                {
                    MedicationID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MedicationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrescribingDoctorId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Medications", x => x.MedicationID);
                });

            migrationBuilder.CreateTable(
                name: "Wellness_Plans",
                columns: table => new
                {
                    PlanID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Recommended_Duration = table.Column<int>(type: "int", nullable: false),
                    Frequency_Count = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Frequency_Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    is_custom = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wellness_Plans", x => x.PlanID);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DoctorID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Appointment_Date_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctor_Details_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctor_Details",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient_Plan_Mapper",
                columns: table => new
                {
                    AssignmentID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Wellness_PlanPlanID = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Plan_Mapper", x => x.AssignmentID);
                    table.ForeignKey(
                        name: "FK_Patient_Plan_Mapper_Wellness_Plans_Wellness_PlanPlanID",
                        column: x => x.Wellness_PlanPlanID,
                        principalTable: "Wellness_Plans",
                        principalColumn: "PlanID");
                });

            migrationBuilder.CreateTable(
                name: "Patient_Diagnoses",
                columns: table => new
                {
                    DiagnosisID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DiseaseId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppointmentId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient_Diagnoses", x => x.DiagnosisID);
                    table.ForeignKey(
                        name: "FK_Patient_Diagnoses_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_Diagnoses_Diseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Diseases",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_Diagnoses_Patient_Details_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patient_Details",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorID",
                table: "Appointments",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Diagnoses_AppointmentId",
                table: "Patient_Diagnoses",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Diagnoses_DiseaseId",
                table: "Patient_Diagnoses",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Diagnoses_PatientID",
                table: "Patient_Diagnoses",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_Plan_Mapper_Wellness_PlanPlanID",
                table: "Patient_Plan_Mapper",
                column: "Wellness_PlanPlanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment_Alerts");

            migrationBuilder.DropTable(
                name: "Patient_Diagnoses");

            migrationBuilder.DropTable(
                name: "Patient_Doctor_Mapper");

            migrationBuilder.DropTable(
                name: "Patient_Medications");

            migrationBuilder.DropTable(
                name: "Patient_Plan_Mapper");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "Patient_Details");

            migrationBuilder.DropTable(
                name: "Wellness_Plans");

            migrationBuilder.DropTable(
                name: "Doctor_Details");
        }
    }
}
