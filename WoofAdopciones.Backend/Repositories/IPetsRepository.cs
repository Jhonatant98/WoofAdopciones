using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;
namespace WoofAdopciones.Backend.Repositories
{
    public interface IPetsRepository
    { 
     Task<Response<Pet>> GetAsync(int id);

    Task<Response<IEnumerable<Pet>>> GetAsync(PaginationDTO pagination);

    Task<Response<int>> GetTotalPagesAsync(PaginationDTO pagination);

    Task<Response<Pet>> AddFullAsync(PetDTO petDTO);

    Task<Response<Pet>> UpdateFullAsync(PetDTO petDTO);

    Task<Response<ImageDTO>> AddImageAsync(ImageDTO imageDTO);

    Task<Response<ImageDTO>> RemoveLastImageAsync(ImageDTO imageDTO);
}
}
