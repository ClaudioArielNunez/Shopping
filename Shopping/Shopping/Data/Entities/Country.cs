using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name = "País")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]        
        [Required(ErrorMessage ="el campo {0} es obligatorio")]
        public string Name { get; set; }


    }
}
