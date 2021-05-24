using Application.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ViewModels.Catalog.ProductImages;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _publicProductService.GetAll(languageId);
            return Ok(product);
        }
        //get all by categoryId
        // http://localhost:port/product/public-paging
        [HttpGet("public-paging/{languageId}")]
        public async Task<IActionResult> Get([FromQuery] GetPublicProductPagingRequest request)//lay du lieu, tham so tu cau query ra
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _manageProductService.GetById(id,languageId);
            if (product == null) return BadRequest("Cannot find Id product");//400
            return Ok(product);//200
        }

        //Create_Product
        // http://localhost:port/product
        [HttpPost]
        public async Task<IActionResult> Create_Product([FromForm] CreateProduct_DTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affected_result = await _manageProductService.Update_Product(request);
            if (affected_result == 0) return BadRequest();//trả về 400:lỗi
            return Ok(); //trả về 200:ok
        
        }
        //Delete_Product
        // http://localhost:port/product
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete_Product(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affected_result = await _manageProductService.Delete_Product(id);
            if (affected_result == 0) return BadRequest();//trả về 400:lỗi
            return Ok(); //trả về 200:ok
        }

        //Update_Price
        // http://localhost:port/product
        [HttpPut("Cập nhật giá sản phẩm")]
        public async Task<IActionResult> Update_Price(int id, decimal newPrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isSuccessful = await _manageProductService.Update_Price(id, newPrice);
            if (isSuccessful)
                return Ok();

            return BadRequest();
        }


        //image
        [HttpGet("Lấy id ảnh")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var image = await _manageProductService.GetImageById(imageId);
            if (image == null) return BadRequest("Cannot find Id product");//400
            return Ok(image);//200
        }

        [HttpPost("{Thêm ảnh Sp}")]
        public async Task<IActionResult> AddImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _manageProductService.Add_Image(productId,request);
            if (imageId == 0) return BadRequest();//trả về 400:lỗi
            //return Ok(result); //trả về 200:ok

            var image = _manageProductService.GetImageById(imageId);
            //muốn trả về 201 thì trả về object
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpDelete("{Xóa Ảnh SP}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var image= await _manageProductService.Remove_Image(imageId);
            if (image == 0) return BadRequest();//trả về 400:lỗi
            return Ok(); //trả về 200:ok
        }
        
        [HttpPut("Cập Nhật Ảnh Sp")]
        public async Task<IActionResult> UpdateImage(int imageId,[FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var image = await _manageProductService.Update_Image(imageId, request);
            if (image == 0) return BadRequest();//trả về 400:lỗi
            return Ok(); //trả về 200:ok
        }
    }
        

}
