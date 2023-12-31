using BlazorServerCRUD.Api.Models;
using BlazorServerCRUD.Models;
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
            return Ok(await _departmentRepository.GetDepartments());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDepartment(int id)
        {
            return Ok(await _departmentRepository.GetDepartment(id));
        }

        [HttpPost]
        public async Task<ActionResult> AddDepartment(Department department)
        {
            return Ok(await _departmentRepository.AddDepartment(department));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateDepartment(Department department)
        {
            return Ok(await _departmentRepository.UpdateDepartment(department));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            await _departmentRepository.DeleteDepartment(id);

            return Ok();
        }
    }
}
