// You'll need to add 'using' statements for your project's Models and DbContext namespaces
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Enums;
using Patient_Monitoring.Models;

namespace Patient_Monitoring.Data
{
    public class DataSeeder
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PatientMonitoringDbContext>();
                var random = new Random();
                var passwordHasher = new PasswordHasher<Patient>();

                if (context == null)
                {
                    return;
                }
                
                //Creates the DB with tables from scratch only if the db does not exists using the Models
                //as blueprint. If the db exists, it doesn't do anything.
                context.Database.EnsureCreated();

                if (context.Patients.Any())
                {
                    Console.WriteLine("--- Database already contains data. Skipping seed. ---");
                    return;
                }

                #region Seeding Doctors Data
                // Seeding 'Doctors' Table
                var doctors = new List<Doctor>
                {

                    new Doctor
                    {
                        //DoctorId = Guid.NewGuid().ToString(),
                        FirstName = "Eleanor",
                        LastName = "Vance",
                        Specialization = "General Practice",
                        Education = "MD, Johns Hopkins University",
                        ContactNumber = "5550100111",
                        Email = "eleanor.vance@wellnessclinic.com",
                        ProfileImage = "/images/doctors/eleanor_vance.png",
                        DoctorSince = new DateTime(2010, 8, 15),
                        Password = passwordHasher.HashPassword(null, "DoctorPass123")
                    },

                    new Doctor
                    {
                        //DoctorId = Guid.NewGuid().ToString(),
                        FirstName = "John",
                        LastName = "Montague",
                        Specialization = "Cardiology",
                        Education = "MD, Harvard Medical School",
                        ContactNumber = "5550100222",
                        Email = "john.montague@wellnessclinic.com",
                        ProfileImage = "/images/doctors/john_montague.png",
                        DoctorSince = new DateTime(2005, 6, 20),
                        Password = passwordHasher.HashPassword(null, "DoctorPass123")
                    },

                    new Doctor
                    {
                        //DoctorId = Guid.NewGuid().ToString(),
                        FirstName = "Theodora",
                        LastName = "Crain",
                        Specialization = "Psychiatry",
                        Education = "MD, Stanford University",
                        ContactNumber = "5550100333",
                        Email = "theodora.crain@wellnessclinic.com",
                        ProfileImage = "/images/doctors/theodora_crain.png",
                        DoctorSince = new DateTime(2015, 11, 1),
                        Password = passwordHasher.HashPassword(null, "DoctorPass123")
                    },

                    new Doctor
                    {
                        //DoctorId = Guid.NewGuid().ToString(),
                        FirstName = "Luke",
                        LastName = "Sanderson",
                        Specialization = "Family Medicine",
                        Education = "MD, Mayo Clinic Alix School of Medicine",
                        ContactNumber = "5550100444",
                        Email = "luke.sanderson@wellnessclinic.com",
                        ProfileImage = "/images/doctors/luke_sanderson.png",
                        DoctorSince = new DateTime(2018, 7, 30),
                        Password = passwordHasher.HashPassword(null, "DoctorPass123")
                    },
                    new Doctor
                    {
                        //DoctorId = Guid.NewGuid().ToString(),
                        FirstName = "Jonas",
                        LastName = "Salk",
                        Specialization = "Pediatrics",
                        Education = "MD, New York University",
                        ContactNumber = "5550100555",
                        Email = "jonas.salk@wellnessclinic.com",
                        ProfileImage = "/images/doctors/jonas_salk.png",
                        DoctorSince = new DateTime(2002, 3, 12),
                        Password = passwordHasher.HashPassword(null, "DoctorPass123")
                    } 
                };

                context.Doctors.AddRange(doctors);

                #endregion


                #region Seeding Patients Data
                // Seeding 'Patients' Table

