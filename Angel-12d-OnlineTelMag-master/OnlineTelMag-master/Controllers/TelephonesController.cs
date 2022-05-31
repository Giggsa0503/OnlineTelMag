
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTelMag.Data;
using OnlineTelMag.Models;

namespace OnlineTelMag.Controllers
{
    public class TelephonesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string wwwroot;

        public TelephonesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwroot = $"{this._hostEnvironment.WebRootPath}";
        }

        // GET: Telephones
        public async Task<IActionResult> Index()
        {
            List<Telephone> model = await _context.Telephones
                .Include(t => t.Brands)
                .Include(img => img.TelephoneImages)
                .ToListAsync();
            foreach (var item in model)
            {
                item.TelephoneImages = _context.TelephonesImages.Where(x => x.TelephoneId == item.Id).ToList();
            }

            return View(model);
            
        }
        public async Task<IActionResult> Index1()
        {
            List<Telephone> model = await _context.Telephones
                .Include(t => t.Brands)
                .Where(x => x.BrandId == 21)
                .Include(img => img.TelephoneImages)
                .ToListAsync();
            foreach (var item in model)
            {
                item.TelephoneImages = _context.TelephonesImages.Where(x => x.TelephoneId == item.Id).ToList();
            }
            
            return View(model);
        }


        // GET: Telephones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Telephone telephone = await _context.Telephones
                .Include(img => img.TelephoneImages)
                .Include(t => t.Brands)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (telephone == null)
            {
                return NotFound();
            }

            TelephoneDetailsVM modelVM = new TelephoneDetailsVM();
            modelVM.TelName = telephone.TelName;
            modelVM.Prise = telephone.Prise;
            modelVM.Description = telephone.Description;
            modelVM.Date = telephone.Date;
            modelVM.Color = telephone.Color;
            modelVM.Broi = telephone.Broi;
            modelVM.BrandId = telephone.BrandId;
            modelVM.ImagesPaths = _context.TelephonesImages
                .Where(img => img.TelephoneId == telephone.Id)
                .Select(x => $"/Images/{x.ImagePath}").ToList<string>();
            
            return View(modelVM);
        }

        // GET: Telephones/Create
        [Authorize (Roles="Admin")]
        public IActionResult Create()
        { 
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "NameBrand");
            return View();
        }
 

        // POST: Telephones/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] TelephoneVM telephone)
        {
            if (!ModelState.IsValid)
            {
                telephone.Date = DateTime.Now;
                ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "NameBrand", telephone.BrandId);
                return View(telephone);
            }
            ImagesBuilder imagesBuilder = new ImagesBuilder(_context, _hostEnvironment);
            await imagesBuilder.CreateImages(telephone);
            return RedirectToAction("Index");
        }

        // GET: Telephones/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telephone = await _context.Telephones.FindAsync(id);
            if (telephone == null)
            {
                return NotFound();
            }
            TelephoneDetailsVM modelVM = new TelephoneDetailsVM();
            //modelVM.Id = (int)id;
            modelVM.TelName = telephone.TelName;
            modelVM.Prise = telephone.Prise;
            modelVM.Description = telephone.Description;
            modelVM.Date = telephone.Date;
            modelVM.Color = telephone.Color;
            modelVM.Broi = telephone.Broi;
            modelVM.BrandId = telephone.BrandId;
            modelVM.ImagesPaths = _context.TelephonesImages
                .Where(img => img.TelephoneId == telephone.Id)
                .Select(x => $"/Images/{x.ImagePath}").ToList<string>();
            modelVM.Brands = _context.Brands.Select(x => new SelectListItem
            {
                Text = x.NameBrand,
                Value = x.Id.ToString(),
                Selected=(x.Id==modelVM.BrandId)

            }).ToList();

            return View(modelVM);
        }

        // POST: Telephones/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TelName,BrandId,Prise,Description,Color,Broi,Date")] TelephoneDetailsVM telephone)
        {

            if (!ModelState.IsValid)
            {
               return RedirectToAction(nameof(Index));
            }
            Telephone telefoneToDB =await _context.Telephones.FindAsync(id);
            telefoneToDB.TelName = telephone.TelName;
            telefoneToDB.Prise = telephone.Prise;
            telefoneToDB.Description = telephone.Description;
            telefoneToDB.Date = telephone.Date;
            telefoneToDB.Color = telephone.Color;
            telefoneToDB.Broi = telephone.Broi;
            telefoneToDB.BrandId = telephone.BrandId;
            
            try
            {
                telefoneToDB.Date = DateTime.Now;
                _context.Update(telefoneToDB);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelephoneExists(telefoneToDB.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
           
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Telephones/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telephone = await _context.Telephones
                .Include(t => t.Brands)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (telephone == null)
            {
                return NotFound();
            }

            return View(telephone);
        }

        // POST: Telephones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var telephone = await _context.Telephones.FindAsync(id);
            _context.Telephones.Remove(telephone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelephoneExists(int id)
        {
            return _context.Telephones.Any(e => e.Id == id);
        }
    }
}
