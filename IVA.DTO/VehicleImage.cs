using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class VehicleImage : IVehicleImage
    {
        public long Id { get; set; }
        public long SRId { get; set; }
        public DateTime Date { get; set; }
        public byte[] ImageBytes { get; set; }
        public int? Order { get; set; }
    }
}