                var patients = new List<Patient>
                {
                    new Patient
                    {
                        //PatientID = Guid.NewGuid().ToString(),
                        FirstName = "Leia",
                        LastName = "Organa",
                        DateOfBirth = new DateTime(1977, 5, 25),
                        Gender = "Female",
                        ContactNumber = "9876543210",
                        Email = "leia.organa@resistance.com",
                        Address = "123 Alderaan Ave, Core Worlds",
                        EmergencyContactName = "Han Solo",
                        EmergencyContactNumber = "9876511111",
                        BloodGroup = "O+ve",
                        RegistrationDate = DateTime.UtcNow,
                        ProfileImage = "/images/patients/leia_organa.png",
                        Password = passwordHasher.HashPassword(null, "Leia123")
                    },
                    new Patient
                    {
                        //PatientID = Guid.NewGuid().ToString(),
                        FirstName = "Luke",
                        LastName = "Skywalker",
                        DateOfBirth = new DateTime(1977, 5, 25),
                        Gender = "Male",
                        ContactNumber = "9876543211",
                        Email = "luke.skywalker@jedicouncil.org",
                        Address = "456 Moisture Farm, Tatooine",
                        EmergencyContactName = "Obi-Wan Kenobi",
                        EmergencyContactNumber = "9876522222",
                          BloodGroup = "B+ve",
                        RegistrationDate = DateTime.UtcNow,
                        ProfileImage = "/images/patients/luke_skywalker.png",
                        Password = passwordHasher.HashPassword(null, "Luke123")
                    },
                    new Patient
                    {
                        //PatientID = Guid.NewGuid().ToString(),
                        FirstName = "Anakin",
                        LastName = "Skywalker",
                        DateOfBirth = new DateTime(1955, 1, 15),
                        Gender = "Male",
                        ContactNumber = "9876543212",
                        Email = "anakin.s@republic.gov",
                        Address = "789 Jedi Temple, Coruscant",
                        EmergencyContactName = "Padmé Amidala",
                        EmergencyContactNumber = "9876533333",
                          BloodGroup = "O-ve",
                        RegistrationDate = DateTime.UtcNow,
                        ProfileImage = "/images/patients/anakin_skywalker.png",
                        Password = passwordHasher.HashPassword(null, "Anakin123")
                    },
                    new Patient
                    {
                        //PatientID = Guid.NewGuid().ToString(),
                        FirstName = "Padmé",
                        LastName = "Amidala",
                        DateOfBirth = new DateTime(1975, 11, 20),
                        Gender = "Female",
                        ContactNumber = "9876543213",
                        Email = "padme.a@naboo.gov",
                        Address = "101 Royal Palace, Theed, Naboo",
                        EmergencyContactName = "Captain Typho",
                        EmergencyContactNumber = "9876544444",
                          BloodGroup = "B+ve",
                        RegistrationDate = DateTime.UtcNow,
                        ProfileImage = "/images/patients/padme_amidala.png",
                        Password = passwordHasher.HashPassword(null, "Padme123")
                    },
                    new Patient
                    {
                        //PatientID = Guid.NewGuid().ToString(),
                        FirstName = "Han",
                        LastName = "Solo",
                        DateOfBirth = new DateTime(1970, 7, 3),
                        Gender = "Male",
                        ContactNumber = "9876543214",
                        Email = "han.solo@falcon.net",
                        Address = "Docking Bay 94, Mos Eisley",
                        EmergencyContactName = "Chewbacca",
                        EmergencyContactNumber = "9876555555",
                          BloodGroup = "A1+ve",
                        RegistrationDate = DateTime.UtcNow,
                        ProfileImage = "/images/patients/han_solo.png",
                        Password = passwordHasher.HashPassword(null, "Han123")
                    },
                };

                context.Patients.AddRange(patients);

                #endregion


                #region Seeding Diseases Data
                // Seeding 'Diseases' Table

                var diseases = new List<Disease>
                {
                    new Disease
                    {
        
                        DiseaseName = "Hypertension",
                        DiseaseDescription = "A condition in which the force of the blood against the artery walls is consistently too high."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Type 2 Diabetes",
                        DiseaseDescription = "A chronic condition that affects the way the body metabolizes sugar (glucose), its main source of fuel."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Asthma",
                        DiseaseDescription = "A respiratory condition where airways narrow and swell and may produce extra mucus, making breathing difficult."
                    },
                    new Disease
                    {
       
                        DiseaseName = "Coronary Artery Disease",
                        DiseaseDescription = "Damage or disease in the heart's major blood vessels, typically caused by a buildup of plaque."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Migraine",
                        DiseaseDescription = "A type of headache that can cause severe throbbing pain or a pulsing sensation, usually on one side of the head."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Gastroesophageal Reflux Disease (GERD)",
                        DiseaseDescription = "A digestive disorder that affects the ring of muscle between the esophagus and stomach, causing acid reflux."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Major Depressive Disorder",
                        DiseaseDescription = "A mood disorder causing a persistent feeling of sadness and loss of interest."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Osteoarthritis",
                        DiseaseDescription = "The most common form of arthritis, occurring when the protective cartilage that cushions the ends of bones wears down over time."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Chronic Kidney Disease (CKD)",
                        DiseaseDescription = "A condition characterized by a gradual loss of kidney function over time."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Allergic Rhinitis",
                        DiseaseDescription = "An allergic response causing itchy, watery eyes, sneezing, and other similar symptoms, commonly known as hay fever."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Hypothyroidism",
                        DiseaseDescription = "A condition in which the thyroid gland doesn't produce enough of certain crucial hormones."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Iron Deficiency Anemia",
                        DiseaseDescription = "A condition in which blood lacks adequate healthy red blood cells due to insufficient iron."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Insomnia",
                    },
                    new Disease
                    {
       
                        DiseaseName = "Influenza",
                        DiseaseDescription = "A viral infection that attacks your respiratory system — your nose, throat and lungs."
                    },
                    new Disease
                    {
       
                        DiseaseName = "Pneumonia",
                       DiseaseDescription = "An infection that inflames the air sacs in one or both lungs, which may fill with fluid or pus."
                    },
                    new Disease
                    {
       
                        DiseaseName = "Acne Vulgaris",
                        DiseaseDescription = "A common skin condition that occurs when hair follicles become plugged with oil and dead skin cells."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Eczema (Atopic Dermatitis)",
                        DiseaseDescription = "A condition that causes dry, itchy and inflamed skin. It's common in young children but can occur at any age."
                    },
                    new Disease
                    {
        
                        DiseaseName = "Gout",
                        DiseaseDescription = "A common and complex form of arthritis characterized by sudden, severe attacks of pain, swelling, and redness in the joints."
                    },
                    new Disease
                    {
       
                        DiseaseName = "Urinary Tract Infection (UTI)",
                        DiseaseDescription = "An infection in any part of the urinary system, the kidneys, bladder, or urethra."
                    },
                    new Disease
                    {
       
                        DiseaseName = "Glaucoma",
                        DiseaseDescription = "A group of eye conditions that damage the optic nerve, the health of which is vital for good vision."
                    }
                };

