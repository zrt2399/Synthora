using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Synthora.Demo.Models
{
    public class Employee : ObservableObject
    {
        public int Id { get;   set; }  
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
    }
}