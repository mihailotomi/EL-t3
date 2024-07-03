using EL_t3.Core.Entities;
using EL_t3.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Infrastructure.Database.Repository;

public class ClubRepository : BaseRepository<Club>, IClubRepository
{
    public ClubRepository(AppDatabaseContext context) : base(context)
    { }
}