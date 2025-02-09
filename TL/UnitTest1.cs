using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using PL.Controllers;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace TL
{
    public class ProductosControllerTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly ProductosController _controller;

        public ProductosControllerTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["AppSettings:EndPointProducto"]).Returns("http://localhost:5000/");
            _controller = new ProductosController(_configurationMock.Object);
        }

        [Fact]
        public void Create_ReturnsViewWithModel_WhenPriceIsNegative()
        {
            // Arrange
            ML.Producto producto = new ML.Producto();
            producto.Nombre = "Manzana";
            producto.Descripcion = "Roja";
            producto.Precio = -10;
            producto.CantidadStock = 50;
            producto.FechaCreacion = DateTime.Now;

            _controller.ModelState.AddModelError("Precio", "El precio no puede ser negativo");
            // Act
            var result = _controller.Create(producto) as ViewResult;

            Assert.NotNull(result); // Verifica que el resultado no sea nulo
            Assert.Equal("Form", result.ViewName); // Verifica que se devuelva la vista correcta
            Assert.False(_controller.ModelState.IsValid); // Verifica que el modelo no es válido
            
        }

        [Fact]
        public void Create_ReturnsViewWithModel_WhenNameIsEmpty()
        {
            // Arrange
            ML.Producto producto = new ML.Producto();
            producto.Nombre = "";
            producto.Descripcion = "Roja";
            producto.Precio = 10;
            producto.CantidadStock = 50;
            producto.FechaCreacion = DateTime.Now;

            _controller.ModelState.AddModelError("Nombre", "El nombre no puede estar vacio");
            // Act
            var result = _controller.Create(producto) as ViewResult;

            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Form", result.ViewName);            
            
        }

        [Fact]
        public void Create_ReturnsViewWithModel_WhenNameIsTooShort()
        {
            // Arrange
            ML.Producto producto = new ML.Producto();
            producto.Nombre = "AB";
            producto.Descripcion = "Roja";
            producto.Precio = 10;
            producto.CantidadStock = 50;
            producto.FechaCreacion = DateTime.Now;

            _controller.ModelState.AddModelError("Nombre", "El nombre es demasiado corto");
            // Act
            var result = _controller.Create(producto) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Form", result.ViewName);            
            
        }


    }

    // Clase para simular respuestas HTTP
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly object _responseContent;

        public FakeHttpMessageHandler(HttpStatusCode statusCode, object responseContent)
        {
            _statusCode = statusCode;
            _responseContent = responseContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = JsonContent.Create(_responseContent)
            };
            return Task.FromResult(response);
        }
    }
}