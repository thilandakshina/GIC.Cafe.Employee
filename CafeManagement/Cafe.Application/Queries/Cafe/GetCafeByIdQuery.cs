using Cafe.Application.DTOs;
using MediatR;

namespace Cafe.Application.Queries.Cafe
{
    public record GetCafeByIdQuery : IRequest<CafeDto>
    {
        public Guid Id { get; init; }
    }
}
