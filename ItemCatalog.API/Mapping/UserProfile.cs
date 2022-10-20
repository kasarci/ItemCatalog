using AutoMapper;
using ItemCatalog.API.Dtos;
using ItemCatalog.API.Dtos.UserDtos.Create;
using ItemCatalog.API.Dtos.UserDtos.Remove;
using ItemCatalog.API.Entities;

namespace ItemCatalog.API.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserRequestDto, ApplicationUser>().ReverseMap();
        CreateMap<ApplicationUser, CreateUserResponseDto>().ReverseMap();

        CreateMap<RemoveUserRequestDto, ApplicationUser>().ReverseMap();
    }
}