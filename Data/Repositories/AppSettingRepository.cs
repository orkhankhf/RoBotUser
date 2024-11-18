using Data.Context;
using Data.Interfaces;
using Entities.Models;

namespace Data.Repositories
{
    public class AppSettingRepository : GenericRepository<AppSetting>, IAppSettingRepository
    {
        private readonly AppDbContext _context;

        public AppSettingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
