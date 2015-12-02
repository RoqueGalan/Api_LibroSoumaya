using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WepApi_Libro1800.Models.ModelViews
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiObra
    {

        /// <summary>
        /// 
        /// </summary>
        public List<ApiAutor> Autores { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ApiMultimedia> Multimedias { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ApiCampo> Campos { get; set; }
    }
}