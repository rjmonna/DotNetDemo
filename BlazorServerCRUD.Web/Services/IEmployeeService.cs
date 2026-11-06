using BlazorServerCRUD.Models.DTOs;

namespace BlazorServerCRUD.Web.Services
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