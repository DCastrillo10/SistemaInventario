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

        [Required]
        [MaxLength(30,ErrorMessage ="El {0} debe ser hasta {1} caracteres")]
        [Display(Name = "Numero de Serie")]
        public string NumeroSerie { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El {0} debe ser hasta {1} caracteres")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }
        
        public string Tipo { get; set; }

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Precio")]
        public double Precio { get; set; }

        [Required]
        [Range(1,10000)]
        [Display(Name ="Costo")]
        public double Costo { get; set; }

        public string ImgUrl { get; set; }

        //Foreign Keys
        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }


        [Required]
        public int MarcaId { get; set; }
        
        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }

        //Recursividad
        public int? PadreId { get; set; }
        public virtual Producto Padre { get; set; }
    }
}
