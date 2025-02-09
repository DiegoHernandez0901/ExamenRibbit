
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ML;
using PL.Models;
namespace PL.Controllers
{
    public class ProductosController : Controller
    {
        private readonly string _endpoint;       
        public ProductosController(IConfiguration configuration)
        {
            _endpoint = configuration["AppSettings:EndPointProducto"];
             
        }


        public IActionResult Index()
        {
            ML.Producto producto = new ML.Producto();
            producto.Productos = new List<object>();
            ML.Result result = new ML.Result();
            try
            {
                using (var client = new HttpClient())
                {


                    client.BaseAddress = new Uri(_endpoint);
                    var responseTask = client.GetAsync("Productos");
                    responseTask.Wait();
                    var resultServicio = responseTask.Result;
                    if (resultServicio.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                        var readTask = resultServicio.Content.ReadFromJsonAsync<ML.Result>();
                        readTask.Wait();
                        result.Objects = new List<object>();
                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Producto resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                        producto.Productos = result.Objects;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View(producto);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            ML.Result result = new ML.Result();
            ML.Producto producto = new ML.Producto();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_endpoint);
                    var responseTask = client.GetAsync("Productos/" + Id);
                    responseTask.Wait();
                    var resultServicio = responseTask.Result;
                    if (resultServicio.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                        var readTask = resultServicio.Content.ReadFromJsonAsync<ML.Result>();
                        readTask.Wait();
                        ML.Producto resultItem = new ML.Producto();
                        resultItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(readTask.Result.Object.ToString());
                        result.Object = resultItem;
                        result.Correct = true;
                        producto = (ML.Producto)result.Object;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existen registros en la tabla productos";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View(producto);
        }

        [HttpGet]
        public IActionResult Form(int? Id, ML.Producto? productoVS)
        {
            ML.Producto producto = new ML.Producto();
            if (Id != null)
            {
                ML.Result result = new ML.Result();
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_endpoint);
                        var responseTask = client.GetAsync("Productos/" + Id.Value);
                        responseTask.Wait();
                        var resultServicio = responseTask.Result;
                        if (resultServicio.IsSuccessStatusCode)
                        {
                            result.Correct = true;
                            var readTask = resultServicio.Content.ReadFromJsonAsync<ML.Result>();
                            readTask.Wait();
                            ML.Producto resultItem = new ML.Producto();
                            resultItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(readTask.Result.Object.ToString());
                            result.Object = resultItem;
                            result.Correct = true;
                            producto = (ML.Producto)result.Object;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No existen registros en la tabla productos";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                producto = productoVS;
            }

            return View(producto);
        }

        [HttpPost]
        public IActionResult Create(ML.Producto producto)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_endpoint);
                    var postTask = client.PostAsJsonAsync<ML.Producto>("Productos", producto);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Producto Agregado correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "ocurrio un error al agregar el producto";
                    }
                }
                return PartialView("Modal");
            }
            else
            {
                
                var errorMessages = new List<string>();
                foreach (
                    var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    errorMessages.Add(error.ErrorMessage);
                }

                foreach (var errorMessage in errorMessages)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            return View("Form", producto);
        }

        [HttpPost]
        public JsonResult Edit(ML.Producto producto)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    producto.Productos = new List<object>();
                    client.BaseAddress = new Uri(_endpoint);
                    var putTask = client.PutAsJsonAsync<ML.Producto>($"Productos/{producto.Id}", producto);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        // Devuelve un objeto JSON indicando éxito
                        return Json(new { success = true, message = "Producto modificado correctamente." });
                    }
                    else
                    {
                        // Devuelve un objeto JSON indicando error
                        return Json(new { success = false, message = "Ocurrió un error al modificar el producto." });
                    }
                }                
            }
            else
            {
                var errorMessages = new List<string>();
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    errorMessages.Add(error.ErrorMessage);
                }

                foreach (var errorMessage in errorMessages)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
                return Json(new { success = false, message = string.Join(", ", errorMessages) });
            }

            


        }

        [HttpGet]
        public IActionResult Delete(int Id, string Nombre)
        {
            if (Nombre != null)
            {
                ML.Producto producto = new ML.Producto();
                producto.Id = Id;
                producto.Nombre = Nombre;
                return View(producto);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_endpoint);
                    var deleteTask = client.DeleteAsync($"Productos/{Id}");
                    deleteTask.Wait();

                    var result = deleteTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Producto eliminado correctamente";
                        
                    }
                    else
                    {
                        ViewBag.Message = "Ocurrió un error al eliminar el producto";
                        
                    }
                    
                }
                return PartialView("Modal");
                
            }
            
        }

        
    }
}
