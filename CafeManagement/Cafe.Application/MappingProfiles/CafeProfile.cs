using AutoMapper;
using Cafe.Application.Commands.Cafe;
using Cafe.Application.DTOs;
using Cafe.Domain.Entities;

namespace Cafe.Application.MappingProfiles
{
    public class CafeProfile : Profile
    {
        public CafeProfile()
        {
            CreateMap<CafeEntity, CafeDto>();
            CreateMap<CreateCafeCommand, CafeEntity>();
            CreateMap<UpdateCafeCommand, CafeEntity>();
        }
    }
}
