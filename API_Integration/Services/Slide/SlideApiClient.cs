using Admin_APP.Services;
using Admin_APP.Services.Categories;
using Admin_APP.Services.Slide;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ViewModels.Utilities.Slide;

namespace API_Integration.Services.Slide
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {
        public SlideApiClient(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<List<SlideViewModel>> GetAll()
        {
            return await GetListAsync<SlideViewModel>("/api/slides");
        }
    }
}