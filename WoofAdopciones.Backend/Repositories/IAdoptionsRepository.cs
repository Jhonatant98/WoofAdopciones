using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Repositories
{
    public interface IAdoptionsRepository
    {
        Task<Response<IEnumerable<Adoption>>> GetAsync(string email, PaginationDTO pagination);

        Task<Response<int>> GetTotalPagesAsync(string email, PaginationDTO pagination);

        Task<Response<Adoption>> GetAsync(int id);

        Task<Response<Adoption>> UpdateFullAsync(string email, AdoptionDTO adoptionDTO);

        Task<Response<bool>> ProcessAdoptionAsync(string email, int petId);
    }
}
