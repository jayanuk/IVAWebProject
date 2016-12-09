using IVA.DTO;
using System;
using System.Collections.Generic;

public class PhoneValidateRequest
{
    public string Name { get; set; }
    public string Phone { get; set; }
}

public class LoginRequest
{
    public string Phone { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int UserType { get; set; }
}

public class UserModel
{
    public long Id { get; set; }
    public long LoginId { get; set; }
    public int UserType { get; set; }
    public String Name { get; set; }
    public String UserName { get; set; }
    public String Password { get; set; }
    public bool PasswordValidated { get; set; }
    public String Token { get; set; }
    public String ConnectionId { get; set; }    
    public long CompanyId { get; set; }

    public UserProfileModel UserProfile { get; set; }
}

public class UserProfileModel
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Gender { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Mobile { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public byte[] Image { get; set; }
    public string Location { get; set; }
    public string LocationLongitude { get; set; }
    public string LocationLatitude { get; set; }
    public int ContactMethod { get; set; }
    public int? BankId { get; set; }
    public string AccountName { get; set; }
    public string BankBranch { get; set; }
    public string AccountNo { get; set; }
    public int NotificationFrequencyMinutes { get; set; }
}

public class ServiceRequestModel
{
    public long Id { get; set; }
    public int InsuranceTypeId { get; set; }

    public string Code { get; set; }    
    public long UserId { get; set; }
    public string CreatedDate { get; set; }

    public int ClaimType { get; set; }
    public int UsageType { get; set; }
    public int RegistrationCategory { get; set; }

    public decimal VehicleValue { get; set; }
    public string VehicleNo { get; set; }
    public int VehicleYear { get; set; }
    public bool IsFinanced { get; set; }
    public int Status { get; set; }
    public string ExpiryDate { get; set; }

    public List<RequestQuotationViewModel> QuotationList { get; set; }
    public List<VehicleImage> Images { get; set; }
}

public class RequestQuotationViewModel
{
    public long Id { get; set; }
    public long ServiceRequestId { get; set; }
    public long QuotationTemplateId { get; set; }
    public string Premimum { get; set; }
    public string Cover { get; set; }
    public long AgentId { get; set; }
    public string AgentName { get; set; }
    public string AgentContact { get; set; }
    public int CompanyId { get; set; }
    public int Status { get; set; }
    public string CompanyName { get; set; }
    public string QuotationTemplateName { get; set; }
    public string CreatedTime { get; set; }
    public string ModifiedTime { get; set; }
}

public class CompanyModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}