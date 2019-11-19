namespace ProductShop
{
    using AutoMapper;
    using ProductShop.DTOs;
    using ProductShop.Models;
    using System.Linq;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserImputDTO, User>();
            CreateMap<ProductImputDTO, Product>();
            CreateMap<CategoryImputDTO, Category>();
            CreateMap<CategoryProductImputDTO, CategoryProduct>();

            CreateMap<Product, ProductsInRangeExportDTO>()
                .ForMember(x => x.Buyer, y => y.MapFrom(s => $"{s.Buyer.FirstName} {s.Buyer.LastName}"));

            CreateMap<User, GetSoldProductsExportDTO>();

            CreateMap<Category, CategoriesByProductsExportDTO>()
                .ForMember(x => x.NumberOfProducts, y => y.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(x => x.AveragePriceProducts, y => y.MapFrom(s => s.CategoryProducts.Average(cp => cp.Product.Price)))
                .ForMember(x => x.TotalPriceSum, y => y.MapFrom(s => s.CategoryProducts.Sum(cp => cp.Product.Price)));


            CreateMap<Product, SoldProductExportDTO>();

            CreateMap<User, SoldProductsExportDTO>()
                .ForMember(x => x.Count, y => y.MapFrom(s => s.ProductsSold.Count))
                .ForMember(x => x.Products, y => y.MapFrom(s => s.ProductsSold));

            CreateMap<User, UsersAndProductsExportDTO>()
                .ForMember(x => x.ProductsSold, y => y.MapFrom(s => s.ProductsSold));

        }
    }
}
