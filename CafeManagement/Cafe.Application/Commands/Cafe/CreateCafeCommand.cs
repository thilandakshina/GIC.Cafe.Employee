﻿using MediatR;

namespace Cafe.Application.Commands.Cafe
{
    public class CreateCafeCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
    }

}
