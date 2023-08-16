using AutoMapper;
using Core.SharedKernel.DTO;
using UserData.Domain.Entities;

namespace UserData.Application;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDTO>();
    }
}
