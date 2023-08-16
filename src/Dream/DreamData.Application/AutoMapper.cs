using AutoMapper;
using Core.SharedKernel.DTO;
using DreamData.Domain.Entities;

namespace DreamData.Application;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Dream, DreamDTO>();
    }
}
