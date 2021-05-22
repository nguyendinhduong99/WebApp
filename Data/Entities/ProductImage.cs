using LongViet_Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public DateTime DateCreated { get; set; }
        public int SortOrder { get; set; } //thứ  tự sắp xếp
        public long FileSize { get; set; }
        //do bảng này liên kết  khóa ngoại product, nên
        public Product Product { get; set; }
    }
}
