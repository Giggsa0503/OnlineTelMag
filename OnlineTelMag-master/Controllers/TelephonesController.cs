
using System;
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
            
            var applicationDbContext = _context.Telephones.Include(t => t.Brands);
            return View(await applicationDbContext.ToListAsync());
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
 // var imagePath = Path.Combine(wwwroot, "Images");//modelVM.Brands = _context.Brands
 //.Where(x => x.Id == modelVM.BrandId).ToList<SelectListItem>();

        // POST: Telephones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            //ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "NameBrand", telephone.BrandId);
            return View(modelVM);
        }

        // POST: Telephones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TelName,BrandId,Prise,Description,Color,Broi,Date")] Telephone telephone)
        {
            if (id != telephone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    telephone.Date = DateTime.Now;
                    _context.Update(telephone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelephoneExists(telephone.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "NameBrand", telephone.BrandId);
            return View(telephone);
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
