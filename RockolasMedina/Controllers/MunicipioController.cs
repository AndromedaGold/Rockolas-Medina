using RockolasMedina.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RockolasMedina.Controllers
{
    [Security]
    public class MunicipioController : Controller
    {
        // GET: Municipio
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarMunicipio()
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Municipios.Where(p => p.Habilitado.Equals(1)).Select(p => new
            {
                p.IDmunicipio,
                p.Nombre,
                p.Departamento,
                p.IDruta,
                p.Habilitado
            });
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult buscarDepartamento(string depto)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Municipios.Where(p => p.Departamento.Contains(depto)).Select(p => new
            {
                p.IDmunicipio,
                p.Nombre,
                p.Departamento,
                p.IDruta,
                p.Habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult recuperarInfo(int id)
        {
            ConexionDataContext bd = new ConexionDataContext();
            var lista = bd.Municipios.Where(p => p.IDmunicipio.Equals(id)).Select(p => new
            {
                p.IDmunicipio,
                p.Nombre,
                p.Departamento,
                p.IDruta,
                p.Habilitado
            }).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public int guardarDatos(Municipio oMuni)
        {
            ConexionDataContext bd = new ConexionDataContext();
            int nregistrosAfectados = 0;
            try
            {
                //Inserta si es nuevo
                int idmuni = oMuni.IDmunicipio;
                if (idmuni == 0)
                {
                    int nVeces = bd.Municipios.Where(p => p.Nombre.Equals(oMuni.Nombre)).Count();
                    if (nVeces == 0)
                    {
                        bd.Municipios.InsertOnSubmit(oMuni);
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
                    int nVeces = bd.Municipios.Where(p => p.Nombre.Equals(oMuni.Nombre) && !p.IDmunicipio.Equals(idmuni)).Count();
                    if (nVeces == 0)
                    {
                        Municipio obj = bd.Municipios.Where(p => p.IDmunicipio.Equals(oMuni.IDmunicipio)).First();
                        obj.Nombre = oMuni.Nombre;
                        obj.Departamento = oMuni.Departamento;
                        obj.IDruta = oMuni.IDruta;
                        obj.Habilitado = oMuni.Habilitado;

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
                Municipio oMuni = bd.Municipios.Where(p => p.IDmunicipio.Equals(id)).First();
                oMuni.Habilitado = 0;
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