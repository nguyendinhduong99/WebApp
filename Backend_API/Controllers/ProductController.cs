using Application.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var product = await _publicProductService.GetAll();
            return Ok(product);
        }
        //get all by categoryId
        // http://localhost:port/product/public-paging
        [HttpGet("public-paging")]
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
        [HttpGet]
        public async Task<IActionResult> GetById(int productId)
        {
            var product = await _manageProductService.GetById(productId);
            if (product == null) return BadRequest("Cannot find Id product");//400
            return Ok(product);//200
        }

        //Create_Product
        // http://localhost:port/product
        [HttpPost]
        public async Task<IActionResult> Create_Product([FromBody] CreateProduct_DTO request)
        {
            var result = await _manageProductService.Create_Product(request);
            if (result == 0) return BadRequest();//trả về 400:lỗi
            //return Ok(result); //trả về 200:ok
            //muốn trả về 201 thì trả về object
            return Created()
        }

    }
}
