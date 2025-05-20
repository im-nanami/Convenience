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
    public class ShohinMastersController : Controller
    {
        private readonly ConvenienceContext _context;

        public ShohinMastersController(ConvenienceContext context)
        {
            _context = context;
        }

        // GET: ShohinMasters
        public async Task<IActionResult> Index()
        {
              return _context.ShohinMaster != null ? 
                          View(await _context.ShohinMaster.ToListAsync()) :
                          Problem("Entity set 'ConvenienceContext.ShohinMaster'  is null.");
        }

        // GET: ShohinMasters/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ShohinMaster == null)
            {
                return NotFound();
            }

            var shohinMaster = await _context.ShohinMaster
                .FirstOrDefaultAsync(m => m.ShohinId == id);
            if (shohinMaster == null)
            {
                return NotFound();
            }

            return View(shohinMaster);
        }

        // GET: ShohinMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShohinMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShohinId,ShohinName,ShohinTanka,ShohiZeiritsu,ShohiZeiritsuEatIn")] ShohinMaster shohinMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shohinMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shohinMaster);
        }

        // GET: ShohinMasters/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ShohinMaster == null)
            {
                return NotFound();
            }

            var shohinMaster = await _context.ShohinMaster.FindAsync(id);
            if (shohinMaster == null)
            {
                return NotFound();
            }
            return View(shohinMaster);
        }

        // POST: ShohinMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ShohinId,ShohinName,ShohinTanka,ShohiZeiritsu,ShohiZeiritsuEatIn")] ShohinMaster shohinMaster)
        {
            if (id != shohinMaster.ShohinId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shohinMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShohinMasterExists(shohinMaster.ShohinId))
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
            return View(shohinMaster);
        }

        // GET: ShohinMasters/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ShohinMaster == null)
            {
                return NotFound();
            }

            var shohinMaster = await _context.ShohinMaster
                .FirstOrDefaultAsync(m => m.ShohinId == id);
            if (shohinMaster == null)
            {
                return NotFound();
            }

            return View(shohinMaster);
        }

        // POST: ShohinMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ShohinMaster == null)
            {
                return Problem("Entity set 'ConvenienceContext.ShohinMaster'  is null.");
            }
            var shohinMaster = await _context.ShohinMaster.FindAsync(id);
            if (shohinMaster != null)
            {
                _context.ShohinMaster.Remove(shohinMaster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShohinMasterExists(string id)
        {
          return (_context.ShohinMaster?.Any(e => e.ShohinId == id)).GetValueOrDefault();
        }
    }
}
