using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TestTaskApplication.Data;
using TestTaskApplication.Models;

#nullable enable

namespace TestTaskApplication.Data
{
    public class EmployeeRepository
    {
        private string _connectionString = "";
        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ExecuteCommand(string command)
        {
            Employee employee = new Employee();
           
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e) { throw new Exception("Не удалось подключиться"); }
                SqlCommand sqlCommand = new SqlCommand(command,connection);
                sqlCommand.ExecuteNonQuery();

            }
        }

        public DataTable LoadDataByCommand(string command)
        {
            Employee employee = new Employee();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e) { throw new Exception("Не удалось подключиться"); }
               

                SqlDataAdapter? sqlAdapter = new SqlDataAdapter(command, connection);
                if (sqlAdapter is null)
                {
                    throw new Exception("Ошибка при загрузке данных.");
                }
                DataTable dataTable = new DataTable();
                sqlAdapter.Fill(dataTable);

                if (dataTable == null)
                {
                    throw new Exception("Ошибка! По данному запросу не нашлось данных.");
                }
                return dataTable;
            }
        }

        public int CalculateAge(DateTime BirthDate)
        {
            int age = DateTime.Now.Year - BirthDate.Year;
            if (BirthDate.Date > DateTime.Now.AddYears(-age))
            {
                age--;
            }
            return age;
        }



        public List<Employee> GetByDepartmentId(Guid id)
        {
            List<Employee> employees;
            string query = $"SELECT * FROM dbo.Empoyee WHERE DepartmentID = '{id}' ;";

            DataTable employeesTable = LoadDataByCommand(query);
            employees = employeesTable.AsEnumerable().Select(emp =>
                    new Employee
                    {
                        ID = emp.Field<decimal>("ID"),
                        FirstName = emp.Field<string>("FirstName"),
                        SurName = emp.Field<string>("SurName"),
                        DocNumber = emp.Field<string?>("DocNumber"),
                        Position = emp.Field<string>("Position"),
                        DateOfBirth = emp.Field<DateTime>("DateOfBirth"),
                        Age = CalculateAge(emp.Field<DateTime>("DateOfBirth")),
                        DepartmentID = emp.Field<Guid>("DepartmentID")
                    }).ToList();


            return employees;
        }

        public List<Employee>? GetAll()
        {
            List<Employee> employees;
            string query = $"SELECT * FROM dbo.Empoyee ;";

            DataTable employeesTable = LoadDataByCommand(query);
            employees = employeesTable.AsEnumerable().Select(emp =>
                    new Employee
                    {
                        ID = emp.Field<decimal>("ID"),
                        FirstName = emp.Field<string>("FirstName"),
                        SurName = emp.Field<string>("SurName"),
                        DocNumber = emp.Field<string?>("DocNumber"),
                        Position = emp.Field<string>("Position"),
                        DateOfBirth = emp.Field<DateTime>("DateOfBirth"),
                        Age = CalculateAge(emp.Field<DateTime>("DateOfBirth")),
                        DepartmentID = emp.Field<Guid>("DepartmentID")
                    }).ToList();

            return employees;
        }


        public Employee GetById(decimal id)
        {
            Employee employee = new Employee();
            string command = $"SELECT * FROM dbo.Empoyee WHERE id = {id};";

            DataTable employeeTable = LoadDataByCommand(command);
            DataRow? employeeRow = employeeTable.AsEnumerable().FirstOrDefault();
            if (employeeRow == null)
            {
                throw new Exception("Данный пользователь не найден в базе данных!");
            }
            employee.ID = employeeRow.Field<decimal>("ID");
            employee.FirstName = employeeRow.Field<string>("FirstName")??"Not loaded";
            employee.SurName = employeeRow.Field<string>("SurName")?? "Not loaded";
            employee.Position = employeeRow.Field<string>("Position")?? "Not loaded";
            employee.DocNumber = employeeRow.Field<string>("DocNumber");
            employee.DepartmentID = employeeRow.Field<Guid>("DepartmentID");
            employee.DateOfBirth = employeeRow.Field<DateTime>("DateOfBirth");

            return employee;
        }

      

        public void Update(Employee employee)
        {
            string formatDate = employee.DateOfBirth.ToString("MM-dd-yyyy");

            string command = $"UPDATE dbo.Empoyee " +
                 $"Set " +
                $"FirstName = '{employee.FirstName}',"
                + $" SurName = '{employee.SurName}', DateOfBirth = " +
                $"'{formatDate}', DocNumber = {employee.DocNumber}," +
                $" Position = '{employee.Position}', DepartmentID = '{employee.DepartmentID}'"+
                $"WHERE ID = {employee.ID};" ;

            ExecuteCommand(command);
           
        }


        public void Create(Employee employee)
        {
            string formatDate = employee.DateOfBirth.ToString("MM-dd-yyyy");

            string command = $"INSERT INTO dbo.Empoyee ( FirstName, SurName, DocNumber, DateOfBirth, Position, DepartmentID) " 
            + $"VALUES( '{ employee.FirstName}','{ employee.SurName}', { employee.DocNumber}," +
            $" '{formatDate}','{ employee.Position}','{ employee.DepartmentID}');";

             ExecuteCommand(command);

        }


        public void Delete(decimal id)
        {
            Employee employee = GetById(id);

            string command = $"DELETE FROM dbo.Empoyee WHERE ID={id};";

            ExecuteCommand(command);
            
            
        }

    }
}
