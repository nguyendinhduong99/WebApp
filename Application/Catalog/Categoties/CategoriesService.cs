using Data.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Catalog.Categories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Categoties
{
    public class CategoriesService : ICategoriesService
    {
        private readonly DB_Context _dB_Context;

        public CategoriesService(DB_Context dB_Context)
        {
            _dB_Context = dB_Context;
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            var query = from c in _dB_Context.Category
                        join ct in _dB_Context.Category_Translations
                        on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };

            return await query.Select(d => new CategoryViewModel()
            {
                Id = d.c.Id,
                Name = d.ct.Name,
                ParentId = d.c.ParentId
            }).ToListAsync();
        }

        public async Task<CategoryViewModel> GetById(string languageId, int id)
        {
            var query = from c in _dB_Context.Category
                        join ct in _dB_Context.Category_Translations
                        on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId && c.Id == id
                        select new { c, ct };

            return await query.Select(d => new CategoryViewModel()
            {
                Id = d.c.Id,
                Name = d.ct.Name,
                ParentId = d.c.ParentId
            }).FirstOrDefaultAsync();
        }
    }
}