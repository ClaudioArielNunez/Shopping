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
            //si se rompe el boton crear puede ser porque la lista states sea null en ese caso:
            //Country country = new() { States = new List<State>() };
            //return View(country);
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
                    TempData["successMessage"] = "País creado con exito!";
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
            TempData["errorMessage"] = "No se pudo ingresar el país";
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
                    TempData["successMessage"] = "Estado agregado con exito!";
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
            TempData["errorMessage"] = "No se pudo agregar el estado";
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
                    TempData["successMessage"] = "País editado con exito!";
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
            TempData["errorMessage"] = "No se pudo editar este país";
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
            try
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
                TempData["successMessage"] = "País eliminado con exito!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction(nameof(Delete),new { Id = id });                    
            }
        }

        // GET: Countries/EditState/5
        public async Task<IActionResult> EditState(int? id) //el id es de un state
        {
            if (id == null || _context.States == null) //id puede llegar a ser nulo si lo modifica en la url
            {
                return NotFound();
            }

            State state = await _context.States
                .Include(s =>s.Country)  //quiero saber a q pais pertenece
                .FirstOrDefaultAsync(s => s.Id == id);

            if (state == null)
            {
                return NotFound();
            }
            //creamos un stateviewModel, conseguimos estos datos gracias al include
            StateViewModel model = new()
            {
                CountryId = state.Country.Id,
                Id = state.Id,
                Name = state.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Este atributo es una medida de seguridad contra ataques CSRF (Cross-Site Request Forgery). Asegura que el formulario que envía la solicitud POST contiene un token válido generado por ASP.NET Core.
        public async Task<IActionResult> EditState(int id, StateViewModel model)
        {
            if (id != model.Id) //chequeamos coincidencia de los idis
            {
                return NotFound();//mandamos 404
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //creo el state a partir del model
                    State state = new()
                    {
                        Id = model.Id,
                        Name = model.Name,

                    };

                    _context.States.Update(state);
                    await _context.SaveChangesAsync();
                    TempData["successMessage"] = "Estado editado con exito!";
                    return RedirectToAction(nameof(Details), new {Id = model.CountryId});
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("duplicada"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un departamento/estado con el mismo nombre en ese país");
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
            TempData["errorMessage"] = "No se pudo editar el estado";
            return View(model);
        }

        //private bool CountryExists(int id)
        //{
        //  return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
