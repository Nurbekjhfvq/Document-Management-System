using AutoMapper;
using Document.Dto;
using Document.Entity;


namespace Document.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Document.Entity.Document, DocumentDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<DocumentDto, Entity.Document>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Archive, ArchiveDto>()
                .ForMember(dest => dest.ArchivedAt, opt => opt.MapFrom(src => src.ArchivedAt!.Value));

            CreateMap<ArchiveDto, Archive>()
                .ForMember(dest => dest.ArchivedAt, opt => opt.MapFrom(src => src.ArchivedAt));


            CreateMap<CreateDocumentDto, Entity.Document>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Entity.Version, VersionDto>();
            CreateMap<CreateVersionDto, Entity.Version>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.FileContent.Length));
        }
    }
}
