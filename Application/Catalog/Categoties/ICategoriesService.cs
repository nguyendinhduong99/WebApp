using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Catalog.Categories;

namespace Application.Catalog.Categoties
{
    public interface ICategoriesService
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);
    }
}