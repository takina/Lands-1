using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lands.Helpers
{
    public class FilesHelper
    {
        //Convertimos la foto que tomamos desde la aplicación, en un array de bytes
        //para poderlo enviar a la base de datos por medio del API
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
