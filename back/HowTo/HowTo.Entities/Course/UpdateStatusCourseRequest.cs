using System.ComponentModel.DataAnnotations;

namespace HowTo.Entities.Course;

public class UpdateStatusCourseRequest
{
    [Required]
    public int CourseId { get; set; }
    [Required]
    public EntityStatus Status { get; set; }
}