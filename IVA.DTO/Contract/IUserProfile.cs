using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DTO.Contract
{
    public interface IUserProfile
    {
        long Id { get; set; }
        long UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int? Gender { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string Mobile { get; set; }
        string Street { get; set; }
        string City { get; set; }
        byte[] Image { get; set; }
        string Location { get; set; }
        string LocationLongitude { get; set; }
        string LocationLatitude { get; set; }
        int? ContactMethod { get; set; }
        int? BankId { get; set; }
        string AccountName { get; set; }
        string BankBranch { get; set; }
        string AccountNo { get; set; }        
        int? NotificationFrequencyMinutes { get; set; }
    }
}
