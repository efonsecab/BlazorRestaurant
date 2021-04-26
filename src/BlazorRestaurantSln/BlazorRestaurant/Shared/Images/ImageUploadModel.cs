using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Shared.Images
{
    public class ImageUploadModel
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        public byte[] ImageFileBytes { get; set; }
        public string FileExtension { get; set; }
    }
}
