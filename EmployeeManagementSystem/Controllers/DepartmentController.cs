using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IRepository<Department> _repository;
        public DepartmentController(IRepository<Department> repository)
        {
           _repository = repository;
        }
        [HttpPost]
        [Route("createdepartment")]
        public async Task<IActionResult> CreateDepartment([FromBody]Department model)
        {
            await _repository.AddAsync(model);
            await _repository.SaveChangesAsync();
            return Ok(model);
        }
        [HttpPut]
        [Route("editdepartment/{id}")]
        public async Task<IActionResult> UpdateDepartment([FromBody]Department model,[FromRoute]int id)
        {
            var entity= await _repository.FindByIdAsync(id);
            entity.Name = model.Name;
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        [Route("getalldepartment")]
        public async Task<IActionResult> GetAllDepartment()
        {
            var list=await _repository.GetAll();
            return Ok(list);
        }
        [HttpDelete]
        [Route("deletedepartment/{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute]int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
            return Ok();
        }
    }
}
