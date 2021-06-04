using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ViewModels.Catalog.Products
{
    public class CreateProduct_DTO
    {
        //gộp 2 tp product và productTranslation trong Entities
        [Required(ErrorMessage = "Chưa nhập Giá sản phẩm")]
        public decimal Price { set; get; }

        [Required(ErrorMessage = "Chưa nhập Giá gốc")]
        public decimal OriginalPrice { get; set; }

        public int Stock { get; set; }

        [Required(ErrorMessage = "Chưa nhập Tên sản phẩm")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Details { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }
        public IFormFile ThumbnailImage { get; set; }
    }
}