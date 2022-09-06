using RockolasMedina.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    [Security]
    public class VentaController : Controller
    {
        // GET: Venta
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarVentas()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Ventas.Where(p=> p.Habilitado.Equals(1)).Select(p=> new
            {
                p.IDventa,
                p.Color,
                p.Precio,
                p.NombreCliente,
                fechaVen = ((DateTime)p.FechaVenta).ToShortDateString(),
                p.IDrockola,
                p.IDtiporockola,
                p.Habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listarTipo()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.TipoRockolas.Select(p => new
            {
                ID = p.IDtiporockola,
                p.tipo
            });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult filtrarTipo(int idTipo)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Ventas.Where(p => p.IDtiporockola.Equals(idTipo)).Select(p => new
            {
                p.IDventa,               
                p.Color,
                p.Precio,
                p.NombreCliente,
                fechaVen = ((DateTime)p.FechaVenta).ToShortDateString(),
                p.IDrockola,
                p.IDtiporockola,
                p.Habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int borrar(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                Venta oVenta = bd.Ventas.Where(p => p.IDventa.Equals(id)).First();
                oVenta.Habilitado = 0;
                bd.SubmitChanges();
                nregistrosAfectados = 1;
            }
            catch (Exception ex)
            {
                nregistrosAfectados = 0;
            }
            return nregistrosAfectados;
        }

        public JsonResult recuperarInfo(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Ventas.Where(p => p.IDventa.Equals(id)).Select(p => new
            {
                p.IDventa,
                p.Color,
                p.Precio,
                p.NombreCliente,
                fechaVen = ((DateTime)p.FechaVenta).ToShortDateString(),
                p.IDrockola,
                p.IDtiporockola,
                p.Habilitado
            });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int guardarInfo(Venta oVenta)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                //Guarda nuevo
                int venta = oVenta.IDventa;
                if (venta.Equals(0))
                {
                    int nVeces = bd.Ventas.Where(p => p.IDrockola.Equals(oVenta.IDrockola)).Count();
                    if (nVeces.Equals(0))
                    {
                        bd.Ventas.InsertOnSubmit(oVenta);
                        bd.SubmitChanges();
                        nregistrosAfectados = 1;
                    }
                    else
                    {
                        nregistrosAfectados = -1;
                    }
                }
                //edita
                else
                {
                    int nVeces = bd.Ventas.Where(p => p.IDrockola.Equals(oVenta.IDrockola) && !p.IDventa.Equals(venta)).Count();
                    if (nVeces.Equals(0))
                    {
                        Venta obj = bd.Ventas.Where(p => p.IDventa.Equals(venta)).First();
                        obj.Color = oVenta.Color;
                        obj.Precio = oVenta.Precio;
                        obj.NombreCliente = oVenta.NombreCliente;
                        obj.FechaVenta = oVenta.FechaVenta;
                        obj.IDrockola = oVenta.IDrockola;
                        obj.IDtiporockola = oVenta.IDtiporockola;
                        obj.Habilitado = oVenta.Habilitado;
                        bd.SubmitChanges();
                        nregistrosAfectados = 1;
                    }
                    else
                    {
                        nregistrosAfectados = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                nregistrosAfectados = 0;
            }
            return nregistrosAfectados;
        }
    }
}