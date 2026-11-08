using DotNetDemo.Infrastructure.Contracts;
using DotNetDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetDemo.Infrastructure
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;
        public EmployeeRepository(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }
        
        public async Task<Employee?> AddEmployee(Employee employee)
        {
            await _appDbContext.Employees.AddAsync(employee);

            await _appDbContext.SaveChangesAsync();

            return employee;
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var result = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (result != null)
            {
                _appDbContext.Employees.Remove(result);

                await _appDbContext.SaveChangesAsync();
            }

            return;
        }

        public async Task<Employee?> GetEmployee(int employeeId)
        {
            var result = await _appDbContext
                .Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            return result;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _appDbContext
                .Employees
                .Include(e => e.Department)
                .ToListAsync();
        }

        public async Task<Employee?> UpdateEmployee(Employee employee)
        {
            var result = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

            if (result != null)
            {
                result.EmployeeName = employee.EmployeeName;
                result.Gender = employee.Gender;
                result.DepartmentId = employee.DepartmentId;
                result.DateOfBirth = employee.DateOfBirth;

                await _appDbContext.SaveChangesAsync();
            }

            return result;
        }
    }
}