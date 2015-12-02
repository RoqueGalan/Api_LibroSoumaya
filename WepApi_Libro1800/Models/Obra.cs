using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WepApi_Libro1800.Models
{

    /// <summary>
    /// Clase que contiene los campos requeridos para el libro web con 1800 obras
    /// </summary>
    public class Obra
    {
        /// <summary> Identificador único en la base de datos </summary>
        [JsonIgnore]
        [Key]
        public int ObraID { get; set; }

        /// <summary> Numero de inventario </summary>
        [Required]
        [Display(Name = "Numero de inventario")]
        public int NoInventario { get; set; }

        /// <summary> Numero de catálogo </summary>
        [Display(Name = "Numero de catálogo")]
        [DefaultValue(0)]
        public int NoCatalogo { get; set; }


        /// <summary>Coleccion </summary>
        [Display(Name = "Colección")]
        [DefaultValue("")]
        public string Coleccion { get; set; }


        /// <summary> Ruta de la imagen</summary>
        [Display(Name = "Ruta de la imagen")]
        [DefaultValue("")]
        public string ImagenRuta { get; set; }

        /// <summary> Ancho de la imagen</summary>
        [NotMapped]
        [Display(Name = "Ancho de la imagen")]
        [DefaultValue(0)]
        public int ImagenAncho { get; set;  }

        /// <summary> Alto de la imagen</summary>
        [NotMapped]
        [Display(Name = "Alto de la imagen")]
        [DefaultValue(0)]
        public int ImagenAlto { get; set; }

        /// <summary> Posición de la imagen</summary>
        [NotMapped]
        [Display(Name = "Posicion de la imagen")]
        [DefaultValue("")]
        public string ImagenPosicion { get; set; }


        /// <summary> Ruta de la imagen dos</summary>
        [Display(Name = "Ruta de la imagen dos")]
        [DefaultValue("")]
        public string ImagenRuta2 { get; set; }

        /// <summary> Ruta de la imagen tres</summary>
        [Display(Name = " Ruta de la imagen tres")]
        [DefaultValue("")]
        public string ImagenRuta3 { get; set; }

        /// <summary> Atribución o situación del autor </summary>
        [Display(Name = "Atribución o situación del autor")]
        [DefaultValue("")]
        public string Aut_Prefijo { get; set; }

        /// <summary> Seudónimo del autor </summary>
        [Display(Name = "Seudónimo del autor")]
        [DefaultValue("")]
        public string Aut_Pseudonimo { get; set; }

        /// <summary> Nombre del autor </summary>
        [Display(Name = "Nombre del autor")]
        [DefaultValue("")]
        public string Aut_Nombre { get; set; }

        /// <summary> Apellido Paterno del autor </summary>
        [Display(Name = "Apellido Paterno del autor")]
        [DefaultValue("")]
        public string Aut_ApellidoPaterno { get; set; }

        /// <summary> Apellido Materno del autor </summary>
        [Display(Name = "Apellido Materno del autor")]
        [DefaultValue("")]
        public string Aut_ApellidoMaterno { get; set; }

        /// <summary> Lugar de Nacimiento del autor </summary>
        [Display(Name = "Fecha de Nacimiento del autor")]
        [DefaultValue("")]
        public string Aut_NacimientoLugar { get; set; }

        /// <summary> Fecha de Nacimiento del autor </summary>
        [Display(Name = "Fecha de Nacimiento del autor")]
        [DefaultValue("")]
        public string Aut_NacimientoFecha { get; set; }

        /// <summary> Lugar de Muerte del autor </summary>
        [Display(Name = "Lugar de Muerte del autor")]
        [DefaultValue("")]
        public string Aut_MuerteLugar { get; set; }

        /// <summary> Fecha de Muerte del autor </summary>
        [Display(Name = "Fecha de Muerte del autor")]
        [DefaultValue("")]
        public string Aut_MuerteFecha { get; set; }

        /// <summary> Lugar de Actividad del autor</summary>
        [Display(Name = "Lugar de Actividad del autor")]
        [DefaultValue("")]
        public string Aut_ActividadLugar { get; set; }

        /// <summary> Inicio de actividad del autor</summary>
        [Display(Name = "Inicio de actividad del autor")]
        [DefaultValue("")]
        public string Aut_ActividadFechaInicio { get; set; }

        /// <summary> Fin de actividad del autor</summary>
        [Display(Name = "Fin de actividad del autor")]
        [DefaultValue("")]
        public string Aut_ActividadFechaFin { get; set; }

        /// <summary> Nombre del autor del marco</summary>
        [Display(Name = "Nombre del autor del marco")]
        [DefaultValue("")]
        public string AutMarco_Nombre { get; set; }

        /// <summary> Apellido Paterno del autor del marco</summary>
        [Display(Name = "Apellido Paterno del autor del marco")]
        [DefaultValue("")]
        public string AutMarco_ApellidoPaterno { get; set; }

        /// <summary> Apellido Materno del autor del marco</summary>
        [Display(Name = "Apellido Materno del autor del marco")]
        [DefaultValue("")]
        public string AutMarco_ApellidoMaterno { get; set; }

        /// <summary> Lugar de Nacimiento del autor del marco </summary>
        [Display(Name = "Lugar de Nacimiento del autor del marco")]
        [DefaultValue("")]
        public string AutMarco_NacimientoLugar { get; set; }

        /// <summary> Fecha de Nacimiento del autor del marco</summary>
        [Display(Name = "Fecha de Nacimiento del autor del marco")]
        [DefaultValue("")]
        public string AutMarco_NacimientoFecha { get; set; }

        /// <summary> Lugar de Muerte del autor del marco</summary>
        [Display(Name = "Lugar de Muerte del autor del marco")]
        [DefaultValue("")]
        public string AutMarco_MuerteLugar { get; set; }

        /// <summary> Fecha de Muerte del autor del marco</summary>
        [Display(Name = "Fecha de Muerte del autor del marco")]
        [DefaultValue("")]
        public string AutMarco_MuerteFecha { get; set; }

        /// <summary> Título de la obra</summary>
        [Display(Name = "Título de la obra")]
        [DefaultValue("")]
        public string Titulo { get; set; }

        /// <summary> Subtitulo de la obra</summary>
        [Display(Name = "Subtitulo de la obra")]
        [DefaultValue("")]
        public string SubTitulo { get; set; }

        /// <summary> Título en lengua original de la obra</summary>
        [Display(Name = "Título en lengua original de la obra")]
        [DefaultValue("")]
        public string TituloLenguaOriginal { get; set; }

        /// <summary> Título en ingles de la obra</summary>
        [Display(Name = "Título en ingles de la obra")]
        [DefaultValue("")]
        public string TituloIngles { get; set; }

        /// <summary> Fecha de Factura de la Obra</summary>
        [Display(Name = "Fecha de Factura de la Obra")]
        [DefaultValue("")]
        public string FechaFacturaObra { get; set; }

        /// <summary> Fecha de Concepción de la obra</summary>
        [Display(Name = "Fecha de Concepción de la obra")]
        [DefaultValue("")]
        public string FechaConcepcion { get; set; }

        /// <summary> Fecha de ejecución de la obra</summary>
        [Display(Name = "Fecha de ejecución de la obra")]
        [DefaultValue("")]
        public string FechaEjecucion { get; set; }

        /// <summary> Técnica de la obra</summary>
        [Display(Name = "Técnica de la obra")]
        [DefaultValue("")]
        public string TecnicaObra { get; set; }

        /// <summary>Técnica del marco </summary>
        [Display(Name = "Técnica del marco")]
        [DefaultValue("")]
        public string TecnicaMarco { get; set; }

        /// <summary> Medidas de la obra</summary>
        [Display(Name = "Medidas de la obra")]
        [DefaultValue("")]
        public string MedidasObra { get; set; }

        /// <summary>Medidas del marco </summary>
        [Display(Name = "Medidas del marco")]
        [DefaultValue("")]
        public string MedidasMarco { get; set; }

        /// <summary> Medidas de la base</summary>
        [Display(Name = "Medidas de la base")]
        [DefaultValue("")]
        public string MedidasBase { get; set; }

        /// <summary> Firma</summary>
        [Display(Name = "Firma")]
        [DefaultValue("")]
        public string Firma { get; set; }

        /// <summary> Sello</summary>
        [Display(Name = "Sello")]
        [DefaultValue("")]
        public string Sello { get; set; }

        /// <summary> Serie</summary>
        [Display(Name = "Serie")]
        [DefaultValue("")]
        public string Serie { get; set; }

        /// <summary> Inscripciones</summary>
        [Display(Name = "Inscripciones")]
        [DefaultValue("")]
        public string Inscripciones { get; set; }

        /// <summary> Fundidor autorización</summary>
        [Display(Name = "Fundidor autorización")]
        [DefaultValue("")]
        public string FundidorAutorizacion { get; set; }

        /// <summary> Procedencia</summary>
        [Display(Name = "Procedencia")]
        [DefaultValue("")]
        public string Procedencia { get; set; }

        /// <summary>Autor del texto </summary>
        [Display(Name = "Autor del texto")]
        [DefaultValue("")]
        public string AutorTexto { get; set; }

        /// <summary> Estado del texto</summary>
        [Display(Name = "Estado del texto")]
        [DefaultValue("")]
        public string StatusTexto { get; set; }

        /// <summary> Observaciones de la pintura</summary>
        [Display(Name = "Observaciones de la pintura")]
        [DefaultValue("")]
        public string ObservacionesObra { get; set; }

        /// <summary> Párrafo 1 </summary>
        [Display(Name = "Párrafo 1")]
        [DefaultValue("")]
        public string Parrafo1 { get; set; }

        /// <summary>  Párrafo 2 </summary>
        [Display(Name = "Párrafo 2")]
        [DefaultValue("")]
        public string Parrafo2 { get; set; }

        /// <summary>  Párrafo 3 </summary>
        [Display(Name = "Párrafo 3")]
        [DefaultValue("")]
        public string Parrafo3 { get; set; }

        /// <summary>  Párrafo 4 </summary>
        [Display(Name = "Párrafo 4")]
        [DefaultValue("")]
        public string Parrafo4 { get; set; }

        /// <summary>  Párrafo 5 </summary>
        [Display(Name = "Párrafo 5")]
        [DefaultValue("")]
        public string Parrafo5 { get; set; }

        /// <summary>  Párrafo 6 </summary>
        [Display(Name = "Párrafo 6")]
        [DefaultValue("")]
        public string Parrafo6 { get; set; }
        /// <summary>  Párrafo 7 </summary>
        [Display(Name = "Párrafo 7")]
        [DefaultValue("")]
        public string Parrafo7 { get; set; }
        /// <summary>  Párrafo 8 </summary>
        [Display(Name = "Párrafo 8")]
        [DefaultValue("")]
        public string Parrafo8 { get; set; }


        /// <summary>  Párrafo 9 </summary>
        [Display(Name = "Párrafo 9")]
        [DefaultValue("")]
        public string Parrafo9 { get; set; }

        /// <summary>  Párrafo 10 </summary>
        [Display(Name = "Párrafo 10")]
        [DefaultValue("")]
        public string Parrafo10 { get; set; }

        /// <summary>  Párrafo 11 </summary>
        [Display(Name = "Párrafo 11")]
        [DefaultValue("")]
        public string Parrafo11 { get; set; }

        /// <summary>  Párrafo 12 </summary>
        [Display(Name = "Párrafo 12")]
        [DefaultValue("")]
        public string Parrafo12 { get; set; }

        /// <summary>  Párrafo 13 </summary>
        [Display(Name = "Párrafo 13")]
        [DefaultValue("")]
        public string Parrafo13 { get; set; }

        /// <summary>  Párrafo 14 </summary>
        [Display(Name = "Párrafo 14")]
        [DefaultValue("")]
        public string Parrafo14 { get; set; }

        /// <summary>  Párrafo 15 </summary>
        [Display(Name = "Párrafo 15")]
        [DefaultValue("")]
        public string Parrafo15 { get; set; }

        /// <summary>  Párrafo 16 </summary>
        [Display(Name = "Párrafo 16")]
        [DefaultValue("")]
        public string Parrafo16 { get; set; }

        /// <summary> RA Ruta del Audio</summary>
        [Display(Name = "RA Ruta del Audio")]
        [DefaultValue("")]
        public string Ra_Audio { get; set; }

        /// <summary> RA Ruta del Video</summary>
        [Display(Name = "RA Ruta del Video")]
        [DefaultValue("")]
        public string Ra_Video { get; set; }

        /// <summary> RA Ruta del Interactivo</summary>
        [Display(Name = "RA Ruta del Interactivo")]
        [DefaultValue("")]
        public string Ra_Interactivo { get; set; }

        /// <summary> RA Ruta del  Otro</summary>
        [Display(Name = "RA Ruta del  Otro")]
        [DefaultValue("")]
        public string Ra_Otro { get; set; }

        /// <summary>Observaciones </summary>
        [Display(Name = "Observaciones")]
        [DefaultValue("")]
        public string Observaciones { get; set; }

        /// <summary>Revista mensual </summary>
        [Display(Name = "Revista mensual")]
        [DefaultValue("")]
        public string RevistaMensual { get; set; }

       
    }
}