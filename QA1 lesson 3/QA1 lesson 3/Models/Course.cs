using System;
using System.Collections.Generic;

namespace QA1_lesson_3.Models;

public partial class Course
{
    public long Id { get; set; }

    public string? Group { get; set; }

    public string? CourseName { get; set; }

    public byte? Credit { get; set; }

    public string? SubCode { get; set; }

    public string? Major { get; set; }

    public string? Note { get; set; }
}
