using StudentInfoManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StudentInfoManagementSystem.Data
{
	public class StudentDbContext:DbContext
	{
        public StudentDbContext():base(nameOrConnectionString:"DatabaseConnectionString")
        {
            
        }

        public DbSet<Student> students { get; set; }
    }
}