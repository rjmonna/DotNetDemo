namespace DotNetDemo.Models.DTOs;

public class EmployeeDto
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }
}
