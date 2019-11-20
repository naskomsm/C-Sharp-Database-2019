namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Dtos.Export;
    using CarDealer.Dtos.Import;
    using CarDealer.Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //import
            CreateMap<SupplierDTO, Supplier>();
            CreateMap<PartDTO, Part>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<CarDTO, Car>();
            
            //export
            CreateMap<Car, CarWithDistanceExportDTO>();

        }
    }
}
