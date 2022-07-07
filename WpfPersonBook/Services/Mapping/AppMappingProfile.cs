using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPersonBook.Models;
using Splat;
using AutoMapper.Configuration;

namespace WpfPersonBook.Services.Mapping
{
    public class AppMappingProfile
    {
        public static Mapper ConfigureMapping()
        {
            var cfg = new MapperConfigurationExpression();
            ConfigureModelMapping(cfg);
            var mapperConfiguration = new MapperConfiguration(cfg);
            mapperConfiguration.AssertConfigurationIsValid();
            mapperConfiguration.CompileMappings();
            var mapper = new Mapper(mapperConfiguration);
            return mapper;
        }

        private static void ConfigureModelMapping(MapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Contact, WpfContact>()
                .ForMember(m => m.Avatar, e => e.MapFrom(c => c.Avatar.ConvertToBitmapimage()));
            cfg.CreateMap<WpfContact, WpfContact>();
        }
    }
}
