using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Catalog.Categories;
using ViewModels.Utilities.Slide;

namespace Admin_APP.Services.Slide
{
    public interface ISlideApiClient
    {
        Task<List<SlideViewModel>> GetAll();
    }
}