using AutoMapper;
using Clawfoot.Status;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clawfoot.Extensions.Automapper
{
    public static class StatusExtensions
    {
        public static Status<TToResult> MapResultTo<TFromResult, TToResult>(this Status<TFromResult> status, IMapper mapper)
        {
            TToResult result = mapper.Map<TToResult>(status.Result);
            return status.To<TToResult>(result);
        }
    }
}
