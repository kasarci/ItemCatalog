using AutoMapper;
using ItemCatalog.API.Dtos;
using ItemCatalog.API.Entities;

namespace ItemCatalog.API.Mapping;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemDto>().ReverseMap();
        CreateMap<CreateItemDto, Item>()
        .ForMember(i => i.Id, 
                    opt =>
                    opt.MapFrom(d => Guid.NewGuid()))
        .ForMember(i => i.CreatedDate,
                    opt =>
                    opt.MapFrom(x => DateTimeOffset.UtcNow))
        .ReverseMap();

        CreateMap<Item,UpdateItemDto>().ReverseMap();
    }
}