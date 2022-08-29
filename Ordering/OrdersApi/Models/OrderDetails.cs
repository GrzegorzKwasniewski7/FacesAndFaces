using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersApi.Models
{
    public class OrderDetails
    {        
        [Key]
        public int OrderDetailsId { get; set; }        
        public Guid OrderId { get; set; }        
        public byte[] FaceData { get; set; }
    }
}
