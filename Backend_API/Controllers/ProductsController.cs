﻿using Application.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ViewModels.Catalog.ProductImages;
using ViewModels.Catalog.Products;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;

        public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }

        #region phân trang

        //get all by categoryId
        // http://localhost:port/product? pageIndex= , pageSize= , categoryId=
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllPaging(string langugeId, [FromQuery] GetPublicProductPagingRequest request)//lay du lieu, tham so tu cau query ra
        {
            var product = await _publicProductService.GetAllByCategoryId(langugeId, request);
            return Ok(product);
        }

        #endregion phân trang

        //trong thawngf có Create_Product, Update_Product, UpdatePrice, UpdateStock, Delete_Product, AddViewCount, GetAllPaging,
        //AddImage, RemoveImage, UpdateImage, GetImageById, GetListImages

        //select= Get
        //create= Post
        //update= Put
        //remove,delete= Delete
        //patch
        /*.....................................................................................................*/

        #region product

        // http://localhost:port/product/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _manageProductService.GetById(productId, languageId);
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

            var product = _manageProductService.GetById(productId, request.LanguageId);
            //muốn trả về 201 thì trả về object
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
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
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete_Product(int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affected_result = await _manageProductService.Delete_Product(productId);
            if (affected_result == 0) return BadRequest();//trả về 400:lỗi
            return Ok(); //trả về 200:ok
        }

        #endregion product

        #region price

        //Update_Price
        // http://localhost:port/product
        [HttpPatch("{productId}/{newPrice}")]//update 1 phần thì dùng HttpPatch
        public async Task<IActionResult> Update_Price(int productId, decimal newPrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isSuccessful = await _manageProductService.Update_Price(productId, newPrice);
            if (isSuccessful)
                return Ok();

            return BadRequest();
        }

        #endregion price

        #region image

        //image
        [HttpGet("Lấy id ảnh")]
        public async Task<IActionResult> GetImageById(int imageId)
        {
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
            var imageId = await _manageProductService.Add_Image(productId, request);
            if (imageId == 0) return BadRequest();//trả về 400:lỗi
            //return Ok(result); //trả về 200:ok

            var image = _manageProductService.GetImageById(imageId);
            //muốn trả về 201 thì trả về object
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var image = await _manageProductService.Remove_Image(imageId);
            if (image == 0) return BadRequest();//trả về 400:lỗi
            return Ok(); //trả về 200:ok
        }

        [HttpPut("{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var image = await _manageProductService.Update_Image(imageId, request);
            if (image == 0) return BadRequest();//trả về 400:lỗi
            return Ok(); //trả về 200:ok
        }

        #endregion image
    }
}