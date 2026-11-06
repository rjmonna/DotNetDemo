using BlazorServerCRUD.Models;

namespace BlazorServerCRUD.Web.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddEmployee(Employee employee)
        {
            await _httpClient.PostAsJsonAsync($"api/employee", employee);
        }

        public async Task DeleteEmployee(int id)
        {
            await _httpClient.DeleteAsync($"api/employee/{id}");
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await _httpClient.GetFromJsonAsync<Employee>($"api/employee/{id}")
                ?? throw new InvalidDataException("The employee response was empty.");
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _httpClient.GetFromJsonAsync<Employee[]>("api/employee")
                ?? throw new InvalidDataException("The employees response was empty.");
        }

        public async Task UpdateEmployee(Employee employee)
        {
            await _httpClient.PutAsJsonAsync($"api/employee", employee);
        }
    }
}