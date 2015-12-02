using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using WepApi_Libro1800.Helpers;
using WepApi_Libro1800.Models;

namespace WepApi_Libro1800.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiObraController : ApiController
    {
        private Libro1800Context db = new Libro1800Context();

        private const string rutaImagen = "~/Content/Multimedia/imagen/";
        private const string rutaVideo = "~/Content/Multimedia/video/";
        private const string rutaAudio = "~/Content/Multimedia/audio/";
        private const string rutaInteractivo = "~/Content/Multimedia/interactivo/";
        private const string rutaOtro = "~/Content/Multimedia/otro/";

        private List<string> listaCampos = new List<string>()
        {
            "NoCatalogo",
            "NoInventario",
            "Coleccion",
            "ImagenRuta",
            "ImagenRuta2",
            "ImagenRuta3",
            "Aut_Pseudonimo",
            "Aut_Prefijo",
            "Aut_Nombre",
            "Aut_ApellidoPaterno",
            "Aut_ApellidoMaterno",
            "Aut_NacimientoLugar",
            "Aut_NacimientoFecha",
            "Aut_MuerteLugar",
            "Aut_MuerteFecha",
            "Aut_ActividadLugar",
            "Aut_ActividadFechaInicio",
            "Aut_ActividadFechaFin",
            "AutMarco_Nombre",
            "AutMarco_ApellidoPaterno",
            "AutMarco_ApellidoMaterno",
            "AutMarco_NacimientoLugar",
            "AutMarco_NacimientoFecha",
            "AutMarco_MuerteLugar",
            "AutMarco_MuerteFecha",
            "Titulo",
            "SubTitulo",
            "TituloLenguaOriginal",
            "TituloIngles",
            "FechaFacturaObra",
            "FechaConcepcion",
            "FechaEjecucion",
            "TecnicaObra",
            "TecnicaMarco",
            "MedidasObra",
            "MedidasMarco",
            "MedidasBase",
            "Firma",
            "Sello",
            "Serie",
            "Inscripciones",
            "FundidorAutorizacion",
            "Procedencia",
            "AutorTexto",
            "StatusTexto",
            "ObservacionesObra",
            "Parrafo1",
            "Parrafo2",
            "Parrafo3",
            "Parrafo4",
            "Parrafo5",
            "Parrafo6",
            "Parrafo7",
            "Parrafo8",
            "Parrafo9",
            "Parrafo10",
            "Parrafo11",
            "Parrafo12",
            "Parrafo13",
            "Parrafo14",
            "Parrafo15",
            "Parrafo16",
            "Ra_Audio",
            "Ra_Video",
            "Ra_Interactivo",
            "Ra_Otro",
            "Observaciones",
            "RevistaMensual"
        };



        private bool esCampo(string campoTexto)
        {
            bool x = false;

            if (!string.IsNullOrWhiteSpace(campoTexto))
                x = listaCampos.Any(a => a.ToLower() == campoTexto.ToLower());

            return x;
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
                    default:

                        listaExtensiones = new string[] { "" };
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

        private Obra verificarMultimedia(Obra obra)
        {
            //Imagen
            obra.ImagenRuta = verificarSoloArchivo(rutaImagen, obra.NoInventario.ToString());
            obtenerMedidas(obra);
            //Imagen2
            obra.ImagenRuta2 = verificarSoloArchivo(rutaImagen, obra.NoInventario.ToString() + "-2");
            //Imagen3
            obra.ImagenRuta3 = verificarSoloArchivo(rutaImagen, obra.NoInventario.ToString() + "-3");

            //Audio
            obra.Ra_Audio = verificarSoloArchivo(rutaAudio, obra.NoInventario.ToString());

            //Video
            obra.Ra_Video = verificarSoloArchivo(rutaVideo, obra.NoInventario.ToString());

            //Interactivo
            obra.Ra_Interactivo = verificarSoloArchivo(rutaInteractivo, obra.NoInventario.ToString());

            //Otro
            obra.Ra_Otro = verificarSoloArchivo(rutaOtro, obra.NoInventario.ToString());

            return obra;
        }


        private void obtenerMedidas(Obra obra)
        {
            if (obra.ImagenRuta != "")
            {

                var url = obra.ImagenRuta;
                var uri = new Uri(url);
                var NombreArchivo = Path.GetFileName(uri.AbsolutePath);
                var rutaMapeada = System.Web.Hosting.HostingEnvironment.MapPath(rutaImagen + NombreArchivo);

                Image img = Image.FromFile(rutaMapeada);

                obra.ImagenAncho = img.Width;
                obra.ImagenAlto = img.Height;

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

        /// <summary>
        /// Obtiene una Obra por Numero de inventario
        /// </summary>
        /// <param name="id">Numero de Inventario</param>
        /// <returns>Obra</returns>
        [HttpGet, ActionName("getObra")]
        public Obra getObra(int id)
        {
            var obra = db.Obras.FirstOrDefault(a => a.NoInventario == id);

            if (obra != null)
                obra = verificarMultimedia(obra);

            return obra;
        }

        /// <summary>
        /// Obtiene una lista de Obras ordenada por Numero de inventario
        /// </summary>
        /// <returns>Lista Obras</returns>
        [HttpGet, ActionName("getObras")]
        public List<Obra> getObras()
        {
            List<Obra> lista = new List<Obra>();

            foreach (var item in db.Obras.OrderBy(a => a.NoInventario).ToList())
            {
                lista.Add(verificarMultimedia(item));
            }

            return lista;
        }


        /// <summary>
        /// Obtiene el valor de un campo de una obra 
        /// </summary>
        /// <param name="id">Numero Inventario de la obra</param>
        /// <param name="campo">Nombre del campo a consultar</param>
        /// <returns>Valor del campo</returns>
        [HttpGet, ActionName("getValor")]
        public string getValor(int id, string campo)
        {
            string valor = "";

            var obra = db.Obras.FirstOrDefault(a => a.NoInventario == id);

            if (obra != null && esCampo(campo))
            {
                //completar esto hasta no tener los campos 
                switch (campo)
                {
                    case "NoCatalogo": valor = obra.NoCatalogo.ToString(); break;
                    case "Coleccion": valor = obra.Coleccion; break;
                    case "ImagenRuta": valor = verificarSoloArchivo(rutaImagen, obra.NoInventario.ToString()); break;
                    case "ImagenRuta2": valor = verificarSoloArchivo(rutaImagen, obra.NoInventario.ToString() + "-2"); break;
                    case "ImagenRuta3": valor = verificarSoloArchivo(rutaImagen, obra.NoInventario.ToString() + "-3"); break;
                    case "Aut_Pseudonimo": valor = obra.Aut_Pseudonimo; break;
                    case "Aut_Prefijo": valor = obra.Aut_Prefijo; break;
                    case "Aut_Nombre": valor = obra.Aut_Nombre; break;
                    case "Aut_ApellidoPaterno": valor = obra.Aut_ApellidoPaterno; break;
                    case "Aut_ApellidoMaterno": valor = obra.Aut_ApellidoMaterno; break;
                    case "Aut_NacimientoLugar": valor = obra.Aut_NacimientoLugar; break;
                    case "Aut_NacimientoFecha": valor = obra.Aut_NacimientoFecha; break;
                    case "Aut_MuerteLugar": valor = obra.Aut_MuerteLugar; break;
                    case "Aut_MuerteFecha": valor = obra.Aut_MuerteFecha; break;
                    case "Aut_ActividadLugar": valor = obra.Aut_ActividadLugar; break;
                    case "Aut_ActividadFechaInicio": valor = obra.Aut_ActividadFechaInicio; break;
                    case "Aut_ActividadFechaFin": valor = obra.Aut_ActividadFechaFin; break;
                    case "AutMarco_Nombre": valor = obra.AutMarco_Nombre; break;
                    case "AutMarco_ApellidoPaterno": valor = obra.AutMarco_ApellidoPaterno; break;
                    case "AutMarco_ApellidoMaterno": valor = obra.AutMarco_ApellidoMaterno; break;
                    case "AutMarco_NacimientoLugar": valor = obra.AutMarco_NacimientoLugar; break;
                    case "AutMarco_NacimientoFecha": valor = obra.AutMarco_NacimientoFecha; break;
                    case "AutMarco_MuerteLugar": valor = obra.AutMarco_MuerteLugar; break;
                    case "AutMarco_MuerteFecha": valor = obra.AutMarco_MuerteFecha; break;
                    case "Titulo": valor = obra.Titulo; break;
                    case "SubTitulo": valor = obra.SubTitulo; break;
                    case "TituloLenguaOriginal": valor = obra.TituloLenguaOriginal; break;
                    case "TituloIngles": valor = obra.TituloIngles; break;
                    case "FechaFacturaObra": valor = obra.FechaFacturaObra; break;
                    case "FechaConcepcion": valor = obra.FechaConcepcion; break;
                    case "FechaEjecucion": valor = obra.FechaEjecucion; break;
                    case "TecnicaObra": valor = obra.TecnicaObra; break;
                    case "TecnicaMarco": valor = obra.TecnicaMarco; break;
                    case "MedidasObra": valor = obra.MedidasObra; break;
                    case "MedidasMarco": valor = obra.MedidasMarco; break;
                    case "MedidasBase": valor = obra.MedidasBase; break;
                    case "Firma": valor = obra.Firma; break;
                    case "Sello": valor = obra.Sello; break;
                    case "Serie": valor = obra.Serie; break;
                    case "Inscripciones": valor = obra.Inscripciones; break;
                    case "FundidorAutorizacion": valor = obra.FundidorAutorizacion; break;
                    case "Procedencia": valor = obra.Procedencia; break;
                    case "AutorTexto": valor = obra.AutorTexto; break;
                    case "StatusTexto": valor = obra.StatusTexto; break;
                    case "ObservacionesObra": valor = obra.ObservacionesObra; break;
                    case "Parrafo1": valor = obra.Parrafo1; break;
                    case "Parrafo2": valor = obra.Parrafo2; break;
                    case "Parrafo3": valor = obra.Parrafo3; break;
                    case "Parrafo4": valor = obra.Parrafo4; break;
                    case "Parrafo5": valor = obra.Parrafo5; break;
                    case "Parrafo6": valor = obra.Parrafo6; break;
                    case "Parrafo7": valor = obra.Parrafo7; break;
                    case "Parrafo8": valor = obra.Parrafo8; break;
                    case "Parrafo9": valor = obra.Parrafo9; break;
                    case "Parrafo10": valor = obra.Parrafo10; break;
                    case "Parrafo11": valor = obra.Parrafo11; break;
                    case "Parrafo12": valor = obra.Parrafo12; break;
                    case "Parrafo13": valor = obra.Parrafo13; break;
                    case "Parrafo14": valor = obra.Parrafo14; break;
                    case "Parrafo15": valor = obra.Parrafo15; break;
                    case "Parrafo16": valor = obra.Parrafo16; break;
                    case "Ra_Audio": valor = verificarSoloArchivo(rutaAudio, obra.NoInventario.ToString()); break;
                    case "Ra_Video": valor = verificarSoloArchivo(rutaVideo, obra.NoInventario.ToString()); break;
                    case "Ra_Interactivo": valor = verificarSoloArchivo(rutaInteractivo, obra.NoInventario.ToString()); break;
                    case "Ra_Otro": valor = verificarSoloArchivo(rutaOtro, obra.NoInventario.ToString()); break;
                    case "Observaciones": valor = obra.Observaciones; break;
                    case "RevistaMensual": valor = obra.RevistaMensual; break;

                    default: valor = ""; break;
                }
            }

            return valor;
        }


        /// <summary>
        /// Busca las obras que contengan el valor en cualquier campo
        /// </summary>
        /// <param name="valor">Valor a buscar</param>
        /// <returns>Lista de Obras</returns>
        [HttpGet, ActionName("findValor")]
        public List<Obra> find(string valor)
        {

            List<Obra> lista = new List<Obra>();

            foreach (var item in db.Obras.Where(a =>
                a.Coleccion.Contains(valor) ||
                a.Aut_Pseudonimo.Contains(valor) ||
                a.Aut_Prefijo.Contains(valor) ||
                a.Aut_Nombre.Contains(valor) ||
                a.Aut_ApellidoPaterno.Contains(valor) ||
                a.Aut_ApellidoMaterno.Contains(valor) ||
                a.Aut_NacimientoLugar.Contains(valor) ||
                a.Aut_NacimientoFecha.Contains(valor) ||
                a.Aut_MuerteLugar.Contains(valor) ||
                a.Aut_MuerteFecha.Contains(valor) ||
                a.Aut_ActividadLugar.Contains(valor) ||
                a.Aut_ActividadFechaInicio.Contains(valor) ||
                a.Aut_ActividadFechaFin.Contains(valor) ||
                a.AutMarco_Nombre.Contains(valor) ||
                a.AutMarco_ApellidoPaterno.Contains(valor) ||
                a.AutMarco_ApellidoMaterno.Contains(valor) ||
                a.AutMarco_NacimientoLugar.Contains(valor) ||
                a.AutMarco_NacimientoFecha.Contains(valor) ||
                a.AutMarco_MuerteLugar.Contains(valor) ||
                a.AutMarco_MuerteFecha.Contains(valor) ||
                a.Titulo.Contains(valor) ||
                a.SubTitulo.Contains(valor) ||
                a.TituloLenguaOriginal.Contains(valor) ||
                a.TituloIngles.Contains(valor) ||
                a.FechaFacturaObra.Contains(valor) ||
                a.FechaConcepcion.Contains(valor) ||
                a.FechaEjecucion.Contains(valor) ||
                a.TecnicaObra.Contains(valor) ||
                a.TecnicaMarco.Contains(valor) ||
                a.MedidasObra.Contains(valor) ||
                a.MedidasMarco.Contains(valor) ||
                a.MedidasBase.Contains(valor) ||
                a.Firma.Contains(valor) ||
                a.Sello.Contains(valor) ||
                a.Serie.Contains(valor) ||
                a.Inscripciones.Contains(valor) ||
                a.FundidorAutorizacion.Contains(valor) ||
                a.Procedencia.Contains(valor) ||
                a.AutorTexto.Contains(valor) ||
                a.StatusTexto.Contains(valor) ||
                a.ObservacionesObra.Contains(valor) ||
                a.Parrafo1.Contains(valor) ||
                a.Parrafo2.Contains(valor) ||
                a.Parrafo3.Contains(valor) ||
                a.Parrafo4.Contains(valor) ||
                a.Parrafo5.Contains(valor) ||
                a.Parrafo6.Contains(valor) ||
                a.Parrafo7.Contains(valor) ||
                a.Parrafo8.Contains(valor) ||
                a.Parrafo9.Contains(valor) ||
                a.Parrafo10.Contains(valor) ||
                a.Parrafo11.Contains(valor) ||
                a.Parrafo12.Contains(valor) ||
                a.Parrafo13.Contains(valor) ||
                a.Parrafo14.Contains(valor) ||
                a.Parrafo15.Contains(valor) ||
                a.Parrafo16.Contains(valor) ||
                a.Observaciones.Contains(valor)
                ).OrderBy(a => a.NoCatalogo).ToList())
            {
                lista.Add(verificarMultimedia(item));
            }

            lista = lista.OrderBy(a => a.NoInventario).ToList();

            return lista;
        }

        /// <summary>
        /// Buscar las obras que contengan el valor en el campo
        /// </summary>
        /// <param name="campo">Nombre del campo a consultar</param>
        /// <param name="valor">Valor a buscar</param>
        /// <returns>Lista de obras</returns>
        [HttpGet, ActionName("findCampoValor")]
        public List<Obra> find(string campo, string valor)
        {
            IQueryable<Obra> lista = null;


            if (esCampo(campo) && !string.IsNullOrWhiteSpace(valor))
            {
                //terminar hasta saber el total de campos
                switch (campo)
                {
                    case "Aut_Pseudonimo": lista = db.Obras.Where(a => a.Aut_Pseudonimo.Contains(valor)); break;
                    case "Coleccion": lista = db.Obras.Where(a=> a.Coleccion.Contains(valor)); break;
                    case "Aut_Prefijo": lista = db.Obras.Where(a => a.Aut_Prefijo.Contains(valor)); break;
                    case "Aut_Nombre": lista = db.Obras.Where(a => a.Aut_Nombre.Contains(valor)); break;
                    case "Aut_ApellidoPaterno": lista = db.Obras.Where(a => a.Aut_ApellidoPaterno.Contains(valor)); break;
                    case "Aut_ApellidoMaterno": lista = db.Obras.Where(a => a.Aut_ApellidoMaterno.Contains(valor)); break;
                    case "Aut_NacimientoLugar": lista = db.Obras.Where(a => a.Aut_NacimientoLugar.Contains(valor)); break;
                    case "Aut_NacimientoFecha": lista = db.Obras.Where(a => a.Aut_NacimientoFecha.Contains(valor)); break;
                    case "Aut_MuerteLugar": lista = db.Obras.Where(a => a.Aut_MuerteLugar.Contains(valor)); break;
                    case "Aut_MuerteFecha": lista = db.Obras.Where(a => a.Aut_MuerteFecha.Contains(valor)); break;
                    case "Aut_ActividadLugar": lista = db.Obras.Where(a => a.Aut_ActividadLugar.Contains(valor)); break;
                    case "Aut_ActividadFechaInicio": lista = db.Obras.Where(a => a.Aut_ActividadFechaInicio.Contains(valor)); break;
                    case "Aut_ActividadFechaFin": lista = db.Obras.Where(a => a.Aut_ActividadFechaFin.Contains(valor)); break;
                    case "AutMarco_Nombre": lista = db.Obras.Where(a => a.AutMarco_Nombre.Contains(valor)); break;
                    case "AutMarco_ApellidoPaterno": lista = db.Obras.Where(a => a.AutMarco_ApellidoPaterno.Contains(valor)); break;
                    case "AutMarco_ApellidoMaterno": lista = db.Obras.Where(a => a.AutMarco_ApellidoMaterno.Contains(valor)); break;
                    case "AutMarco_NacimientoLugar": lista = db.Obras.Where(a => a.AutMarco_NacimientoLugar.Contains(valor)); break;
                    case "AutMarco_NacimientoFecha": lista = db.Obras.Where(a => a.AutMarco_NacimientoFecha.Contains(valor)); break;
                    case "AutMarco_MuerteLugar": lista = db.Obras.Where(a => a.AutMarco_MuerteLugar.Contains(valor)); break;
                    case "AutMarco_MuerteFecha": lista = db.Obras.Where(a => a.AutMarco_MuerteFecha.Contains(valor)); break;
                    case "Titulo": lista = db.Obras.Where(a => a.Titulo.Contains(valor)); break;
                    case "SubTitulo": lista = db.Obras.Where(a => a.SubTitulo.Contains(valor)); break;
                    case "TituloLenguaOriginal": lista = db.Obras.Where(a => a.TituloLenguaOriginal.Contains(valor)); break;
                    case "TituloIngles": lista = db.Obras.Where(a => a.TituloIngles.Contains(valor)); break;
                    case "FechaFacturaObra": lista = db.Obras.Where(a => a.FechaFacturaObra.Contains(valor)); break;
                    case "FechaConcepcion": lista = db.Obras.Where(a => a.FechaConcepcion.Contains(valor)); break;
                    case "FechaEjecucion": lista = db.Obras.Where(a => a.FechaEjecucion.Contains(valor)); break;
                    case "TecnicaObra": lista = db.Obras.Where(a => a.TecnicaObra.Contains(valor)); break;
                    case "TecnicaMarco": lista = db.Obras.Where(a => a.TecnicaMarco.Contains(valor)); break;
                    case "MedidasObra": lista = db.Obras.Where(a => a.MedidasObra.Contains(valor)); break;
                    case "MedidasMarco": lista = db.Obras.Where(a => a.MedidasMarco.Contains(valor)); break;
                    case "MedidasBase": lista = db.Obras.Where(a => a.MedidasBase.Contains(valor)); break;
                    case "Firma": lista = db.Obras.Where(a => a.Firma.Contains(valor)); break;
                    case "Sello": lista = db.Obras.Where(a => a.Sello.Contains(valor)); break;
                    case "Serie": lista = db.Obras.Where(a => a.Serie.Contains(valor)); break;
                    case "Inscripciones": lista = db.Obras.Where(a => a.Inscripciones.Contains(valor)); break;
                    case "FundidorAutorizacion": lista = db.Obras.Where(a => a.FundidorAutorizacion.Contains(valor)); break;
                    case "Procedencia": lista = db.Obras.Where(a => a.Procedencia.Contains(valor)); break;
                    case "AutorTexto": lista = db.Obras.Where(a => a.AutorTexto.Contains(valor)); break;
                    case "StatusTexto": lista = db.Obras.Where(a => a.StatusTexto.Contains(valor)); break;
                    case "ObservacionesObra": lista = db.Obras.Where(a => a.ObservacionesObra.Contains(valor)); break;
                    case "Parrafo1": lista = db.Obras.Where(a => a.Parrafo1.Contains(valor)); break;
                    case "Parrafo2": lista = db.Obras.Where(a => a.Parrafo2.Contains(valor)); break;
                    case "Parrafo3": lista = db.Obras.Where(a => a.Parrafo3.Contains(valor)); break;
                    case "Parrafo4": lista = db.Obras.Where(a => a.Parrafo4.Contains(valor)); break;
                    case "Parrafo5": lista = db.Obras.Where(a => a.Parrafo5.Contains(valor)); break;
                    case "Parrafo6": lista = db.Obras.Where(a => a.Parrafo6.Contains(valor)); break;
                    case "Parrafo7": lista = db.Obras.Where(a => a.Parrafo7.Contains(valor)); break;
                    case "Parrafo8": lista = db.Obras.Where(a => a.Parrafo8.Contains(valor)); break;
                    case "Parrafo9": lista = db.Obras.Where(a => a.Parrafo9.Contains(valor)); break;
                    case "Parrafo10": lista = db.Obras.Where(a => a.Parrafo10.Contains(valor)); break;
                    case "Parrafo11": lista = db.Obras.Where(a => a.Parrafo11.Contains(valor)); break;
                    case "Parrafo12": lista = db.Obras.Where(a => a.Parrafo12.Contains(valor)); break;
                    case "Parrafo13": lista = db.Obras.Where(a => a.Parrafo13.Contains(valor)); break;
                    case "Parrafo14": lista = db.Obras.Where(a => a.Parrafo14.Contains(valor)); break;
                    case "Parrafo15": lista = db.Obras.Where(a => a.Parrafo15.Contains(valor)); break;
                    case "Parrafo16": lista = db.Obras.Where(a => a.Parrafo16.Contains(valor)); break;
                    case "Observaciones": lista = db.Obras.Where(a => a.Observaciones.Contains(valor)); break;
                    case "RevistaMensual": lista = db.Obras.Where(a => a.Observaciones.Contains(valor)); break;
                    default: break;
                }
            }

            lista = lista.OrderBy(a => a.NoInventario);


            List<Obra> listaReturn = new List<Obra>();

            foreach (var item in lista.ToList())
            {
                listaReturn.Add(verificarMultimedia(item));
            }


            return listaReturn;
        }


        /// <summary>
        /// Devuelve una lista de solo No de catalogo y No de inventario, ordenada por No de Catalogo
        /// </summary>
        /// <returns>Lista relación {Numero de Catalogo} con {Numero de Inventario}</returns>
        [HttpGet, ActionName("getNoCatalogoInventario")]
        public List<ItemNoCatInv> getNoCatalogoInventario()
        {
            var lista = db.Obras.OrderBy(a => a.NoInventario).Select(a => new ItemNoCatInv() { NoCat = a.NoCatalogo, NoInventario = a.NoInventario }).ToList();

            return lista;
        }


        /// <summary>
        /// Devuelve una lista de los nombres de los campos
        /// </summary>
        /// <returns>Lista de nombre de campos</returns>
        [HttpGet, ActionName("getNombreCampos")]
        public List<string> getNombreCampos()
        {
            var lista = listaCampos.OrderBy(a => a).ToList();

            return lista;
        }



    }
}