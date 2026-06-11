using Microsoft.EntityFrameworkCore;
using RecAI.Domain.Entities;

namespace RecAI.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Recommendation> Recommendations { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}