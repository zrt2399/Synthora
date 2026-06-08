using System;
using System.Collections.Generic;
using Synthora.Demo.Models;

namespace Synthora.Demo.SampleData
{
    public static class EmployeeGenerator
    {
        private static readonly string[] FirstNames =
        [
            "James", "John", "Robert", "Michael", "William",
            "David", "Richard", "Joseph", "Thomas", "Charles",
            "Christopher", "Daniel", "Matthew", "Anthony", "Mark",
            "Emily", "Olivia", "Sophia", "Isabella", "Mia",
            "Charlotte", "Amelia", "Harper", "Evelyn", "Abigail",
            "Ella", "Avery", "Scarlett", "Grace", "Chloe"
        ];
         
        private static readonly string[] LastNames =
        [
            "Smith", "Johnson", "Williams", "Brown", "Jones",
            "Garcia", "Miller", "Davis", "Wilson", "Anderson",
            "Taylor", "Thomas", "Jackson", "White", "Martin",
            "Thompson", "Moore", "Harris", "Clark", "Lewis"
        ];

        private static readonly string[] EmailDomains =
        [
            "gmail.com",
            "outlook.com",
            "hotmail.com",
            "yahoo.com",
            "example.com",
            "company.com"
        ];

        public static List<Employee> Generate(int count = 1000)
        {
            Random random = Random.Shared;

            List<Employee> employees = new(count);

            for (int i = 1; i <= count; i++)
            {
                string firstName = FirstNames[random.Next(FirstNames.Length)];
                string lastName = LastNames[random.Next(LastNames.Length)];

                string name = $"{firstName} {lastName}";

                string email =
                    $"{firstName.ToLowerInvariant()}." +
                    $"{lastName.ToLowerInvariant()}" +
                    $"{random.Next(100, 999)}@" +
                    $"{EmailDomains[random.Next(EmailDomains.Length)]}";

                employees.Add(new Employee
                {
                    Id = i,
                    Name = name,
                    Email = email,
                    Age = random.Next(18, 66),
                    Salary = random.Next(5000, 50001),
                    HireDate = DateTime.Today.AddDays(-random.Next(0, 3650)),
                    IsActive = random.NextDouble() >= 0.4
                });
            }

            return employees;
        }
    }
}