﻿using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Data
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter);
        Task<T> FindByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
