using StudentInfoManagementSystem.Data;
using StudentInfoManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StudentInfoManagementSystem.DAL
{
	public class StudentRepository : IStudentRepository, IDisposable
	{
		private StudentDbContext db;

		public StudentRepository(StudentDbContext db)
		{
			this.db = db;
		}
		public void AddNewStudent(Student student)
		{
			db.students.Add(student);
		}

		public void DeleteStudent(int studentID)
		{
			Student student = db.students.Find(studentID);
			db.students.Remove(student);
		}

		public IEnumerable<Student> GetAllStudents()
		{
			return db.students.ToList();
		}

		public void UpdateStudent(Student student)
		{
			db.Entry(student).State = EntityState.Modified;
		}

		public IQueryable<Student> GetActiveStudents()
		{
			return db.students.Where(s => !s.IsDeleted);
		}


		public void Save()
		{
			db.SaveChanges();
		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					db.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public Student GetStudentById(int id)
		{
			return db.students.FirstOrDefault(s => s.StudentId == id && !s.IsDeleted);
		}

		public void SoftDeleteStudent(int id)
		{
			var student = db.students.Find(id);
			if (student != null)
			{
				student.IsDeleted = true;
			}
		}
	}
}