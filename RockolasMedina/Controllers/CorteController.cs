using RockolasMedina.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    [Security]
    public class CorteController : Controller
    {
        
        // GET: Corte
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarRuta()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Rutas.Select(p => new
            {
                ID = p.IDruta,
                p.Nombre
            });
            return Json(lista, JsonRequestBehavior.AllowGet);        
        }

        public JsonResult listarCortes()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = (bd.Cortes.Where(p=> p.Habilitado.Equals(1)).Select(p => new
            {
                p.IDcorte,
                p.ContadorParcial,
                p.ContadorFinal,
                p.Efectivo,
                FechaCorte = ((DateTime)p.FechaCorte).ToShortDateString(),
                p.IDrockola,
                p.IDruta,
                p.IDtecnico,
                p.Habilitado
            })).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult filtrarRuta(int idruta)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Cortes.Where(p => p.IDruta.Equals(idruta)).Select(p => new
            {
                p.IDcorte,
                p.ContadorParcial,
                p.ContadorFinal,
                p.Efectivo,
                FechaCorte = ((DateTime)p.FechaCorte).ToShortDateString(),
                p.IDrockola,
                p.IDruta,
                p.IDtecnico,
                p.Habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int eliminar(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                Corte oCorte = bd.Cortes.Where(p => p.IDcorte.Equals(id)).First();
                oCorte.Habilitado = 0;
                bd.SubmitChanges();
                nregistrosAfectados = 1;
            }
            catch (Exception ex)
            {
                nregistrosAfectados = 0;
            }
            return nregistrosAfectados;
        }

        public JsonResult recuperarInformacion(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var consulta = bd.Cortes.Where(p => p.IDcorte.Equals(id)).Select(p => new
            {
                p.IDcorte,
                p.ContadorParcial,
                p.ContadorFinal,
                p.Efectivo,
                Fecha = ((DateTime)p.FechaCorte).ToShortDateString(),
                p.IDrockola,
                p.IDruta,
                p.IDtecnico,
                p.Habilitado
            });
            return Json(consulta, JsonRequestBehavior.AllowGet);
        }

        public int guardar(Corte oCorte)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                int idCorte = oCorte.IDcorte;
                if (idCorte == 0)
                {
                    int nVeces = bd.Cortes.Where(p => p.FechaCorte.Equals(oCorte.FechaCorte) && p.IDrockola.Equals(oCorte.IDrockola)).Count();
                    if (nVeces == 0)
                    {
                        //agregar nuevo
                        bd.Cortes.InsertOnSubmit(oCorte);
                        bd.SubmitChanges();
                        nregistrosAfectados = 1;
                    }
                    else
                    {
                        //El dato ya existe
                        nregistrosAfectados = -1;
                    }
                }
                else
                {
                    //Actualizar
                    int nVeces = bd.Cortes.Where(p => p.FechaCorte.Equals(oCorte.FechaCorte) && p.IDrockola.Equals(oCorte.IDrockola) && !p.IDcorte.Equals(idCorte)).Count();
                    if (nVeces == 0)
                    {
                        Corte obj = bd.Cortes.Where(p => p.IDcorte.Equals(idCorte)).First();
                        obj.ContadorParcial = oCorte.ContadorParcial;
                        obj.ContadorFinal = oCorte.ContadorFinal;
                        obj.Efectivo = oCorte.Efectivo;
                        obj.FechaCorte = oCorte.FechaCorte;
                        obj.IDrockola = oCorte.IDrockola;
                        obj.IDruta = oCorte.IDruta;
                        obj.IDtecnico = oCorte.IDtecnico;
                        obj.Habilitado = oCorte.Habilitado;
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