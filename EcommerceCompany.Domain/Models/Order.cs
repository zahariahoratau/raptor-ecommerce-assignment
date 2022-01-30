namespace EcommerceCompany.Domain.Models;

public record Order
{
    public int Id { get; init; }
    public double Amount { get; init; }
    public string? Description { get; init; }
    public DateTime TimestampUtc { get; init; }

    public int CustomerId { get; init; }
}
