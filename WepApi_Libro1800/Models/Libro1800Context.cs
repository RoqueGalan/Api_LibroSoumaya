using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WepApi_Libro1800.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Libro1800Context : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public Libro1800Context() : base("name=DefaultConnection") { }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Obra> Obras{ get; set; }



        /// <summary>
        /// 
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }
    }
}