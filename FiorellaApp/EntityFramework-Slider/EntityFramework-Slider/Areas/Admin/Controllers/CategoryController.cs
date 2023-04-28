using EntityFramework_Slider.Areas.Admin.ViewModels;
using EntityFramework_Slider.Data;
using EntityFramework_Slider.Helpers;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //public class CategoryController : Controller
    //{
    //    private readonly ICategoryService _categoryService;
    //    private readonly AppDbContext _context;
    //    public CategoryController(ICategoryService categoryService, AppDbContext context)
    //    {
    //        _categoryService = categoryService;
    //        _context = context;
    //    }
    //    public async Task<IActionResult> Index(int page =1, int take = 2)
    //    {
    //        List<Category> categories = await _categoryService.GetPaginatedDatas(page, take); 

    //        List<CategoryListVM> mappeddatas = GetMappedDatas(categories);  

    //        int pageCount = await GetPageCountAsync(take);

    //        Paginate<CategoryListVM> paginatedDatas = new(mappeddatas, page, pageCount);

    //        ViewBag.take = take;

    //        return View(paginatedDatas);


    //        //return View(await _categoryService.GetAll());
    //    }


    //    [HttpGet]
    //    public IActionResult Create()     /*async-elemirik cunku data gelmir databazadan*/ //sadece indexe gedir hansiki inputa data elavve edib category yaradacaq
    //    {
    //        return View();
    //    }


    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create(Category category)     // bazaya neyise save edirik deye asinxron olmalidi
    //    {
    //        try
    //        {
    //            if (!ModelState.IsValid)    // create eden zaman input null olarsa, yeni isvalid deyilse(isvalid olduqda data daxil edilir) view a qayit
    //            {
    //                return View();
    //            }

    //            var existData = await _context.Categories.FirstOrDefaultAsync(m => m.Name.Trim().ToLower() == category.Name.Trim().ToLower());
    //            // yoxlayiriq bize gelen yeni inputa yazilan name databazada varsa  error chixartmaq uchun

    //            if (existData is not null) /*gelen data databazamizda varsa yeni null deyilse*/
    //            {
    //                ModelState.AddModelError("Name", "This data already exist!"); // bu inputa  daxil olan name bazada vaxrsa error chixartsin. Name propertisinin altinda. buradaki Name hemin input-un adidir.

    //                return View();
    //            }

    //            //int num = 1;
    //            //int num2 = 0;
    //            //int result = num / num2;  // her hansi error olduqda (ozumuzden error yaratmiwiq)

    //            await _context.Categories.AddAsync(category);  //bazadaki categorie tablesine category ni add edir liste
    //            await _context.SaveChangesAsync();    //save edir bazaya 
    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch (Exception ex)
    //        {
    //            ViewBag.error = ex.Message;
    //            return RedirectToAction("Error", new { msj = ex.Message}); // bashqa actiona arqument kimi data gonderirik.
    //        }

    //    }


    //    public IActionResult Error(string msj)
    //    {
    //        ViewBag.error = msj;   // bawqa actiondan gelen argumenti parametr kimi qebul edirik viewbag vasitesile;
    //        return View();
    //    }



    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Delete(int? id)   //bu halda dele etdikde her yerden silinir. 
    //    {
    //        if (id is null) return BadRequest();

    //        Category category = await _context.Categories.FindAsync(id);   //databazada id li elementi tapiriq

    //        if(category is null) return NotFound();

    //        _context.Categories.Remove(category);    //gelen categoryni databazadan silirik

    //        await _context.SaveChangesAsync();   // deyiwikliyi databazaya save edirik

    //        return RedirectToAction(nameof(Index));

    //    }



    //    [HttpPost]
    //   //softdelete with ajax
    //    public async Task<IActionResult> SoftDelete(int? id)  // bele ancaq softdeleti deyiwir
    //    {
    //        if (id is null) return BadRequest();
    //        Category category = await _context.Categories.FindAsync(id);   //databazada id li elementi tapiriq
    //        if (category is null) return NotFound();

    //        category.SoftDelete = true;   // bazadan silmir  softdaleteni deyiwib true edir.

    //        await _context.SaveChangesAsync();   // deyiwikliyi databazaya save edirik

    //        return Ok();
    //    }




    //    [HttpGet]
    //    public async Task<IActionResult> Edit(int? id)   // 1ci addimburada gelen id li categoryni bazadan tapib view a gonderib gostetririk sadece(yeni sehife avhilmalidi edit edeceyimiz categroy orada gorsenmelidir)
    //    {
    //        if (id is null) return BadRequest();
    //        Category category = await _context.Categories.FindAsync(id);   //databazada id li elementi tapiriq
    //        if (category is null) return NotFound();

    //        return View(category);
    //    }



    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int? id, Category category)   // 2ci addim ise hemin sehifede olan categoryni tezede bazadan tapib, onun nameni deyiowmeliyik gelen teze name ye(yeni teze inputa daxil olana)
    //    {
    //        try
    //        {
    //            if (!ModelState.IsValid)    // create eden zaman input null olarsa, yeni isvalid deyilse(isvalid olduqda data daxil edilir) view a qayit
    //            {
    //                return View();
    //            }

    //            if (id is null) return BadRequest();

    //            Category dbCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);   // databazani mewgul etmemek uchun(bir nov databazaya muraciet etdikde elaqe achiq qalmasin deye) AsNoTracking yaziriq

    //            if (dbCategory is null) return NotFound();

    //            if (dbCategory.Name.Trim().ToLower() == category.Name.Trim().ToLower())  // eyni adi tekrar yazib update elemek istedikde bura girir ve index e qayidir. 
    //            {
    //                return RedirectToAction(nameof(Index));
    //            }

    //            //dbCategory.Name= category.Name;   // tek bir datasi varsa modelin bele edirik. bir neche propertisi varsa update olunaasi bu zaman awagidaki kimi edirik. her defe her propertini ayriliqda beraberlewdirib yoxlamamaq uchun

    //            _context.Categories.Update(category);  // modelin bir neche propertisi olduqda update elemek uchun bele yazilir

    //            await _context.SaveChangesAsync();

    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch (Exception ex)
    //        {
    //            return RedirectToAction("Error", new { msj = ex.Message}); // bashqa actiona arqument kimi data gonderirik.
    //        }
    //    }



    //    [HttpGet]
    //    public async Task<IActionResult> Detail(int? id) 
    //    {
    //        if (id is null) return BadRequest();
    //        Category category = await _context.Categories.FindAsync(id);   //databazada id li elementi tapiriq
    //        if (category is null) return NotFound();

    //        return View(category);
    //    }




    //    //pagination
    //    private async Task<int> GetPageCountAsync(int take)
    //    {
    //        var productCount = await _categoryService.GetCountAsync();   

    //        return (int)Math.Ceiling((decimal)(productCount / take));

    //    }



    //    private List<CategoryListVM> GetMappedDatas(List<Category> categories) 
    //    {
    //        List<CategoryListVM> mappedDatas = new(); 

    //        foreach (var category in categories) 
    //        {
    //            CategoryListVM categoryVM = new()   
    //            {
    //                Id = category.Id,
    //                Name = category.Name,

    //            };

    //            mappedDatas.Add(categoryVM); 
    //        }

    //        return mappedDatas;

    //    }

    //}






    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        private readonly AppDbContext _context;
        public CategoryController(ICategoryService categoryService,
                                  AppDbContext context)
        {
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 2)
        {
            List<Category> categories = await _categoryService.GetPaginatedDatas(page, take);
            List<CategoryListVM> mappedDatas = GetMappedDatas(categories);
            int pageCount = await GetPageCountAsync(take);
            Paginate<CategoryListVM> paginatedDatas = new(mappedDatas, page, pageCount);

            ViewBag.take = take;

            return View(paginatedDatas);

        }


        private List<CategoryListVM> GetMappedDatas(List<Category> categories)
        {
            List<CategoryListVM> mappedDatas = new();
            foreach (var category in categories)
            {
                CategoryListVM categorytVM = new()
                {
                    Id = category.Id,
                    Name = category.Name,


                };
                mappedDatas.Add(categorytVM);
            }
            return mappedDatas;
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var categoryCount = await _categoryService.GetCountAsync();

            return (int)Math.Ceiling((decimal)categoryCount / take);

        }

        //------CREATE VIEW-------
        [HttpGet]
        public IActionResult Create()    /*async-elemirik cunku data gelmir databazadan*/
        {

            return View();
        }




        //------CREATE-----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)    /*async-elemirik cunku data gelmir databazadan*/
        {
            try  //databazada problem olarsa cathca girsin
            {
                if (!ModelState.IsValid)  //inputdan gelen data valid deyilse (yani bosdursa) yeniden viewa qayit.yeniden istesin yazmaqi
                {
                    return View();
                }

                var existData = await _context.Categories.FirstOrDefaultAsync(m => m.Name.Trim().ToLower() == category.Name.Trim().ToLower());
                if (existData is not null)  //databazada bu adda data varsa create eleme
                {
                    ModelState.AddModelError("Name", "This data already exist");  //ModelState.AddModelError-error text gonderik viewa(orada asp-validation-for var gebul edir errorlar)
                    return View();
                }
                await _context.Categories.AddAsync(category);  //validdisa(dolu) gelen Category tipden categorini save ele Databazadaya
                await _context.SaveChangesAsync();  //ve yaddasda saxla yeni dataynan
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", new { msj = "hi there!" });  //basqa actionun  parametrina data gonderik
            }


        }
        public IActionResult Error(string msj)
        {
            ViewBag.error = msj;  //oz yazdqimiz texti gonderik error Viewa
            return View();
        }




        //------DELETE-------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();  //eyer Id(null) gelmirse exseption cixsin

            Category category = await _context.Categories.FindAsync(id);  //eyni id-li datani tap

            if (category is null) return NotFound();  //eyer bucur Id yoxdursa exseption cixsin

            _context.Categories.Remove(category);   //ve silinmis vezyetde yaddasda saxla

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));   //qayit ana seyfeye

        }







        //------SOFDETELE ACTION(DATANI TRUE ETMEK UCUN(DATABAZADA QALSIN)-------
        [HttpPost]
        public async Task<IActionResult> SoftDelete(int? id)
        {
            if (id is null) return BadRequest();  //eyer Id(null) gelmirse exseption cixsin

            Category category = await _context.Categories.FindAsync(id);  //eyni id-li datani tap

            if (category is null) return NotFound();  //eyer bucur Id yoxdursa exseption cixsin

            category.SoftDelete = true;

            await _context.SaveChangesAsync();

            return Ok();

        }






        //---------------EDIT VIEW-------------------//birinci get edirik edit etdiyimiz gorek deye(UI-da datalari cixsin
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)  //id gelecek(id-ye gore tapsin ) ve Viewa aparsin
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);

            if (category is null) return NotFound();

            return View(category); //viewa o id-li datani gonderik gorsetmek ucun

        }




        //-----------------EDIT ----------------------//indi Datani Edit edirik
        [HttpPost] //indi Datani Edit edirik
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Category category)  //id gelecek(id-ye gore tapsin )
        {
            try
            {
                if (id is null) return BadRequest();  //eyer Id(null) gelmirse exseption cixsin

                if (!ModelState.IsValid)
                {
                    return View();
                }

                Category dbCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id); //eyni id-li datani tap

                //AsNoTracking()--yalniz update edende lazim olur bir nov maniyeni aradan qaldirmaq uucn yazriq .Databazaynan elaqe kesmek ucun,cunku asagida yene miraciyet etmeliyik databazaya Update yazanda.databaza cavab versin deye mesgul olmasin

                if (dbCategory is null) return NotFound();  //eyer bucur Id yoxdursa exseption cixsin

                if (dbCategory.Name.Trim().ToLower() == category.Name.Trim().ToLower()) //eyer databazada deyismek istediyin Adda data varsa hecne elemesin
                {
                    return RedirectToAction("Index");
                }
                //dbCategory.Name = category.Name;   //tapdiqmiz name-(data) = gelen name-(data) beraberlesdirik(asign)

                _context.Categories.Update(category);  //herdefe tek tek asign elememek ucun gelen datanin icinde olanlari

                await _context.SaveChangesAsync();  //save edirik

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", new { msj = "hi there!" });

            }


        }





        //----------DETAIL------------------------
        [HttpGet]  //birinci get edirik edit etdiyimiz gorek deye(UI-da datalari cixsin)
        public async Task<IActionResult> Detail(int? id)  //id gelecek(id-ye gore tapsin ) ve Viewa aparsin
        {
            if (id is null) return BadRequest();

            Category category = await _context.Categories.FindAsync(id);

            if (category is null) return NotFound();

            return View(category); //viewa o id-li datani gonderik gorsetmek ucun

        }





    }
}
