using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO
{
    public class UserProfile : IUserProfile
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public byte[] Image { get; set; }
        public string Location { get; set; }
        public string LocationLongitude { get; set; }
        public string LocationLatitude { get; set; }
        public int? ContactMethod { get; set; }
        public int? BankId { get; set; }
        public string AccountName { get; set; }
        public string BankBranch { get; set; }
        public string AccountNo { get; set; }
        public int? NotificationFrequencyMinutes { get; set; }        
    }
}
