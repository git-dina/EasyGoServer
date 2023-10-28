using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS_Server.Models
{
    public class CountryModel
    {
        public int CountryId { get; set; }
        public string Code { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public byte IsDefault { get; set; }
        public int CurrencyId { get; set; }
        public string TimeZoneName { get; set; }
        public string TimeZoneOffset { get; set; }
    }
}