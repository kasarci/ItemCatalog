using AutoMapper;
using ItemCatalog.API.Models.Dtos;
using ItemCatalog.API.Models.Dtos.UserDtos.Create;
using ItemCatalog.API.Models.Dtos.UserDtos.Remove;
using ItemCatalog.API.Models.Entities;

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