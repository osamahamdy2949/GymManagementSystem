using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Data.Models.Enums;
using System;

namespace GymManagement.DAL.Data
{
    public static class Seeder
    {
        public static Category[] GetCategories() => new[]
        {
            new Category { Id = 1, CategoryName = "Cardio" },
            new Category { Id = 2, CategoryName = "Strength" },
            new Category { Id = 3, CategoryName = "Yoga" },
            new Category { Id = 4, CategoryName = "Boxing" },
            new Category { Id = 5, CategoryName = "CrossFit" }
        };

        public static Plan[] GetPlans() => new[]
        {
            new Plan { Id = 1, Name = "Basic", Description = "Basic monthly plan", DurationDays = 30, Price = 19.99m, IsActive = true },
            new Plan { Id = 2, Name = "Standard", Description = "Standard 3-month plan", DurationDays = 90, Price = 49.99m, IsActive = true },
            new Plan { Id = 3, Name = "Premium", Description = "Yearly premium plan", DurationDays = 365, Price = 199.99m, IsActive = true }
        };

        public static Trainer[] GetTrainers() => new[]
        {
            new Trainer
                {
                    Id = 1,
                    Name = "Ahmed Ali",
                    Email = "ahmed.ali@example.com",
                    PhoneNumber = "01000000001",
                    DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)),
                    Gender = Gender.Male,
                    Speciality = Speciality.GeneralFitness
                },
            new Trainer
            {
                Id = 2,
                Name = "Sara Hassan",
                Email = "sara.hassan@example.com",
                PhoneNumber = "01000000002",
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1992, 6, 15)),
                Gender = Gender.Female,
                Speciality = Speciality.Yoga
            }
        };

        public static Member[] GetMembers() => new[]
        {
            new Member
            {
                Id = 1,
                Name = "Mohamed Salah",
                Email = "m.salah@example.com",
                PhoneNumber = "01000000010",
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1995, 5, 15)),
                Gender = Gender.Male,
                Photo = null
            },

            new Member
            {
                Id = 2,
                Name = "Aya Ibrahim",
                Email = "aya.ibrahim@example.com",
                PhoneNumber = "01000000011",
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1998, 3, 22)),
                Gender = Gender.Female,
                Photo = null
            }
        };

        public static HealthRecord[] GetHealthRecords() => new[]
        {
            new HealthRecord { Id = 1, Height = 180.5m, Weight = 80.2m, BloodType = BloodType.Oplus, Note = "No issues", MemberId = 1, CreatedAt = new DateTime(2026, 6, 3) },
            new HealthRecord { Id = 2, Height = 165.0m, Weight = 60.0m, BloodType = BloodType.Aplus, Note = "Allergic to nuts", MemberId = 2, CreatedAt = new DateTime(2025, 5, 12) }
        };

        public static Session[] GetSessions() => new[]
        {
            new Session { Id = 1, Description = "Morning Cardio", Capacity = 15, StartDate = new DateTime(2026, 6, 3), EndDate = new DateTime(2026, 7, 3), CategoryId = 1, TrainerId = 1, CreatedAt = new DateTime(2026, 6, 3) },
            new Session { Id = 2, Description = "Evening Yoga", Capacity = 12, StartDate = new DateTime(2026, 6, 5), EndDate = new DateTime(2026, 9, 4), CategoryId = 3, TrainerId = 2, CreatedAt = new DateTime(2026, 6, 3) }
        };

        public static Membership[] GetMemberships() => new[]
        {
            new Membership { Id = 1, MemberId = 1, PlanId = 1, CreatedAt = new DateTime(2026, 6, 3), EndDate = new DateOnly(2026, 7, 3) },
            new Membership { Id = 2, MemberId = 2, PlanId = 2, CreatedAt = new DateTime(2026, 6, 3), EndDate = new DateOnly(2026, 7, 3) }
        };

        public static Booking[] GetBookings() => new[]
        {
            new Booking { MemberId = 1, SessionId = 1, IsAttended = false, CreatedAt = new DateTime(2026, 6, 3), EndDate = new DateOnly(2026, 6, 3) },
            new Booking { MemberId = 2, SessionId = 2, IsAttended = false, CreatedAt = new DateTime(2026, 6, 3), EndDate = new DateOnly(2026, 6, 3) }
        };
    }
}
