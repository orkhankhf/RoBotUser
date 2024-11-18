using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPhoneNumberRepository : IGenericRepository<PhoneNumber>
    {
        Task<IEnumerable<PhoneNumber>> GetRecentPhoneNumbersAsync(int hours);
    }
}
