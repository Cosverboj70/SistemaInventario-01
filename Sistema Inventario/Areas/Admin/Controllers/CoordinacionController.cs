using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace Sistema_Inventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoordinacionController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CoordinacionController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region API
        [HttpGet]
        public async Task<ActionResult> Obtenertodos()
        {
            var todos = await _unidadTrabajo.Coordinacion.ObtenerTodos();
            return Json(new { data = todos });
        }
        #endregion

        //public async Task<IActionResult> Upsert(int? id)
        //{
        //    Bodega bodega = new Bodega();

        //    if (id == null)
        //    {
        //        // Crear una nueva Bodega
        //        bodega.Estado = true;
        //        return View(bodega);
        //    }
        //    // Actualizamos Bodega
        //    bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
        //    if (bodega == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(bodega);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Upsert(Bodega bodega)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (bodega.Id == 0)
        //        {
        //            await _unidadTrabajo.Bodega.Agregar(bodega);
        //            TempData[DS.Exitosa] = "Bodega creada Exitosamente";
        //        }
        //        else
        //        {
        //            _unidadTrabajo.Bodega.Actualizar(bodega);
        //            TempData[DS.Exitosa] = "Bodega actualizada Exitosamente";
        //        }
        //        await _unidadTrabajo.Guardar();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    TempData[DS.Error] = "Error al grabar Bodega";
        //    return View(bodega);
        //}


    }
}
