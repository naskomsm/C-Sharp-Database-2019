﻿namespace TeisterMask.DataProcessor.ImportDto.XML
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Task")]
    public class TaskXMLDto
    {
        [Required]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Invalid length")]
        public string Name { get; set; }

        [Required]
        public string OpenDate { get; set; }

        [Required]
        public string DueDate { get; set; }

        [Required]
        public string ExecutionType { get; set; }

        [Required]
        public string LabelType { get; set; }
    }
}
