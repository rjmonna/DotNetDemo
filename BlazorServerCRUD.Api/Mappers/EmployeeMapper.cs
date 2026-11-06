using BlazorServerCRUD.Models;
using BlazorServerCRUD.Models.DTOs;

namespace BlazorServerCRUD.Api.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDto ToDto(Employee employee) =>
            new()
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                DateOfBirth = employee.DateOfBirth,
                Gender = employee.Gender,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.DepartmentName
            };

        public static Employee ToEntity(EmployeeDto employee) =>
            new()
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                DateOfBirth = employee.DateOfBirth,
                Gender = employee.Gender,
                DepartmentId = employee.DepartmentId
            };
    }
}
