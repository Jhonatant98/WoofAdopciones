using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Repositories
{
    public interface IAdoptionCenterRepository
    {
        Task<Response<IEnumerable<AdoptionCenter>>> GetAsync(PaginationDTO pagination);

        Task<Response<int>> GetTotalPagesAsync(PaginationDTO pagination);

        Task<IEnumerable<AdoptionCenter>> GetComboAsync();

        Task<Response<AdoptionCenter>> AddAsync(AdoptionCenter model);
    }
}
