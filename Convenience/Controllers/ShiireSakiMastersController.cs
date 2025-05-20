using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Convenience.Data;
using Convenience.Models.DataModels;

namespace Convenience.Controllers
{
    public class ShiireSakiMastersController : Controller
    {
        private readonly ConvenienceContext _context;

        public ShiireSakiMastersController(ConvenienceContext context)
        {
            _context = context;
        }

        // GET: ShiireSakiMasters
        public async Task<IActionResult> Index()
        {
              return _context.ShiireSakiMaster != null ? 
                          View(await _context.ShiireSakiMaster.ToListAsync()) :
                          Problem("Entity set 'ConvenienceContext.ShiireSakiMaster'  is null.");
        }

        // GET: ShiireSakiMasters/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ShiireSakiMaster == null)
            {
                return NotFound();
            }

            var shiireSakiMaster = await _context.ShiireSakiMaster
                .FirstOrDefaultAsync(m => m.ShiireSakiId == id);
            if (shiireSakiMaster == null)
            {
                return NotFound();
            }

            return View(shiireSakiMaster);
        }

        // GET: ShiireSakiMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShiireSakiMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShiireSakiId,ShiireSakiKaisya,ShiireSakiBusho,YubinBango,Todoufuken,Shikuchoson,Banchi,Tatemonomei")] ShiireSakiMaster shiireSakiMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shiireSakiMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shiireSakiMaster);
        }

        // GET: ShiireSakiMasters/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ShiireSakiMaster == null)
            {
                return NotFound();
            }

            var shiireSakiMaster = await _context.ShiireSakiMaster.FindAsync(id);
            if (shiireSakiMaster == null)
            {
                return NotFound();
            }
            return View(shiireSakiMaster);
        }

        // POST: ShiireSakiMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ShiireSakiId,ShiireSakiKaisya,ShiireSakiBusho,YubinBango,Todoufuken,Shikuchoson,Banchi,Tatemonomei")] ShiireSakiMaster shiireSakiMaster)
        {
            if (id != shiireSakiMaster.ShiireSakiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shiireSakiMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiireSakiMasterExists(shiireSakiMaster.ShiireSakiId))
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
            return View(shiireSakiMaster);
        }

        // GET: ShiireSakiMasters/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ShiireSakiMaster == null)
            {
                return NotFound();
            }

            var shiireSakiMaster = await _context.ShiireSakiMaster
                .FirstOrDefaultAsync(m => m.ShiireSakiId == id);
            if (shiireSakiMaster == null)
            {
                return NotFound();
            }

            return View(shiireSakiMaster);
        }

        // POST: ShiireSakiMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ShiireSakiMaster == null)
            {
                return Problem("Entity set 'ConvenienceContext.ShiireSakiMaster'  is null.");
            }
            var shiireSakiMaster = await _context.ShiireSakiMaster.FindAsync(id);
            if (shiireSakiMaster != null)
            {
                _context.ShiireSakiMaster.Remove(shiireSakiMaster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiireSakiMasterExists(string id)
        {
          return (_context.ShiireSakiMaster?.Any(e => e.ShiireSakiId == id)).GetValueOrDefault();
        }
    }
}
