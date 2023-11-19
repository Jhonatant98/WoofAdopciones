using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.UnitsOfWork
{
    public class AdoptionsUnitOfWork : GenericUnitOfWork<Adoption>, IAdoptionsUnitOfWork
    {
        private readonly IAdoptionsRepository _adoptionsRepository;

        public AdoptionsUnitOfWork(IGenericRepository<Adoption> repository, IAdoptionsRepository adoptionsRepository) : base(repository)
        {
            _adoptionsRepository = adoptionsRepository;
        }

        public async Task<Response<IEnumerable<Adoption>>> GetAsync(string email, PaginationDTO pagination) => await _adoptionsRepository.GetAsync(email, pagination);

        public async Task<Response<int>> GetTotalPagesAsync(string email, PaginationDTO pagination) => await _adoptionsRepository.GetTotalPagesAsync(email, pagination);

        public override async Task<Response<Adoption>> GetAsync(int id) => await _adoptionsRepository.GetAsync(id);

        public async Task<Response<Adoption>> UpdateFullAsync(string email, AdoptionDTO adoptionDTO) => await _adoptionsRepository.UpdateFullAsync(email, adoptionDTO);

        public async Task<Response<bool>> ProcessAdoptionAsync(string email, int petId) => await _adoptionsRepository.ProcessAdoptionAsync(email, petId);

    }
}
