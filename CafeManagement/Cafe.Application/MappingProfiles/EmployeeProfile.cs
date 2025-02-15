using AutoMapper;
using Cafe.Application.Commands.Employee;
using Cafe.Application.DTOs;
using Cafe.Domain.Entities;

namespace Cafe.Application.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeEntity, EmployeeDto>()
                .ForMember(dest => dest.DaysWorked, opt => opt.Ignore())
                .ForMember(dest => dest.CafeId, opt => opt.Ignore());

            CreateMap<CreateEmployeeCommand, EmployeeEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

            CreateMap<UpdateEmployeeCommand, EmployeeEntity>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
