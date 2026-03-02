using Admin.Data;
using Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Admin.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // LOGIN (BY EMAIL)
        public string Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new Exception("Thiếu email hoặc mật khẩu");

            // ⚠️ SELECT CỤ THỂ – KHÔNG LẤY FullName
            var emp = _db.Employees
                .AsNoTracking()
                .Where(e => e.Email == email)
                .Select(e => new Employee
                {
                    Id = e.Id,
                    Email = e.Email,
                    Password = e.Password,
                    Role = e.Role
                })
                .FirstOrDefault();

            if (emp == null)
                throw new Exception("Tài khoản không tồn tại");

            if (string.IsNullOrWhiteSpace(emp.Password))
                throw new Exception("Tài khoản chưa có mật khẩu hợp lệ");

            if (!BCrypt.Net.BCrypt.Verify(password, emp.Password))
                throw new Exception("Mật khẩu không đúng");

            return GenerateJwtToken(emp);
        }

        // RESET PASSWORD
        public void ResetPasswordByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Thiếu email");

            var emp = _db.Employees.FirstOrDefault(e => e.Email == email);
            if (emp == null)
                throw new Exception("Không tìm thấy nhân viên");

            emp.Password = BCrypt.Net.BCrypt.HashPassword("123456");
            _db.SaveChanges();
        }

        // JWT TOKEN
        private string GenerateJwtToken(Employee emp)
        {
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];

            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("Thiếu cấu hình Jwt:Key");
            if (string.IsNullOrWhiteSpace(jwtIssuer))
                throw new Exception("Thiếu cấu hình Jwt:Issuer");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, emp.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, emp.Email),
                new Claim(ClaimTypes.Role, emp.Role ?? "EMPLOYEE")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
