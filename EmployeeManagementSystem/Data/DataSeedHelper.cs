using EmployeeManagementSystem.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.Data
{
    public class DataSeedHelper
    {
        private readonly AppDbContext _context;
        public DataSeedHelper(AppDbContext context)
        {
            _context = context;  
        }
        public void InsertData()
        {
            if (!_context.Employees.Any())
            {
                _context.Employees.Add(new Entity.Employee {Name = "Employee 01" });
                _context.Employees.Add(new Entity.Employee { Name = "Employee 02" });
            }

            if (!_context.Users.Any())
            {
                var passwordHelper=new PasswordHelper();
                _context.Users.Add(new Entity.User { 
                    Email = "admin@gmail.com",
                    Password= passwordHelper.HashPassword("Admin@123"),
                    Role="Admin"
                });
                _context.Users.Add(new Entity.User
                {
                    Email = "emp01@gmail.com",
                    Password = passwordHelper.HashPassword("Emp@123"),
                    Role = "Employee"
                });
            }
            
            _context.SaveChanges();
        }
    }
}
