using EntityFramework_Slider.Areas.Admin.ViewModels;
using EntityFramework_Slider.Data;
using EntityFramework_Slider.Helpers;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //public class ProductController : Controller
    //{
    //    private readonly IProductService _productService;
    //    private readonly ICategoryService _categoryService;
    //    private readonly IWebHostEnvironment _env;
    //    private readonly AppDbContext _context;

    //    public ProductController(IProductService productService,
    //                            ICategoryService categoryService,
    //                            IWebHostEnvironment env,
    //                            AppDbContext context)
    //    {
    //        _productService = productService;
    //        _categoryService = categoryService;
    //        _env = env;
    //        _context = context;
    //    }

    //    public async Task<IActionResult> Index(int page = 1 , int take = 4) //page yeni hansi sehifededi, take yeni neche dene product gosterecek
    //    {
    //        List<Product> products = await _productService.GetPaginatedDatas(page, take); //databazada olan butun productlari gotururuk

    //        List<ProductListVM> mappeddatas = GetMappedDatas(products);   // elimizde olan databazadan goturduyumuz productlari birlewdririk viewmodele gonderirik viewa

    //        int pageCount = await GetPageCountAsync(take);

    //        Paginate<ProductListVM> paginatedDatas = new(mappeddatas, page, pageCount);
    //                                                     //mappeddata-productlarimizin listesidi(fora salib wekili falan gosterdiklerimiz)
    //        ViewBag.take = take;                        //pageCount- ne qeder page count varsa onlardi
    //                                                   //page -de hal hazirda hansi sehifedeyikse odur
    //        return View(paginatedDatas);
    //    }


    //    private async Task<int> GetPageCountAsync(int take) //sehifedeki pagelerin sayini tapmaq uchun method
    //    {
    //        var productCount = await _productService.GetCountAsync();   //productServicenin ichindeki method vasitesile productlarin sayini elde edirik

    //        return (int)Math.Ceiling((decimal)(productCount / take)); //productlarin sayini take e(yeni her sehifede neche product olacaq) boluruk ki  neche eded sehife oldugunu tapa bilek
    //        // burada metodun typena gore int-e cast edirik. math.cellingin ichinde ise neticeni decimala cast edirik. chunki math decimal tipi teleb edir.
    //        //math.celing i ona gore ist edirikki productcount u take e boldukde qaliq qalirsa yuvarlawdirsin deye
    //    }



    //    private List<ProductListVM> GetMappedDatas(List<Product> products) //bir modelin ichindekileri bir bawqa  modelin ichindekilere beraberlewdirmek datalari mapp etmek adlanir
    //    {
    //        List<ProductListVM> mappedDatas = new(); // viewmodelden instans aliriq.
    //        // var olan databazadki productlari goturub secvirmeliyik viewmodele gondermeliyik view-a
    //        foreach (var product in products)  //databazadan goturduyumuz productlari fora saliriq. ichinde bir productun propertilerini viewmodelin propertilerine beraberlewdiririk
    //        {
    //            ProductListVM productVM = new()   // list<ProductListVm> den birininin prorpertilerini beraberlewdiririk databazadan gelen productun proplarina
    //            {
    //                Id = product.Id,
    //                Name = product.Name,
    //                Description = product.Description,
    //                CategoryName = product.Category.Name,
    //                Count = product.Count,
    //                Price = product.Price,
    //                MainImage = product.Images.Where(m => m.IsMain).FirstOrDefault()?.Image
    //            };

    //            mappedDatas.Add(productVM);  //List<ProductVM e add edirik elimzde olan beraberlewdririklerimizi>
    //        }

    //        return mappedDatas;  

    //    }



    //    [HttpGet]
    //    public async Task<IActionResult> Create()
    //    {
    //        ViewBag.categories = await GetCategoriesAsync();

    //        return View();
    //    }



    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create(ProductCreateVM model)
    //    {
    //        try
    //        {
    //            ViewBag.categories = await GetCategoriesAsync();

    //            if (!ModelState.IsValid)
    //            {
    //                return View(model);
    //            }

    //            foreach (var photo in model.Photos)
    //            {
    //                if (!photo.CheckFileType("image/")) 
    //                {
    //                    ModelState.AddModelError("Photo", "File type must be image");
    //                    return View();
    //                }

    //                if (!photo.CheckFileSize(200)) 
    //                {
    //                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
    //                    return View();
    //                }
    //            }

    //            //productla productImageler oneToMany-dir.productun ichinde imageler var. ilk once imageden instans yaradib imageleri yigmaliyiq(vhunki productImage ayri tabledir),
    //            //sonra productlari yaradib, productun ichinde olan image listini birinci instan alib yaratdigimiz productImagelerimize beraberlewdireceyik...

    //            //1ci productImageleri list formasinda yaradiriq 
    //            List<ProductImage> productImages= new ();


    //            foreach (var photo in model.Photos)
    //            {
    //                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;  

    //                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);  

    //                await FileHelper.SaveFileAsync(path, photo);

    //                //productImage den instans yaradiriq
    //                ProductImage productImage = new()
    //                {
    //                    Image = fileName  //productImage-in ichindeki image i beraber edirik yaratdigimiz fileName e
    //                };  

    //                productImages.Add(productImage);  // productImages listimize add edirik productImage e(yeni liste tek bir productImage i elave edirik)
    //            }

    //            productImages.FirstOrDefault().IsMain = true; // yeni productun imagelerinden birini true etmekmuchun


    //            Product newProduct = new()  //product dan instans aliriqki yeni productda yaradaq
    //            {
    //                Name = model.Name,   //name beraber olur gelen modelin name-ne
    //                Price = model.Price,   //price beraber olur gelen modelin price-ne
    //                Count = model.Count,
    //                Description = model.Description,   //desc beraber olur gelen modelin desc-ne
    //                CategoryId = model.CategoryId,    //modelimizin ichinde olan categoryId beraber olur gelen modelin categId-ne
    //                Images = productImages,   // product modelimizde olan image list weklindedir deye for salmaq olmur. buna gore yuxarida productImageden list yaradib instans alib, ichini doldururuq. yaratdigimiz productImage listini beraber edirik productun ichinde olan Image listimize
    //            };

    //            await _context.ProductImages.AddRangeAsync(productImages); //Listi listin ichine qoymaq uchun addRange methodundan ist edirik
    //            //yaratdigimiz productImageleri(list weklinde bir neche dene ola biler) elave edirik db-da olan productImages tablesine

    //            await _context.Products.AddAsync(newProduct);
    //            //yaratdigimiz yeni productu elave edirik db-da olan products tablesinin ichine

    //            await _context.SaveChangesAsync();  


    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }


    //    }



    //    private async Task<SelectList> GetCategoriesAsync()
    //    {
    //        IEnumerable<Category> categories = await _categoryService.GetAll();

    //        return  new SelectList(categories, "Id", "Name"); // bu o demekdirki select listin ichine categoriyalari yigiriq,  id ve name i gotururuk. id gedir valuesine, name gedir selectin textine
    //        // name ne gore categorylerin id sini gotururuk

    //    }
    //}




    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public ProductController(IProductService productService,
                                 ICategoryService categoryService,
                                 IWebHostEnvironment env, AppDbContext context)
        {
            _productService = productService;
            _categoryService = categoryService;
            _env = env;
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 4) //page-hansi seyfededi hecne gondermesek Default 1-ci seyfeni goturur ama gelirse hemin reqemin seyfesin goturur,take-necedene gotursun data
        {
            List<Product> products = await _productService.GetPaginatedDatas(page, take);  //databzadaki butun produktlar

            List<ProductListVM> mappedDatas = GetMappedDatas(products);  //VM -nen gonderik datanin lazimsiz propertileri getmesin deye          

            int pageCount = await GetPageCountAsync(take); //method seyfedeki pagination sayini tapmaq ucun

            ViewBag.take = take;

            Paginate<ProductListVM> paginaDatas = new(mappedDatas, page, pageCount);//paginate- gonderirik secilmis propertili datalari=datas,
                                                                                    //page=hansi seyfedeyik,
                                                                                    //pagecountu=paginatlari siralamaq ucun fora salib

            return View(paginaDatas);

        }



        private List<ProductListVM> GetMappedDatas(List<Product> products)
        {
            List<ProductListVM> mappedDatas = new(); //bir clasi istifade etmek ucun instans almaq lazimdi
            foreach (var product in products)
            {
                ProductListVM productVM = new()   //her VM bir product(VM propertisini beraber edirik productun bize lazim olan propertilerine)
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Count = product.Count,
                    CategoryName = product.Category.Name,
                    MainImage = product.Images.Where(m => m.IsMain).FirstOrDefault()?.Image

                };
                mappedDatas.Add(productVM); //sonra VM-leri yani her producti yiqiriq Liste
            }
            return mappedDatas;
        }  //method mapped-yani beraberlesdirilmis-VM modele beraberlesdiririk databazadaki productun propertilerini.
           //bu method bize lazimdiki butun propertileri gondermemek ucun viewa ancaq lazim olanlari!

        private async Task<int> GetPageCountAsync(int take)  //method seyfedeki pagination sayini tapmaq ucun
        {
            var productCount = await _productService.GetCountAsync();  //productlari cem sayi

            return (int)Math.Ceiling((decimal)productCount / take);  //productlari cem sayi boluruk gotureceymiz product sayina(her seyfede nece olsun product sayina)
                                                                     //Math.Ceeling-istifade edirik yuvarlasdirmaq. meselen(product 5 / page 2)-bize 3 versin deye
        }                                                               //math ceeling methodu bizden decimal teleb edir deye kast edirik decimala
                                                                        //sonra return etdiyimizi yeniden kast edirik int-e return type int-di deye.




        //-----------------Product-Create-With Relation------------------
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //IEnumerable<Category> categories = await _categoryService.GetAll();  //GetAll-IEnumerable<Category> qaytarir
            //ViewBag.categories = new SelectList(categories, "Id", "Name");  //select tagi ici ucun  categorileri yiqiram,Id ve Name goturem icinden.Id-gedecek selectin valusuna,Name-selectin textine
            ViewBag.categories = await GetCategoriesAsync();
            return View();
        }



        //-----------------Product-Create-With Relation------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            try
            {
                ViewBag.categories = await GetCategoriesAsync(); //categorileri gonderirik viewa

                if (!ModelState.IsValid) //bos gelirse inputda nese  seyfeye qaytar
                {
                    return View(model);
                }
                foreach (var photo in model.Photos)     //sekil type yoxla
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();
                    }
                    //if (!photo.CheckFileSize(200))  //sekil size yoxla
                    //{
                    //    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    //    return View();
                    //}
                }

                List<ProductImage> productImages = new();  //bir productun bir nece sekli olar deye Sekiler tipinden Liste yiq
                foreach (var photo in model.Photos)  //her gelen seklilleri foreacha sal
                {

                    string fileName = Guid.NewGuid().ToString() + " " + photo.FileName;
                    string newPath = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);
                    await FileHelper.SaveFileAsync(newPath, photo); //her gelen sekli save ele projecte

                    ProductImage newProductImage = new()
                    {
                        Image = fileName  // //her sekli beraber ele databazadaki Image tablin propertisine
                    };
                    productImages.Add(newProductImage); //her productu beraber eleyende sonr propertiye yiq Liste

                }

                productImages.FirstOrDefault().IsMain = true; //yeni yuklediyimiz sekilerden biri ismaini true olsunki ekranda gorsensin
                decimal convertedPrice = decimal.Parse(model.Price);  //databazada decimaldi deye price decimal kimi gonderik.ve viewdan bize string gelir deye price, onu decimala parse edirik ve noqteni vergule deyisirik(deyismesek bize ancaq butov eded verecek qaliq yox)
                //viewmodelde type string yazdiq deye rplace edib noqteni vergule chevire bilirik.
                //noqteni vergule deyiwenden sonra hemin stringi decimala cast edirik.

                Product product = new()
                {
                    Name = model.Name, //iputdan gelir
                    Price = convertedPrice, //price inputdan gelir
                    Count = model.Count, //count inputdan gelir
                    Description = model.Description, //description inputdan gelir
                    CategoryId = model.CategoryId,  //categori Id-si gelir select-inputdan
                    Images = productImages   //ve Image Listi gelir controllerden (viewdan gebul edib Liste yiqiriq controllerde)
                };

                await _context.ProductImages.AddRangeAsync(productImages); //Listin icinde List elave edende Addrange methodun istifade edirik(Product Image Table-Listdi,butun tablar Listdi!)
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }

        }
        private async Task<SelectList> GetCategoriesAsync()
        {
            IEnumerable<Category> categories = await _categoryService.GetAll(); //submit olandada categoriler chekbox gorsensin deye burdada cagiririq
            return new SelectList(categories, "Id", "Name");
        }




        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Product product = await _productService.GetFullDataById((int)id);
            if (product == null) return NotFound();
            ViewBag.description = Regex.Replace(product.Description, "<.*?>", String.Empty);  // n=bunu yaziriqki teglernen gelen datalari viewda tegsiz gostere bilek. burada yazib viewda descriptionda viewbagnan qebul edib yazdiririq
           
            return View(product);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]   //submit edende eyni adli olanda problem edir deye action nameni qeyd edib actionun adini ferqlid yaziriq
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            Product product = await _productService.GetFullDataById((int)id);

            foreach (var item in product.Images)  // productun wekli choxdur deye fora salib her birini silirik
            {
                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", item.Image);   // path vasitesi ile roota chatiriq. yeni proyektimizde yuklenen wekli hara qoyacayiqsa ora chatmaq uchun

                FileHelper.DeleteFile(path);
            }
            _context.Products.Remove(product); //databazadan click elediyimiz idli productu silirik

            await _context.SaveChangesAsync(); 

            return RedirectToAction(nameof(Index));

        }



        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            Product dbProduct = await _productService.GetFullDataById((int)id);
            if (dbProduct == null) return NotFound();
            ViewBag.categories = await GetCategoriesAsync();

            ProductUpdateVM model = new()
            {
                Images = dbProduct.Images,
                Name = dbProduct.Name,
                Description = dbProduct.Description,
                Price = dbProduct.Price,
                Count = dbProduct.Count,
                CategoryId = dbProduct.CategoryId,

            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductUpdateVM model)
        {
            try
            {
                if (id == null) return BadRequest();
                Product dbProduct = await _productService.GetFullDataById((int)id);
                if (dbProduct == null) return NotFound();
                ViewBag.categories = await GetCategoriesAsync();


                ProductUpdateVM newProduct = new()
                {
                    Images = dbProduct.Images,
                    Name = dbProduct.Name,
                    Description = dbProduct.Description,
                    Price = dbProduct.Price,
                    Count = dbProduct.Count,
                    CategoryId = dbProduct.CategoryId,
                };

                if (!ModelState.IsValid)
                {
                    return View(newProduct);
                }

                if (model.Photos != null)
                {
                    foreach (var photo in model.Photos)
                    {
                        if (!photo.CheckFileType("image/"))
                        {
                            ModelState.AddModelError("Photo", "File type must be image");
                            return View(newProduct);
                        }
                        if (!photo.CheckFileSize(200))
                        {
                            ModelState.AddModelError("Photo", "Image size must be max 200kb");
                            return View(newProduct);
                        }
                    }

                    foreach (var item in dbProduct.Images) 
                    {
                        string path = FileHelper.GetFilePath(_env.WebRootPath, "img", item.Image);  

                        FileHelper.DeleteFile(path);
                    }


                    List<ProductImage> productImages = new();  
                    foreach (var photo in model.Photos)  
                    {

                        string fileName = Guid.NewGuid().ToString() + " " + photo.FileName;
                        string newPath = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);
                        await FileHelper.SaveFileAsync(newPath, photo); 

                        ProductImage newProductImage = new()
                        {
                            Image = fileName  
                        };
                        productImages.Add(newProductImage);
                    }
                    productImages.FirstOrDefault().IsMain = true;
                    _context.ProductImages.AddRange(productImages);
                    dbProduct.Images = productImages;

                }
                else
                {
                    Product prod = new()
                    {
                        Images = dbProduct.Images
                    };
                }

                dbProduct.Name = model.Name;
                dbProduct.Price = model.Price;
                dbProduct.Count = model.Count;
                dbProduct.Description = model.Description;
                dbProduct.CategoryId = model.CategoryId;  
                   
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
             
            }
            catch (Exception)
            {

                throw;
            }


        }






        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Product dbProduct = await _productService.GetFullDataById((int) id);
            if (dbProduct is null) return NotFound();
            ViewBag.description = Regex.Replace(dbProduct.Description, "<.*?>", String.Empty);
            return View(dbProduct);
        }

    }

}