                context.Diseases.AddRange(diseases);

                #endregion


                context.SaveChanges();


                #region Seeding PatientDoctorMapper Data
                // Seeding 'PatientDoctorMappers' Table
                // ------------------------------------
                // Randomly 3 doctors are chosen to be personlized doctors for all patients in the DB.

                var selectedPersonalizedDoctors = doctors.OrderBy(d => d.FirstName).Take(3).ToList();

                var patientDoctorMaps = new List<PatientDoctorMapper>();


                for (int i = 0; i < patients.Count; i++)
                {
                    var patient = patients[i];

                    var assignedDoctor = selectedPersonalizedDoctors[i % selectedPersonalizedDoctors.Count];

                    patientDoctorMaps.Add(new PatientDoctorMapper
                    {
                        PatientId = patient.PatientId,
                        DoctorId = assignedDoctor.DoctorId,
                        AssignedDate = DateTime.UtcNow.AddDays(-random.Next(30, 1825))
                    });
                }

                context.PatientDoctorMapper.AddRange(patientDoctorMaps);

                #endregion


                #region Seeding DoctorAvailability Data
                // Seeding 'DoctorAvailabilties' Table
                // -----------------------------------
                // 4 Shift Patterns are defined. Each doctor may work 5 days or 6 days (by including Saturday)
                // in a week. Here we are using random to randomize the assignment of 'Saturday' as a working day.

                var doctorAvailabilities = new List<DoctorAvailability>();

