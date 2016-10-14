using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class Company : ICompany
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public int IsActive { get; set; }
    }
}
