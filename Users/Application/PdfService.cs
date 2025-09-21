using Entities.Dtos;
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

        public PdfService(RazorRender razorRender) 
        { 
            _renderer = new ChromePdfRenderer();
            _razorRender = razorRender;
        }

        public async Task<PdfDocument> CreateUserPagePdfAsync(UserDto user)
        {
            var html = await _razorRender.RenderAsync("UserPage", user);

            var pdf = _renderer.RenderHtmlAsPdf(html);

            return pdf;
        }
    }
}
