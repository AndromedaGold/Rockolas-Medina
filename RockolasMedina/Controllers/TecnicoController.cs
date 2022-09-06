using RockolasMedina.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    [Security]
    public class TecnicoController : Controller
    {
        // GET: Tecnico
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarTecnico()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Tecnicos.Where(p=> p.IDestado.Equals(1)).Select(p => new
            {
                p.IDtecnico,
                p.Nombre,
                p.Apellido,
                p.Telefono,
                FechaAlt = ((DateTime)p.FechaAlt).ToShortDateString(),
                p.IDestado
            }).ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listarEstado()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Estados.Select(p => new
            {
                ID = p.IDestado,
                p.Estados
            });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult filtrarEstado(int idestado)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Tecnicos.Where(p => p.IDestado.Equals(idestado)).Select(p => new
            {
                p.IDtecnico,
                p.Nombre,
                p.Apellido,
                p.Telefono,
                FechaAlt = ((DateTime)p.FechaAlt).ToShortDateString(),
                p.IDestado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int eliminar(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                Tecnico oTecnico = bd.Tecnicos.Where(p => p.IDtecnico.Equals(id)).First();
                oTecnico.IDestado = 0;
                bd.SubmitChanges();
                nregistrosAfectados = 1;
            }
            catch (Exception ex)
            {
                nregistrosAfectados = 0;
            }
            return nregistrosAfectados;
        }

        public JsonResult recuperarDatos(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Tecnicos.Where(p => p.IDtecnico.Equals(id)).Select(p => new
            {
                p.IDtecnico,
                p.Nombre,
                p.Apellido,
                p.Telefono,
                fechaAlta = ((DateTime)p.FechaAlt).ToShortDateString(),
                p.IDestado
            });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int guardarDatos(Tecnico oTecnico)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                int idtecnico = oTecnico.IDtecnico;               
                if (idtecnico.Equals(0))
                {                   
                    int nVeces = bd.Tecnicos.Where(p => p.Nombre.Equals(oTecnico.Nombre) && p.Apellido.Equals(oTecnico.Apellido)).Count();
                    if (nVeces == 0)
                    {
                        oTecnico.IDtipousuario = 'T';
                        oTecnico.bTieneUsuario = 0;
                        //Guarda nuevo
                        bd.Tecnicos.InsertOnSubmit(oTecnico);
                        bd.SubmitChanges();
                        nregistrosAfectados = 1;
                    }
                    else
                    {
                        //el dato ya existe
                        nregistrosAfectados = -1;
                    }
                }                
                else
                {
                    //editar
                    int nVeces = bd.Tecnicos.Where(p => p.Nombre.Equals(oTecnico.Nombre) && p.Apellido.Equals(oTecnico.Apellido) && !p.IDtecnico.Equals(idtecnico)).Count();
                    if (nVeces == 0)
                    {
                        Tecnico obj = bd.Tecnicos.Where(p => p.IDtecnico.Equals(idtecnico)).First();
                        obj.Nombre = oTecnico.Nombre;
                        obj.Apellido = oTecnico.Apellido;
                        obj.Telefono = oTecnico.Telefono;
                        obj.FechaAlt = oTecnico.FechaAlt;
                        obj.IDestado = oTecnico.IDestado;
                        bd.SubmitChanges();
                        nregistrosAfectados = 1;
                    }
                    else
                    {
                        //no edita si ya existe
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