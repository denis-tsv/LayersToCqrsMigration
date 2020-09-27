using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace UseCases.Common.Queries.GetEntity
{
    public abstract class GetEntityQuery<TDto> : IRequest<TDto>
    {
        public int Id { get; set; }
    }
}
