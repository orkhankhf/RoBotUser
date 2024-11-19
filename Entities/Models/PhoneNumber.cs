using Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class PhoneNumber : BaseEntity
    {
        [StringLength(10)]
        public string FormattedNumber { get; set; }
        public decimal PriceOfProduct { get; set; }
        public CurrencyEnum Currency { get; set; }
        public CityEnum City { get; set; }
        public bool HasMessageSent { get; set; }
    }
}
