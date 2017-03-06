using IVA.DTO.Contract;
using System.ComponentModel.DataAnnotations;

namespace IVA.DTO
{
    public class CustomerSupportType : ICustomerSupportType
    {
        [Key]
        public int Id { get; set; }
        public string SupportType { get; set; }
        public bool IsActive { get; set; }
    }
}
