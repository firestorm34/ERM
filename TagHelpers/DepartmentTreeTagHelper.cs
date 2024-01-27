using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskApplication.Data;
using TestTaskApplication.Models;

namespace TestTaskApplication.TagHelpers
{
    #nullable enable
    public class DepartmentTreeTagHelper : TagHelper
    {

        UnitOfWork unit;
        
        delegate void GetHtmlContent(Guid id, ref string output);

        public DepartmentTreeTagHelper(UnitOfWork unit){
                this.unit = unit;
            }

        public override void Process(TagHelperContext context, TagHelperOutput output){
                output.TagName = null;
                output.Content.SetHtmlContent(GetDepartmentsHtml());
            }

        // находим корневой узел и от него запускаем генерацию дерева.
        public string GetDepartmentsHtml()
        {
            GetHtmlContent getHtmlContent = ConstructDeportamentsHtml;

            string output = null;
            Department rootDepartment = unit.DepartmentRepository.GetRootDepartment();
            
            getHtmlContent.Invoke(rootDepartment.ID, ref output);
            return output;

        }

        // шаблон, добавляющий кнопку показа/скрытия child узла.
        // Добавляется онли если департамент имеет наследников
        string ReturnChildren(bool hasChildren = false)
        {
            if (hasChildren == true)
            {
                return "<a class=\"btn  btn-primary  anchor\" " +
                    "style = \"color: white;\" " +
                    "onclick=\"children_list_click(this)\">  Show children  </a> ";
            }
            else 
            {
                return "";
            }
        }

        // шаблон для построения строки с департаментом
        public string GetDepartmentRowHtmlString(Department department, bool hasChildren=false)
        {
           return 
            "<div class=\"row\">" +
                    "<span class=\"col-3\">" +department.Name + "</span>" +
                    "<span class=\"col-3 Actions\"> " +
                    "<a class=\"anchor\" id = \"a-view\"  href=\"Employee/ShowByDepartmentId/" + department.ID
                    +"\"  class=\"btn btn-primary anchor\" style=\"border-radius:20px;\"> View Employees</a></span>" +
                    "<span class=\"col-3\">" +  ReturnChildren(hasChildren) +"</span>" 
                    + "<span class=\"col -3\"> </span>" +
            " </div>";
        }


        // метод, который по ID департамента определяет наличие наследников и определяет структуру
        public void ConstructDeportamentsHtml(Guid id, ref string childoutput)
        {
            Department department = unit.DepartmentRepository.GetById(id);
            List<Department>? child_departments = unit.DepartmentRepository.GetByParentId(id);
            //если есть наследники, то заворачиваем в <parent> dep <child> dep's child  </child> </parent>
            if (child_departments !=null)
            {

                childoutput +=  "<div class =\"parent_department\">"+ GetDepartmentRowHtmlString(department, true) +
               "<div class=\"child_department\">";

                  foreach (Department child_department in child_departments.ToList())
                  {
                    ConstructDeportamentsHtml(child_department.ID, ref childoutput);
                  }
                childoutput += "</div></div>";
            }

            //если это последний элемент в узле
            else
            {
                childoutput += GetDepartmentRowHtmlString(department,false);
            }


        }

    }
}

