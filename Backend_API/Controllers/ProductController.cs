using Application.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ViewModels.Catalog.Products;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }

        //get product
        // http://localhost:port/product
        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get( string languageId)
        {
            var product = await _publicProductService.GetAll(languageId);
            return Ok(product);
        }
        //get all by categoryId
        // http://localhost:port/product/public-paging
        [HttpGet("public-paging/{languageId}")]
        public async Task<IActionResult> Get([FromQuery] GetPublicProductPagingRequest request)//lay du lieu, tham so tu cau query ra
        {
            var product = await _publicProductService.GetAllByCategoryId(request);
            return Ok(product);
        }

        //trong thawngf có Create_Product, Update_Product, UpdatePrice, UpdateStock, Delete_Product, AddViewCount, GetAllPaging,
        //AddImage, RemoveImage, UpdateImage, GetImageById, GetListImages

        //select= Get
        //create= Post
        //update= Put
        //remove,delete= Delete
        //patch
        /*.....................................................................................................*/

        //get by id
        // http://localhost:port/product/1
        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById(int id,string languageId)
        {
            var product = await _manageProductService.GetById(id,languageId);
            if (product == null) return BadRequest("Cannot find Id product");//400
            return Ok(product);//200
        }

        //Create_Product
        // http://localhost:port/product
        [HttpPost]
        public async Task<IActionResult> Create_Product([FromForm] CreateProduct_DTO request)
        {
            var productId = await _manageProductService.Create_Product(request);
            if (productId == 0) return BadRequest();//trả về 400:lỗi
            //return Ok(result); //trả về 200:ok

            var product = _manageProductService.GetById(productId,request.LanguageId);
            //muốn trả về 201 thì trả về object
            return CreatedAtAction(nameof(GetById),new { id=productId}, product);
        }
        //Update_Product
        // http://localhost:port/product
        [HttpPut]
        public async Task<IActionResult> Update_Product([FromForm] UpdateProduct_DTO request)
        {
            var affected_result = await _manageProductService.Update_Product(request);
            if (affected_result == 0) return BadRequest();//trả về 400:lỗi
            return Ok(); //trả về 200:ok
        
        }
        //Delete_Product
        // http://localhost:port/product
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete_Product(int id)
        {
            var affected_result = await _manageProductService.Delete_Product(id);
            if (affected_result == 0) return BadRequest();//trả về 400:lỗi
            return Ok(); //trả về 200:ok
        }

        //Update_Price
        // http://localhost:port/product
        [HttpPut("Cập nhật giá sản phẩm")]
        public async Task<IActionResult> Update_Price(int id, decimal newPrice)
        {
            var isSuccessful = await _manageProductService.Update_Price(id, newPrice);
            if (isSuccessful)
                return Ok();

            return BadRequest();
        }

    }
        

}
