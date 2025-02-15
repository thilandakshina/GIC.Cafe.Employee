using Cafe.Application.DTOs;
using MediatR;

namespace Cafe.Application.Queries.Cafe
{
    public record GetCafesQuery : IRequest<IEnumerable<CafeDto>>
    {
        public string? Location { get; init; }
    }
}
