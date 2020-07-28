using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace 
{
   public class RazorModel : PageModel
   {
        private readonly ILogger<RazorModel> _logger;

        public RazorModel(ILogger<RazorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}

