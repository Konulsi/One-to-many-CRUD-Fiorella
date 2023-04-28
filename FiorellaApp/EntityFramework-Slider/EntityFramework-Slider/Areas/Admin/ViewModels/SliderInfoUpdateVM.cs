using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework_Slider.Areas.Admin.ViewModels
{
    public class SliderInfoUpdateVM  //viewmodele chixartmaqda meqsed edit eden zaman Image i bow qoya bilmek uchun. Modelin ozunde create etmeye gore required qoymuwuqki create eden zaman bow qoymaq olmasin, hemin modelin ozu update zaman iwimize yaramir deye Viewmodele chiardiriq
    {
        [Required(ErrorMessage = "Don`t be empty")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Don`t be empty")]
        public string Description { get; set; }

        public string SignatureImage { get; set; }
        public IFormFile Photo { get; set; }
    }
}
