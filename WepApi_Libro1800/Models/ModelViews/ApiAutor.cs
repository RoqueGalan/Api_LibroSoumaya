using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WepApi_Libro1800.Models.ModelViews
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiAutor
    {
        /// <summary>
        /// 
        /// </summary>
        public int Orden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Prefijo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Seudonimo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ApellidoPaterno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ApellidoMaterno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NacimientoLugar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NacimientoFecha { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MuerteLugar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MuerteFecha { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivoLugar { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivoFechaInicio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivoFechaFin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Observaciones { get; set; }
    }
}