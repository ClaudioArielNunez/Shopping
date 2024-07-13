using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class CountriesController : Controller
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            
            //con include mostramos los estados de addState/ por cada pais incluya los estados
            return View(await _context.Countries.Include(c => c.States).ToListAsync());
            //return View(await _context.Countries.ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }
            //para q muestre los estados agregamos include
            var country = await _context.Countries.Include(c => c.States).FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                //para evitar que se ingrese un pais repetido
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException) //si fallo la actualizacion, lanza excepcion
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicada")) //en la excepcion dice 'duplicate'
                    {
                        //agregamos un nuevo error al ModelState                        
                        //string.Empty, significa que el error no está asociado con un campo específico del modelo, sino que es un error general 
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    }
                    else
                    {
                        //si el error no tiene la palabra duplicada, muestro el mensaje de error detallado
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    //si es un error general, igual lo muestro
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }
            return View(country);
        }

        // GET: Countries/AddState
        public async Task<IActionResult> AddState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Country country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            //si encontro el pais creamos el ViewModel
            StateViewModel model = new()
            {
                CountryId = country.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddState(StateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    State state = new State()
                    {
                        Cities = new List<City>(),
                        Country = await _context.Countries.FindAsync(model.CountryId),
                        Name = model.Name,
                    };
                    _context.Add(state);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = model.CountryId });
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("duplicada"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una provincia con el mismo nombre en este país.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Countries == null) //id puede llegar a ser nulo si lo modifica en la url
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // Este atributo es una medida de seguridad contra ataques CSRF (Cross-Site Request Forgery). Asegura que el formulario que envía la solicitud POST contiene un token válido generado por ASP.NET Core.
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id) //chequeamos coincidencia de los idis
            {
                return NotFound();//mandamos 404
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Countries.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("duplicada"))
                    {
                        ModelState.AddModelError(string.Empty, "Ese país ya existe en la lista");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }
            //mostramos los estados con include
            var country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'DataContext.Countries'  is null.");
            }
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _context.Countries.Remove(country);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool CountryExists(int id)
        //{
        //  return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
