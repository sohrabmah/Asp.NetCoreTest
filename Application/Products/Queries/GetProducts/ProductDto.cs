using AutoMapper;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Products.Queries.GetProducts;

public class ProductDto : IMapFrom<Product>
{
    public int Id { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
    public string Name { get; set; }
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductDto>();
    }
}
