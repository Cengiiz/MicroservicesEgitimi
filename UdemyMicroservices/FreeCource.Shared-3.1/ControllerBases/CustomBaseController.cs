using FreeCource.Shared_3._1.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCource.Shared_3._1.ControllerBases
{
    public class CustomBaseController:ControllerBase
    {
        public IActionResult CreateActionResultInsance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode=response.StatusCode,
            };
        }
    }
}
