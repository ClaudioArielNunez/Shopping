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

        //Relacion 1:N 
        //1 pais tiene una coleccion de estados
        public ICollection<State> States { get; set;}

        //Numero de estados
        //public int StatesNumber => States.Count; //necesito operador ternario, ya que al inicio es null
        [Display(Name ="Provincias/Estados")]
        public int StatesNumber => States == null? 0 : States.Count;
    }
}
