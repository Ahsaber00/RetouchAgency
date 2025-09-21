using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationContext _context;
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return  _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            if (entity != null)
            {
                return entity;
            }
            return null;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }


        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }



        //---------------------------------------------------------------------------------------------------------------------------
        //will be implemented when needed

        public Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
