using Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Languages;

namespace Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly DB_Context _dB_Context;
        private readonly IConfiguration _configuration;

        public LanguageService(DB_Context dB_Context, IConfiguration configuration)
        {
            _dB_Context = dB_Context;
            _configuration = configuration;
        }

        public async Task<ApiResult<List<LanguagesViewModel>>> GetAll()
        {
            var languages = await _dB_Context.Languges.Select(d => new LanguagesViewModel()
            {
                Id = d.Id,
                Name = d.Name
            }).ToListAsync();
            return new ApiSuccessResult<List<LanguagesViewModel>>(languages);
        }
    }
}