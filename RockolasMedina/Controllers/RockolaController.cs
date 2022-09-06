using RockolasMedina.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    [Security]
    public class RockolaController : Controller
    {
        // GET: Rockola
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarRockola()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Rockolas.Where(p=> p.habilitado.Equals(1)).Select(p => new
            {
                p.IDrockola,
                p.motherb,
                p.ram,
                p.procesador,
                p.fuentep,
                p.hddcapacidad,
                p.monitor,         
                p.teclado,
                fechaFabricacion = ((DateTime)p.fechaFabricacion).ToShortDateString(),
                p.habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult buscarRockola(int nombre)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Rockolas.Where(p => p.IDrockola.Equals(nombre))
                .Select(p => new {
                p.IDrockola,
                p.motherb,
                p.ram,
                p.procesador,
                p.fuentep,
                p.hddcapacidad,
                p.monitor,
                p.teclado,
                fechaFabricacion = ((DateTime)p.fechaFabricacion).ToShortDateString(),
                p.habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int eliminar(Rockola orockola)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                int idrockola = orockola.IDrockola;
                Rockola obj = bd.Rockolas.Where(p => p.IDrockola.Equals(idrockola)).First();
                obj.habilitado = 0;
                bd.SubmitChanges();
                nregistrosAfectados = 1;
            }
            catch (Exception ex)
            {

            }
            return nregistrosAfectados;
        }

        public JsonResult llamarInfo(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Rockolas.Where(p => p.IDrockola.Equals(id)).Select(p => new
            {
                p.IDrockola,
                p.motherb,
                p.ram,
                p.procesador,
                p.fuentep,
                p.hddcapacidad,
                p.monitor,
                p.teclado,
                fechaFabricacion = ((DateTime)p.fechaFabricacion).ToShortDateString(),
                p.habilitado
            });

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int guardar(Rockola oRockola)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                //inserta si es nuevo
                if(oRockola.IDrockola == 0)
                {
                    bd.Rockolas.InsertOnSubmit(oRockola);
                    bd.SubmitChanges();
                    nregistrosAfectados = 1;
                }
                //Edita si ya existe
                else
                {
                    Rockola obj = bd.Rockolas.Where(p => p.IDrockola.Equals(oRockola.IDrockola)).First();
                    obj.motherb = oRockola.motherb;
                    obj.ram = oRockola.ram;
                    obj.procesador = oRockola.procesador;
                    obj.fuentep = oRockola.fuentep;
                    obj.hddcapacidad = oRockola.hddcapacidad;
                    obj.monitor = oRockola.monitor;
                    obj.teclado = oRockola.teclado;
                    obj.fechaFabricacion = oRockola.fechaFabricacion;
                    obj.habilitado = oRockola.habilitado;

                    bd.SubmitChanges();
                    nregistrosAfectados = 1;
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