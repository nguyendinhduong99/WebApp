using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.User;

namespace Admin_APP.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);//Tuần tự hóa đối tượng
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DiaChiMacDinh"]);
            var response = await client.PostAsync("/api/Users/Login", httpContent);//post ra 1 cái link
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<PagedResult<UserViewModel>> GetUserPaging(GetUserPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DiaChiMacDinh"]);//địa chỉ mặc định 5001
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);
            var response = await client.GetAsync($"/api/Users/Paging?pageIndex={request.pageIndex}" +
                $"&pageSize={request.pageSize}&Keyword={request.Keyword}");//post ra 1 cái link
            var body = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<PagedResult<UserViewModel>>(body);
            return user;
        }
    }
}