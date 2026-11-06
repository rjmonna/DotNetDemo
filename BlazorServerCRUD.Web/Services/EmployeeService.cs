using BlazorServerCRUD.Models.DTOs;

namespace BlazorServerCRUD.Web.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddEmployee(EmployeeDto employee)
        {
            await _httpClient.PostAsJsonAsync($"api/employee", employee);
        }

        public async Task DeleteEmployee(int id)
        {
            await _httpClient.DeleteAsync($"api/employee/{id}");
        }

        public async Task<EmployeeDto> GetEmployee(int id)
        {
            return await _httpClient.GetFromJsonAsync<EmployeeDto>($"api/employee/{id}")
                ?? throw new InvalidDataException("The employee response was empty.");
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployees()
        {
            return await _httpClient.GetFromJsonAsync<EmployeeDto[]>("api/employee")
                ?? throw new InvalidDataException("The employees response was empty.");
        }

        public async Task UpdateEmployee(EmployeeDto employee)
        {
            await _httpClient.PutAsJsonAsync($"api/employee", employee);
        }
    }
}