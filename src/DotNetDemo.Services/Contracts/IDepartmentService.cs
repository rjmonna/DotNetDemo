using DotNetDemo.Models.DTOs;

namespace DotNetDemo.Services.Contracts
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetDepartments();

        Task<DepartmentDto> GetDepartment(int id);

        Task DeleteDepartment(int id);

        Task AddDepartment(DepartmentDto department);

        Task UpdateDepartment(DepartmentDto department);
    }
}