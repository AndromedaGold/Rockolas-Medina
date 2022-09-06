using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using RockolasMedina.Filtros;

namespace RockolasMedina.Controllers
{
    public class RolPaginaController : Controller
    {
        // GET: RolPagina
        public ActionResult Index()
        {
            return View();
        }
        //Aqui listamos todos los roles
        public JsonResult listarRol()
        {
            using (ConexionDataContext bd = new ConexionDataContext())
            {
                var lista = bd.Rols.Where(p => p.Habilitado == 1).Select(p => new
                {
                    p.IDrol,
                    p.Nombre,
                    p.Descripcion
                }).ToList();
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
        }
        //Aqui listamos las paginas
        public JsonResult listarPaginas()
        {
            using(ConexionDataContext bd = new ConexionDataContext())
            {
                var lista = bd.Paginas.Where(p => p.Habilitado == 1).Select(p => new
                {
                    p.IDpagina,
                    p.Mensaje,
                    p.Habilitado,
                }).ToList();
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult obtenerRol(int idRol)
        {
            using(ConexionDataContext bd = new ConexionDataContext())
            {
                var rol = bd.Rols.Where(p => p.IDrol == idRol).Select(p => new
                {
                    p.IDrol,
                    p.Nombre,
                    p.Descripcion
                }).First();
                return Json(rol, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult obtenerRolPagina(int idRol)
        {
            using (ConexionDataContext bd = new ConexionDataContext())
            {
                var lista = bd.RolPaginas.Where(p => p.IDrol == idRol && p.Habilitado == 1).Select(p => new
                {
                    p.IDrol,
                    p.IDpagina,
                    p.Habilitado
                }).ToList();
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
        }

        public int guardarDatos(Rol oRol, string dataEnviar)
        {
            int rpta = 0;
            try
            {
                using (ConexionDataContext bd = new ConexionDataContext())
                {
                    using(var transaccion = new TransactionScope())
                    {
                        if(oRol.IDrol == 0)
                        {
                            Rol orol = new Rol();
                            orol.Nombre = oRol.Nombre;
                            orol.Descripcion = oRol.Descripcion;
                            orol.Habilitado = oRol.Habilitado;
                            bd.Rols.InsertOnSubmit(orol);
                            bd.SubmitChanges();

                            string[] codes = dataEnviar.Split('$');
                            for(var i = 0; i < codes.Length; i++)
                            {
                                RolPagina oRolpagina = new RolPagina();
                                oRolpagina.IDrol = orol.IDrol;
                                oRolpagina.IDpagina = int.Parse(codes[i]);
                                oRolpagina.Habilitado = 1;
                                bd.RolPaginas.InsertOnSubmit(oRolpagina);
                            }
                            rpta = 1;
                            bd.SubmitChanges();
                            transaccion.Complete();
                        }
                        else
                        {
                            //Editamos
                            Rol orol = bd.Rols.Where(p => p.IDrol == oRol.IDrol).First();
                            orol.Nombre = oRol.Nombre;
                            orol.Descripcion = oRol.Descripcion;

                            //Deshabilitamos
                            var lista = bd.RolPaginas.Where(p => p.IDrol == oRol.IDrol);
                            foreach(RolPagina oRolpagina in lista){
                                oRolpagina.Habilitado = 0;
                            }
                            //habilitamos
                            string[] codes = dataEnviar.Split('$');
                            for (var i = 0; i < codes.Length; i++)
                            {
                                int cantidad = bd.RolPaginas.Where(p => p.IDrol == oRol.IDrol && p.IDpagina == int.Parse(codes[i])).Count();
                                if (cantidad == 0)
                                {
                                    RolPagina oRolpagina = new RolPagina();
                                    oRolpagina.IDrol = orol.IDrol;
                                    oRolpagina.IDpagina = int.Parse(codes[i]);
                                    oRolpagina.Habilitado = 1;
                                    bd.RolPaginas.InsertOnSubmit(oRolpagina);
                                }
                                else
                                {
                                    RolPagina oRolpagina = bd.RolPaginas.Where(p => p.IDrol == oRol.IDrol && p.IDpagina == int.Parse(codes[i])).First();
                                    oRolpagina.Habilitado = 1;
                                }                              
                            }
                            rpta = 1;
                            bd.SubmitChanges();
                            transaccion.Complete();
                        }
                    }
                }
            }catch(Exception ex)
            {
                rpta = 0;
            }
            return rpta;
        }

        public int eliminar(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                Rol oRol = bd.Rols.Where(p => p.IDrol.Equals(id)).First();
                oRol.Habilitado = 0;
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