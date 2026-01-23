using Admin.Models;
using Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [Authorize(Roles = "ADMIN,HR")]
    [ApiController]
    [Route("api/admin/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService service)
        {
            _service = service;
        }
        // GET ALL
        // GET: api/admin/employees
        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _service.GetAll();
            return Ok(employees);
        }
        // GET BY ID
        // GET: api/admin/employees/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var employee = _service.GetById(id);

            if (employee == null)
                return NotFound(new { message = "Employee not found" });

            return Ok(employee);
        }
        // CREATE
        // POST: api/admin/employees
        [HttpPost]
        public IActionResult Create([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = _service.Create(employee);
            return Ok(created);
        }
        // UPDATE
        // PUT: api/admin/employees/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = _service.Update(id, employee);

            if (updated == null)
                return NotFound(new { message = "Employee not found" });

            return Ok(updated);
        }
        // DELETE
        // DELETE: api/admin/employees/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")] // chỉ ADMIN được xóa
        public IActionResult Delete(int id)
        {
            var success = _service.Delete(id);

            if (!success)
                return NotFound(new { message = "Employee not found" });

            return Ok(new { message = "Deleted successfully" });
        }
    }
}
