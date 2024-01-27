using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;


namespace TestTaskApplication.Data
{
    public class UnitOfWork
    {
        private static string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;" +
            "Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;" +
            "Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"; 
        public  EmployeeRepository EmployeeRepository{ get; } = new EmployeeRepository(_connectionString);
        public DepartmentRepository DepartmentRepository { get; } = new DepartmentRepository(_connectionString);
                     

    }
}
