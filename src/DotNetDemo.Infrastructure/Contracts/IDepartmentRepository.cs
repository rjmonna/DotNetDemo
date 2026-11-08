using DotNetDemo.Models;

namespace DotNetDemo.Infrastructure.Contracts
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetDepartment(int departmentId);

        Task<IEnumerable<Department>> GetDepartments();

        Task<Department?> AddDepartment(Department department);

        Task DeleteDepartment(int departmentId);

        Task<Department?> UpdateDepartment(Department department);
    }
}