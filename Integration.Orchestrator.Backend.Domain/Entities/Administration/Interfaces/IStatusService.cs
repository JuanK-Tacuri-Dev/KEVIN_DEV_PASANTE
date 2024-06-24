﻿using Integration.Orchestrator.Backend.Domain.Models;
using System.Threading.Tasks;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface IStatusService<T>
    {
        Task InsertAsync(T connection);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
