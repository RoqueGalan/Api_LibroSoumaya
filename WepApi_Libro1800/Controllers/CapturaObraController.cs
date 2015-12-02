using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WepApi_Libro1800.Models;

namespace WepApi_Libro1800.Controllers
{
    public class CapturaObraController : Controller
    {
        private Libro1800Context db = new Libro1800Context();

        private const string rutaImagen = "~/Content/Multimedia/imagen/";
        private const string rutaVideo = "~/Content/Multimedia/video/";
        private const string rutaAudio = "~/Content/Multimedia/audio/";
        private const string rutaInteractivo = "~/Content/Multimedia/interactivo/";
        private const string rutaOtro = "~/Content/Multimedia/otro/";


        // GET: CapturaObra
        public ActionResult Index(int? Pagina)
        {
            bool EstaLogeado = Session["login"] == null ? false : (bool)Session["login"];

            //por prueba siempre es true
            //EstaLogeado = true;
            //Session["login"] = true;

            if (EstaLogeado)
            {

                var lista = db.Obras.OrderBy(a => a.NoInventario);

                //paginador
                int registrosPorPagina = 100;
                int pagActual = 1;
                pagActual = Pagina.HasValue ? Convert.ToInt32(Pagina) : 1;

                IPagedList<Obra> listaPagina = lista.ToPagedList(pagActual, registrosPorPagina);

                return View(listaPagina);
            }

            return View("IniciarSesion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IniciarSesion(string UserName, string UserPassword)
        {
            bool estadoLogin = false;

            if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(UserPassword))
            {
                string UserNameValido = "usu4ri0_1800";
                string UserPasswordValido = "R3cS0uM4@2015_Fc$";

                if (UserName == UserNameValido && UserPassword == UserPasswordValido)
                {
                    estadoLogin = true;
                }

            }

            Session["login"] = estadoLogin;

            return RedirectToAction("Index");
        }



        // GET: CapturaObra/Details/5
        public ActionResult Details(int? id)
        {
            bool EstaLogeado = Session["login"] == null ? false : (bool)Session["login"];
            if (!EstaLogeado) return RedirectToAction("Index");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Obra obra = db.Obras.Find(id);
            if (obra == null)
            {
                return HttpNotFound();
            }

            //extraer el multimedia
            //Imagen
            obra.ImagenRuta = verificarSoloArchivo(rutaImagen, obra.NoInventario.ToString());
            obtenerMedidas(obra);

            obra.ImagenRuta2 = verificarSoloArchivo(rutaImagen, obra.NoInventario.ToString() + "-2");

            obra.ImagenRuta3 = verificarSoloArchivo(rutaImagen, obra.NoInventario.ToString() + "-3");


            obra.Ra_Video = verificarSoloArchivo(rutaVideo, obra.NoInventario.ToString());

            obra.Ra_Audio = verificarSoloArchivo(rutaAudio, obra.NoInventario.ToString());



            Obra Obratemp = null;

            Obratemp = db.Obras.Where(a => a.NoInventario < obra.NoInventario).OrderByDescending(a => a.NoInventario).FirstOrDefault();
            ViewBag.ObraAnterior = Obratemp == null ? 0 : Obratemp.ObraID;

            Obratemp = db.Obras.Where(a => a.NoInventario > obra.NoInventario).OrderBy(a => a.NoInventario).FirstOrDefault();
            ViewBag.ObraSiguiente = Obratemp == null ? 0 : Obratemp.ObraID;

            return View(obra);
        }

        private string verificarSoloArchivo(string rutaArchivo, string nombreArchivo)
        {
            var rutaTemp = "";
            var rutaMapeada = "";

            var url = "";

            if (!string.IsNullOrWhiteSpace(nombreArchivo))
            {
                string[] listaExtensiones = null;

                switch (rutaArchivo)
                {
                    case rutaImagen:
                        listaExtensiones = new string[] { ".jpg", ".png", ".tiff" };
                        break;

                    case rutaVideo:
                        listaExtensiones = new string[] { ".mp4", ".avi" };
                        break;

                    case rutaAudio:
                        listaExtensiones = new string[] { ".mp3", ".mp4", ".ogg" };
                        break;
                }

                foreach (var extension in listaExtensiones)
                {
                    rutaTemp = rutaArchivo + nombreArchivo + extension;
                    rutaMapeada = System.Web.Hosting.HostingEnvironment.MapPath(rutaTemp);
                    if (System.IO.File.Exists(rutaMapeada))
                    {
                        url = Url.Content(rutaTemp);//devuelve una url
                        break;
                    }
                }

            }

            return url;
        }


        /// <summary>Obtiene las medidas de las imagenes </summary>

        public void obtenerMedidas(Obra obra)
        {
            if (obra.ImagenRuta != "")
            {
                System.Drawing.Image img = new Bitmap(Server.MapPath(obra.ImagenRuta));

                obra.ImagenAncho = img.Width;
                obra.ImagenAlto = img.Height;

                // 10 * 2 = 20 > 10

                if (obra.ImagenAlto > obra.ImagenAncho)
                {
                    obra.ImagenPosicion = "Vertical";

                }
                else
                {
                    int porcentaje = 2;
                    int altoMaximo = obra.ImagenAlto * porcentaje;

                    if (altoMaximo > obra.ImagenAncho)
                    {
                        obra.ImagenPosicion = "Vertical";

                    }
                    else
                    {
                        obra.ImagenPosicion = "Horizontal";


                    }
                }

                

            }
        }


        // GET: CapturaObra/Create
        public ActionResult Create()
        {
            bool EstaLogeado = Session["login"] == null ? false : (bool)Session["login"];
            if (!EstaLogeado) return RedirectToAction("Index");

            return View();
        }

        // POST: CapturaObra/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Obra obra)
        {
            bool EstaLogeado = Session["login"] == null ? false : (bool)Session["login"];
            if (!EstaLogeado) return RedirectToAction("Index");

            //Validar que el numero de inventario no sea repetido
            var obraTemp1 = db.Obras.Select(a => new { a.NoInventario, a.ObraID }).FirstOrDefault(a => a.NoInventario == obra.NoInventario);
            if (obraTemp1 != null)
                ModelState.AddModelError("NoInventario", "No. de Inventario ya existe.");


            if (ModelState.IsValid)
            {
                db.Obras.Add(obra);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = obra.ObraID });
            }

