using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> _repository;
        public EmployeeController(IRepository<Employee> repository)
        {
            _repository=repository;     
        }
        [HttpGet]
        [Route("getallemployee")]
        public async Task<IActionResult> GetAllEmployee()
        {
            return Ok(await _repository.GetAll());
        }

        [HttpGet]
        [Route("getemployeebyid/{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute]int id)
        {
            return Ok(await _repository.FindByIdAsync(id));
        }

        [HttpPost]
        [Route("addemployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee model)
        {
            await _repository.AddAsync(model);
            await _repository.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        [Route("updateemployee/{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute]int id,[FromBody] Employee model)
        {
            var employee=await _repository.FindByIdAsync(id);
            employee.LastWorkingDate=model.LastWorkingDate;
            employee.Email=model.Email;
            employee.Name=model.Name;
            employee.Phone=model.Phone;
            employee.JobTitle=model.JobTitle;
            employee.JoiningDate=model.JoiningDate;
            employee.DateOfBirth=model.DateOfBirth;
            employee.DepartmentId=model.DepartmentId;
            employee.Gender=model.Gender;
            _repository.Update(employee);
            await _repository.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        [Route("deleteemployee/{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return Ok();
        }
    }
}
