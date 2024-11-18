using Data.Context;
using Data.Interfaces;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PhoneNumberRepository : GenericRepository<PhoneNumber>, IPhoneNumberRepository
    {
        private readonly AppDbContext _context;

        public PhoneNumberRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhoneNumber>> GetRecentPhoneNumbersAsync(int hours)
        {
            DateTime cutoffTime = DateTime.UtcNow.AddHours(-hours);
            return await _context.Set<PhoneNumber>()
                .Where(p => p.DateCreated >= cutoffTime)
                .ToListAsync();
        }
    }
}
