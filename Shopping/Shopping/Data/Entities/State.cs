using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "Provincia/Estado")]
        [MaxLength(50, ErrorMessage ="El campo {0} debe tener máximo {1} caractéres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        //Creamos relacion 1:N
        //Pertenece a 1 Country
        public Country Country { get; set; }

        //Relacion 1:N 
        //1 state tiene una coleccion de cities
        public ICollection<City> Cities { get; set; }

        //Cantidad de ciudades
        [Display(Name= "Ciudades")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}
