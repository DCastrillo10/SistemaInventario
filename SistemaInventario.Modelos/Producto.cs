using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El {0} es requerido")]
        [MaxLength(30,ErrorMessage ="El {0} debe ser hasta {1} caracteres")]
        [Display(Name = "Numero de Serie")]
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El {0} debe ser hasta {1} caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        //[Range(1, 10000)]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        //[Range(1,10000)]
        public double Costo { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        public bool Estado { get; set; }

        public string ImgUrl { get; set; }


        //Foreign Keys
        [Required(ErrorMessage = "El {0} es requerido")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }


        [Required(ErrorMessage = "El {0} es requerido")]
        public int MarcaId { get; set; }
        
        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }

        //Recursividad
        public int? PadreId { get; set; }
        public virtual Producto Padre { get; set; }
    }
}
