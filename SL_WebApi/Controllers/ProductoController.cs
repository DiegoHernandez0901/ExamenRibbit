using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace SL_WebApi.Controllers
{
    [ApiController]    
    public class ProductoController : Controller
    {
        /// <summary>
        /// Agrega un nuevo producto.
        /// </summary>
        /// <param name="producto">El producto a agregar.</param>
        /// <returns></returns>
        [HttpPost]        
        [Route("api/Productos")]
        public IActionResult Add([FromBody] ML.Producto producto)
        {
            var result = BL.Producto.Add(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        /// <returns>Lista de productos.</returns>
        [HttpGet]        
        [Route("api/Productos")]
        public IActionResult GetAll()
        {
            var result = BL.Producto.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto.</param>
        /// <returns>Producto correspondiente al ID.</returns>        
        [HttpGet]
        [Route("/api/Productos/{id}")]
        public IActionResult GetById(int id)
        {
            var result = BL.Producto.GetById(id);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="producto">El producto con los nuevos datos.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut]        
        [Route("api/Productos/{id}")]
        public IActionResult Update(int id, [FromBody] ML.Producto producto)
        {
            producto.Id = id;
            var result = BL.Producto.Update(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete]        
        [Route("api/Productos/{id}")]
        public IActionResult Delete(int id)
        {
            var result = BL.Producto.Delete(id);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

