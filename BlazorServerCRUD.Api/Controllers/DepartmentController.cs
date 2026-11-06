using BlazorServerCRUD.Api.Models;
using BlazorServerCRUD.Api.Mappers;
using BlazorServerCRUD.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BlazorServerCRUD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRempository _departmentRepository;

        public DepartmentController(IDepartmentRempository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetDepartments()
        {
            var departments = await _departmentRepository.GetDepartments();

            return Ok(departments.Select(DepartmentMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDepartment(int id)
        {
            var department = await _departmentRepository.GetDepartment(id);

            return Ok(department is null ? null : DepartmentMapper.ToDto(department));
        }

        [HttpPost]
        public async Task<ActionResult> AddDepartment(DepartmentDto department)
        {
            var result = await _departmentRepository.AddDepartment(DepartmentMapper.ToEntity(department));

            return Ok(result is null ? null : DepartmentMapper.ToDto(result));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateDepartment(DepartmentDto department)
        {
            var result = await _departmentRepository.UpdateDepartment(DepartmentMapper.ToEntity(department));

            return Ok(result is null ? null : DepartmentMapper.ToDto(result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            await _departmentRepository.DeleteDepartment(id);

            return Ok();
        }
    }
}
