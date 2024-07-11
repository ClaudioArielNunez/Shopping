using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class StateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Provincia/Estado")]
        [MaxLength(50, ErrorMessage ="El campo {0} debe tener como máximo {1} caratéres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        //Para agregar un estado necesito saber q país es
        public int CountryId { get; set; }

    }
}
