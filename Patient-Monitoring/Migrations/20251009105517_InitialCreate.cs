using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient_Monitoring.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    DiseaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiseaseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.DiseaseId);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Education = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorSince = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorID);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmergencyContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientID);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentSlots",
                columns: table => new
                {
                    SlotID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentSlots", x => x.SlotID);
                    table.ForeignKey(
                        name: "FK_AppointmentSlots_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorAvailabilities",
                columns: table => new
                {
                    AvailabilityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAvailabilities", x => x.AvailabilityID);
                    table.ForeignKey(
                        name: "FK_DoctorAvailabilities_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorTimeOffs",
                columns: table => new
                {
                    TimeOffID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorTimeOffs", x => x.TimeOffID);
                    table.ForeignKey(
                        name: "FK_DoctorTimeOffs_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellnessPlans",
                columns: table => new
                {
                    PlanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByDoctorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellnessPlans", x => x.PlanId);
                    table.ForeignKey(
                        name: "FK_WellnessPlans_Doctors_CreatedByDoctorId",
                        column: x => x.CreatedByDoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientDoctorMapper",
                columns: table => new
                {
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDoctorMapper", x => x.PatientID);
                    table.ForeignKey(
                        name: "FK_PatientDoctorMapper_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientDoctorMapper_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrescribingDoctorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Appointment_Date_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SlotID = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_Appointments_AppointmentSlots_SlotID",
                        column: x => x.SlotID,
                        principalTable: "AppointmentSlots",
                        principalColumn: "SlotID");
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientPlanAssignments",
                columns: table => new
                {
                    AssignmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedByDoctorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FrequencyCount = table.Column<int>(type: "int", nullable: false),
                    FrequencyUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AssignedWellnessPlanPlanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssigningDoctorDoctorID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPlanAssignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_PatientPlanAssignments_Doctors_AssigningDoctorDoctorID",
                        column: x => x.AssigningDoctorDoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPlanAssignments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPlanAssignments_WellnessPlans_AssignedWellnessPlanPlanId",
                        column: x => x.AssignedWellnessPlanPlanId,
                        principalTable: "WellnessPlans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellnessPlanDetails",
                columns: table => new
                {
                    DetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    WellnessPlanPlanId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellnessPlanDetails", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_WellnessPlanDetails_WellnessPlans_WellnessPlanPlanId",
                        column: x => x.WellnessPlanPlanId,
                        principalTable: "WellnessPlans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationSchedules",
                columns: table => new
                {
                    ScheduleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrescriptionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeOfDay = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationSchedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_MedicationSchedules_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentAlerts",
                columns: table => new
                {
                    AlertID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppointmentID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentAlerts", x => x.AlertID);
                    table.ForeignKey(
                        name: "FK_AppointmentAlerts_Appointments_AppointmentID",
                        column: x => x.AppointmentID,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    DiagnosisID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiseaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnoses", x => x.DiagnosisID);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Diseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Diseases",
                        principalColumn: "DiseaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentPlanDetails",
                columns: table => new
                {
                    CustomDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    PatientPlanAssignmentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentPlanDetails", x => x.CustomDetailId);
                    table.ForeignKey(
                        name: "FK_AssignmentPlanDetails_PatientPlanAssignments_PatientPlanAssignmentId",
                        column: x => x.PatientPlanAssignmentId,
                        principalTable: "PatientPlanAssignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyTaskLogs",
                columns: table => new
                {
                    LogId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientPlanAssignmentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTaskLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_DailyTaskLogs_PatientPlanAssignments_PatientPlanAssignmentId",
                        column: x => x.PatientPlanAssignmentId,
                        principalTable: "PatientPlanAssignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentAlerts_AppointmentID",
                table: "AppointmentAlerts",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorID",
                table: "Appointments",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientID",
                table: "Appointments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SlotID",
                table: "Appointments",
                column: "SlotID",
                unique: true,
                filter: "[SlotID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentSlots_DoctorID",
                table: "AppointmentSlots",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentPlanDetails_PatientPlanAssignmentId",
                table: "AssignmentPlanDetails",
                column: "PatientPlanAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTaskLogs_PatientPlanAssignmentId",
                table: "DailyTaskLogs",
                column: "PatientPlanAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_AppointmentId",
                table: "Diagnoses",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_DiseaseId",
                table: "Diagnoses",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_PatientID",
                table: "Diagnoses",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAvailabilities_DoctorID",
                table: "DoctorAvailabilities",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorTimeOffs_DoctorID",
                table: "DoctorTimeOffs",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationSchedules_PrescriptionId",
                table: "MedicationSchedules",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDoctorMapper_DoctorID",
                table: "PatientDoctorMapper",
                column: "DoctorID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientPlanAssignments_AssignedWellnessPlanPlanId",
                table: "PatientPlanAssignments",
                column: "AssignedWellnessPlanPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPlanAssignments_AssigningDoctorDoctorID",
                table: "PatientPlanAssignments",
                column: "AssigningDoctorDoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPlanAssignments_PatientId",
                table: "PatientPlanAssignments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DoctorID",
                table: "Prescriptions",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientId",
                table: "Prescriptions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Token_Unique",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellnessPlanDetails_WellnessPlanPlanId",
                table: "WellnessPlanDetails",
                column: "WellnessPlanPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WellnessPlans_CreatedByDoctorId",
                table: "WellnessPlans",
                column: "CreatedByDoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentAlerts");

            migrationBuilder.DropTable(
                name: "AssignmentPlanDetails");

            migrationBuilder.DropTable(
                name: "DailyTaskLogs");

            migrationBuilder.DropTable(
                name: "Diagnoses");

            migrationBuilder.DropTable(
                name: "DoctorAvailabilities");

            migrationBuilder.DropTable(
                name: "DoctorTimeOffs");

            migrationBuilder.DropTable(
                name: "MedicationSchedules");

            migrationBuilder.DropTable(
                name: "PatientDoctorMapper");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "WellnessPlanDetails");

            migrationBuilder.DropTable(
                name: "PatientPlanAssignments");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "WellnessPlans");

            migrationBuilder.DropTable(
                name: "AppointmentSlots");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Doctors");
        }
    }
}
