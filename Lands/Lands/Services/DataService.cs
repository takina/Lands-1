namespace Lands.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;

    //Este servicio es el encargado de manejar todo lo relacionado con los datos
    // de la aplicación.

        //Esta clase empaqueta la funcionalidad de la base de datos para que sea más manipulable
    public class DataService
    {
        public bool DeleteAll<T>() where T : class
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecords = da.GetList<T>(false);
                    foreach (var oldRecord in oldRecords)
                    {
                        da.Delete(oldRecord);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        //Borra todo e inserta nuevo
        public T DeleteAllAndInsert<T>(T model) where T : class
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecords = da.GetList<T>(false);
                    foreach (var oldRecord in oldRecords)
                    {
                        da.Delete(oldRecord);
                    }

                    da.Insert(model);

                    return model;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return model;
            }
        }

        //Yo le mando un modelo, si existe lo actualiza y si no existe lo inserta
        public T InsertOrUpdate<T>(T model) where T : class
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecord = da.Find<T>(model.GetHashCode(), false);
                    if (oldRecord != null)
                    {
                        da.Update(model);
                    }
                    else
                    {
                        da.Insert(model);
                    }

                    return model;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return model;
            }
        }

        //Insertar
        public T Insert<T>(T model)
        {
            using (var da = new DataAccess())
            {
                da.Insert(model);
                return model;
            }
        }

        //Encontrar
        public T Find<T>(int pk, bool withChildren) where T : class
        {
            using (var da = new DataAccess())
            {
                return da.Find<T>(pk, withChildren);
            }
        }

        //El primer registro 
        public T First<T>(bool withChildren) where T : class
        {
            using (var da = new DataAccess())
            {
                return da.GetList<T>(withChildren).FirstOrDefault();
            }
        }

        //Deme la lista de todos
        public List<T> Get<T>(bool withChildren) where T : class
        {
            using (var da = new DataAccess())
            {
                return da.GetList<T>(withChildren).ToList();
            }
        }

        //Actualiza
        public void Update<T>(T model)
        {
            using (var da = new DataAccess())
            {
                da.Update(model);
            }
        }

        //Borre
        public void Delete<T>(T model)
        {
            using (var da = new DataAccess())
            {
                da.Delete(model);
            }
        }

        //Guarde una lista completa
        public void Save<T>(List<T> list) where T : class
        {
            using (var da = new DataAccess())
            {
                foreach (var record in list)
                {
                    InsertOrUpdate(record);
                }
            }
        }
    }
}
