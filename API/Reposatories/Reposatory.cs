using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Reposatories
{
    public class Reposatory<T> : IReposatory<T> where T : class
    {
        private readonly NewsContext _context;

        public Reposatory(NewsContext context)
        {
            _context = context;
        }

        public async Task Create(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
