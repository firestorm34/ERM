using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskApplication.Data;
using TestTaskApplication.Models;


namespace TestTaskApplication.Controllers
{
    public class EmployeeController : Controller
    {
        UnitOfWork _unit;

        public EmployeeController(UnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }
        
        public IActionResult Index()
        {
            List<Employee> employees;
            try
            {
                employees = _unit.EmployeeRepository.GetAll();
            }
            catch (Exception e) { 
                ModelState.AddModelError("", e.Message); 
                return View(); }
            if (employees != null)
            {
                foreach(Employee employee in employees)
                {
                    employee.Department = _unit.DepartmentRepository.GetById(employee.DepartmentID);
                }
            }
            return View(employees);
        }


        public IActionResult ShowByDepartmentId(Guid id)
        {
            
            List<Employee> employees;
            try
            {
                employees = _unit.EmployeeRepository.GetByDepartmentId(id);
            }
            catch (Exception e) { 
                ModelState.AddModelError("", e.Message); return View(); 
            }
            foreach (Employee employee in employees)
            {
                employee.Department = _unit.DepartmentRepository.GetById(employee.DepartmentID);
            }
            return View("Index", employees);
        }

        
        public IActionResult Create()
        {
            ViewBag.Date = DateTime.Now;
            ViewBag.SelectList = GetDepartmentsSelectList();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _unit.EmployeeRepository.Create(employee);
                }
                catch (Exception e) { 
                    ModelState.AddModelError("", e.Message);
                    List<Department> departments = _unit.DepartmentRepository.GetAll();
                    SelectList selectList = new SelectList(departments, nameof(Department.ID), nameof(Department.Name));
                    ViewBag.SelectList = selectList;
                    return View(employee); 
                }
 
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Date = DateTime.Now;
                ViewBag.SelectList = GetDepartmentsSelectList();
                return View(employee);
            }
        }

       
        public IActionResult Edit(int id)
        {
            Employee? employee = _unit.EmployeeRepository.GetById((decimal)id);
            if (employee == null)
            {
                ModelState.AddModelError("", "User with such id is not found");
                return View("Index");
            }
            ViewBag.SelectList = GetDepartmentsSelectList();
            return View(employee);
        }

        private SelectList GetDepartmentsSelectList()
        {
            List<Department> departments = _unit.DepartmentRepository.GetAll();
            SelectList selectList = new SelectList(departments, nameof(Department.ID), nameof(Department.Name));
            return selectList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid) {

                try
                {
                    
                    _unit.EmployeeRepository.Update(employee);
                }
                catch (Exception e) {
                    ModelState.AddModelError("", e.Message); 
                    ViewBag.SelectList = GetDepartmentsSelectList();
                    return View(employee); 
                }
                
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.SelectList = GetDepartmentsSelectList();
                return View(employee);
            }
        }

        
     
        public IActionResult Delete(int id)
        {
            try
            {
                _unit.EmployeeRepository.Delete((decimal)id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
            return RedirectToAction("Index");
        }
    }
   
}
