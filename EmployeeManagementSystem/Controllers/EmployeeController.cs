using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> _empRepo;

        private readonly IRepository<User> _userRepo;

        public EmployeeController(IRepository<Employee> empRepo,IRepository<User> userRepo)
        {
            _empRepo = empRepo;
            _userRepo = userRepo;
        }
        [HttpGet]
        [Route("getallemployee")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllEmployee()
        {
            return Ok(await _empRepo.GetAll());
        }

        [HttpGet]
        [Route("getemployeebyid/{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeById([FromRoute]int id)
        {
            return Ok(await _empRepo.FindByIdAsync(id));
        }

        [HttpPost]
        [Route("addemployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee model)
        {
            var user = new User()
            {
                Email=model.Email!,
                Role="Employee",
                Password=(new PasswordHelper()).HashPassword("Emp@123")
            };
            await _userRepo.AddAsync(user);
            model.User= user;
            //model.UserId = (await _userRepo.GetAll(x=>x.Email==user.Email)).FirstOrDefault()!.Id;
            await _empRepo.AddAsync(model);
            await _empRepo.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        [Route("updateemployee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee([FromRoute]int id,[FromBody] Employee model)
        {
            var employee=await _empRepo.FindByIdAsync(id);
            employee.LastWorkingDate=model.LastWorkingDate;
            employee.Email=model.Email;
            employee.Name=model.Name;
            employee.Phone=model.Phone;
            employee.JobTitle=model.JobTitle;
            employee.JoiningDate=model.JoiningDate;
            employee.DateOfBirth=model.DateOfBirth;
            employee.DepartmentId=model.DepartmentId;
            employee.Gender=model.Gender;
            _empRepo.Update(employee);
            await _empRepo.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete]
        [Route("deleteemployee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            await _empRepo.DeleteAsync(id);
            await _empRepo.SaveChangesAsync();
            return Ok();
        }
    }
}
