namespace CarDealer
{
    using AutoMapper;
    using CarDealer.DTO;
    using CarDealer.Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<CarImportDTO, Car>();
        }
    }
}
