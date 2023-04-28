using EntityFramework_Slider.Areas.Admin.ViewModels;
using EntityFramework_Slider.Data;
using EntityFramework_Slider.Helpers;
using EntityFramework_Slider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EntityFramework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;   // uploaddan qabaq wwwroot a chatmaq uchun bu interface den istifade edib chatiriq
        public SliderController(AppDbContext context,
                                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders.ToListAsync();
            return View(sliders);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            //badrequest routingde falan nese sehvlik olduqda chixan exseptiondur
            //meselen routingden sehven axtariw edilen id silinerse bu zaman chixan errordur

            //gelen id li elementi bazadan tapmaq uchun edirik bunu
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            //eyer kimse sehv regem yazibsa Url-e
            //NotFound-tapilmadi deye Exception 
            if (slider is null) return NotFound();
            //gelen id bazamizda yoxdursa chixan error

            return View(slider);
        }




        //[HttpGet]-datani sadece goturub gosterende
        [HttpGet]
        public IActionResult Create()     /*async-elemirik cunku data gelmir databazadan*/
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken] // main create
        public async Task<IActionResult> Create(SliderCreateVM slider)
        {
            try
            {
                if (!ModelState.IsValid)  // eger inputa wekil daxil edilmeyibse view a qayitsin. ve error chixartsin.
                {
                    return View(slider);
                }

                foreach (var photo in slider.Photos)
                {
                    if (!photo.CheckFileType("image/")) // yoxlayiriq bize gelen wekil formatidir yoxsa yox. wekil formati deyilse error chixart
                    {
                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();
                    }

                    if (!photo.CheckFileSize(200)) // sheklin olchusu max 200 den kichik deyilse error chixart
                    {
                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View();
                    }
                }

                foreach (var photo in slider.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;  // guid vasitesile ferqli adlarda yazdiririrq gelen filelarin adini. eyni adli wekiller yuklenmesin deye

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);  // path vasitesi ile roota chatiriq. yeni proyektimizde yuklenen wekli hara qoyacayiqsa ora chatmaq uchun

                    await FileHelper.SaveFileAsync(path, photo);

                    Slider newSlider = new()  //sliderden object yaradiriq , ona goreki yeni bir slider yaradib onun imagesini viewmodelden gelen filename e beraberlewdirib, add edirik databazadaki slidere ,save edirik bazaya
                    {
                        Image = fileName
                    };

                    await _context.Sliders.AddAsync(newSlider); // bize gelen slideri bazaya elave edirik
                }

                await _context.SaveChangesAsync(); // bazadaki deyiwikliyi save edirik

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Slider slider)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)  // eger inputa wekil daxil edilmeyibse view a qayitsin. ve error chixartsin.
        //        {
        //            return View(slider);
        //        }

        //        //if (!slider.Photo.ContentType.Contains("image/"))
        //        //{
        //        //    ModelState.AddModelError("Photo", "Please choose correct image type");
        //        //    return View();
        //        //}  ashagida extentiona chixardib yaziriq

        //        if (!slider.Photo.CheckFileType("image/")) // yoxlayiriq bize gelen wekil formatidir yoxsa yox. wekil formati deyilse error chixart
        //        {
        //            ModelState.AddModelError("Photo", "File type must be image");
        //            return View();
        //        }


        //        //if ((slider.Photo.Length / 1024) > 200)
        //        //{
        //        //    ModelState.AddModelError("Photo", "Please choose correct image size");
        //        //    return View();
        //        //}

        //        if (!slider.Photo.CheckFileSize(200)) // sheklin olchusu max 200 den kichik deyilse error chixart
        //        {
        //            ModelState.AddModelError("Photo", "Image size must be max 200kb");
        //            return View();
        //        }


        //        //1ci faylin adini gotururuk(her defe ferqli ad)
        //        string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;  // guid vasitesile ferqli adlarda yazdiririrq gelen filelarin adini. eyni adli wekiller yuklenmesin deye

        //        //2ci path -ini yaradirirq(yeni hara qoyacayiq)
        //        //string path = Path.Combine(_env.WebRootPath, "img", fileName);
        //        string path = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName);  // path vasitesi ile roota chatiriq. yeni proyektimizde yuklenen wekli hara qoyacayiqsa ora chatmaq uchun


        //        //3cu stream yaradiriq
        //        //using nedir: fayllar ile iwleyende proses bitdikden sonra(yeni File lazim olan hisseye azildiqdan sonra) databaza ile elaqenin kesilmesi uchun yaziriq
        //        //using (FileStream stream = new FileStream(path, FileMode.Create)) // filemode file yaradiriqsa moodunu gosterir. yeni file create olunur
        //        //{

        //        //    //4cu fayli streama kopyalayiriq
        //        //    //file stream odurko bir fayli fiziki olaraq harasa save etmek isteyirikse, bir axin yaradiriq(muhit) ve bunun vasitesile save edirik ora(hara save edeceyikse ora-- yeni yaratdigimiz patha)
        //        //    await slider.Photo.CopyToAsync(stream);  //fotonu yaratdigimiz streama copy etmek uchundur(yeni fiziki olaraq komputere kopyala). streamda faylin path(yeni rooto hara qoyacayiqsa o hisse) ve moodun novu create olur.
        //        //}

        //        await FileHelper.SaveFileAsync(path, slider.Photo);


        //        slider.Image = fileName; //yuklemek istediyimiz image-i beraber edirik filename(yeni her defe teze ad olan)

        //        await _context.Sliders.AddAsync(slider); // bize gelen slideri bazaya elave edirik

        //        await _context.SaveChangesAsync(); // bazadaki deyiwikliyi save edirik


        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

                if (slider is null) return NotFound();


                //1ci patha sileceyimiz imageni qowub , sistemden onu silib, sonra bazadan remove etmeliyik

                // 1ci pathi tapiriq
                string path = FileHelper.GetFilePath(_env.WebRootPath, "img", slider.Image);   // path vasitesi ile roota chatiriq. yeni proyektimizde yuklenen wekli hara qoyacayiqsa ora chatmaq uchun

                //if (System.IO.File.Exists(path))
                //{
                //    System.IO.File.Delete(path);
                //}  ashagida metod sheklinde yaziriq

                FileHelper.DeleteFile(path);  //pathda hemin file varsa delete edirik

                _context.Sliders.Remove(slider);//databazadan silirik

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
           
                if (id is null) return BadRequest();

                Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

                if (slider is null) return NotFound();

                return View(slider);

          
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int? id, Slider slider)
        //{
        //    try
        //    {
        ////        if (!ModelState.IsValid)
        ////        {
        ////            return View(slider);
        ////        }
        //        if (id == null) return BadRequest();

        //        Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id); // update edeceyimiz sliderin id sini databazadan tapiriq

        //        if (dbSlider is null) return NotFound();

        //        if (!slider.Photo.CheckFileType("image/"))
        //        {
        //            ModelState.AddModelError("Photo", "Please choose correct image type");
        //            return View(dbSlider);
        //        }

        //        if (!slider.Photo.CheckFileSize(200))
        //        {
        //            ModelState.AddModelError("Photo", "Image size must be max 200kb");
        //            return View(dbSlider);
        //        }



        //        //kohne dbda olan pathi tapib silirik
        //        string dbPath = FileHelper.GetFilePath(_env.WebRootPath, "img", dbSlider.Image);  //dbdaki pathimizi tapiriq

        //        FileHelper.DeleteFile(dbPath);   //dbPath da hemin file i delete edirik


        //        //yenisini yaradiriq
        //        string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

        //        string newPath = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName); //slider image in pathini tapiriq

        //        using (FileStream stream = new FileStream(newPath, FileMode.Create))     // streama copy edirik patha qoymaq uchun
        //        {
        //            await slider.Photo.CopyToAsync(stream);
        //        }

        //        //dbdaki slideri beraber edirik yeni filename e
        //        dbSlider.Image = fileName;    // slider image yenisine beraber edirik

        //        await _context.SaveChangesAsync();   // deyiwikliyi dbya save edirik

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        @ViewBag.error = ex.Message;
        //        return View();
        //    }



        //}




        
        [HttpPost]    //Teacher version
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Slider slider)
        {
            try
            {
                if (id == null) return BadRequest();
                Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
                if (dbSlider is null) return NotFound();

                if (slider.Photo == null)  //shekili deyishdirmeden submit etdikde null gelir bele olduqda  indexe getsin
                {
                    return RedirectToAction(nameof(Index));
                }

                if (!slider.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View(dbSlider);
                }

                if (!slider.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be max 200kb");
                    return View(dbSlider);
                }

                string oldPath = FileHelper.GetFilePath(_env.WebRootPath, "img", dbSlider.Image);  //dbdaki pathimizi tapiriq

                FileHelper.DeleteFile(oldPath);   //dbPath da hemin file i delete edirik

                string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

                string newPath = FileHelper.GetFilePath(_env.WebRootPath, "img", fileName); //slider image in pathini tapiriq

                //using (FileStream stream = new FileStream(newPath, FileMode.Create))     // streama copy edirik patha qoymaq uchun
                //{
                //    await slider.Photo.CopyToAsync(stream);
                //} //ashagidaki bunu qisaldilmish formasidir

                await FileHelper.SaveFileAsync(newPath, slider.Photo);

                dbSlider.Image = fileName; 

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View();
            }
        
        }


        [HttpPost]
        public async Task<IActionResult> SetStatus(int? id)
        {
            if (id is null) return BadRequest();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider == null) return NotFound();

            //if (slider.SoftDelete)
            //{
            //    slider.SoftDelete = false;
            //}
            //else
            //{
            //    slider.SoftDelete = true;
            //}

            slider.SoftDelete = !slider.SoftDelete;  // sliderin softdelete i beraberdir sliderin softdeletinin eksine( yeni  her iki halda truedirse falsa , falsedirse tru ya beraber olsun)

            await _context.SaveChangesAsync();

            return Ok(slider.SoftDelete);
        }
    }
}
