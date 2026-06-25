namespace dotnetapp.Models
{
    public class Student
    {
        public int Studentld { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Grade { get; set; } = string.Empty;
    }
}

using dotnetapp.Models;
using System.Collections.Generic;
using System.Linq;

namespace dotnetapp.Services
{
    public class StudentService
    {
        private readonly List<Student> students = new List<Student>
        {
            new Student { Studentld = 1, Name = "Alice", Age = 18, Grade = "A" },
            new Student { Studentld = 2, Name = "Bob", Age = 17, Grade = "B" },
            new Student { Studentld = 3, Name = "Charlie", Age = 16, Grade = "C" }
        };

        public IEnumerable<Student> GetAllStudents()
        {
            return students;
        }

        public Student? GetStudentByld(int studentld)
        {
            return students.FirstOrDefault(s => s.Studentld == studentld);
        }

        public void CreateStudent(Student newStudent)
        {
            students.Add(newStudent);
        }

        public bool UpdateStudent(int studentld, Student updatedStudent)
        {
            var existingStudent = students.FirstOrDefault(s => s.Studentld == studentld);
            if (existingStudent == null)
            {
                return false;
            }

            existingStudent.Name = updatedStudent.Name;
            existingStudent.Age = updatedStudent.Age;
            existingStudent.Grade = updatedStudent.Grade;
            return true;
        }

        public bool DeleteStudent(int studentld)
        {
            var existingStudent = students.FirstOrDefault(s => s.Studentld == studentld);
            if (existingStudent == null)
            {
                return false;
            }

            students.Remove(existingStudent);
            return true;
        }
    }
}

using dotnetapp.Models;
using dotnetapp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            var list = _studentService.GetAllStudents().ToList();
            if (!list.Any())
            {
                return NoContent();
            }
            return Ok(list);
        }

        [HttpGet("{studentld}")]
        public ActionResult<Student> GetStudentByld(int studentld)
        {
            var student = _studentService.GetStudentByld(studentld);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public ActionResult<Student> CreateStudent([FromBody] Student newStudent)
        {
            if (newStudent == null || string.IsNullOrWhiteSpace(newStudent.Name))
            {
                return BadRequest();
            }

            _studentService.CreateStudent(newStudent);
            return CreatedAtAction(nameof(GetStudentByld), new { studentld = newStudent.Studentld }, newStudent);
        }

        [HttpPut("{studentld}")]
        public IActionResult UpdateStudent(int studentld, [FromBody] Student updatedStudent)
        {
            if (updatedStudent == null)
            {
                return BadRequest();
            }

            var success = _studentService.UpdateStudent(studentld, updatedStudent);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{studentld}")]
        public IActionResult DeleteStudent(int studentld)
        {
            var success = _studentService.DeleteStudent(studentld);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}


