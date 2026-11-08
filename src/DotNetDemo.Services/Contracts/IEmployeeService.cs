using DotNetDemo.Models.DTOs;

namespace DotNetDemo.Services.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetEmployees();

        Task<EmployeeDto> GetEmployee(int id);

        Task DeleteEmployee(int id);

        Task AddEmployee(EmployeeDto employee);

        Task UpdateEmployee(EmployeeDto employee);
    }
}