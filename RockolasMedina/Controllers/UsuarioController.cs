using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RockolasMedina.Models;
using System.Transactions;
using System.Security.Cryptography;
using System.Text;
using RockolasMedina.Filtros;

namespace RockolasMedina.Controllers
{
    [Security]
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarUsuarios()
        {
            List<Usuarios> listaUsuario = new List<Usuarios>();
            using (ConexionDataContext bd = new ConexionDataContext())
            {
                List<Usuarios> listaTecnico = (from usuario in bd.Usuarios
                                                join tecnico in bd.Tecnicos
                                                on usuario.ID equals tecnico.IDtecnico
                                                join rol in bd.Rols
                                                on usuario.IDrol equals rol.IDrol
                                                where usuario.Habilitado == 1 && usuario.TipoUsuario == 'T'
                                                select new Usuarios
                                                {
                                                    idUsuario = usuario.IDusuario,
                                                    nombrePersona = tecnico.Nombre + " " + tecnico.Apellido,
                                                    nombreUsuario = usuario.Nombre,
                                                    nombreRol = rol.Nombre,
                                                    nombreTipoEmpleado = "Tecnico"
                                                }).ToList();
                listaUsuario.AddRange(listaTecnico);
                List<Usuarios> listaSupervisor = (from usuario in bd.Usuarios
                                                 join supervisor in bd.Supervisors
                                                 on usuario.ID equals supervisor.IDsupervisor
                                                 join rol in bd.Rols
                                                 on usuario.IDrol equals rol.IDrol
                                                 where usuario.Habilitado == 1 && usuario.TipoUsuario == 'S'
                                                 select new Usuarios
                                                 {
                                                     idUsuario = usuario.IDusuario,
                                                     nombrePersona = supervisor.Nombre + " " + supervisor.Apellido,
                                                     nombreUsuario = usuario.Nombre,
                                                     nombreRol = rol.Nombre,
                                                     nombreTipoEmpleado = "Supervisor"
                                                 }).ToList();
                listaUsuario.AddRange(listaSupervisor);
                listaUsuario = listaUsuario.OrderBy(p => p.idUsuario).ToList();
            }
            return Json(listaUsuario, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listarRol()
        {
            using(ConexionDataContext bd = new ConexionDataContext())
            {
                var lista = bd.Rols.Where(p => p.Habilitado == 1).Select(p => new
                {
                    ID = p.IDrol,
                    p.Nombre
                }).ToList();
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult listarPersonas()
        {
            List<Personas> listarPersona = new List<Personas>();
            //Lista tecnicos
            using (ConexionDataContext bd = new ConexionDataContext())
            {
                List<Personas> listaTecnico = (from item in bd.Tecnicos
                                   where item.bTieneUsuario == 0
                                   select new Personas
                                   {
                                       ID = item.IDtecnico,
                                       Nombre = item.Nombre + " " + item.Apellido + " (T)"
                                   }).ToList();
                listarPersona.AddRange(listaTecnico);
                var listaSupervisor = (from item in bd.Supervisors
                                    where item.bTieneUsuario == 0
                                    select new Personas
                                    {
                                        ID = item.IDsupervisor,
                                        Nombre = item.Nombre + " " + item.Apellido + " (S)"
                                    }).ToList();
                listarPersona.AddRange(listaSupervisor);
                listarPersona = listarPersona.OrderBy(p => p.Nombre).ToList();
                return Json(listarPersona, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult recuperarDatos(int idUsuario)
        {
            using (ConexionDataContext bd = new ConexionDataContext())
            {

                var oUsuario = bd.Usuarios.Where(p => p.IDusuario == idUsuario).Select(p => new
                    {
                        p.IDusuario,
                        p.Nombre,
                        p.IDrol
                    }).First();
                return Json(oUsuario, JsonRequestBehavior.AllowGet);
            }
        }

        public int eliminar(Usuario oUsuario)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                int idusuario = oUsuario.IDusuario;
                Usuario obj = bd.Usuarios.Where(p => p.IDusuario.Equals(idusuario)).First();
                obj.Habilitado = 0;
                bd.SubmitChanges();
                nregistrosAfectados = 1;
            }
            catch (Exception ex)
            {

            }
            return nregistrosAfectados;
        }

        public int guardarDatos(Usuario oUsuario, string nombreCompleto)
        {
            int rpta = 0;
            try
            {              
                int idUsuario = oUsuario.IDusuario;
                using (ConexionDataContext bd = new ConexionDataContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (idUsuario == 0)
                        {
                            //Cifrar contraseña
                            string clave = oUsuario.Contraseña;
                            SHA256Managed sha = new SHA256Managed();
                            byte[] dataNoCifrada = Encoding.Default.GetBytes(clave);
                            byte[] dataCifrada = sha.ComputeHash(dataNoCifrada);
                            //Contraseña
                            oUsuario.Contraseña = BitConverter.ToString(dataCifrada).Replace("-", "");

                            char tipo = char.Parse(nombreCompleto.Substring(nombreCompleto.Length - 2, 1));
                            oUsuario.TipoUsuario = tipo;
                            bd.Usuarios.InsertOnSubmit(oUsuario);

                            if (tipo.Equals('T'))
                            {
                                Tecnico oTecnico = bd.Tecnicos.Where(p => p.IDtecnico == oUsuario.ID).First();
                                oTecnico.bTieneUsuario = 1;
                            }
                            else
                            {
                                Supervisor oSupervisor = bd.Supervisors.Where(p => p.IDsupervisor == oUsuario.ID).First();
                                oSupervisor.bTieneUsuario = 1;
                            }
                            bd.SubmitChanges();
                            transaccion.Complete();
                            rpta = 1;
                        }
                        else
                        {
                            Usuario oUsuarioCL = bd.Usuarios.Where(p => p.IDusuario == idUsuario).First();
                            oUsuarioCL.IDrol = oUsuario.IDrol;
                            oUsuarioCL.Nombre = oUsuario.Nombre;
                            bd.SubmitChanges();
                            transaccion.Complete();
                            rpta = 1;
                        }
                    }
                }
            }
            catch (Exception ex) {
                rpta = 0;
            }
            return rpta;
        }
    }
}