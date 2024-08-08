using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using System.Linq;

namespace Shopping.Helpers
{
    public class ComboHelper : IComboHelper
    {
        private readonly DataContext _context;

        //llamamos a dataContext
        public ComboHelper(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync()
        {
            List<SelectListItem> list = await _context.Categories.Select(cat => new SelectListItem
            {
                Text = cat.Name,
                Value= cat.Id.ToString()
            })
                .OrderBy(cat => cat.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "Seleccione una categoría...", Value = "0"});

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
        {
            List<SelectListItem> list = await _context.Cities.Where(city =>city.State.Id == stateId)
                .Select(city => new SelectListItem
                {
                    Text = city.Name,
                    Value = city.Id.ToString()
                })
                .OrderBy(city => city.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "Seleccione una ciudad...", Value = "0"});

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboCountriesAsync()
        {
            List<SelectListItem> list = await _context.Countries.Select(country => new SelectListItem
            {
                Text = country.Name,
                Value= country.Id.ToString()
            })
            .OrderBy(country => country.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "Seleccione un país...", Value = "0"});

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboStateAsync(int countryId)
        {
            List<SelectListItem> list = await _context.States.Where(state => state.Country.Id == countryId)
                .Select(state => new SelectListItem
            {
                Text = state.Name,
                Value = state.Id.ToString()
            })
            .OrderBy(state => state.Text)
            .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "Seleccione un estado/departamento...", Value = "0"});

            return list;
        }
    }
}
