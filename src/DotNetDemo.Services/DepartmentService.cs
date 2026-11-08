using System.Net.Http.Json;
using DotNetDemo.Services.Contracts;
using DotNetDemo.Models.DTOs;

namespace DotNetDemo.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _httpClient;

        public DepartmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartments()
        {
            return await _httpClient.GetFromJsonAsync<DepartmentDto[]>("api/department")
                ?? throw new InvalidDataException("The departments response was empty.");
        }

        public async Task AddDepartment(DepartmentDto department)
        {
            await _httpClient.PostAsJsonAsync($"api/department", department);
        }

        public async Task DeleteDepartment(int id)
        {
            await _httpClient.DeleteAsync($"api/department/{id}");
        }

        public async Task<DepartmentDto> GetDepartment(int id)
        {
            return await _httpClient.GetFromJsonAsync<DepartmentDto>($"api/department/{id}")
                ?? throw new InvalidDataException("The department response was empty.");
        }

        public async Task UpdateDepartment(DepartmentDto department)
        {
            await _httpClient.PutAsJsonAsync($"api/department", department);
        }
    }
}