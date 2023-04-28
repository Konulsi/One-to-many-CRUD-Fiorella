using EntityFramework_Slider.Data;
using EntityFramework_Slider.Models;
using EntityFramework_Slider.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Slider.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAll() => await _context.Products.Include(m => m.Images).ToListAsync();
        public async Task<Product> GetById(int id) => await _context.Products.FindAsync(id);

        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();
   

        public async Task<Product> GetFullDataById(int id) => await _context.Products.Include(m => m.Images).Include(m => m.Category)?.FirstOrDefaultAsync(m => m.Id == id); // yoxlayiriq databazadaki productun idisinnen cookiedeki productun(yeni basketdeki productun) idsi eynidirse

        public async Task<List<Product>> GetPaginatedDatas(int page, int take)  //butun product datalarini elde etmek uchun
        {
            return await _context.Products.Include(m => m.Category).Include(m => m.Images).Skip((page * take) - take).Take(take).ToListAsync(); 
                                                                                         // skip ona goredir ki her defe neche dene product atlayib gostersin.
                                                                                         //bunun uchun oldugumuz sehifeni her defe gosterilen productlarin sayina vurub hasilden gosterlen taki i chixiriq.
                                                                                         //yeni meselen 3cu pagedeyikse her defe 5 dene take edib gosteririrkse. 3*5= 15 -5 = 10
        }
    }
}
