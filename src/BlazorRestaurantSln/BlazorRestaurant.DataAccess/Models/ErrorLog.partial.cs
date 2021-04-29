using BlazorRestaurant.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.DataAccess.Models
{
    public partial class ErrorLog: IOriginatorInfo
    {
        public string SourceApplication { get; set; }
        public string OriginatorIpaddress { get; set; }
        public DateTimeOffset RowCreationDateTime { get; set; }
        public string RowCreationUser { get; set; }
    }
}
