using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.UnitsOfWork
{
    public interface IPetsUnitOfWork
    { 
   Task<Response<Pet>> GetAsync(int id);

    Task<Response<IEnumerable<Pet>>> GetAsync(PaginationDTO pagination);

    Task<Response<int>> GetTotalPagesAsync(PaginationDTO pagination);

    Task<Response<Pet>> AddFullAsync(PetDTO pétDTO);

    Task<Response<Pet>> UpdateFullAsync(PetDTO petDTO);

    Task<Response<ImageDTO>> AddImageAsync(ImageDTO imageDTO);

    Task<Response<ImageDTO>> RemoveLastImageAsync(ImageDTO imageDTO);

    Task<Response<Pet>> UpdateAsync(Pet pet);
}
}
