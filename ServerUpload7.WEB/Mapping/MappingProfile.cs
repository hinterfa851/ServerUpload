using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ServerUpload7.DAL.Entities;
using ServerUpload7.WEB.Resources;
using Version = ServerUpload7.DAL.Entities.Version;

namespace ServerUpload7.WEB.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Material, MaterialView>()
            .ForMember("Vers_num", opt => opt.MapFrom(src => src.Versions.Count));
            CreateMap<Version, VersionView>();
            /*
           .ForMember("Material", opt => opt.);
            CreateMap<Version, VersionView>()
           .ForMember("Material", opt => opt.MapFrom(src => mapperMat.Map<MaterialView>(src.Material)));
            */
        }
    }
}
