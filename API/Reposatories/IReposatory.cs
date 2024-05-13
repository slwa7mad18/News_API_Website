namespace API.Reposatories
{
    public interface IReposatory<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T?> GetById(string id);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task Save();
    }
}