                var shiftPatterns = new List<(TimeSpan StartTime, TimeSpan EndTime)>
                {
                    (new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0)),   // 9:00 AM - 5:00 PM
                    (new TimeSpan(8, 30, 0), new TimeSpan(16, 30, 0)),  // 8:30 AM - 4:30 PM
                    (new TimeSpan(10, 0, 0), new TimeSpan(18, 0, 0)),  // 10:00 AM - 6:00 PM
                    (new TimeSpan(9, 0, 0), new TimeSpan(13, 0, 0))    // 09:00 AM - 1:00 PM
                };

                foreach (var doctor in doctors)
                {
                    var assignedShift = shiftPatterns[random.Next(shiftPatterns.Count)];

                    var workingDays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                    if (random.Next(0, 2) == 1)
                    {
                        workingDays.Add("Saturday");
                    }

                    foreach (var day in workingDays)
                    {
                        doctorAvailabilities.Add(new DoctorAvailability
                        {
                            DoctorId = doctor.DoctorId,
                            DayOfWeek = day,
                            StartTime = assignedShift.StartTime,
                            EndTime = assignedShift.EndTime,
                            IsRecurring = true
                        });
                    }
                }

                context.DoctorAvailabilities.AddRange(doctorAvailabilities);

                #endregion


                #region Seeding DoctorTimeOff Data

                // Seeding 'DoctorTimeOff' Table
                // ---------------------------------
                // We will assign leaves to doctors for next week (for ease of showcasing using postman)
                // starting with reference to next monday. With this I can randomize leaves on any 5 days
                // in next week. Then the doctor's availability is fetched - this is because each doctor can
                // have different start and end timings in their schedules. So to ensure the leave timing is 
                // in sync with the schedule we retrieve their availability schedules.

                var doctorTimeOffs = new List<DoctorTimeOff>();

                var today = DateTime.Today;
                int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                daysUntilMonday = daysUntilMonday == 0 ? 7 : daysUntilMonday;
                var targetDay = today.AddDays(daysUntilMonday);

                var doctorsOnLeave = doctors.OrderBy(d => d.Education).Take(2).ToList();
                foreach (var leaveDoctor in doctorsOnLeave)
                {
                    var leaveDayAvailability = doctorAvailabilities
                        .Where(a => a.DoctorId == leaveDoctor.DoctorId)
                        .FirstOrDefault();

                    if (leaveDayAvailability != null)
                    {
                        doctorTimeOffs.Add(new DoctorTimeOff
                        {
                            DoctorId = leaveDoctor.DoctorId,
                            StartDateTime = targetDay.Date.AddDays(random.Next(0, 6)).Add(leaveDayAvailability.StartTime),
                            EndDateTime = targetDay.Date.AddDays(random.Next(0, 6)).Add(leaveDayAvailability.EndTime),
                            Reason = "Work Leave"
                        });
                    }
                }

                context.DoctorTimeOffs.AddRange(doctorTimeOffs);

                #endregion


                #region Seeding AppointmentSlots Data
                // Seeding 'AppointmentSlots' Table
                // --------------------------------
                // Here dummy appointment slots will be created for 8 days starting today, except Saturday & Sunday
                // Each slot will be of 20 minutes and will respect the doctor availability schedule and
                // break times (which is assumed to be fixed at 11:00 AM - 11:20 AM and 4:00 PM - 4:20 PM).

                var appointmentSlots = new List<AppointmentSlot>();
                var slotDuration = TimeSpan.FromMinutes(20);
                var endDate = targetDay.AddDays(8);

                foreach (var doctor in doctors)
                {
                    for (var day = today; day < endDate.Date; day = day.AddDays(1))
                    {
                        var availability = doctorAvailabilities.FirstOrDefault(a =>
                            a.DoctorId == doctor.DoctorId &&
                            a.DayOfWeek == day.DayOfWeek.ToString()
                        );

                        if (availability == null) continue;

                        var timeOffsForDay = doctorTimeOffs
                            .Where(t => t.DoctorId == doctor.DoctorId && t.StartDateTime.Date == day.Date)
                            .ToList();

                        var shiftStart = day.Date.Add(availability.StartTime);
                        var shiftEnd = day.Date.Add(availability.EndTime);

                        for (var slotTime = shiftStart; slotTime < shiftEnd; slotTime = slotTime.Add(slotDuration))
                        {
                            var slotEnd = slotTime.Add(slotDuration);
                            if (slotEnd > shiftEnd) continue;

                            bool isTimeOff = timeOffsForDay.Any(t =>
                                slotTime < t.EndDateTime && slotEnd > t.StartDateTime
                            );

                            if (!isTimeOff)
                            {
                                appointmentSlots.Add(new AppointmentSlot
                                {

                                    DoctorId = doctor.DoctorId,
                                    StartDateTime = slotTime,
                                    EndDateTime = slotEnd,
                                    IsBooked = random.Next(0, 4) == 0
                                });
                            }
                        }
                    }
                }

                context.AppointmentSlots.AddRange(appointmentSlots);

                #endregion


                #region Seeding WellnessPlan Data
                // Seeding 'WellnessPlan' Table
                // ----------------------------
                // Here we are creating 5 template plans & 4 general plans & 8 Custom plans.

                var wellnessPlans = new List<WellnessPlan>();

                wellnessPlans.Add(new WellnessPlan
                {
                    
                    PlanName = "DASH Diet Template",
                    Goal = "Blueprint for Lowering BP",
                    ImageUrl = "/images/wellness/template_diet.png",
                    Category = "Diet",
                    IsTemplate = true,
                    CreatedByDoctorId = null
                });
                wellnessPlans.Add(new WellnessPlan
                {
                    
                    PlanName = "Cardio Start Template",
                    Goal = "Blueprint for Heart Health",
                    ImageUrl = "/images/wellness/template_cardio.png",
                    Category = "Exercise",
                    IsTemplate = true,
                    CreatedByDoctorId = null
                });
                wellnessPlans.Add(new WellnessPlan
                {
                    
                    PlanName = "Stress Relief Templ.",
                    Goal = "Blueprint for Mindfulness",
                    ImageUrl = "/images/wellness/template_mindfulness.png",
                    Category = "Mental Wellness",
                    IsTemplate = true,
                    CreatedByDoctorId = null
                });
                wellnessPlans.Add(new WellnessPlan
                {
                    
                    PlanName = "Strength Base Templ.",
                    Goal = "Blueprint for Muscle Gain",
                    ImageUrl = "/images/wellness/template_strength.png",
                    Category = "Exercise",
                    IsTemplate = true,
                    CreatedByDoctorId = null
                });
                wellnessPlans.Add(new WellnessPlan
                {
                    
                    PlanName = "Post-Op Recovery T.",
                    Goal = "Blueprint for Healing",
                    ImageUrl = "/images/wellness/template_recovery.png",
                    Category = "Recovery",
                    IsTemplate = true,
                    CreatedByDoctorId = null
                });

                wellnessPlans.Add(new WellnessPlan
                {
                    
                    PlanName = "Hydration Challenge",
                    Goal = "Drink 8 Glasses of Water Daily",
                    ImageUrl = "/images/wellness/general_hydration.png",
                    Category = "Habit Building",
                    IsTemplate = false,
                    CreatedByDoctorId = null
                });
                wellnessPlans.Add(new WellnessPlan
                {
                   
                    PlanName = "Sleep Hygiene Basics",
                    Goal = "Achieve 7-8 Hours of Sleep",
                    ImageUrl = "/images/wellness/general_sleep.png",
                    Category = "Sleep",
                    IsTemplate = false,
                    CreatedByDoctorId = null
                });
                wellnessPlans.Add(new WellnessPlan
                {
                    
                    PlanName = "Daily Stretching",
                    Goal = "Improve Flexibility",
                    ImageUrl = "/images/wellness/general_stretching.png",
                    Category = "Exercise",
                    IsTemplate = false,
                    CreatedByDoctorId = null
                });
                wellnessPlans.Add(new WellnessPlan
                {
                    
                    PlanName = "Balanced Plate Guide",
                    Goal = "Eat a Balanced Meal",
                    ImageUrl = "/images/wellness/general_diet.png",
                    Category = "Diet",
                    IsTemplate = false,
                    CreatedByDoctorId = null
                });


                var customPlanAuthors = patientDoctorMaps.Select(pdm => pdm.DoctorId).Distinct().ToList();
                //var customPlanAuthors = doctors.OrderBy(d => Guid.NewGuid()).Take(8).ToList();
                var categories = new[] { "Diet", "Exercise", "Recovery", "Mental Wellness" };

                for (int i = 0; i < 8; i++)
                {
                    var author = customPlanAuthors[random.Next(customPlanAuthors.Count)];
                    var category = categories[random.Next(categories.Length)];

                    wellnessPlans.Add(new WellnessPlan
                    {
                        PlanName = $"Flu Recovery #{i + 1}",
                        Goal = $"Regain strength after illness",
                        ImageUrl = $"/images/wellness/custom_plan_{i + 1}.png",
                        Category = category,
                        IsTemplate = false,
                        CreatedByDoctorId = author
                    });
                }

                context.WellnessPlans.AddRange(wellnessPlans);

                #endregion


                context.SaveChanges();


                #region Seeding Appointment Data
                // Seed 'Appointment' Table
                // Here we have some pre-defined appointment reasons and rejection reasons. We first find
                // all slots that are booked. For each booked slot we create an appointment which could take
                // any of the following status (with percentage in the brackets) - pending (20), approved (70), rejected (10).

                var bookedSlots = appointmentSlots.Where(s => s.IsBooked).ToList();
                var appointments = new List<Appointment>();

                var appointmentReasons = new[] {
                    "Annual physical checkup", "Follow-up on recent test results", "Persistent headache and fatigue",
                    "Request for prescription renewal", "Consultation for a recurring skin rash", "Seasonal allergy symptoms",
                    "General wellness consultation", "Minor injury assessment"
                };

                var rejectionReasons = new[] {
                    "Doctor has an emergency. Please reschedule for a later date.",
                    "This concern requires a specialist. Please book an appointment with Cardiology.",
                    "The doctor suggests a video consultation instead. Please re-book accordingly."
                };

                foreach (var slot in bookedSlots)
                {
                    var patient = patients[random.Next(patients.Count)];

                    string status;
                    string? rejectionReason = null;

                    int statusRoll = random.Next(1, 101);
                    if (statusRoll <= 70)
                    {
                        status = "Confirmed";
                    }
                    else if (statusRoll <= 90)
                    {
                        status = "Pending Approval";
                    }
                    else
                    {
                        status = "Rejected";
                        rejectionReason = rejectionReasons[random.Next(rejectionReasons.Length)];
                    }

                    appointments.Add(new Appointment
                    {
                        PatientId = patient.PatientId,
                        DoctorId = slot.DoctorId,
                        AppointmentDate = slot.StartDateTime,
                        Reason = appointmentReasons[random.Next(appointmentReasons.Length)],
                        SlotId = slot.SlotId,
                        Status = status,
                        RejectionReason = rejectionReason,
                        RequestedOn = DateTime.Now.AddDays(-random.Next(15))
                    });
                }

                context.Appointments.AddRange(appointments);

                #endregion


                #region Seeding WellnessPlanDetails
                // Seeding 'WellnessPlanDetails' Table
                // ------------------------------------
                // Details are fed in for plans that are templates or general plans. We have some pre-defined
                // description, instructions, benefits and safety - with 2 to 4 bullet points each.

                var plansToDetail = wellnessPlans
                    .Where(p => p.IsTemplate || p.CreatedByDoctorId == null)
                    .ToList();

                var wellnessPlanDetails = new List<WellnessPlanDetail>();

                var dietInstructions = new[] { "Eat at least five portions of a variety of fruit and vegetables every day.", "Base meals on higher fibre starchy foods like potatoes, bread, rice or pasta.", "Have some dairy or dairy alternatives (such as soya drinks).", "Eat some beans, pulses, fish, eggs, meat and other protein.", "Choose unsaturated oils and spreads and eat in small amounts." };
                var exerciseBenefits = new[] { "Improves cardiovascular health and circulation.", "Helps maintain a healthy weight and body composition.", "Boosts mood and reduces symptoms of depression and anxiety.", "Strengthens bones and muscles, reducing risk of injury.", "Increases energy levels throughout the day." };
                var mentalWellnessSafety = new[] { "Practice in a quiet space where you will not be disturbed.", "Start with shorter sessions (5-10 minutes) and gradually increase the duration.", "Do not practice while driving or operating heavy machinery.", "If you experience significant distress, stop and consult a healthcare professional.", "Consistency is more important than duration. Aim for daily practice." };
                var generalDescription = "This plan provides a structured approach to improving your overall well-being through consistent, daily actions. Following these guidelines can lead to significant long-term health benefits.";


                foreach (var plan in plansToDetail)
                {

                    wellnessPlanDetails.Add(new WellnessPlanDetail
                    {
                        
                        PlanId = plan.PlanId,
                        DetailType = "Description",
                        Content = $"A comprehensive plan designed to help you achieve your goal of '{plan.Goal}'. {generalDescription}"
                    });


                    int instructionCount = random.Next(2, 5);
                    for (int i = 0; i < instructionCount; i++)
                    {
                        wellnessPlanDetails.Add(new WellnessPlanDetail
                        {
                            
                            PlanId = plan.PlanId,
                            DetailType = "Instruction",
                            Content = dietInstructions[random.Next(dietInstructions.Length)]
                        });
                    }

                    int benefitCount = random.Next(2, 5);
                    for (int i = 0; i < benefitCount; i++)
                    {
                        wellnessPlanDetails.Add(new WellnessPlanDetail
                        {
                            
                            PlanId = plan.PlanId,
                            DetailType = "Benefit",
                            Content = exerciseBenefits[random.Next(exerciseBenefits.Length)]
                        });
                    }

                    int safetyCount = random.Next(2, 5);
                    for (int i = 0; i < safetyCount; i++)
                    {
                        wellnessPlanDetails.Add(new WellnessPlanDetail
                        {
                           
                            PlanId = plan.PlanId,
                            DetailType = "Safety",
                            Content = mentalWellnessSafety[random.Next(mentalWellnessSafety.Length)]
                        });
                    }
                }

                context.WellnessPlanDetails.AddRange(wellnessPlanDetails);

                #endregion


                #region Seeding PatientPlanAssignment Data
                // Seeding 'PatientPlanAssignment' Table
                // -------------------------------------
                // Here we are assigning every custom plans to one of the authoring doctor's patient.
                // And we are randomly picking 2 template plans to be assigned to  random patients
                // from the PatientDoctorMapper table.

                var patientPlanAssignments = new List<PatientPlanAssignment>();

                var customPlans = wellnessPlans.Where(p => p.CreatedByDoctorId != null).ToList();

                var frequencyUnits = new[] { "Daily", "Weekly", "Monthly" };

                foreach (var plan in customPlans)
                {
                    var eligiblePatients = patientDoctorMaps
                        .Where(m => m.DoctorId == plan.CreatedByDoctorId)
                        .ToList();

                    if (eligiblePatients.Any())
                    {
                        var patientToAssign = eligiblePatients[random.Next(eligiblePatients.Count)];

                        patientPlanAssignments.Add(new PatientPlanAssignment
                        {
                            
                            PatientId = patientToAssign.PatientId,
                            PlanId = plan.PlanId,
                            AssignedByDoctorId = plan.CreatedByDoctorId!, // The author is also the first assigner.
                            FrequencyCount = random.Next(1,4),
                            FrequencyUnit = frequencyUnits[random.Next(frequencyUnits.Length)],
                            StartDate = DateTime.Today,
                            EndDate = DateTime.Today.AddMonths(3),
                            IsActive = true
                        });
                    }
                }

                var templatePlans = wellnessPlans.Where(p => p.CreatedByDoctorId == null && p.IsTemplate).OrderBy(p => p.Category).Take(2).ToList();

                foreach (var plan in templatePlans)
                {
                    var randomPatients = patientDoctorMaps.Select(pdm => pdm.PatientId).Distinct().ToList();

                    foreach (var patient in randomPatients)
                    {
                        var assigner = patientDoctorMaps.FirstOrDefault(m => m.PatientId == patient);

                        patientPlanAssignments.Add(new PatientPlanAssignment
                        {
                            
                            PatientId = patient,
                            PlanId = plan.PlanId,
                            AssignedByDoctorId = assigner!.DoctorId,
                            FrequencyCount = random.Next(1, 4),
                            FrequencyUnit = frequencyUnits[random.Next(frequencyUnits.Length)],
                            StartDate = DateTime.Today,
                            EndDate = DateTime.Today.AddMonths(6),
                            IsActive = true
                        });
                    }
                }

                context.PatientPlanAssignments.AddRange(patientPlanAssignments);

                #endregion

                context.SaveChanges();


                #region Seeding Prescription Data

                // Seeding 'Prescriptions' Table
                // -----------------------------
                // Medication names, dosages and instruction are predefined here. We have randomly chosen
                // doctors & patients who have records of prescription being assigned by and assigned to.
                // The following is defined such that each patient will intake atmost 3 prescription (upper limit
                // of random is 3)

                var medicationNames = new[] {
                    "Lisinopril", "Atorvastatin", "Metformin", "Amoxicillin", "Amlodipine",
                    "Ibuprofen", "Paracetamol", "Omeprazole", "Sertraline", "Albuterol"
                };

                var dosages = new[] { "10mg", "20mg", "40mg", "250mg", "500mg", "5mg", "600mg" };

                var instructions = new[] {
                    "Take one tablet daily in the morning with water.",
                    "Take one tablet twice a day with meals.",
                    "As needed for pain, not to exceed 4 doses in 24 hours.",
                    "Take one capsule every 8 hours for 7 days. Finish all medication.",
                    "Take one tablet 30 minutes before breakfast."
                };

                var prescriptions = new List<Prescription>();

                var confirmedAppointments = appointments.Where(a => a.Status == "Confirmed").ToList();

                var appointmentsWithPrescriptions = confirmedAppointments
                    .OrderBy(a => a.DoctorId)
                    .Take((int)(confirmedAppointments.Count * 0.8))
                    .ToList();

                foreach (var appointment in appointmentsWithPrescriptions)
                {
                    int numberOfPrescriptions = random.Next(1, 4);

                    for (int i = 0; i < numberOfPrescriptions; i++)
                    {
                        var startDate = appointment.AppointmentDate;

                        prescriptions.Add(new Prescription
                        {
                            PatientId = appointment.PatientId,
                            PrescribingDoctorId = appointment.DoctorId,
                            AppointmentId = appointment.AppointmentId,
                            MedicationName = medicationNames[random.Next(medicationNames.Length)],
                            Dosage = dosages[random.Next(dosages.Length)],
                            Instructions = instructions[random.Next(instructions.Length)],
                            StartDate = startDate,
                            EndDate = startDate.AddDays(random.Next(7, 90))
                        });
                    }
                }

                context.Prescriptions.AddRange(prescriptions);

                #endregion


                #region Seeding Diagnosis Data

                // Seeding 'Diagnosis' Table
                // --------------------------
                // We have a set of pre-defined diagnosis descriptions. We will only create diagnosis for
                // appointments that have been confirmed. 80% of these confirmed appointments will have
                // diagnosed results, others won't have any. Each diagnosis can have 1 (75% chance) or 2
                // (25% Chance) diseases.

                var diagnosisDescriptions = new[] {
                    "Patient presents with classic symptoms. Prescribing medication as per guidelines.",
                    "Initial diagnosis based on physical examination. Recommending further blood tests for confirmation.",
                    "The condition appears to be chronic and stable. Advised to continue the current treatment plan and monitor symptoms.",
                    "Acute onset of symptoms. This requires immediate attention and a follow-up visit next week.",
                    "Mild case identified. Lifestyle and dietary changes are recommended as the first line of treatment."
                };

                var diagnoses = new List<Diagnosis>();

                var confirmedAppointment = appointments.Where(a => a.Status == "Confirmed").ToList();

                var appointmentsToDiagnose = confirmedAppointment
                    .OrderBy(a => a.DoctorId)
                    .Take((int)(confirmedAppointment.Count * 0.8))
                    .ToList();

                foreach (var appointment in appointmentsToDiagnose)
                { 
                    int numberOfDiagnoses = random.Next(1, 101) <= 75 ? 1 : 2;

                    var selectedDiseases = diseases.OrderBy(d => d.DiseaseName).Take(numberOfDiagnoses).ToList();

                    foreach (var disease in selectedDiseases)
                    {
                        diagnoses.Add(new Diagnosis
                        {
                            AppointmentId = appointment.AppointmentId,
                            PatientId = appointment.PatientId,
                            DiseaseId = disease.DiseaseId,
                            DiagnosisDescription = diagnosisDescriptions[random.Next(diagnosisDescriptions.Length)]
                        });
                    }
                }

                context.Diagnoses.AddRange(diagnoses);

                #endregion


                #region Seeding AssignmentPlanDetails Data
                // Seeding 'AssignmentPlanDetails' Table
                // -------------------------------------

                var assignmentPlanDetails = new List<AssignmentPlanDetail>();

                var customInstructions = new[] {
                    "Reduce sodium intake specifically to under 1500mg per day.",
                    "Walk for at least 45 minutes, focusing on maintaining a brisk pace.",
                    "Incorporate the provided resistance bands into your morning stretch routine.",
                    "Ensure you take the supplement with your evening meal, not in the morning."
                };

                var customBenefits = new[] {
                    "Specifically tailored to improve recovery time for your recent procedure.",
                    "Aims to directly address your LDL cholesterol levels within the next 3 months.",
                    "Helps build foundational strength in your lower back to prevent future strain."
                };

                var customSafety = new[] {
                    "Do not exceed a heart rate of 140 bpm during cardio sessions.",
                    "Stop immediately if you feel any sharp pain in your left knee and report it.",
                    "Monitor your blood sugar 30 minutes after each main meal and log the results."
                };


                var plans = wellnessPlans.Where(wp => !wp.IsTemplate & wp.CreatedByDoctor != null).Select(wp => wp.PlanId).ToList();

                var assignmentsToCustomize = patientPlanAssignments.Where(p => p.PlanId.HasValue && plans.Contains(p.PlanId.Value)).ToList();

                foreach (var assignment in assignmentsToCustomize)
                {
                    assignmentPlanDetails.Add(new AssignmentPlanDetail
                    {
                        
                        AssignmentId = assignment.AssignmentId,
                        DetailType = "Description",
                        Content = "This is a personalized version of the plan, modified by your doctor to better suit your specific health goals and current condition."
                    });

                    int instructionCount = random.Next(2, 4);
                    for (int i = 0; i < instructionCount; i++)
                    {
                        assignmentPlanDetails.Add(new AssignmentPlanDetail
                        {
                            
                            AssignmentId = assignment.AssignmentId,
                            DetailType = "Instruction",
                            Content = customInstructions[random.Next(customInstructions.Length)]
                        });
                    }

                    int benefitCount = random.Next(2, 4);
                    for (int i = 0; i < benefitCount; i++)
                    {
                        assignmentPlanDetails.Add(new AssignmentPlanDetail
                        {
                            
                            AssignmentId = assignment.AssignmentId,
                            DetailType = "Benefit",
                            Content = customBenefits[random.Next(customBenefits.Length)]
                        });
                    }

                    int safetyCount = random.Next(1, 3);
                    for (int i = 0; i < safetyCount; i++)
                    {
                        assignmentPlanDetails.Add(new AssignmentPlanDetail
                        {
                            
                            AssignmentId = assignment.AssignmentId,
                            DetailType = "Safety",
                            Content = customSafety[random.Next(customSafety.Length)]
                        });
                    }
                }

                context.AssignmentPlanDetails.AddRange(assignmentPlanDetails);

                #endregion


                #region Seeding TaskLogs Data
                // Seeding 'TaskLogs' Table
                // -----------------------------
                var taskLogs = new List<TaskLog>();

                var patientNotes = new[] {
                    "Felt great today, the routine was easy to follow.",
                    "A bit tired, but managed to complete the task.",
                    "Feeling more energetic since starting this plan.",
                    "Forgot to do this in the morning, but got it done in the evening.",
                    "I think this is really helping."
                };

                var todayDate = DateTime.Today;
                var historyStartDate = todayDate.AddMonths(-1);

                var activeAssignments = patientPlanAssignments.Where(a => a.IsActive).ToList();

                foreach (var assignment in activeAssignments)
                {
                    var loopStartDate = assignment.StartDate > historyStartDate ? assignment.StartDate : historyStartDate;

                    switch (assignment.FrequencyUnit)
                    {
                        case "Daily":
                            // Create one log for each day from the start date until today.
                            for (var day = loopStartDate.Date; day <= todayDate; day = day.AddDays(1))
                            {
                                var (status, completedAt, note) = GetTaskOutcome(day, todayDate, random, patientNotes);
                                taskLogs.Add(CreateTaskLog(assignment.AssignmentId, day, status, completedAt, note));
                            }
                            break;

                        case "Weekly":
                            // Find the first day of the week to start generating logs.
                            var weekStart = loopStartDate.AddDays(-(int)loopStartDate.DayOfWeek);
                            for (var day = weekStart; day <= todayDate; day = day.AddDays(7))
                            {
                                var dueDate = day.AddDays(6); // Due date is the Saturday of that week.
                                var (status, completedAt, note) = GetTaskOutcome(dueDate, todayDate, random, patientNotes);
                                taskLogs.Add(CreateTaskLog(assignment.AssignmentId, dueDate, status, completedAt, note));
                            }
                            break;

                        case "Monthly":
                            // Find the first day of the month to start generating logs.
                            var monthStart = new DateTime(loopStartDate.Year, loopStartDate.Month, 1);
                            for (var day = monthStart; day <= todayDate; day = day.AddMonths(1))
                            {
                                var dueDate = new DateTime(day.Year, day.Month, DateTime.DaysInMonth(day.Year, day.Month));
                                var (status, completedAt, note) = GetTaskOutcome(dueDate, todayDate, random, patientNotes);
                                taskLogs.Add(CreateTaskLog(assignment.AssignmentId, dueDate, status, completedAt, note));
                            }
                            break;
                    }
                }

                context.TaskLogs.AddRange(taskLogs);

                // Helper function to create a TaskLog instance. (This remains unchanged)
                TaskLog CreateTaskLog(int assignmentId, DateTime dueDate, string status, DateTime? completedAt, string? note)
                {
                    return new TaskLog
                    {
                       
                        AssignmentId = assignmentId,
                        DueDate = dueDate,
                        Status = status,
                        CompletedAt = completedAt,
                        PatientNotes = note
                    };
                }


                // Helper function to determine the outcome of a task. (This remains unchanged)
                (string Status, DateTime? CompletedAt, string? Note) GetTaskOutcome(DateTime dueDate, DateTime today, Random random, string[] notesPool)
                {
                    if (dueDate < today) // Task is in the past
                    {
                        if (random.Next(1, 101) <= 85)
                        {
                            var completedAt = dueDate.AddHours(random.Next(9, 20));
                            var note = notesPool[random.Next(notesPool.Length)];
                            return ("Completed", completedAt, note);
                        }
                        else
                        {
                            return ("Missed", null, null);
                        }
                    }
                    else // Task is due today or in the future
                    {
                        if (dueDate == today && random.Next(1, 101) <= 30)
                        {
                            var completedAt = dueDate.AddHours(random.Next(8, 11));
                            var note = notesPool[random.Next(notesPool.Length)];
                            return ("Completed", completedAt, note);
                        }
                        else
                        {
                            return ("Pending", null, null);
                        }
                    }
                }

                #endregion


                context.SaveChanges();


                #region Seeding MedicationSchedule Data
                // Seeding 'MedicationSchedule' Table
                // ----------------------------------
                // Here 5 possible times in a day is predefined - times when a patient takes their prescriptions.
                // 4 common quantity intakes are defined.
                // A patient atmost takes their prescription 3 times a day (which is why upper limit of frequency is 3)

                var timesOfDay = new[] { "Morning", "Noon", "Afternoon", "Evening", "Bedtime" };

                var commonQuantities = new[] { 0.5f, 1.0f, 1.5f, 2.0f };

                var medicationSchedules = new List<MedicationSchedule>();

                foreach (var prescription in prescriptions)
                {
                    int frequency = random.Next(1, 4);

                    var selectedTimes = timesOfDay.OrderBy(t => t).Take(frequency).ToList();

                    foreach (var time in selectedTimes)
                    {
                        medicationSchedules.Add(new MedicationSchedule
                        {
                            
                            PrescriptionId = prescription.PrescriptionId,
                            TimeOfDay = time,
                            Quantity = commonQuantities[random.Next(commonQuantities.Length)]
                        });
                    }
                }

                context.MedicationSchedules.AddRange(medicationSchedules);

                #endregion


                context.SaveChanges();
            }
        }
    }
}
