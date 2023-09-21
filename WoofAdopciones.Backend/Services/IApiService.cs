using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Services
{
    public interface IApiService
    {
        Task<Response<T>> GetAsync<T>(string servicePrefix, string controller);
    }
}
