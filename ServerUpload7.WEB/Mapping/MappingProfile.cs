using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ServerUpload7.DAL.Entities;
using ServerUpload7.WEB.Resources;
using ServerUpload7.BLL.ModelDTO;
using Version = ServerUpload7.DAL.Entities.Version;

namespace ServerUpload7.WEB.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MaterialDTO, MaterialView>()
            .ForMember("Vers_num", opt => opt.MapFrom(src => src.Versions.Count));
            CreateMap<VersionDTO, VersionView>();
            CreateMap<Material, MaterialDTO>();
            CreateMap<MaterialDTO, Material>();
            CreateMap<Version, VersionDTO>();
            CreateMap<VersionDTO, Version>();
        }
    }
}
