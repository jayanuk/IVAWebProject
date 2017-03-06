namespace IVA.DTO.Contract
{
    public interface ICustomerSupportType
    {
        int Id { get; set; }
        string SupportType { get; set; }
        bool IsActive { get; set; }
    }
}
