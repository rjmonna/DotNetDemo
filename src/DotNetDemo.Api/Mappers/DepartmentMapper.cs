using DotNetDemo.Models;
using DotNetDemo.Models.DTOs;

namespace DotNetDemo.Api.Mappers;

public static class DepartmentMapper
{
    public static DepartmentDto ToDto(Department department) =>
        new()
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName
        };

    public static Department ToEntity(DepartmentDto department) =>
        new()
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName
        };
}
