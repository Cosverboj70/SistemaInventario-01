using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;
using System.Data;

namespace Sistema_Inventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class CodigoFuenteController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CodigoFuenteController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            CodigoFuente codigoFuente = new CodigoFuente();

            if (id == null)
            {
                // Crear una nueva Bodega
                codigoFuente.Estado = true;
                return View(codigoFuente);
            }
            // Actualizamos Bodega
            codigoFuente = await _unidadTrabajo.CodigoFuente.Obtener(id.GetValueOrDefault());
            if (codigoFuente == null)
            {
                return NotFound();
            }
            return View(codigoFuente);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CodigoFuente codigoFuente)
        {
            //checar si funciona, eta linea la adicione yo
            codigoFuente.FechaCreacion = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (codigoFuente.Id == 0)
                {
                    
                    await _unidadTrabajo.CodigoFuente.Agregar(codigoFuente);
                    TempData[DS.Exitosa] = "Codigo Fuente creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.CodigoFuente.Actualizar(codigoFuente);
                    TempData[DS.Exitosa] = "Codigo Fuente actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Codigo Fuente";
            return View(codigoFuente);
        }


        #region API
        [HttpGet]
        public async Task<ActionResult> Obtenertodos()
        {
            var todos = await _unidadTrabajo.CodigoFuente.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var codigoFuenteDb = await _unidadTrabajo.CodigoFuente.Obtener(id);
            if (codigoFuenteDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Codigo Fuente" });
            }
            _unidadTrabajo.CodigoFuente.Remover(codigoFuenteDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Codigo Fuente borrada exitosamente" });
        }

        //ActionName==>Para poder ser llamado desde JS
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.CodigoFuente.ObtenerTodos();
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
