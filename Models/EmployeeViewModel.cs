using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskApplication.Models
{
    public class EmployeeViewModel
    {
        public decimal ID { get; set; }
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [DataType(DataType.Text)]
        public string SurName { get; set; }
        public string? DocNumber { get; set; }
        [DataType(DataType.Text)]
        public string Position { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid DepartmentID { get; set; }

        public SelectList DepartmentsSelectList { get; set; }

        public Department Department { get; set; }
    }
}
