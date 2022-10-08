using AutoMapper;
using ItemCatalog.API.Dtos;
using ItemCatalog.API.Entities;

namespace ItemCatalog.API.Mapping;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemDto>();
    }
}