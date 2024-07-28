using System.ComponentModel.DataAnnotations;

namespace Shopping.Data.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        //Pertenece a un estado
        public State State { get; set; }

        //Luego de tener la clase user, agregamos esta prop
        public ICollection<User> Users { get; set; } //una ciudad puede tener muchos usuarios
    }
}
