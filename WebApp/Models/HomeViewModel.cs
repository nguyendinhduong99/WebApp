using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Catalog.Products;
using ViewModels.Utilities.Slide;

namespace WebApp.Models
{
    public class HomeViewModel
    {
        public List<SlideViewModel> Slides { get; set; }
        public List<ProductViewModel> FeaturedProduct { get; set; }
        public List<ProductViewModel> LatestProduct { get; set; }
    }
}