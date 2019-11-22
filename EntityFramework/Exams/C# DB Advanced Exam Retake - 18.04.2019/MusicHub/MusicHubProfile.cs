namespace MusicHub
{
    using AutoMapper;
    using MusicHub.Data.Models;
    using MusicHub.DataProcessor.ImportDtos;
    using System;
    using System.Globalization;

    public class MusicHubProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public MusicHubProfile()
        {
            // json
            this.CreateMap<WriterImportDTO, Writer>();
            this.CreateMap<ProducerImportDTO, Producer>();
            this.CreateMap<AlbumImportDTO, Album>();

            // xml
            this.CreateMap<SongImportDTO, Song>()
                .ForMember(x => x.Duration, y => y.MapFrom(s => TimeSpan.ParseExact(s.Duration, @"hh\:mm\:ss", CultureInfo.InvariantCulture)))
                .ForMember(x => x.CreatedOn, y => y.MapFrom(s => DateTime.ParseExact(s.CreatedOn, @"dd/MM/yyyy", CultureInfo.InvariantCulture)));

            this.CreateMap<PerformerImputDTO, Performer>();
        }
    }
}
