﻿using AutoMapper;
using ServerUpload7.Web.Dto;
using DataVersion = ServerUpload7.DAL.Entities.Version;
using DataMaterial = ServerUpload7.DAL.Entities.Material;
using Material = ServerUpload7.BLL.BusinessModels.Material;
using Version = ServerUpload7.BLL.BusinessModels.Version;

namespace ServerUpload7.WEB.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Material, MaterialDto>()
            .ForMember("NumberOfVersions", opt => opt.MapFrom(src => src.Versions.Count));
            CreateMap<Version, VersionDto>();
            CreateMap<DataMaterial, Material>();
            CreateMap<Material, DataMaterial>();
            CreateMap<Version, DataVersion>()
                .ForMember("StrHash", opt => opt.MapFrom(src => src.HashString));
            CreateMap<DataVersion, Version>()
                .ForMember("HashString", opt => opt.MapFrom(src => src.StrHash));
        }
    }
}
