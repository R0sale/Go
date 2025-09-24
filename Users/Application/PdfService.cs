using Entities.Contracts;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class PdfService
    {
        private readonly ChromePdfRenderer _renderer;
        private readonly RazorRender _razorRender;
        private readonly IUserService _userService;

        public PdfService(RazorRender razorRender, IUserService userService) 
        { 
            _renderer = new ChromePdfRenderer();
            _razorRender = razorRender;
            _userService = userService;
        }

        public async Task<PdfDocument> CreateUserPagePdfAsync(string uid)
        {
            var user = await _userService.GetUserByUidAsync(uid);

            var html = await _razorRender.RenderAsync("UserPage", user);

            var pdf = _renderer.RenderHtmlAsPdf(html);

            return pdf;
        }
    }
}
