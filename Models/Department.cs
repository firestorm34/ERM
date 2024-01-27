using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskApplication.Models
{
    public class Department
    {
        public Guid ID { get; set; }
        [DataType(DataType.Text), Required, MaxLength(50)]
        public string Name { get; set; }
        public Guid? ParentDepartmentID { get; set; }

        public List<Employee> Employees { get; set; }

    }
}
