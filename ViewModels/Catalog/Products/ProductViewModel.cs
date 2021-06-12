using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Catalog.Products
{
    public class ProductViewModel
    {
        //hiển thị cái chi

        //lấy tt product +
        public int Id { set; get; }

        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }

        // tt của productTraslation
        public string Name { set; get; }

        public string Description { set; get; }
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }
        public List<string> Categories { get; set; } = new List<string>();
        public bool? IsFeatured { get; set; }
        public string ThumbnailImage { get; set; }
    }
}