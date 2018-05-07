using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lands.Interfaces
{
    public interface IConfig
    {
        //DIRECTORIO EN DONDE SE VA ALMACENAR LA BASE DE DATOS EN EL TELÉFONO
        string DirectoryDB { get; }

        //CUÁLES SON LAS LIBRERÍAS QUE SE VAN A USAR DE LA BASE DE DATOS
        ISQLitePlatform Platform { get; }

    }
}
