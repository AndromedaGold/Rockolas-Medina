using RockolasMedina.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    [Security]
    public class SupervisorController : Controller
    {
        // GET: Supervisor
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarSupervisor()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Supervisors.Where(p => p.IDestado.Equals(1)).Select(p => new
            {
                p.IDsupervisor,
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
            var lista = bd.Supervisors.Where(p => p.IDestado.Equals(idestado)).Select(p => new
            {
                p.IDsupervisor,
                p.Nombre,
                p.Apellido,
                p.Telefono,
                FechaAlt = ((DateTime)p.FechaAlt).ToShortDateString(),
                p.IDestado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult recuperarDatos(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Supervisors.Where(p => p.IDsupervisor.Equals(id)).Select(p => new
            {
                p.IDsupervisor,
                p.Nombre,
                p.Apellido,
                p.Telefono,
                fechaAlta = ((DateTime)p.FechaAlt).ToShortDateString(),
                p.IDestado
            });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int guardarDatos(Supervisor oSupervisor)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                int idsupervisor = oSupervisor.IDsupervisor;
                if (idsupervisor.Equals(0))
                {
                    //Guarda nuevo
                    int nVeces = bd.Supervisors.Where(p => p.Nombre.Equals(oSupervisor.Nombre) && p.Apellido.Equals(oSupervisor.Apellido)).Count();
                    if (nVeces == 0)
                    {
                        oSupervisor.IDtipousuario = 'S';
                        oSupervisor.bTieneUsuario = 0;
                        bd.Supervisors.InsertOnSubmit(oSupervisor);
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
                    int nVeces = bd.Supervisors.Where(p => p.Nombre.Equals(oSupervisor.Nombre) && p.Apellido.Equals(oSupervisor.Apellido) && !p.IDsupervisor.Equals(idsupervisor)).Count();
                    if (nVeces == 0)
                    {
                        Supervisor obj = bd.Supervisors.Where(p => p.IDsupervisor.Equals(idsupervisor)).First();
                        obj.Nombre = oSupervisor.Nombre;
                        obj.Apellido = oSupervisor.Apellido;
                        obj.Telefono = oSupervisor.Telefono;
                        obj.FechaAlt = oSupervisor.FechaAlt;
                        obj.IDestado = oSupervisor.IDestado;
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

        public int eliminar(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                Supervisor oSupervisor = bd.Supervisors.Where(p => p.IDsupervisor.Equals(id)).First();
                oSupervisor.IDestado = 0;
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