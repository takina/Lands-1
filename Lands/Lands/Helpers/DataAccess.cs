namespace Lands.Helpers
{
    using Interfaces;
    using Models;
    using SQLite.Net;
    using SQLiteNetExtensions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Xamarin.Forms;

    //Se implementa la interfaz IDisposable para no mantener objetos en memoria
    public class DataAccess : IDisposable
    {
        private SQLiteConnection connection;

        public DataAccess()
        {
            //Obteniendo la configuarción de la base de datos
            //es decir , esta línea es lo que nos indica como se debe comportar la aplicación en android y en IOS
            var config = DependencyService.Get<IConfig>();

            //Creando la conexión, dependiendo de la plataforma en que se está ejecutando la aplicación y de una vez
            //se crea el directorio en donde se va a guardar la base de datos
            this.connection = new SQLiteConnection(
                config.Platform,
                Path.Combine(config.DirectoryDB, "Lands.db3"));

            //Aquí se esta creando la tabla, con el modelo User Local
            connection.CreateTable<UserLocal>();
        }

        //Insertando un modelo a la base de datos SQLITE
        public void Insert<T>(T model)
        {
            this.connection.Insert(model);
        }

        //Actualizando un modelo en la base de datos SQLITE
        public void Update<T>(T model)
        {
            this.connection.Update(model);
        }

        //Borrando un modelo a la base de datos SQLITE
        public void Delete<T>(T model)
        {
            this.connection.Delete(model);
        }

        //Este método devuelve el primer registro de la tabla
        public T First<T>(bool WithChildren) where T : class
        {
            if (WithChildren)
            {
                //El parámetro withchildren, representa relaciones. 
                //Devuelveme todos los registros que están relacionados con el primer registro de la tabla que especifiquemos
                return connection.GetAllWithChildren<T>().FirstOrDefault();
            }
            else
            {
                //Que nos devuelva solamente el primer registro sin relaciones
                return connection.Table<T>().FirstOrDefault();
            }
        }

        //Este método devuelve todos los usuarios de la tabla especificada
        public List<T> GetList<T>(bool WithChildren) where T : class
        {
            
            if (WithChildren)
            {
                
                return connection.GetAllWithChildren<T>().ToList();
            }
            else
            {
                return connection.Table<T>().ToList();
            }
        }

        //Para buscar un registro
        public T Find<T>(int pk, bool WithChildren) where T : class
        {
            if (WithChildren)
            {
                return connection.GetAllWithChildren<T>().FirstOrDefault(m => m.GetHashCode() == pk);
            }
            else
            {
                return connection.Table<T>().FirstOrDefault(m => m.GetHashCode() == pk);
            }
        }

        //Este método es el que cierra la conexión
        public void Dispose()
        {
            connection.Dispose();
        }
    }
}

