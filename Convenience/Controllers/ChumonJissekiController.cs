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
    public class ChumonJissekiController : Controller
    {
        private readonly ConvenienceContext _context;

        public ChumonJissekiController(ConvenienceContext context)
        {
            _context = context;
        }

        // GET: ChumonJissekiMeisais
        public async Task<IActionResult> Index()
        {
            var convenienceContext = _context.ChumonJissekiMeisai.Include(c => c.ChumonJisseki).Include(c => c.ShiireMaster);
            return View(await convenienceContext.ToListAsync());
        }

        // GET: ChumonJissekiMeisais/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ChumonJissekiMeisai == null)
            {
                return NotFound();
            }

            var chumonJissekiMeisai = await _context.ChumonJissekiMeisai
                .Include(c => c.ChumonJisseki)
                .Include(c => c.ShiireMaster)
                .FirstOrDefaultAsync(m => m.ChumonId == id);
            if (chumonJissekiMeisai == null)
            {
                return NotFound();
            }

            return View(chumonJissekiMeisai);
        }

        // GET: ChumonJissekiMeisais/Create
        public IActionResult Create()
        {
            ViewData["ChumonId"] = new SelectList(_context.ChumonJisseki, "ChumonId", "ChumonId");
            ViewData["ShiireSakiId"] = new SelectList(_context.Set<ShiireMaster>(), "ShiireSakiId", "ShiireSakiId");
            return View();
        }

        // POST: ChumonJissekiMeisais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChumonId,ShiireSakiId,ShiirePrdId,ShohinId,ChumonSu,ChumonZan,Version")] ChumonJissekiMeisai chumonJissekiMeisai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chumonJissekiMeisai);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChumonId"] = new SelectList(_context.ChumonJisseki, "ChumonId", "ChumonId", chumonJissekiMeisai.ChumonId);
            ViewData["ShiireSakiId"] = new SelectList(_context.Set<ShiireMaster>(), "ShiireSakiId", "ShiireSakiId", chumonJissekiMeisai.ShiireSakiId);
            return View(chumonJissekiMeisai);
        }

        // GET: ChumonJissekiMeisais/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ChumonJissekiMeisai == null)
            {
                return NotFound();
            }

            var chumonJissekiMeisai = await _context.ChumonJissekiMeisai.FindAsync(id);
            if (chumonJissekiMeisai == null)
            {
                return NotFound();
            }
            ViewData["ChumonId"] = new SelectList(_context.ChumonJisseki, "ChumonId", "ChumonId", chumonJissekiMeisai.ChumonId);
            ViewData["ShiireSakiId"] = new SelectList(_context.Set<ShiireMaster>(), "ShiireSakiId", "ShiireSakiId", chumonJissekiMeisai.ShiireSakiId);
            return View(chumonJissekiMeisai);
        }

        // POST: ChumonJissekiMeisais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ChumonId,ShiireSakiId,ShiirePrdId,ShohinId,ChumonSu,ChumonZan,Version")] ChumonJissekiMeisai chumonJissekiMeisai)
        {
            if (id != chumonJissekiMeisai.ChumonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chumonJissekiMeisai);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChumonJissekiMeisaiExists(chumonJissekiMeisai.ChumonId))
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
            ViewData["ChumonId"] = new SelectList(_context.ChumonJisseki, "ChumonId", "ChumonId", chumonJissekiMeisai.ChumonId);
            ViewData["ShiireSakiId"] = new SelectList(_context.Set<ShiireMaster>(), "ShiireSakiId", "ShiireSakiId", chumonJissekiMeisai.ShiireSakiId);
            return View(chumonJissekiMeisai);
        }

        // GET: ChumonJissekiMeisais/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ChumonJissekiMeisai == null)
            {
                return NotFound();
            }

            var chumonJissekiMeisai = await _context.ChumonJissekiMeisai
                .Include(c => c.ChumonJisseki)
                .Include(c => c.ShiireMaster)
                .FirstOrDefaultAsync(m => m.ChumonId == id);
            if (chumonJissekiMeisai == null)
            {
                return NotFound();
            }

            return View(chumonJissekiMeisai);
        }

        // POST: ChumonJissekiMeisais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ChumonJissekiMeisai == null)
            {
                return Problem("Entity set 'ConvenienceContext.ChumonJissekiMeisai'  is null.");
            }
            var chumonJissekiMeisai = await _context.ChumonJissekiMeisai.FindAsync(id);
            if (chumonJissekiMeisai != null)
            {
                _context.ChumonJissekiMeisai.Remove(chumonJissekiMeisai);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChumonJissekiMeisaiExists(string id)
        {
          return (_context.ChumonJissekiMeisai?.Any(e => e.ChumonId == id)).GetValueOrDefault();
        }
    }
}
