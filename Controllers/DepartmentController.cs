using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTaskApplication.Data;
using TestTaskApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskApplication.Controllers
{
    public class DepartmentController : Controller
    {

        private UnitOfWork _unitOfWork;
        public DepartmentController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            List<Department> departments;
            try
            {
                departments =  _unitOfWork.DepartmentRepository.GetAll();
            }
            catch (Exception e) { ModelState.AddModelError("", e.Message); return View(); }

            return View(departments);

        }

        public ActionResult Details(int id)
        {
            return View();
        }

        
    }
}
