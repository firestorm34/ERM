using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TestTaskApplication.Data;
using TestTaskApplication.Models;

namespace TestTaskApplication.Data
{

#nullable enable

    public class DepartmentRepository
    {
        private string _connectionString = "";
        public DepartmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable CreateCommand(string command)
        {
            Department department = new Department();
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
                    throw new Exception("Ошибка при загрузке  данных.");
                }
                
                DataTable dataTable = new DataTable();
                sqlAdapter.Fill(dataTable);

                if (dataTable == null)
                {
                    throw new Exception(" По данному запросу не нашлось данных.");
                }
                return dataTable;
            }
        }

        public List<Department>? GetAll()
        {
            List<Department> departments;
            string query = "SELECT * FROM dbo.Department";
            DataTable departmentsTable = CreateCommand(query);
            departments = departmentsTable.AsEnumerable().Select(dep =>
            new Department
            {
                ID = dep.Field<Guid>("ID"),
                Name = dep.Field<string>("Name"),
                ParentDepartmentID = dep.Field<Guid?>("ParentDepartmentID")
            }).ToList();
            int i = 2;
            return departments.Count == 0 ? null: departments;
        }

        public Department GetRootDepartment()
        {

            Department department = new Department();
            Guid guid = new Guid("fb9d1a43-5796-4190-abd4-39ffd8c87476");
            string query = $"SELECT * FROM dbo.Department WHERE ID = '{guid}'";

            DataTable departmentsTable = CreateCommand(query);
            DataRow? departmentRow = departmentsTable.AsEnumerable().FirstOrDefault();
           
            if (departmentRow == null)
            {
                throw new Exception("Не найдено такого депортамента!");
            }
            department.ID = departmentRow.Field<Guid>("ID");
            department.Name = departmentRow.Field<string>("Name");
            department.ParentDepartmentID = departmentRow.IsNull("ParentDepartmentID") ? default(Guid) 
                :  departmentRow.Field<Guid>("ParentDepartmentID");
            
            return department;
        }

        public Department GetById(Guid id)
        {

           Department department = new Department();

            string query = $"SELECT * FROM dbo.Department WHERE ID = '{id}'";

            DataTable departmentsTable = CreateCommand(query);
            DataRow? departmentRow = departmentsTable.AsEnumerable().FirstOrDefault();
            if (departmentRow == null)
            {
                throw new Exception("Не найдено такого департамента!");
            }

            department.ID = departmentRow.Field<Guid>("ID");
            department.Name = departmentRow.Field<string>("Name");
            department.ParentDepartmentID = departmentRow.IsNull("ParentDepartmentID") ? default(Guid) 
                : departmentRow.Field<Guid>("ParentDepartmentID");



            return department;
        }

        public List<Department>? GetByParentId(Guid id)
        {

            List<Department>? departments;
            
            string query = $"SELECT * FROM dbo.Department WHERE ParentDepartmentID = '{id}' ";
            
            DataTable departmentsTable = CreateCommand(query);
            departments = departmentsTable.AsEnumerable().Select(dep =>
            new Department
            {
                ID = dep.Field<Guid>("ID"),
                Name = dep.Field<string>("Name"),
                ParentDepartmentID = dep.IsNull("ParentDepartmentID") ? default(Guid) : 
                dep.Field<Guid>("ParentDepartmentID")

        }).ToList();

            return departments.Count ==0? null: departments;
        }


    }

}
