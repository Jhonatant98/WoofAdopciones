using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entites;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.UnitsOfWork
{
    public class VolunteringsUnitOfWorkTests : GenericUnitOfWork<RequestVolunteering>, IAVolunteringsUnitOfWork
    {
        private readonly IAVolunteringsRepository _adoptionsRepository;

        public VolunteringsUnitOfWorkTests(IGenericRepository<RequestVolunteering> repository, IAVolunteringsRepository adoptionsRepository) : base(repository)
        {
            _adoptionsRepository = adoptionsRepository;
        }

        public async Task<Response<VolunteringDTO>> AddFullAsync(string email, VolunteringDTO volunteringDTO) => await _adoptionsRepository.AddFullAsync(email, volunteringDTO!);

        public async Task<Response<IEnumerable<RequestVolunteering>>> GetAsync(string email) => await _adoptionsRepository.GetAsync(email);

        public override async Task<Response<RequestVolunteering>> GetAsync(int id) => await _adoptionsRepository.GetAsync(id);


    }
}
