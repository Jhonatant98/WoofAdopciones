using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entites;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Repositories
{
    public interface IAVolunteringsRepository
    {
     
        Task<Response<VolunteringDTO>> AddFullAsync(string email, VolunteringDTO volunteringDTO);

        Task<Response<IEnumerable<RequestVolunteering>>> GetAsync(string email);

        Task<Response<RequestVolunteering>> GetAsync(int id);
    }
}
