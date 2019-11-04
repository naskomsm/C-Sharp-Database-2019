namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        [MaxLength(MyValidator.StudentNameLength)]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public ICollection<StudentCourse> CourseEnrollments { get; set; } = new HashSet<StudentCourse>();

        public ICollection<Homework> HomeworkSubmissions { get; set; } = new HashSet<Homework>();
    }
}
