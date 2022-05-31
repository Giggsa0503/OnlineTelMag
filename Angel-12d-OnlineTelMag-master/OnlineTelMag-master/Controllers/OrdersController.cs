using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTelMag.Data;

namespace OnlineTelMag.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public OrdersController(ApplicationDbContext context,
                                                            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Test()
        {
            var userLoged = await _userManager.GetUserAsync(User);
            var result = await _userManager.AddToRoleAsync(userLoged, Roles.Admin.ToString()); 
            var roles = _userManager.GetRolesAsync(userLoged);
            return Content("OK !!!");
        }
        // GET: Orders
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var applicationDbContext = _context.Orders
                .Include(o => o.Telephones)
                .Include(o => o.Users);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var currentUser = _userManager.GetUserId(User);
                var myOrders = _context.Orders
                               .Include(o => o.Telephones)
                               .Include(u => u.Users)
                               .Where(x => x.UserId == currentUser.ToString())
                               .ToListAsync();

                return View(await myOrders);
            }

        }
        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index1()
        {
            var applicationDbContext = _context.Orders
                .Include(o => o.Telephones)
                .Include(o => o.Users);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Telephones)
                .Include(o => o.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["TelephoneId"] = new SelectList(_context.Telephones, "Id", "TelName");

            return View();
        }

        // POST: Orders/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TelephoneId")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.UserId=_userManager.GetUserId(User) ;
                order.DateRegister = DateTime.Now;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TelephoneId"] = new SelectList(_context.Telephones, "Id", "TelName", order.TelephoneId);
            
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["TelephoneId"] = new SelectList(_context.Telephones, "Id", "TelName", order.TelephoneId);
            
            return View(order);
        }

        // POST: Orders/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TelephoneId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    order.UserId = _userManager.GetUserId(User);
                    order.DateRegister = DateTime.Now;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["TelephoneId"] = new SelectList(_context.Telephones, "Id", "TelName", order.TelephoneId);
            
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Telephones)
                .Include(o => o.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
