using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;

namespace Shopping.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> lista = await _context.Categories.ToListAsync();
            return View(lista);
            //return View(await _context.Categories.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category categoria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Add(categoria);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicada"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una categoría con ese mismo nombre");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null || _context.Categories != null)
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category edicion)
        {
            if (ModelState.IsValid)
            {
                if (edicion != null)
                {
                    var categoria = await _context.Categories.FirstOrDefaultAsync(x => x.Id == edicion.Id);
                    if (categoria == null)
                    {
                        return NotFound();
                    }
                    try
                    {
                        categoria.Name = edicion.Name;
                        _context.Categories.Update(categoria);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException dbException)
                    {
                        if (dbException.InnerException.Message.Contains("duplicada"))
                        {
                            ModelState.AddModelError(string.Empty, "No pueden existir categorias duplicadas");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, dbException.InnerException.Message);
                        }                
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
            return View(edicion);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if(category == null) return NotFound();
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || _context.Countries == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Category categoria)
        {
            if(categoria == null || _context.Countries == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(categoria.Id);
            if(category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
