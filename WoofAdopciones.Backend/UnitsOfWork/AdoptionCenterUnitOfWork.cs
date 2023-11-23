using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.UnitsOfWork
{
    public class AdoptionCenterUnitOfWork : GenericUnitOfWork<AdoptionCenter>, IAdoptionCenterUnitOfWork
    {
        private readonly IAdoptionCenterRepository _adoptionCenterRepository;

        public AdoptionCenterUnitOfWork(IGenericRepository<AdoptionCenter> repository, IAdoptionCenterRepository categoriesRepository) : base(repository)
        {
            _adoptionCenterRepository = categoriesRepository;
        }

        public virtual async Task<Response<AdoptionCenter>> AddAsync(AdoptionCenter model) => await _adoptionCenterRepository.AddAsync(model);
        public override async Task<Response<IEnumerable<AdoptionCenter>>> GetAsync(PaginationDTO pagination) => await _adoptionCenterRepository.GetAsync(pagination);

        public async Task<IEnumerable<AdoptionCenter>> GetComboAsync() => await _adoptionCenterRepository.GetComboAsync();

        public override async Task<Response<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _adoptionCenterRepository.GetTotalPagesAsync(pagination);
    }
}
