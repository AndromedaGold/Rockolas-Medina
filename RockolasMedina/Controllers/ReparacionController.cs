using RockolasMedina.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    [Security]
    public class ReparacionController : Controller
    {
        // GET: Reparacion
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarReparacion()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Reparacions.Where(p=> p.Habilitado.Equals(1)).Select(p => new
            {
                p.IDreparacion,
                p.ParteNombre,
                p.IDruta,
                p.IDtecnico,
                FechaReparacion = ((DateTime)p.FechaReparacion).ToShortDateString(),
                p.Comentario,
                p.IDrockola,
                p.Habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult buscarReparacion(string nombreParte)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Reparacions.Where(p => p.ParteNombre.Contains(nombreParte)).Select(p => new
                {
                p.IDreparacion,
                p.ParteNombre,
                p.IDruta,
                p.IDtecnico,
                FechaReparacion = ((DateTime)p.FechaReparacion).ToShortDateString(),
                p.Comentario,
                p.IDrockola,
                p.Habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult recuperarDatos(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Reparacions.Where(p => p.IDreparacion.Equals(id)).Select(p => new
            {
                p.IDreparacion,
                p.ParteNombre,
                p.IDruta,
                p.IDtecnico,
                FechaReparacion = ((DateTime)p.FechaReparacion).ToShortDateString(),
                p.Comentario,
                p.IDrockola,
                p.Habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);

        }

        public int guardarDatos(Reparacion oreparacion)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                //Inserta si es nuevo
                int idRep = oreparacion.IDreparacion;
                if ( idRep == 0)
                {
                    //valida si ya existe
                    int nVeces = bd.Reparacions.Where(p => p.FechaReparacion.Equals(oreparacion.FechaReparacion) && p.IDrockola.Equals(oreparacion.IDrockola)).Count();
                    if (nVeces == 0) {                       
                        bd.Reparacions.InsertOnSubmit(oreparacion);
                        bd.SubmitChanges();
                        nregistrosAfectados = 1;
                    }
                    else
                    {
                        nregistrosAfectados = -1;
                    }
                }
                //edita si ya existe
                else
                {
                    //valida si ya existe
                    int nVeces = bd.Reparacions.Where(p => p.FechaReparacion.Equals(oreparacion.FechaReparacion) && p.IDrockola.Equals(oreparacion.IDrockola) && !p.IDreparacion.Equals(idRep)).Count();
                    if (nVeces == 0)
                    {
                        Reparacion repSele = bd.Reparacions.Where(p => p.IDreparacion.Equals(oreparacion.IDreparacion)).First();
                        repSele.ParteNombre = oreparacion.ParteNombre;
                        repSele.IDruta = oreparacion.IDruta;
                        repSele.IDtecnico = oreparacion.IDtecnico;
                        repSele.FechaReparacion = oreparacion.FechaReparacion;
                        repSele.Comentario = oreparacion.Comentario;
                        repSele.IDrockola = oreparacion.IDrockola;
                        repSele.Habilitado = oreparacion.Habilitado;
                        bd.SubmitChanges();
                        nregistrosAfectados = 1;
                    }
                    else
                    {
                        nregistrosAfectados = -1;
                    }
                }
            }catch(Exception ex)
            {
                nregistrosAfectados = 0;
            }
            return nregistrosAfectados;
        }

        public int eliminarDatos(Reparacion id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                Reparacion repSele = bd.Reparacions.Where(p => p.IDreparacion.Equals(id.IDreparacion)).First();
                repSele.Habilitado = 0;
                bd.SubmitChanges();
                nregistrosAfectados = 1;
            }
            catch(Exception ex)
            {
                nregistrosAfectados = 0;
            }
            return nregistrosAfectados;
        }
    }
}