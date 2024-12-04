using EL_t3.Application.Common.Interfaces.Repository;
using EL_t3.Domain.Entities;

namespace EL_t3.Infrastructure.Database.Repository;

public class ClubRepository : BaseRepository<Club>, IClubRepository
{
    public ClubRepository(AppDatabaseContext context) : base(context)
    { }
}