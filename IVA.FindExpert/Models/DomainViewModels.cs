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
    public int ClientType { get; set; }
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

    public string BuyerName { get; set; }
    public string BuyerMobile { get; set; }
    public string BuyerPhone { get; set; }
    public bool IsAllowPhone { get; set; }
    public string Location { get; set; }

    public string TimeToExpire { get; set; }
    public bool IsFollowUp { get; set; }
    public int? ClientType { get; set; }

    public List<RequestQuotationViewModel> QuotationList { get; set; }
    public List<VehicleImage> Images { get; set; }
}

public class RequestQuotationViewModel
{
    public long Id { get; set; }
    public long ServiceRequestId { get; set; }
    public long QuotationTemplateId { get; set; }
    public string QuotationText { get; set; }
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
    public long ThreadId { get; set; }
    public bool IsExpired { get; set; }

    public string ServiceRequestCode { get; set; }
    public int ClaimType { get; set; }
    public string VehicleNo { get; set; }
    public decimal VehicleValue { get; set; }
}

public class CompanyModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}

public class UserFeedbackModel
{
    public long Id { get; set; }
    public int Rating { get; set; }
    public string Description { get; set; }    
    public long UserId { get; set; }
}

public class MessageThreadModel
{
    public long Id { get; set; }
    public long RequestId { get; set; }
    public long BuyerId { get; set; }
    public string BuyerName { get; set; }
    public long AgentId { get; set; }
    public string AgentName { get; set; }
    public string CompanyName { get; set; }
    public string VehicleNo { get; set; }
    public string Description { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public long CreatedBy { get; set; }
    public bool HasQuotation { get; set; }
    public int RequestStatus { get; set; }
    public int UnreadMessageCount { get; set; }

    public List<MessageModel> Messages { get; set; }
    public PromotionModel Promotion { get; set; }
}

public class MessageModel
{
    public long Id { get; set; }
    public long ThreadId { get; set; }
    public long SenderId { get; set; }
    public long RecieverId { get; set; }
    public long RequestId { get; set; }
    public string MessageText { get; set; }
    public int Status { get; set; }
    public string Time { get; set; }
    public long QuotationId { get; set; }
    public string SenderName { get; set; }
}

public class QuotationTemplateModel
{
    public long Id { get; set; }
    public int ValidityId { get; set; }
    public int CompanyId { get; set; }
    public string ValidityName { get; set; }
    public string Name { get; set; }
    public string Body { get; set; }
    public decimal Amount { get; set; }
    public DateTime? CreatedDate { get; set; }
    public long CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public long ModifiedBy { get; set; }
}

public class PromotionModel
{
    public long Id { get; set; }
    public int InsuranceType { get; set; }
    public string Title { get; set; }
    public string Header { get; set; }
    public string Description { get; set; }
    public string CreatedDate { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }
}

public class ServiceLocation
{
    public string Category { get; set; }
    public string ParentCategory { get; set; }
    public string Company { get; set; }
    public double Distance { get; set; }
    public string DistanceKM { get; set; }
    public string Location { get; set; }
    public string Address { get; set; }
    public string Mobile { get; set; }
    public string Phone { get; set; }
}

public class UserDeviceModel
{
    public long UserId { get; set; }
    public string DeviceToken { get; set; }
    public string DeviceId { get; set; }
}

public class NotificationListViewModel
{
    public List<long> Ids { get; set; }
}
