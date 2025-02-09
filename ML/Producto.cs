using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Producto
    {
        public int Id { get; set; }
        
        

        [Required(ErrorMessage = "El Nombre es obligatorio")]        
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre puede contener maximo 100 y minimo 3 caracteres")]
        [RegularExpression(@"^[a-zñA-ZÑáÁéÉíÍóÓúÚ\s]+$", ErrorMessage = "Nombre solo acepta letras")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "Descripción puede contener maximo 500 caracteres")]
        public string Descripcion { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo.")]
        public decimal Precio { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad en stock debe ser mayor o igual a 0.")]
        public int CantidadStock { get; set; }

        public DateTime FechaCreacion { get; set; }

        [NotMapped]
        public List<object> Productos { get; set; } = new List<object>();
    }
}
