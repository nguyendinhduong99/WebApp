using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Languages;

namespace Admin_APP.Services.Language
{
    public class LanguageApiClient : BaseApiClient, ILanguageApiClient
    {
        public LanguageApiClient
            (IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
                IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        #region Lấy ra tất cả các quyền

        public async Task<ApiResult<List<LanguagesViewModel>>> GetAll()
        {
            return await GetAsync<ApiResult<List<LanguagesViewModel>>>("/api/languages");
        }

        #endregion Lấy ra tất cả các quyền
    }
}