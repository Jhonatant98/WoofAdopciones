using WoofAdopciones.Backend.Repositories;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.UnitsOfWork
{
    public class OrderTypeUnitOfWork : GenericUnitOfWork<OrderType>, IOrderTypeUnitOfWork
    {
        private readonly IOrderTypeRepository _orderTypeRepository;

        public OrderTypeUnitOfWork(IGenericRepository<OrderType> repository, IOrderTypeRepository categoriesRepository) : base(repository)
        {
            _orderTypeRepository = categoriesRepository;
        }

        public override async Task<Response<IEnumerable<OrderType>>> GetAsync(PaginationDTO pagination) => await _orderTypeRepository.GetAsync(pagination);

        public async Task<IEnumerable<OrderType>> GetComboAsync() => await _orderTypeRepository.GetComboAsync();

        public override async Task<Response<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _orderTypeRepository.GetTotalPagesAsync(pagination);
    }
}