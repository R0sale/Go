using Entities.Contracts;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application
{
    public class PdfService
    {
        private readonly ChromePdfRenderer _renderer;
        private readonly RazorRender _razorRender;
        private readonly IUserService _userService;
        private readonly IRpcClient _rpcClient;

        public PdfService(RazorRender razorRender, IUserService userService, IRpcClient rpcClient) 
        { 
            _renderer = new ChromePdfRenderer();
            _razorRender = razorRender;
            _userService = userService;
            _rpcClient = rpcClient;
        }

        public async Task<PdfDocument> CreateUserPagePdfAsync(string uid)
        {
            var user = await _userService.GetUserByUidAsync(uid);

            var userHtml = await _razorRender.RenderAsync("UserPage", user);

            var userPdf = _renderer.RenderHtmlAsPdf(userHtml);

            return userPdf;
        }

        public async Task<PdfDocument> CreateTransportPagePdfAsync(string uid)
        {
            var transportJson = await _rpcClient.CallAsync("transport_rpc", uid);

            var transportModel = JsonSerializer.Deserialize<IEnumerable<TransportDto>>(transportJson);

            var transportHtml = await _razorRender.RenderAsync("TransportPage", transportModel);

            var transportPdf = _renderer.RenderHtmlAsPdf(transportHtml);

            return transportPdf;
        }

        public async Task<PdfDocument> CreateFacilitiesPagePdfAsync(string uid)
        {
            var facilitiesJson = await _rpcClient.CallAsync("facility_rpc", uid);

            var facilitiesModel = JsonSerializer.Deserialize<IEnumerable<FacilityDto>>(facilitiesJson);

            var facilitiesHtml = await _razorRender.RenderAsync("FacilityPage", facilitiesModel);

            var facilitiesPdf = _renderer.RenderHtmlAsPdf(facilitiesHtml);

            return facilitiesPdf;
        }

        public async Task<PdfDocument> CreateFullPdfPageAsync(string uid)
        {
            var usersPdf = await CreateUserPagePdfAsync(uid);
            var transportPdf = await CreateTransportPagePdfAsync(uid);
            var facilitiesPdf = await CreateFacilitiesPagePdfAsync(uid);

            var fullPdf = PdfDocument.Merge(new[] { usersPdf, facilitiesPdf, transportPdf });

            return fullPdf;
        }
    }
}
