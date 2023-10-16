using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int id);

        Task<IEnumerable<T>> GetAsync();

        Task<Response<T>> AddAsync(T entity);

        Task<Response<T>> DeleteAsync(int id);

        Task<Response<T>> UpdateAsync(T entity);
    }
}
