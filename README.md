# ExamenRibbit

1. Desacargar el zip o clonar el respositorio.
2. Extraer el zip y abrir la carpeta.
3. Una vez dentro de ExamenRibbit-master abrir el ExamenRibbit.sln
4. Ahora que ya abrió la solución en DL -> Conexión encontraremos lo necesario para crear la conexión a la base de datos.
5. Para poder ejecutar las migraciones y crear la base de datos vamos a la parte superior de la ventana.
6. En la parte superior dentro de Tools -> NuGet Package Manager encontraremos la consola de NuGet (Package manager console).
7. Cuando estemos en la consola verificaremos que nuestro proyecto fuente sea DL.
8. Ahora si podemos crear la migración con  Add-Migration InitialCreate -Project DL -StartupProject SL_WebApi.
9. Ya podemos crear la base de datos con Update-Database -Project DL -StartupProject SL_WebApi.
10. Ahora en Solution Explorer damos click contextual sobre la solución y nos dirigimos a Properties -> Configure Startup Pojects.
11. Seleccionamos la opción Multiple startup projects y sobre PL y SL_API seleccionamos Start.
12. Aplicamos los cambios y aceptamos.
13. Ahora ya podemos ejecutar el proyecto para utilizarlo.

Ejemplos de peticiones para el API

//Crear un nuevo producto 
http://localhost:5081/api/Productos
POST
En el body enviamos el json:
{
  "nombre": "string",
  "descripcion": "string",
  "precio": 1,
  "cantidadStock": 20  
}

//Obtener todos los productos
GET
http://localhost:5081/api/Productos

//Obtener producto por Id
GET
http://localhost:5081/api/Productos/1

//Modificar producto
PUT
http://localhost:5081/api/Productos/1
En el body enviamos el json:
{
  "nombre": "string",
  "descripcion": "string",
  "precio": 25,
  "cantidadStock": 80,
  "Productos": []
}

//Para eliminar un producto por Id
DELETE
http://localhost:5081/api/Productos/1
