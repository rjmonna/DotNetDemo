using DotNetDemo.Infrastructure.Contracts;
using DotNetDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetDemo.Infrastructure
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _appDbContext;

        public DepartmentRepository(AppDbContext appDbContext) =>
            _appDbContext = appDbContext;

        public Task<Department?> GetDepartment(int departmentId)
        {
            return _appDbContext.Departments.FirstOrDefaultAsync(e => e.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _appDbContext.Departments.ToListAsync();
        }

        public async Task<Department?> AddDepartment(Department department)
        {
            await _appDbContext.Departments.AddAsync(department);

            await _appDbContext.SaveChangesAsync();

            return department;
        }

        public async Task DeleteDepartment(int departmentId)
        {
            var result = await _appDbContext.Departments.FirstOrDefaultAsync(e => e.DepartmentId == departmentId);

            if (result != null)
            {
                _appDbContext.Departments.Remove(result);

                await _appDbContext.SaveChangesAsync();
            }

            return;
        }

        public async Task<Department?> UpdateDepartment(Department department)
        {
            var result = await _appDbContext.Departments.FirstOrDefaultAsync(e => e.DepartmentId == department.DepartmentId);

            if (result != null)
            {
                result.DepartmentName = department.DepartmentName;

                await _appDbContext.SaveChangesAsync();
            }

            return result;
        }
    }
}