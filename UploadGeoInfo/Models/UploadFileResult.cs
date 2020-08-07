// ===================================================================================================
// Desarrollado Por		    :   Harold Caicedo.
// Fecha de Creación		:   2015/05/04.
// Producto o sistema	    :   Wizenz.Navi
// Empresa			        :   Wizenz Technologies
// Proyecto			        :   NAVI30
// Cliente			        :   Varios.
// ===================================================================================================
// Versión	        Descripción
// 1.0.0.0	        Propiedades de un archivo subido al sistema desde control de jquery.
// ===================================================================================================
// HISTORIAL DE CAMBIOS:
// ===================================================================================================
// Ver.	 Fecha		    Autor					Descripción
// ---	 -------------	----------------------	------------------------------------------------------
// XX	 yyyy/MM/dd	    [Nombre Completo]	    [Razón del cambio realizado] 
// ===================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UploadGeoInfo.Models
{
    /// <summary>
    /// Propiedades de un archivo subido al sistema desde control de jquery
    /// </summary>
    [DataContract(Name = "uploadFileResult")]
    public class UploadFileResult
    {
        /// <summary>
        /// Nombre de archivo.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Tipo de archivo.
        /// </summary>
        [DataMember(Name = "fileType")]
        public string FileType { get; set; }

        /// <summary>
        /// Directorio de archivo.
        /// </summary>
        [DataMember(Name = "directory")]
        public string Directory { get; set; }

        /// <summary>
        /// Tamaño de archivo.
        /// </summary>
        [DataMember(Name = "length")]
        public int Length { get; set; }
    }
}