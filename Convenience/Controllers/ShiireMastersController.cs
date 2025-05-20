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
    public class ShiireMastersController : Controller
    {
        private readonly ConvenienceContext _context;

        public ShiireMastersController(ConvenienceContext context)
        {
            _context = context;
        }

        // GET: ShiireMasters
        public async Task<IActionResult> Index()
        {
            var convenienceContext = _context.ShiireMaster.Include(s => s.ShiireSakiMaster).Include(s => s.ShohinMaster);
            return View(await convenienceContext.ToListAsync());
        }

        // GET: ShiireMasters/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ShiireMaster == null)
            {
                return NotFound();
            }

            var shiireMaster = await _context.ShiireMaster
                .Include(s => s.ShiireSakiMaster)
                .Include(s => s.ShohinMaster)
                .FirstOrDefaultAsync(m => m.ShiireSakiId == id);
            if (shiireMaster == null)
            {
                return NotFound();
            }

            return View(shiireMaster);
        }

        // GET: ShiireMasters/Create
        public IActionResult Create()
        {
            ViewData["ShiireSakiId"] = new SelectList(_context.Set<ShiireSakiMaster>(), "ShiireSakiId", "ShiireSakiId");
            ViewData["ShohinId"] = new SelectList(_context.Set<ShohinMaster>(), "ShohinId", "ShohinId");
            return View();
        }

        // POST: ShiireMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShiireSakiId,ShiirePrdId,ShohinId,ShiirePrdName,ShiirePcsPerUnit,ShiireUnit,ShireTanka")] ShiireMaster shiireMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shiireMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShiireSakiId"] = new SelectList(_context.Set<ShiireSakiMaster>(), "ShiireSakiId", "ShiireSakiId", shiireMaster.ShiireSakiId);
            ViewData["ShohinId"] = new SelectList(_context.Set<ShohinMaster>(), "ShohinId", "ShohinId", shiireMaster.ShohinId);
            return View(shiireMaster);
        }

        // GET: ShiireMasters/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ShiireMaster == null)
            {
                return NotFound();
            }

            var shiireMaster = await _context.ShiireMaster.FindAsync(id);
            if (shiireMaster == null)
            {
                return NotFound();
            }
            ViewData["ShiireSakiId"] = new SelectList(_context.Set<ShiireSakiMaster>(), "ShiireSakiId", "ShiireSakiId", shiireMaster.ShiireSakiId);
            ViewData["ShohinId"] = new SelectList(_context.Set<ShohinMaster>(), "ShohinId", "ShohinId", shiireMaster.ShohinId);
            return View(shiireMaster);
        }

        // POST: ShiireMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ShiireSakiId,ShiirePrdId,ShohinId,ShiirePrdName,ShiirePcsPerUnit,ShiireUnit,ShireTanka")] ShiireMaster shiireMaster)
        {
            if (id != shiireMaster.ShiireSakiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shiireMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiireMasterExists(shiireMaster.ShiireSakiId))
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
            ViewData["ShiireSakiId"] = new SelectList(_context.Set<ShiireSakiMaster>(), "ShiireSakiId", "ShiireSakiId", shiireMaster.ShiireSakiId);
            ViewData["ShohinId"] = new SelectList(_context.Set<ShohinMaster>(), "ShohinId", "ShohinId", shiireMaster.ShohinId);
            return View(shiireMaster);
        }

        // GET: ShiireMasters/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ShiireMaster == null)
            {
                return NotFound();
            }

            var shiireMaster = await _context.ShiireMaster
                .Include(s => s.ShiireSakiMaster)
                .Include(s => s.ShohinMaster)
                .FirstOrDefaultAsync(m => m.ShiireSakiId == id);
            if (shiireMaster == null)
            {
                return NotFound();
            }

            return View(shiireMaster);
        }

        // POST: ShiireMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ShiireMaster == null)
            {
                return Problem("Entity set 'ConvenienceContext.ShiireMaster'  is null.");
            }
            var shiireMaster = await _context.ShiireMaster.FindAsync(id);
            if (shiireMaster != null)
            {
                _context.ShiireMaster.Remove(shiireMaster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiireMasterExists(string id)
        {
          return (_context.ShiireMaster?.Any(e => e.ShiireSakiId == id)).GetValueOrDefault();
        }
    }
}
