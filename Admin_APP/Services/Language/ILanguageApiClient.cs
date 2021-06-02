using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Languages;

namespace Admin_APP.Services.Language
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguagesViewModel>>> GetAll();
    }
}