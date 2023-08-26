using StudentInfoManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfoManagementSystem.DAL
{
	public interface IStudentRepository:IDisposable
	{
		IEnumerable<Student> GetAllStudents();
		void AddNewStudent(Student student);
		void UpdateStudent(Student student);
		void DeleteStudent(int StudentId);

		Student GetStudentById(int StudentId);

		IQueryable<Student> GetActiveStudents();

		void SoftDeleteStudent(int id);
		void Save();
	}
}
