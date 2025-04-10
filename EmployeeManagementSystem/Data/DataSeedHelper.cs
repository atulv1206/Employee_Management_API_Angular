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
            _context.SaveChanges();
        }
    }
}
