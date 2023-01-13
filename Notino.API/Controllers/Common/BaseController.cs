using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Notino.API.Controllers.Common
{
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {            
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
    }
}
