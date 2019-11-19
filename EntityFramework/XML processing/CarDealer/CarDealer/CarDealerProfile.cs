namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Dtos.Import;
    using CarDealer.Models;
    using System.Linq;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierDTO, Supplier>();
            CreateMap<PartDTO, Part>();
            CreateMap<CustomerDTO, Customer>();
        }
    }
}
