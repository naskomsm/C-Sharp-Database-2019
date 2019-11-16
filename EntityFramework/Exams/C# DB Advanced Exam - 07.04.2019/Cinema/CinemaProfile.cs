namespace Cinema
{
    using AutoMapper;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;

    public class CinemaProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public CinemaProfile()
        {
            this.CreateMap<MovieImportDTO, Movie>();
            this.CreateMap<HallImportDTO, Hall>();
        }
    }
}
