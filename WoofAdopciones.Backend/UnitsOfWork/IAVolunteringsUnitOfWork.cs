
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entites;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.UnitsOfWork
{
    public interface IAVolunteringsUnitOfWork
    {
        Task<Response<VolunteringDTO>> AddFullAsync(string email, VolunteringDTO volunteringDTO);

        Task<Response<IEnumerable<RequestVolunteering>>> GetAsync(string email);

        Task<Response<RequestVolunteering>> GetAsync(int id);
    }
}