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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployees();

            return Ok(employees.Select(EmployeeMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployee(id);

            return Ok(employee is null ? null : EmployeeMapper.ToDto(employee));
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee(EmployeeDto employee)
        {
            var result = await _employeeRepository.AddEmployee(EmployeeMapper.ToEntity(employee));

            return Ok(result is null ? null : EmployeeMapper.ToDto(result));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateEmployee(EmployeeDto employee)
        {
            var result = await _employeeRepository.UpdateEmployee(EmployeeMapper.ToEntity(employee));

            return Ok(result is null ? null : EmployeeMapper.ToDto(result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            await _employeeRepository.DeleteEmployee(id);

            return Ok();
        }
    }
}
