using StudentInfoManagementSystem.DAL;
using StudentInfoManagementSystem.Data;
using StudentInfoManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentInfoManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private IStudentRepository studentRepository;
        public StudentsController()
        {
			this.studentRepository = new StudentRepository(new StudentDbContext());
		}

        public StudentsController(IStudentRepository studentRepository)
        {
            this.studentRepository=studentRepository;
        }
		// GET: Students
		public ActionResult Index(string searchString)
		{
			var students = from s in studentRepository.GetActiveStudents()
						   where !s.IsDeleted 
						   select s;

			if (!String.IsNullOrEmpty(searchString))
			{
				searchString = searchString.ToUpper();

				students = students.Where(s =>
					s.StudentName.ToUpper().Contains(searchString) ||
					s.HomeAddress.ToUpper().Contains(searchString) ||
					s.RegistrationDate.ToString("yyyy-MM-dd").Contains(searchString)
				);
			}

			students = students.OrderBy(s => s.RegistrationDate);

			return View(students);
		}



		public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student student)
        {
			try
			{
				if (ModelState.IsValid)
				{
					studentRepository.AddNewStudent(student);
					studentRepository.Save();
					return RedirectToAction("Index");
				}
			}
			catch (SqlException)
			{
				ModelState.AddModelError(string.Empty, "Please pass the correct data");
			}
			return View(student);
		}

		public ViewResult Details(int id)
		{
			Student student = studentRepository.GetStudentById(id);
			return View(student);
		}

		public ActionResult Edit(int id)
		{
			Student student = studentRepository.GetStudentById(id);
			return View(student);
		}

		[HttpPost]
		public ActionResult Edit(Student student)
		{
			try
			{
				if(ModelState.IsValid)
				{
					studentRepository.UpdateStudent(student);
					studentRepository.Save();
					return RedirectToAction("Index");
				}	
			}
			catch (SqlException)
			{
				ModelState.AddModelError(string.Empty, "Please pass the correct data");
			}
			return View(student);
		}

		public ActionResult Delete(bool? saveChangesError = false, int id = 0)
		{
			if (saveChangesError.GetValueOrDefault())
			{
				ViewBag.ErrorMessage = "Delete failed";
			}
			Student student = studentRepository.GetStudentById(id);
			return View(student);
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			try
			{
				if (ModelState.IsValid)
				{
					studentRepository.SoftDeleteStudent(id);
					studentRepository.Save();
				}
			}
			catch (SqlException)
			{
				ModelState.AddModelError(string.Empty, "Please pass the correct data");
			}
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			studentRepository.Dispose();
			base.Dispose(disposing);
		}
	}
}
