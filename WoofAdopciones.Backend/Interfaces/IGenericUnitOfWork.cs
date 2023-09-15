using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Interfaces
{
    public interface IGenericUnitOfWork<T> where T : class
    {
        Task<Response<IEnumerable<T>>> GetAsync();

        Task<Response<T>> AddAsync(T model);

        Task<Response<T>> UpdateAsync(T model);

        Task<Response<T>> DeleteAsync(int id);

        Task<Response<T>> GetAsync(int id);
    }
}
