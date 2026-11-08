using DotNetDemo.Infrastructure;
using DotNetDemo.Infrastructure.Contracts;
using DotNetDemo.Models;
using DotNetDemo.Models.DTOs;
using DotNetDemo.Api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace DotNetDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
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
