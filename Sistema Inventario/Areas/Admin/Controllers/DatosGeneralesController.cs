using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;
using System.Data;
using System.Security.Claims;

namespace Sistema_Inventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class DatosGeneralesController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public DatosGeneralesController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public async Task<IActionResult> Upsert()
        {
            DatosGenerales datos = new DatosGenerales();

            datos = await _unidadTrabajo.DatosGenerales.ObtenerPrimero();

            if (datos == null)
            {
                datos = new SistemaInventario.Modelos.DatosGenerales();
            }

            return View(datos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(DatosGenerales datos)
        {
            if (ModelState.IsValid)
            {
                if (datos.Id == 0)
                {
                    await _unidadTrabajo.DatosGenerales.Agregar(datos);
                    TempData[DS.Exitosa] = "Informacion de la Comision creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.DatosGenerales.Actualizar(datos);
                    TempData[DS.Exitosa] = "Informacion de la Comision actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction("Index", "Home", new { area = "Inventario" });
            }
            TempData[DS.Error] = "Error al grabar Informacion de la Comision";
            return View(datos);
        }

    }
}
