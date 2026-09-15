
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceStore.Models;
using EcommerceStore.Data;

public class BusinessNatureTypesController : Controller
{
    private readonly ApplicationDbContext _context;

    public BusinessNatureTypesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: BUSINESSNATURETYPES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.BusinessNatureTypes.ToListAsync());
    }

    // GET: BUSINESSNATURETYPES/Details/5
    public async Task<IActionResult> Details(int? naturetypeid)
    {
        if (naturetypeid == null)
        {
            return NotFound();
        }

        var businessnaturetype = await _context.BusinessNatureTypes
            .FirstOrDefaultAsync(m => m.NatureTypeID == naturetypeid);
        if (businessnaturetype == null)
        {
            return NotFound();
        }

        return View(businessnaturetype);
    }

    // GET: BUSINESSNATURETYPES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: BUSINESSNATURETYPES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NatureTypeID,TypeName,Products")] BusinessNatureType businessnaturetype)
    {
        if (ModelState.IsValid)
        {
            _context.Add(businessnaturetype);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(businessnaturetype);
    }

    // GET: BUSINESSNATURETYPES/Edit/5
    public async Task<IActionResult> Edit(int? naturetypeid)
    {
        if (naturetypeid == null)
        {
            return NotFound();
        }

        var businessnaturetype = await _context.BusinessNatureTypes.FindAsync(naturetypeid);
        if (businessnaturetype == null)
        {
            return NotFound();
        }
        return View(businessnaturetype);
    }

    // POST: BUSINESSNATURETYPES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? naturetypeid, [Bind("NatureTypeID,TypeName,Products")] BusinessNatureType businessnaturetype)
    {
        if (naturetypeid != businessnaturetype.NatureTypeID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(businessnaturetype);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessNatureTypeExists(businessnaturetype.NatureTypeID))
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
        return View(businessnaturetype);
    }

    // GET: BUSINESSNATURETYPES/Delete/5
    public async Task<IActionResult> Delete(int? naturetypeid)
    {
        if (naturetypeid == null)
        {
            return NotFound();
        }

        var businessnaturetype = await _context.BusinessNatureTypes
            .FirstOrDefaultAsync(m => m.NatureTypeID == naturetypeid);
        if (businessnaturetype == null)
        {
            return NotFound();
        }

        return View(businessnaturetype);
    }

    // POST: BUSINESSNATURETYPES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? naturetypeid)
    {
        var businessnaturetype = await _context.BusinessNatureTypes.FindAsync(naturetypeid);
        if (businessnaturetype != null)
        {
            _context.BusinessNatureTypes.Remove(businessnaturetype);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BusinessNatureTypeExists(int? naturetypeid)
    {
        return _context.BusinessNatureTypes.Any(e => e.NatureTypeID == naturetypeid);
    }
}