            return View(obra);
        }

        // GET: CapturaObra/Edit/5
        public ActionResult Edit(int? id)
        {
            bool EstaLogeado = Session["login"] == null ? false : (bool)Session["login"];
            if (!EstaLogeado) return RedirectToAction("Index");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obra obra = db.Obras.Find(id);
            if (obra == null)
            {
                return HttpNotFound();
            }
            return View(obra);
        }

        // POST: CapturaObra/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Obra obra)
        {
            bool EstaLogeado = Session["login"] == null ? false : (bool)Session["login"];
            if (!EstaLogeado) return RedirectToAction("Index");




            //Validar que el numero de inventario no sea repetido
            var obraTemp1 = db.Obras.Select(a => new { a.NoInventario, a.ObraID }).FirstOrDefault(a => a.NoInventario == obra.NoInventario);
            if (obraTemp1 != null)
                if (obraTemp1.ObraID != obra.ObraID)
                    ModelState.AddModelError("NoInventario", "No. de Inventario ya existe.");


            if (ModelState.IsValid)
            {
                db.Entry(obra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = obra.ObraID });
            }
            return View(obra);
        }

        // GET: CapturaObra/Delete/5
        public ActionResult Delete(int? id)
        {
            bool EstaLogeado = Session["login"] == null ? false : (bool)Session["login"];
            if (!EstaLogeado) return RedirectToAction("Index");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obra obra = db.Obras.Find(id);
            if (obra == null)
            {
                return HttpNotFound();
            }
            return View(obra);
        }

        // POST: CapturaObra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool EstaLogeado = Session["login"] == null ? false : (bool)Session["login"];
            if (!EstaLogeado) return RedirectToAction("Index");

            Obra obra = db.Obras.Find(id);
            db.Obras.Remove(obra);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private string QuitarNull(string valor)
        {
            valor = string.IsNullOrWhiteSpace(valor) ? "" : valor;

            valor = valor.Replace("\n", "");

            valor = Regex.Replace(valor.Trim(), @"\s+", " ");

            return valor;

        }


        public ActionResult RevalidarQuitarNulos()
        {
            bool EstaLogeado = Session["login"] == null ? false : (bool)Session["login"];
            if (!EstaLogeado) return RedirectToAction("Index");

            foreach (var item in db.Obras.ToList())
            {
                item.Titulo = QuitarNull(item.Titulo);
                item.Coleccion = QuitarNull(item.Coleccion);
                item.ImagenRuta = QuitarNull(item.ImagenRuta);
                item.ImagenRuta = QuitarNull(item.ImagenRuta);
                item.ImagenRuta2 = QuitarNull(item.ImagenRuta2);
                item.ImagenRuta3 = QuitarNull(item.ImagenRuta3);
                item.Aut_Pseudonimo = QuitarNull(item.Aut_Pseudonimo);
                item.Aut_Prefijo = QuitarNull(item.Aut_Prefijo);
                item.Aut_Nombre = QuitarNull(item.Aut_Nombre);
                item.Aut_ApellidoPaterno = QuitarNull(item.Aut_ApellidoPaterno);
                item.Aut_ApellidoMaterno = QuitarNull(item.Aut_ApellidoMaterno);
                item.Aut_NacimientoLugar = QuitarNull(item.Aut_NacimientoLugar);
                item.Aut_NacimientoFecha = QuitarNull(item.Aut_NacimientoFecha);
                item.Aut_MuerteLugar = QuitarNull(item.Aut_MuerteLugar);
                item.Aut_MuerteFecha = QuitarNull(item.Aut_MuerteFecha);
                item.Aut_ActividadLugar = QuitarNull(item.Aut_ActividadLugar);
                item.Aut_ActividadFechaInicio = QuitarNull(item.Aut_ActividadFechaInicio);
                item.Aut_ActividadFechaFin = QuitarNull(item.Aut_ActividadFechaFin);
                item.AutMarco_Nombre = QuitarNull(item.AutMarco_Nombre);
                item.AutMarco_ApellidoPaterno = QuitarNull(item.AutMarco_ApellidoPaterno);
                item.AutMarco_ApellidoMaterno = QuitarNull(item.AutMarco_ApellidoMaterno);
                item.AutMarco_NacimientoLugar = QuitarNull(item.AutMarco_NacimientoLugar);
                item.AutMarco_NacimientoFecha = QuitarNull(item.AutMarco_NacimientoFecha);
                item.AutMarco_MuerteLugar = QuitarNull(item.AutMarco_MuerteLugar);
                item.AutMarco_MuerteFecha = QuitarNull(item.AutMarco_MuerteFecha);
                item.Titulo = QuitarNull(item.Titulo);
                item.SubTitulo = QuitarNull(item.SubTitulo);
                item.TituloLenguaOriginal = QuitarNull(item.TituloLenguaOriginal);
                item.TituloIngles = QuitarNull(item.TituloIngles);
                item.FechaFacturaObra = QuitarNull(item.FechaFacturaObra);
                item.FechaConcepcion = QuitarNull(item.FechaConcepcion);
                item.FechaEjecucion = QuitarNull(item.FechaEjecucion);
                item.TecnicaObra = QuitarNull(item.TecnicaObra);
                item.TecnicaMarco = QuitarNull(item.TecnicaMarco);
                item.MedidasObra = QuitarNull(item.MedidasObra);
                item.MedidasMarco = QuitarNull(item.MedidasMarco);
                item.MedidasBase = QuitarNull(item.MedidasBase);
                item.Firma = QuitarNull(item.Firma);
                item.Sello = QuitarNull(item.Sello);
                item.Serie = QuitarNull(item.Serie);
                item.Inscripciones = QuitarNull(item.Inscripciones);
                item.FundidorAutorizacion = QuitarNull(item.FundidorAutorizacion);
                item.Procedencia = QuitarNull(item.Procedencia);
                item.AutorTexto = QuitarNull(item.AutorTexto);
                item.StatusTexto = QuitarNull(item.StatusTexto);
                item.ObservacionesObra = QuitarNull(item.ObservacionesObra);
                item.Parrafo1 = QuitarNull(item.Parrafo1);
                item.Parrafo2 = QuitarNull(item.Parrafo2);
                item.Parrafo3 = QuitarNull(item.Parrafo3);
                item.Parrafo4 = QuitarNull(item.Parrafo4);
                item.Parrafo5 = QuitarNull(item.Parrafo5);
                item.Parrafo6 = QuitarNull(item.Parrafo6);
                item.Parrafo7 = QuitarNull(item.Parrafo7);
                item.Parrafo8 = QuitarNull(item.Parrafo8);
                item.Parrafo9 = QuitarNull(item.Parrafo9);
                item.Parrafo10 = QuitarNull(item.Parrafo10);
                item.Parrafo11 = QuitarNull(item.Parrafo11);
                item.Parrafo12 = QuitarNull(item.Parrafo12);
                item.Parrafo13 = QuitarNull(item.Parrafo13);
                item.Parrafo14 = QuitarNull(item.Parrafo14);
                item.Parrafo15 = QuitarNull(item.Parrafo15);
                item.Parrafo16 = QuitarNull(item.Parrafo16);
                item.Ra_Audio = QuitarNull(item.Ra_Audio);
                item.Ra_Video = QuitarNull(item.Ra_Video);
                item.Ra_Interactivo = QuitarNull(item.Ra_Interactivo);
                item.Ra_Otro = QuitarNull(item.Ra_Otro);
                item.Observaciones = QuitarNull(item.Observaciones);
                item.RevistaMensual = QuitarNull(item.RevistaMensual);

                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
