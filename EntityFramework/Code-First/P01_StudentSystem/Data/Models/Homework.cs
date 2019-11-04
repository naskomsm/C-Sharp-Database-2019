namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Homework
    {
        public int HomeworkId { get; set; }

        public string Content { get; set; }

        [Required]
        public ContentType ContentType { get; set; }

        [Required]
        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
