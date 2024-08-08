using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shopping.Helpers
{
    public interface IComboHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync();

        Task<IEnumerable<SelectListItem>> GetComboCountriesAsync();

        Task<IEnumerable<SelectListItem>> GetComboStateAsync(int countryId);

        Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId);
        
    }
}
