using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.User
{
    public class UserModel
    {
        [Required]
        public long ApplicationUserId { get; set; }
        [Required]
        [StringLength(150)]
        public string FullName { get; set; }
        [Required]
        [StringLength(150)]
        public string EmailAddress { get; set; }
        [Required]
        public DateTimeOffset LastLogIn { get; set; }
        [Required]
        public Guid AzureAdB2cobjectId { get; set; }
    }
}
