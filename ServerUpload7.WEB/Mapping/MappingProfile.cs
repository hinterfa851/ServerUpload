using AutoMapper;
using ServerUpload.Web.Dto;
using DataVersion = ServerUpload.DAL.Entities.Version;
using DataMaterial = ServerUpload.DAL.Entities.Material;
using Material = ServerUpload.BLL.BusinessModels.Material;
using Version = ServerUpload.BLL.BusinessModels.Version;

namespace ServerUpload.Web.Mapping
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
