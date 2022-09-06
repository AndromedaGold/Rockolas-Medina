using RockolasMedina.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    [Security]
    public class RentaController : Controller
    {
        // GET: Renta
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarRenta()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Rentas.Where(p => p.Habilitado.Equals(1)).Select(p => new
            {
                p.IDrenta,
                p.ClienteRentaNombre,
                p.DPInumero,
                p.TelefonoCliente,
                p.Direccion,
                p.IDrockola,
                p.IDruta,
                p.IDmunicipio,
                FechaRen = ((DateTime)p.FechaRenta).ToShortDateString(),
                p.Habilitado
            });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult buscarRenta(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Rentas.Where(p => p.IDrenta.Equals(id)).Select(p => new
            {
                p.IDrenta,
                p.ClienteRentaNombre,
                p.DPInumero,
                p.TelefonoCliente,
                p.Direccion,
                p.IDrockola,
                p.IDruta,
                p.IDmunicipio,
                FechaRen = ((DateTime)p.FechaRenta).ToShortDateString(),
                p.Habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult llamarInfo(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Rentas.Where(p => p.IDrenta.Equals(id)).Select(p => new
            {
                p.IDrenta,
                p.ClienteRentaNombre,
                p.DPInumero,
                p.TelefonoCliente,
                p.Direccion,
                p.IDrockola,
                p.IDruta,
                p.IDmunicipio,
                FechaRenta = ((DateTime)p.FechaRenta).ToShortDateString(),
                p.Habilitado
            });

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int guardar(Renta oRenta)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                //inserta si es nuevo
                int idRenta = oRenta.IDrenta;
                if ( idRenta.Equals(0))
                {
                    int nVeces = bd.Rentas.Where(p => p.Direccion.Equals(oRenta.Direccion) && p.IDrockola.Equals(oRenta.IDrockola)).Count();
                    if (nVeces.Equals(0))
                    {
                        bd.Rentas.InsertOnSubmit(oRenta);
                        bd.SubmitChanges();
                        nregistrosAfectados = 1;
                    }
                    else
                    {
                        //no agrega si ya existe
                        nregistrosAfectados = -1;
                    }
                }
                //Edita si ya existe
                else
                {
                    int nVeces = bd.Rentas.Where(p => p.Direccion.Equals(oRenta.Direccion) && p.IDrockola.Equals(oRenta.IDrockola) && !p.IDrenta.Equals(idRenta)).Count();
                    if (nVeces == 0)
                    {
                        Renta obj = bd.Rentas.Where(p => p.IDrenta.Equals(oRenta.IDrenta)).First();
                        obj.ClienteRentaNombre = oRenta.ClienteRentaNombre;
                        obj.DPInumero = oRenta.DPInumero;
                        obj.TelefonoCliente = oRenta.TelefonoCliente;
                        obj.Direccion = oRenta.Direccion;
                        obj.IDrockola = oRenta.IDrockola;
                        obj.IDruta = oRenta.IDruta;
                        obj.IDmunicipio = oRenta.IDmunicipio;
                        obj.FechaRenta = oRenta.FechaRenta;
                        obj.Habilitado = oRenta.Habilitado;
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

        public int eliminar(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                Renta oRenta = bd.Rentas.Where(p => p.IDrenta.Equals(id)).First();
                oRenta.Habilitado = 0;
                bd.SubmitChanges();
                nregistrosAfectados = 1;
            }
            catch (Exception ex)
            {
                nregistrosAfectados = 0;
            }
            return nregistrosAfectados;
        }
    }
}