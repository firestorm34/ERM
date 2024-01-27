using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TestTaskApplication.Data;
using TestTaskApplication.Models;
#nullable enable
namespace TestTaskApplication.Models
{
    public class Employee
    {
        public decimal ID { get; set; }
        [DataType(DataType.Text),NotNull, MaxLength(50)]
        public string FirstName { get; set; }
        [DataType(DataType.Text),NotNull, MaxLength(50)]
        public string SurName { get; set; }
        [MaxLength(6)]
        public string? DocNumber { get; set; }
        [DataType(DataType.Text), MaxLength(50)]
        public string Position { get; set; }
        public int Age { get; set; }

        public DateTime DateOfBirth { get; set; }
        public Guid DepartmentID { get; set; }

        public Department? Department { get; set; }
    }
}
