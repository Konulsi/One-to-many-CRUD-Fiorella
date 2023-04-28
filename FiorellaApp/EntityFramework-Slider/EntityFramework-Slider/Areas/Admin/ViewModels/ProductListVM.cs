using EntityFramework_Slider.Models;

namespace EntityFramework_Slider.Areas.Admin.ViewModels
{
    public class ProductListVM  // ViewModeli ona gore achiriqki product modelinin ichinde olan herbir weyi istifade etmeyeceyik deye bow yere yuklenme olmasin. chunki product modelinde include edib chagirdigimiz classlar varidi.
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public string MainImage { get; set; }
        public string CategoryName { get; set; }

    }
}
