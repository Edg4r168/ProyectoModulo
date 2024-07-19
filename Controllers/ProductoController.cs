using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoModulo.Models;
using ProyectoModulo.Services;

namespace ProyectoModulo.Controllers;

[Authorize]
public class ProductoController : Controller
{
    private readonly GenericRepositorie<Producto> _repositorie;

    private readonly ServicioFirebase _servicioFirebase;

    public ProductoController(GenericRepositorie<Producto> repositorie, ServicioFirebase servicioFirebase)
    {
        _repositorie = repositorie;
        _servicioFirebase = servicioFirebase;
    }

    public IActionResult Index()
    {
        return View();
    }


    // Metodo para listar todos los productos
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var productos = _repositorie.Query();

            return View(productos);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return View();
        }
    }

    // Metodo para mostrar vista crear producto
    public IActionResult Create()
    {
        return View();
    }

    // Metodo para procesar solicitud crear producto
    [HttpPost]
    public async Task<IActionResult> Create(Producto producto, IFormFile imagen)
    {
        try
        {
            string nombreImagen = "";
            Stream? streamImagen = null;

            if (imagen != null)
            {
                //Generar nombre de la imagen
                string nombre = Guid.NewGuid().ToString("N");
                string extension = Path.GetExtension(imagen.FileName);

                nombreImagen = string.Concat(nombre, extension);
                streamImagen = imagen.OpenReadStream();

                producto.NombreImagen = nombreImagen;
                // Subir imagen a FireBase
                string urlImagen = await _servicioFirebase.SubirImagen(streamImagen, "carpeta_producto", producto.NombreImagen!);
                producto.UrlImagen = urlImagen;
            }


            var productoCreado = await _repositorie.CreateAsync(producto);

            return RedirectToAction(nameof(GetAll));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return View(producto);
        }
    }

    // Metodo para mostrar vista editar producto
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var productoEditar = await
                _repositorie.GetAsync(p => p.Id == id) ?? throw new TaskCanceledException("No se encontro el producto");

            return View(productoEditar);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(GetAll));
        }
    }

    // Metodo para procesar solicitud editar producto
    [HttpPost]
    public async Task<IActionResult> Edit(
        IFormFile imagen, Producto producto
    )
    {
        try
        {
            Stream? streamImagen = null;

            var productoEditar = await
                _repositorie.GetAsync(p => p.Id == producto.Id) ?? throw new TaskCanceledException("No se encontro el producto");

            productoEditar.Nombre = producto.Nombre;
            productoEditar.Descripcion = producto.Descripcion;
            productoEditar.Stock = producto.Stock;
            productoEditar.Precio = producto.Precio;


            if (imagen != null)
            {
                string nombreImagen = string.Concat(
                    Guid.NewGuid().ToString("N"),
                    Path.GetExtension(imagen.FileName)
                );

                streamImagen = imagen.OpenReadStream();

                productoEditar.NombreImagen = string.IsNullOrEmpty(productoEditar.NombreImagen)
                    ? nombreImagen
                    : productoEditar.NombreImagen;

                productoEditar.UrlImagen = await _servicioFirebase.SubirImagen(
                    streamImagen,
                    "carpeta_producto",
                    nombreImagen
                );
            }

            await _repositorie.EditAsync(productoEditar);

            return RedirectToAction(nameof(GetAll));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            var mensaje = "Ha ocurrido un error al actualizar el producto";

            return View(producto);
        }
    }

    // Metodo para procesar solicitud eliminar producto
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var productoEliminar = await _repositorie.GetAsync(p => p.Id == id) 
                ?? throw new TaskCanceledException("No se encontro el producto");

            var reuslt = await _repositorie.DeleteAsync(productoEliminar);

            var mensaje = "El producto se ha eliminado";

            return RedirectToAction();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            bool reuslt = false;
            var mensaje = "Ha ocurrido un error al eliminar el producto";

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
