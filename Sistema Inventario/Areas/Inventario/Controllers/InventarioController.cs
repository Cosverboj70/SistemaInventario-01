using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
//using Rotativa.AspNetCore;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Globalization;
using System.Security.Claims;

namespace Sistema_Inventario.Areas.Inventario.Controllers
{
	[Area("Inventario")]
	[Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventario)]
	public class InventarioController : Controller
	{

		private readonly IUnidadTrabajo _unidadTrabajo;

		[BindProperty]
		public InventarioVM inventarioVM { get; set; }

		public InventarioController(IUnidadTrabajo unidadTrabajo)
		{
			_unidadTrabajo = unidadTrabajo;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult NuevoInventario()
		{
			inventarioVM = new InventarioVM()
			{
				Inventario = new SistemaInventario.Modelos.Inventario(),
				BodegaLista = _unidadTrabajo.Inventario.ObtenerTodosDropdownLista("Bodega")
			};

			inventarioVM.Inventario.Estado = false;
			// Obtener el Id del Usuario desde la sesion
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
			inventarioVM.Inventario.UsuarioAplicacionId = claim.Value;
			inventarioVM.Inventario.FechaInicial = DateTime.Now;
			inventarioVM.Inventario.FechaFinal = DateTime.Now;

			return View(inventarioVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> NuevoInventario(InventarioVM inventarioVM)
		{
			if (ModelState.IsValid)
			{
				inventarioVM.Inventario.FechaInicial = DateTime.Now;
				inventarioVM.Inventario.FechaFinal = DateTime.Now;
				await _unidadTrabajo.Inventario.Agregar(inventarioVM.Inventario);
				await _unidadTrabajo.Guardar();
				return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
			}
			inventarioVM.BodegaLista = _unidadTrabajo.Inventario.ObtenerTodosDropdownLista("Bodega");
			return View(inventarioVM);
		}

		public async Task<IActionResult> DetalleInventario(int id)
		{
			inventarioVM = new InventarioVM();
			inventarioVM.Inventario = await _unidadTrabajo.Inventario.ObtenerPrimero(i => i.Id == id, incluirPropiedades: "Bodega");
			inventarioVM.InventarioDetalles = await _unidadTrabajo.InventarioDetalle.ObtenerTodos(d => d.InventarioId == id,
																							   incluirPropiedades: "Producto,Producto.Marca");
			return View(inventarioVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DetalleInventario(int InventarioId, int productoId, int cantidadId)
		{
            //int InventarioId, int productoId, int cantidadId==>Estan declarados en la vista con "name" y se pasan
			// a este metodo que es POST, para que se grabe la informacion
            inventarioVM = new InventarioVM();
			
			//llenado del inventario
			inventarioVM.Inventario = await _unidadTrabajo.Inventario.ObtenerPrimero(i => i.Id == InventarioId);
			
			//Filtrado si existe producto y en la bodega correspondiente
			var bodegaProducto = await _unidadTrabajo.BodegaProducto.ObtenerPrimero(b => b.ProductoId == productoId &&
																						b.BodegaId == inventarioVM.Inventario.BodegaId);
			//Verificando si existe un detalle del nuevo inventario para este producto
			var detalle = await _unidadTrabajo.InventarioDetalle.ObtenerPrimero(d => d.InventarioId == InventarioId &&
																				   d.ProductoId == productoId);
			if (detalle == null)
			{
				//nuevo producto, nuevo detalle
				inventarioVM.InventarioDetalle = new InventarioDetalle();
				inventarioVM.InventarioDetalle.ProductoId = productoId;
				inventarioVM.InventarioDetalle.InventarioId = InventarioId;
				if (bodegaProducto != null)
				{
					inventarioVM.InventarioDetalle.StockAnterior = bodegaProducto.Cantidad;
				}
				else
				{
					inventarioVM.InventarioDetalle.StockAnterior = 0;
				}
				inventarioVM.InventarioDetalle.Cantidad = cantidadId;
				await _unidadTrabajo.InventarioDetalle.Agregar(inventarioVM.InventarioDetalle);
				await _unidadTrabajo.Guardar();
			}
			else
			{
				//el producto ya existe, detalle existente
				detalle.Cantidad += cantidadId;
				await _unidadTrabajo.Guardar();
			}

			//Redireccionando a la vista, pasando el Id del Inventario
			return RedirectToAction("DetalleInventario", new { id = InventarioId });
		}


		public async Task<IActionResult> Mas(int id)  // recibe el id del detalle
		{
			inventarioVM = new InventarioVM();
			var detalle = await _unidadTrabajo.InventarioDetalle.Obtener(id);
			inventarioVM.Inventario = await _unidadTrabajo.Inventario.Obtener(detalle.InventarioId);

			detalle.Cantidad += 1;
			await _unidadTrabajo.Guardar();
			return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
		}

		public async Task<IActionResult> Menos(int id)  // recibe el id del detalle
		{
			inventarioVM = new InventarioVM();
			var detalle = await _unidadTrabajo.InventarioDetalle.Obtener(id);
			inventarioVM.Inventario = await _unidadTrabajo.Inventario.Obtener(detalle.InventarioId);
			if (detalle.Cantidad == 1)
			{
				_unidadTrabajo.InventarioDetalle.Remover(detalle);
				await _unidadTrabajo.Guardar();
			}
			else
			{
				detalle.Cantidad -= 1;
				await _unidadTrabajo.Guardar();
			}

			return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
		}

		public async Task<IActionResult> GenerarStock(int id)
		{
			var inventario = await _unidadTrabajo.Inventario.Obtener(id);
			var detalleLista = await _unidadTrabajo.InventarioDetalle.ObtenerTodos(d => d.InventarioId == id);
			
			// Obtener el Id del Usuario desde la sesion
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

			//Recorrer Lista
			foreach (var item in detalleLista)
			{
				var bodegaProducto = new BodegaProducto();
				bodegaProducto = await _unidadTrabajo.BodegaProducto.ObtenerPrimero(b => b.ProductoId == item.ProductoId &&
																						 b.BodegaId == inventario.BodegaId);
				if (bodegaProducto != null) //  El registro de Stock existe, hay que actualizar las cantidades
				{
					await _unidadTrabajo.KardexInventario.RegistrarKardex(bodegaProducto.Id, "Entrada", "Registro de Inventario",
																		  bodegaProducto.Cantidad, item.Cantidad, claim.Value);
					bodegaProducto.Cantidad += item.Cantidad;
					await _unidadTrabajo.Guardar();

				}
				else  // Registro de Stock no existe, hay que crearlo
				{
					bodegaProducto = new BodegaProducto();
					bodegaProducto.BodegaId = inventario.BodegaId;
					bodegaProducto.ProductoId = item.ProductoId;
					bodegaProducto.Cantidad = item.Cantidad;
					await _unidadTrabajo.BodegaProducto.Agregar(bodegaProducto);
					await _unidadTrabajo.Guardar();
					await _unidadTrabajo.KardexInventario.RegistrarKardex(bodegaProducto.Id, "Entrada", "Inventario Inicial",
																		 0, item.Cantidad, claim.Value);
				}

			}
			// Actualizar la Cabecera de Inventario
			inventario.Estado = true;
			inventario.FechaFinal = DateTime.Now;
			await _unidadTrabajo.Guardar();

			//Variable que se pasa a la Vista para que mande el Mensaje 
			TempData[DS.Exitosa] = "Stock Generado con Exito";
			return RedirectToAction("Index");

		}

		public IActionResult KardexProducto()
		{
			return View();
		}

		[HttpPost]
		//Revibe los parametros de lA VISTA
		public IActionResult KardexProducto(string fechaInicioId, string fechaFinalId, int productoId)
		{
            //KardexProductoResultado==>ES REDIRECCIONADO A ESTE PROCEDIMIENTO, QUE SE ENCUENTRA ENSEGUIDA CON LOS PARAMETROS CORRESPONDIENTE
            return RedirectToAction("KardexProductoResultado", new { fechaInicioId, fechaFinalId, productoId });
		}

		public async Task<IActionResult> KardexProductoResultado(string fechaInicioId, string fechaFinalId, int productoId)
		{
			KardexInventarioVM kardexInventarioVM = new KardexInventarioVM();
			kardexInventarioVM.Producto = new Producto();
			kardexInventarioVM.Producto = await _unidadTrabajo.Producto.Obtener(productoId);

            //DateTime.Parse(fechaInicioId)==>CONVERSION DE STRING A FEHAS
            kardexInventarioVM.FechaInicio = DateTime.Parse(fechaInicioId); //  00:00:00
			kardexInventarioVM.FechaFinal = DateTime.Parse(fechaFinalId).AddHours(23).AddMinutes(59);

            //ObtenerTodos==>CON FILTRO O CONDICIONES AND POR FECHAS
            //incluirPropiedades:==>INCLUYE LOS NOMBRES POR MEDIO DE LA NAVEGACION
            //orderBy: o => o.OrderBy(o => o.FechaRegistro)==<ORDENACION POR FECHA DE REGISTRO
            kardexInventarioVM.KardexInventarioLista = await _unidadTrabajo.KardexInventario.ObtenerTodos(
																   k => k.BodegaProducto.ProductoId == productoId &&
																	   (k.FechaRegistro >= kardexInventarioVM.FechaInicio &&
																		k.FechaRegistro <= kardexInventarioVM.FechaFinal),
											incluirPropiedades: "BodegaProducto,BodegaProducto.Producto,BodegaProducto.Bodega",
											orderBy: o => o.OrderBy(o => o.FechaRegistro)
				);

			return View(kardexInventarioVM);
		}

		public async Task<IActionResult> ImprimirKardex(string fechaInicio, string fechaFinal, int productoId)
		{
			KardexInventarioVM kardexInventarioVM = new KardexInventarioVM();
			kardexInventarioVM.Producto = new Producto();
			kardexInventarioVM.Producto = await _unidadTrabajo.Producto.Obtener(productoId);

			kardexInventarioVM.FechaInicio = DateTime.Parse(fechaInicio);
			kardexInventarioVM.FechaFinal = DateTime.Parse(fechaFinal);

			kardexInventarioVM.KardexInventarioLista = await _unidadTrabajo.KardexInventario.ObtenerTodos(
																   k => k.BodegaProducto.ProductoId == productoId &&
																	   (k.FechaRegistro >= kardexInventarioVM.FechaInicio &&
																		k.FechaRegistro <= kardexInventarioVM.FechaFinal),
											incluirPropiedades: "BodegaProducto,BodegaProducto.Producto,BodegaProducto.Bodega",
											orderBy: o => o.OrderBy(o => o.FechaRegistro)
				);

			return new ViewAsPdf("ImprimirKardex", kardexInventarioVM)
			{
				FileName = "KardexProducto.pdf",
				PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
				PageSize = Rotativa.AspNetCore.Options.Size.A4,
				CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
			};
		}

		public async Task<IActionResult> ImprimirKardexEs(string fechaInicio, string fechaFinal, int productoId)
		{
			KardexInventarioVM kardexInventarioVM = new KardexInventarioVM();
			kardexInventarioVM.Producto = new Producto();
			kardexInventarioVM.Producto = await _unidadTrabajo.Producto.Obtener(productoId);

			var cultureInfo = CultureInfo.CreateSpecificCulture("es-ES");

			kardexInventarioVM.FechaInicio = DateTime.ParseExact(fechaInicio, "dd.MM.yyyy HH:mm:ss", cultureInfo);
			kardexInventarioVM.FechaFinal = DateTime.ParseExact(fechaFinal, "dd.MM.yyyy HH:mm:ss", cultureInfo);

			kardexInventarioVM.KardexInventarioLista = await _unidadTrabajo.KardexInventario.ObtenerTodos(
																   k => k.BodegaProducto.ProductoId == productoId &&
																	   (k.FechaRegistro >= kardexInventarioVM.FechaInicio &&
																		k.FechaRegistro <= kardexInventarioVM.FechaFinal),
											incluirPropiedades: "BodegaProducto,BodegaProducto.Producto,BodegaProducto.Bodega",
											orderBy: o => o.OrderBy(o => o.FechaRegistro)
				);

			return new ViewAsPdf("ImprimirKardex", kardexInventarioVM)
			{
				FileName = "KardexProducto.pdf",
				PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
				PageSize = Rotativa.AspNetCore.Options.Size.A4,
				CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
			};
		}

		#region API
		[HttpGet]
		public async Task<IActionResult> ObtenerTodos()
		{
			//incluirPropiedades: "Bodega,Producto"==>TRAE O INCLUYE EL NOMBRE DE LA BODEGA Y EL PRODUCTO 
			var todos = await _unidadTrabajo.BodegaProducto.ObtenerTodos(incluirPropiedades: "Bodega,Producto");
			return Json(new { data = todos });
		}

		[HttpGet]
		public async Task<IActionResult> BuscarProducto(string term)
		{
			if (!string.IsNullOrEmpty(term))
			{
				var listaProductos = await _unidadTrabajo.Producto.ObtenerTodos(p => p.Estado == true);

				//Para filtrar la lista por numero de serie o descripcion, ignorando mayusculas o minusculas
				var data = listaProductos.Where(x => x.NumeroSerie.Contains(term, StringComparison.OrdinalIgnoreCase) ||
													 x.Descripcion.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
				return Ok(data);
			}
			return Ok();
		}

		#endregion
	}
}
