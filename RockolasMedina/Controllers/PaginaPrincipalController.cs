using RockolasMedina.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    [Security]
    public class PaginaPrincipalController : Controller
    {
        // GET: PaginaPrincipal
        public ActionResult Index()
        {
            int idusuario = (int)Session["idusuario"];
           
            using(ConexionDataContext bd = new ConexionDataContext())
            {
                string nombre = "";
                Usuario usuario = bd.Usuarios.Where(p => p.IDusuario == idusuario).First();
                if (usuario.TipoUsuario == 'S')
                {
                    Supervisor oSupervisor = bd.Supervisors.Where(p => p.IDsupervisor == usuario.ID).First();
                    nombre = oSupervisor.Nombre + " " + oSupervisor.Apellido;
                    ViewBag.nombre = nombre;
                }
                else
                {
                    Tecnico oTecnico = bd.Tecnicos.Where(p => p.IDtecnico == usuario.ID).First();
                    nombre = oTecnico.Nombre + " " + oTecnico.Apellido;
                    ViewBag.nombre = nombre;
                }
            }
            return View();
        }
    }
}