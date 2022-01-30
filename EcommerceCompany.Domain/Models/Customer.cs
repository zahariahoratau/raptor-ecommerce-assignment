using System.Collections.Immutable;

namespace EcommerceCompany.Domain.Models;

public record Customer
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string CustomerStatus { get; init; } = CustomerType.Regular;
    public DateTime MemberSinceDateUtc { get; init; }
    public DateTime StatusLastChangedDateUtc { get; init; }
    
    public IImmutableList<Order> Orders { get; init; }
}
