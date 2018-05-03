using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lands.API.Helpers
{
    using System.IO;
    using System.Web;

    public class FilesHelper
    {
        //Le estamos pasando un archivo a este método como parámetro
        public static bool UploadPhoto(MemoryStream stream, string folder, string name)
        {
            try
            { 
                //Coge el archivo pasado como parámetro y lo vuelve un archivo físico y lo introduce en el backend
                stream.Position = 0;
                var path = Path.Combine(HttpContext.Current.Server.MapPath(folder), name);
                File.WriteAllBytes(path, stream.ToArray());
            }
            catch
            {
                //si no lo puede hacer, devuelve un false
                return false;
            }
            //si lo puede hacer devuelve verdadero
            return true;
        }
    }
}