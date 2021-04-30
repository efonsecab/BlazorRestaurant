using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Errors
{
    public class ErrorLogModel
    {
        public long ErrorLogId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string StackTrace { get; set; }
        [Required]
        public string FullException { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset RowCreationDateTime { get; set; }
        [Required]
        [StringLength(256)]
        public string RowCreationUser { get; set; }
        [Required]
        [StringLength(250)]
        public string SourceApplication { get; set; }
        [Required]
        [StringLength(100)]
        public string OriginatorIpaddress { get; set; }
    }
}
