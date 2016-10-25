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
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Token { get; set; }
    public string Phone { get; set; }
    public long UserProfileId { get; set; }
}

public class ServiceRequestModel
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public long UserId { get; set; }
    //public string TimeOccured { get; set; }

    public int ClaimType { get; set; }
    public int UsageType { get; set; }
    public int RegistrationCategory { get; set; }

    public decimal Vehiclevalue { get; set; }
    public string VehicleNo { get; set; }
    public int Status { get; set; }

    public List<VehicleImage> Images { get; set; }
}

public class CompanyModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}