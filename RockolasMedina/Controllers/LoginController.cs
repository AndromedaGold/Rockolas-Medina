using RockolasMedina.Filtros;
using RockolasMedina.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public int validarUsuario(string usuario, string contraseña)
        {
            //Si es igual a cero es un error
            int rpta = 0;
            try
            {
                using (ConexionDataContext bd = new ConexionDataContext())
                {
                    SHA256Managed sha = new SHA256Managed();
                    byte[] dataNoCifrada = Encoding.Default.GetBytes(contraseña);
                    byte[] dataCifrada = sha.ComputeHash(dataNoCifrada);
                    //Contraseña
                    string contraCifrada = BitConverter.ToString(dataCifrada).Replace("-", "");

                    rpta = bd.Usuarios.Where(p => p.Nombre == usuario && p.Contraseña == contraCifrada).Count();
                    if (rpta == 1)
                    {
                        int idUsuario = bd.Usuarios.Where(p => p.Nombre == usuario && p.Contraseña == contraCifrada).First().IDusuario;
                        Session["idusuario"] = idUsuario;

                        var roles = from usu in bd.Usuarios
                                    join rol in bd.Rols
                                    on usu.IDrol equals rol.IDrol
                                    join rolpagina in bd.RolPaginas
                                    on rol.IDrol equals rolpagina.IDrol
                                    join pagina in bd.Paginas
                                    on rolpagina.IDpagina equals pagina.IDpagina
                                    where usu.Habilitado == 1 && rolpagina.Habilitado == 1
                                    && usu.IDusuario == idUsuario
                                    select new
                                    {
                                        accion = pagina.Accion,
                                        controlador = pagina.Controlador,
                                        mensaje = pagina.Mensaje
                                    };
                        //Iniciando
                        Variable.acciones = new List<string>();
                        Variable.controladores = new List<string>();
                        Variable.mensajes = new List<string>();

                        //Llenando valores
                        foreach(var item in roles)
                        {
                            Variable.acciones.Add(item.accion);
                            Variable.controladores.Add(item.controlador);
                            Variable.mensajes.Add(item.mensaje);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                rpta = 0;
            }
            return rpta;
        }

        public ActionResult Cerrar()
        {
            return RedirectToAction("Index");
        }
    }
}