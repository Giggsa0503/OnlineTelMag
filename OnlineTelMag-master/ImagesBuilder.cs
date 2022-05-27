using Microsoft.AspNetCore.Hosting;
using OnlineTelMag.Data;
using OnlineTelMag.Models;
using System.IO;
using System.Threading.Tasks;

namespace OnlineTelMag
{
    public class ImagesBuilder
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string wwwroot;

        public ImagesBuilder (ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwroot = $"{this._hostEnvironment.WebRootPath}";
        }
        
        ///
        public async Task CreateImages(TelephoneVM model)
        {
            Telephone productToDb = new Telephone()
            {
                TelName = model.TelName,
                BrandId=model.BrandId,
                Broi=model.Broi,
                Color=model.Color,
                Date=System.DateTime.Now,
                Prise = model.Prise,
                Description = model.Description                
            };
            await _context.Telephones.AddAsync(productToDb);
            await this._context.SaveChangesAsync();

            //var wwwroot = $"{this._hostEnvironment.WebRootPath}";
            //създаваме папката images, ако не съществува
            Directory.CreateDirectory($"{wwwroot}/Images/");
            var imagePath = Path.Combine(wwwroot, "Images");
            string uniqueFileName = null;
            if (model.ImagePath.Count > 0)
            {
                for (int i = 0; i < model.ImagePath.Count; i++)
                {
                    TelephoneImages dbImage = new TelephoneImages()
                    {
                        TelephoneId = productToDb.Id,
                        Telephones = productToDb
                    };//id се създава автоматично при създаване на обект
                    if (model.ImagePath[i] != null)
                    {
                        uniqueFileName = dbImage.Id + "_" + model.ImagePath[i].FileName;
                        string filePath = Path.Combine(imagePath, uniqueFileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImagePath[i].CopyToAsync(fileStream);
                        }

                        dbImage.ImagePath = uniqueFileName;
                        await _context.TelephonesImages.AddAsync(dbImage);
                        await this._context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
