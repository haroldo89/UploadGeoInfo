using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace UploadGeoInfo.Models
{
    /// <summary>
    /// Funciones de extencion para HttpPostedFile
    /// </summary>
    public static class HttpPostedFileExtensions
    {
        private const int MAX_ALLOWED_MAP_FILE_SIZE = 1024 * 1024 * 100;
        private const int MAX_ALLOWED_GENERIC_FILE_SIZE = 1024 * 1024 * 10;
        private const int MAX_ALLOWED_DATA_FILE_SIZE = 1024 * 1024 * 10;
        private const int MAX_ALLOWED_IMAGE_FILE_SIZE = 1024 * 1024 * 5;
        private const int MAX_ALLOWED_SOUND_FILE_SIZE = 1024 * 1024;

        private static string[] SUPPORTED_MAP_FILE_TYPES = new[] { ".kml" };
        private static string[] SUPPORTED_GENERIC_FILE_TYPES = new[] { ".csv", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf" };
        private static string[] SUPPORTED_DATA_FILE_TYPES = new[] { ".csv", ".xlsx" };
        private static string[] SUPPORTED_IMAGE_FILE_TYPES = new[] { ".jpg", ".jpeg", ".png" };
        private static string[] SUPPORTED_SOUND_FILE_TYPES = new[] { ".mp3", ".acc", ".ogg" };

        /// <summary>
        /// Es archivo valido de imagen
        /// </summary>
        /// <param name="postedFile">archivo cargado</param>
        /// <param name="errorMessage">mensaje de error</param>
        /// <returns></returns>
        public static bool IsImageValidFile(this HttpPostedFileBase postedFile, out string errorMessage)
        {
            bool isValidFile = true;
            errorMessage = string.Empty;
            try
            {
                if (postedFile != null)
                {
                    //Validar longitud del archivo
                    if (postedFile.ContentLength > MAX_ALLOWED_IMAGE_FILE_SIZE)
                    {
                        isValidFile = false;
                        errorMessage = "El tamaño del logo no puede ser superior a 5 mb.";
                    }
                    var fileExtension = Path.GetExtension(postedFile.FileName);
                    //Validar extensión del archivo
                    if (!SUPPORTED_IMAGE_FILE_TYPES.Contains(fileExtension))
                    {
                        isValidFile = false;
                        errorMessage = "El formato del logo no es valido, el formato debe ser (.jpg, .jpeg, .png).";
                    }
                }
                else
                {
                    isValidFile = false;
                }
            }
            catch
            {
                isValidFile = false;
                errorMessage = "Error al momento de cargar el logo";
            }
            return isValidFile;
        }

        /// <summary>
        /// Es archivo valido de sonido
        /// </summary>
        /// <param name="postedFile">archivo cargado</param>
        /// <param name="errorMessage">mensaje de error</param>
        /// <returns></returns>
        public static bool IsSoundValidFile(this HttpPostedFileBase postedFile, out string errorMessage)
        {
            bool isValidFile = true;
            errorMessage = string.Empty;
            try
            {
                if (postedFile != null)
                {
                    //Validar longitud del archivo
                    if (postedFile.ContentLength > MAX_ALLOWED_SOUND_FILE_SIZE)
                    {
                        isValidFile = false;
                        errorMessage = "El tamaño del archivo de audio no puede ser superior a 1 mb.";
                    }

                    var fileExtension = Path.GetExtension(postedFile.FileName);
                    //Validar extensión del archivo
                    if (!SUPPORTED_SOUND_FILE_TYPES.Contains(fileExtension))
                    {
                        isValidFile = false;
                        errorMessage = "El formato del archivo de audio no es valido, el formato debe ser (.mp3, .acc, .ogg).";
                    }
                }
                else
                {
                    isValidFile = false;
                }
            }
            catch
            {
                isValidFile = false;
                errorMessage = "Error al momento de cargar el archivo de audio";
            }
            return isValidFile;
        }

        /// <summary>
        /// Es archivo valido de datos
        /// </summary>
        /// <param name="postedFile">archivo cargado</param>
        /// <param name="errorMessage">mensaje de error</param>
        /// <returns></returns>
        public static bool IsDataValidFile(this HttpPostedFileBase postedFile, out string errorMessage)
        {
            bool isValidFile = true;
            errorMessage = string.Empty;
            try
            {
                if (postedFile != null)
                {
                    //Validar longitud del archivo
                    if (postedFile.ContentLength > MAX_ALLOWED_DATA_FILE_SIZE)
                    {
                        isValidFile = false;
                        errorMessage = "El tamaño del archivo no puede ser superior a 10 MB.";
                    }
                    var fileExtension = Path.GetExtension(postedFile.FileName);
                    //Validar extensión del archivo
                    if (!SUPPORTED_DATA_FILE_TYPES.Contains(fileExtension))
                    {
                        isValidFile = false;
                        errorMessage = string.Format(CultureInfo.InvariantCulture, "El formato del archivo no es valido, los formatos soportados son ({0}).", string.Join(", ", SUPPORTED_DATA_FILE_TYPES));
                    }
                }
                else
                {
                    isValidFile = false;
                }
            }
            catch
            {
                isValidFile = false;
                errorMessage = "Error al momento de cargar el archivo";
            }
            return isValidFile;
        }

        /// <summary>
        /// Permite validar si acepta un archivo determinado, en base a la extension y el tamaño del archivo.
        /// </summary>
        /// <param name="postedFile">archivo cargado</param>
        /// <param name="errorMessage">mensaje de error</param>
        /// <returns></returns>
        public static bool IsGenericValidFile(this HttpPostedFileBase postedFile, out string errorMessage)
        {
            bool isValidFile = true;
            errorMessage = string.Empty;
            try
            {
                if (postedFile != null)
                {
                    //Validar longitud del archivo
                    if (postedFile.ContentLength > MAX_ALLOWED_GENERIC_FILE_SIZE)
                    {
                        isValidFile = false;
                        errorMessage = "El tamaño del archivo no puede ser superior a 10 MB.";
                    }
                    var fileExtension = Path.GetExtension(postedFile.FileName);
                    //Validar extensión del archivo
                    if (!SUPPORTED_GENERIC_FILE_TYPES.Contains(fileExtension))
                    {
                        isValidFile = false;
                        errorMessage = string.Format(CultureInfo.InvariantCulture, "El formato del archivo no es valido, los formatos soportados son ({0}).", string.Join(", ", SUPPORTED_GENERIC_FILE_TYPES));
                    }
                }
                else
                {
                    isValidFile = false;
                }
            }
            catch
            {
                isValidFile = false;
                errorMessage = "Error al momento de cargar el archivo";
            }
            return isValidFile;
        }

        /// <summary>
        /// Permite validar si acepta un archivo de mapas, en base a la extension y el tamaño del archivo.
        /// </summary>
        /// <param name="postedFile">archivo cargado</param>
        /// <param name="errorMessage">mensaje de error</param>
        /// <returns></returns>
        public static bool IsMapValidFile(this HttpPostedFileBase postedFile, out string errorMessage)
        {
            bool isValidFile = true;
            errorMessage = string.Empty;
            try
            {
                if (postedFile != null)
                {
                    //Validar longitud del archivo
                    if (postedFile.ContentLength > MAX_ALLOWED_MAP_FILE_SIZE)
                    {
                        isValidFile = false;
                        errorMessage = "El tamaño del archivo no puede ser superior a 100 MB.";
                    }
                    var fileExtension = Path.GetExtension(postedFile.FileName);
                    //Validar extensión del archivo
                    if (!SUPPORTED_MAP_FILE_TYPES.Contains(fileExtension))
                    {
                        isValidFile = false;
                        errorMessage = string.Format(CultureInfo.InvariantCulture, "El formato del archivo no es valido, los formatos soportados son ({0}).", string.Join(", ", SUPPORTED_MAP_FILE_TYPES));
                    }
                }
                else
                {
                    isValidFile = false;
                }
            }
            catch
            {
                isValidFile = false;
                errorMessage = "Error al momento de cargar el archivo";
            }
            return isValidFile;
        }
    }
}