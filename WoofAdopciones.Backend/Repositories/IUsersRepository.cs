﻿using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;
using WoofAdopciones.Shared.Responses;


namespace WoofAdopciones.Backend.Repositories
{
    public interface IUsersRepository
    {
        Task<Response<User>> GetAsync(string email);

        Task<Response<User>> GetAsync(Guid userId);

        Task<Response<IEnumerable<User>>> GetAsync(PaginationDTO pagination);

        Task<Response<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
