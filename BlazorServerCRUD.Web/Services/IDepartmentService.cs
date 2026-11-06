using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerCRUD.Models.DTOs;

namespace BlazorServerCRUD.Web.Services
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