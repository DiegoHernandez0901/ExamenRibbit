using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BL
{
    public class Producto
    {
        public static ML.Result Add(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.Conexion>();
                optionsBuilder.UseSqlite("Data Source=mydatabase.db");
                using (DL.Conexion context = new DL.Conexion(optionsBuilder.Options))
                {
                    DL.Conexion.Producto nuevoProducto = new DL.Conexion.Producto();
                    nuevoProducto.Nombre = producto.Nombre;
                    nuevoProducto.Descripcion = producto.Descripcion;
                    nuevoProducto.Precio = producto.Precio;
                    nuevoProducto.CantidadStock = producto.CantidadStock;
                    nuevoProducto.FechaCreacion = DateTime.Now;

                    context.Productos.Add(nuevoProducto);
                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo insertar";
                    }

                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.Conexion>();
                optionsBuilder.UseSqlite("Data Source=mydatabase.db");
                using (DL.Conexion context = new DL.Conexion(optionsBuilder.Options))
                {
                    var listaProducto = (from producto in context.Productos
                                         select new
                                         {
                                             producto.Id,
                                             producto.Nombre,
                                             producto.Descripcion,
                                             producto.Precio,
                                             producto.CantidadStock,
                                             producto.FechaCreacion
                                         }).ToList();

                    if (listaProducto.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (var producto in listaProducto)
                        {
                            ML.Producto item = new ML.Producto();
                            item.Id = producto.Id;
                            item.Nombre = producto.Nombre;
                            item.Descripcion = producto.Descripcion;
                            item.Precio = producto.Precio;
                            item.CantidadStock = producto.CantidadStock;
                            item.FechaCreacion = producto.FechaCreacion;

                            result.Objects.Add(item);
                        }
                        result.Correct = true;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result GetById(int Id)
        {
            ML.Result result = new ML.Result();
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.Conexion>();
                optionsBuilder.UseSqlite("Data Source=mydatabase.db");
                using (DL.Conexion context = new DL.Conexion(optionsBuilder.Options))
                {
                    var productoItem = (from producto in context.Productos
                                        where producto.Id == Id
                                        select new
                                        {
                                            producto.Id,
                                            producto.Nombre,
                                            producto.Descripcion,
                                            producto.Precio,
                                            producto.CantidadStock,
                                            producto.FechaCreacion
                                        }).FirstOrDefault();

                    if (productoItem != null)
                    {
                        ML.Producto item = new ML.Producto();
                        item.Id = productoItem.Id;
                        item.Nombre = productoItem.Nombre;
                        item.Descripcion = productoItem.Descripcion;
                        item.Precio = productoItem.Precio;
                        item.CantidadStock = productoItem.CantidadStock;
                        item.FechaCreacion = productoItem.FechaCreacion;

                        result.Object = item;
                        result.Correct = true;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result Update(ML.Producto productoML)
        {
            ML.Result result = new ML.Result();

            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.Conexion>();
                optionsBuilder.UseSqlite("Data Source=mydatabase.db");
                using (DL.Conexion context = new DL.Conexion(optionsBuilder.Options))
                {
                    var productoResult = (from producto in context.Productos
                                          where producto.Id == productoML.Id
                                          select producto).SingleOrDefault();
                    if (productoResult != null)
                    {
                        productoResult.Id = productoML.Id;
                        productoResult.Nombre = productoML.Nombre;
                        productoResult.Descripcion = productoML.Descripcion;
                        productoResult.CantidadStock = productoML.CantidadStock;
                        productoResult.Precio = productoML.Precio;                       
                    }

                    int rowsAffected = context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo actualizar el producto";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result Delete(int Id)
        {
            ML.Result result = new ML.Result();
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<DL.Conexion>();
                optionsBuilder.UseSqlite("Data Source=mydatabase.db");
                using (DL.Conexion context = new DL.Conexion(optionsBuilder.Options))
                {
                    var productoResult = (from producto in context.Productos
                                          where producto.Id == Id
                                          select producto).SingleOrDefault();

                    if (productoResult != null)
                    {
                        context.Productos.Remove(productoResult);
                        int RowsAffected = context.SaveChanges();

                        if (RowsAffected > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo eliminar el producto";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;

            }
            return result;
        }
    }
}