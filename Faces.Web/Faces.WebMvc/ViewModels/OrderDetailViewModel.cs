using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faces.WebMvc.ViewModels
{
    public class OrderDetailViewModel
    {
        public int OrderDetailsId { get; set; }
        public byte[] FaceData { get; set; }
        public string ImageString { get; set; }
    }
}
