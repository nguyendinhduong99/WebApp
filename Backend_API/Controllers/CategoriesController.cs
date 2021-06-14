using Application.Catalog.Categoties;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var product = await _categoriesService.GetAll(languageId);
            return Ok(product);
        }

        [HttpGet("{languageId}/{id}")]
        public async Task<IActionResult> GetById(string languageId, int id)
        {
            var product = await _categoriesService.GetById(languageId, id);
            return Ok(product);
        }
    }
}