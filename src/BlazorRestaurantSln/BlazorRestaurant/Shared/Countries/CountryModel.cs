using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Countries
{
    public class CountryModel
    {
        public long CountryId { get; set; }
        public string Name { get; set; }
        public string Isocode { get; set; }
    }
}
