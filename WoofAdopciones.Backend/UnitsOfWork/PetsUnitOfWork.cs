using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.UnitsOfWork
{
    public class PetsUnitOfWork : GenericUnitOfWork<Pet>, IPetsUnitOfWork
    {
        private readonly IPetsRepository _petsRepository;

        public PetsUnitOfWork(IGenericRepository<Pet> repository, IPetsRepository petsRepository) : base(repository)
        {
            _petsRepository = petsRepository;
        }

        public override async Task<Response<IEnumerable<Pet>>> GetAsync(PaginationDTO pagination) => await _petsRepository.GetAsync(pagination);

        public override async Task<Response<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _petsRepository.GetTotalPagesAsync(pagination);

        public override async Task<Response<Pet>> GetAsync(int id) => await _petsRepository.GetAsync(id);

        public async Task<Response<Pet>> AddFullAsync(PetDTO productDTO) => await _petsRepository.AddFullAsync(productDTO);

        public async Task<Response<Pet>> UpdateFullAsync(PetDTO productDTO) => await _petsRepository.UpdateFullAsync(productDTO);

        public async Task<Response<ImageDTO>> AddImageAsync(ImageDTO imageDTO) => await _petsRepository.AddImageAsync(imageDTO);

        public async Task<Response<ImageDTO>> RemoveLastImageAsync(ImageDTO imageDTO) => await _petsRepository.RemoveLastImageAsync(imageDTO);
    }
}