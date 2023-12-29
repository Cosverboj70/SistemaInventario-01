using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Data;

namespace Sistema_Inventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventario)]
    public class MunicipioController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MunicipioController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {

            MunicipioVM municipioVM = new MunicipioVM()
            {
                Municipio = new Municipio(),
                CoordinacionLista = _unidadTrabajo.Municipio.ObtenerTodosDropdownLista("Coordinacion")
            };

            if (id == null)
            {
                // Crear nuevo Producto
                municipioVM.Municipio.Estado = true;
                return View(municipioVM);
            }
            else
            {
                municipioVM.Municipio = await _unidadTrabajo.Municipio.Obtener(id.GetValueOrDefault());
                if (municipioVM.Municipio == null)
                {
                    return NotFound();
                }
                return View(municipioVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(MunicipioVM municipioVM)
        {
            if (ModelState.IsValid)
            {
                if (municipioVM.Municipio.Id == 0)
                {
                    //pasando al modelo el nombre del archivo
                    await _unidadTrabajo.Municipio.Agregar(municipioVM.Municipio);
                }
                else
                {
                    // Actualizar el Producto

                    var objMunicipio = await _unidadTrabajo.Municipio.ObtenerPrimero(p => p.Id == municipioVM.Municipio.Id, isTracking: false);
                    _unidadTrabajo.Municipio.Actualizar(municipioVM.Municipio);
                }
                TempData[DS.Exitosa] = "Transaccion Exitosa!";
                await _unidadTrabajo.Guardar();
                //return View("Index");
                return RedirectToAction("Index");

            }  // If not Valid
            
            //Si no es valida la grabacion, se cargan de  nuevo los Selects
            municipioVM.CoordinacionLista = _unidadTrabajo.Municipio.ObtenerTodosDropdownLista("Coordinacion");
            return View(municipioVM);
        }





        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Municipio.ObtenerTodos(incluirPropiedades: "Coordinacion");
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var municipioDb = await _unidadTrabajo.Municipio.Obtener(id);
            if (municipioDb == null)
            {
                return Json(new { success = false, message = "Error al borrar el Municipio" });
            }

            _unidadTrabajo.Municipio.Remover(municipioDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Municipio borrado exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Municipio.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }

        #endregion

    }
}
