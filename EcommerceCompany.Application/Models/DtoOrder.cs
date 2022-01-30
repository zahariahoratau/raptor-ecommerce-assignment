namespace EcommerceCompany.Application.Models;

public record DtoOrder
{
    public double Amount { get; init; }
    public string? Description { get; init; }

    public int CustomerId { get; init; }
}
