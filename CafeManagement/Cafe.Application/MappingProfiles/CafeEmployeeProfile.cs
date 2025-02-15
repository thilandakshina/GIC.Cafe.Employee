using AutoMapper;
using Cafe.Application.DTOs;
using Cafe.Domain.Entities;

namespace Cafe.Application.MappingProfiles
{
    public class CafeEmployeeProfile : Profile
    {
        public CafeEmployeeProfile()
        {
            CreateMap<CafeEmployeeEntity, CafeEmployeeDto>();

        }
    }
}


