using Admin.Data;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController
    {
        private readonly AppDbContext _db;

        public TestController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("mysql")]
        public IActionResult TestMySql()
        {
            return Ok(_db.Users.Count());
        }
        
        [HttpGet("mysql")]
        private IActionResult Ok(int v)
        {
            throw new NotImplementedException();
        }
    }
}
