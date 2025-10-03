using System.ComponentModel.DataAnnotations;

namespace Task5.Models;

public class BaseFilter
{
    [Required, Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0.")]
    public int PageSize { get; set; }

    [Required, Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0.")]
    public int PageNumber { get; set; }
}
