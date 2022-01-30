using EcommerceCompany.Domain.Models;

namespace EcommerceCompany.Infrastructure.Models;

public sealed record DbCustomer
{
    public int Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public string CustomerStatus { get; init; } = CustomerType.Regular;
    public DateTime MemberSinceDateUtc { get; init; }
    public DateTime StatusLastChangedDateUtc { get; init; }
}