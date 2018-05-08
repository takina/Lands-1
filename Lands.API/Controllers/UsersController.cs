using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Lands.API.Helpers;
using Lands.Domain;
using Newtonsoft.Json.Linq;

namespace Lands.API.Controllers
{
    //Cuando hagamos lo que hicimos con el método GetUserByEmail
    //Debemos agregar este DataAnnotations Route Prefix a toda la clase
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [Authorize]//Poniéndole seguridad al método, para poder llamarlo hay que enviarle el token
        [HttpPost]//Le cambiamos la naturaleza al método a un post
        [Route("GetUserByEmail")]//el método se va a llamar GetUserByEmail
        public async Task<IHttpActionResult> GetUserByEmail(JObject form)//Aquí le pasamos un objeto JSon como parámetro llamado form
        {
           
                var email = string.Empty;
                dynamic jsonObject = form;
                try
                {
                    email = jsonObject.Email.Value;//El email viene de un objeto llamado JObject
                }
                catch
                {
                    return BadRequest("Missing Parameter");
                }
            
         
            //User user = await db.Users.FindAsync(id);
            var user = await db.Users.
                Where(u => u.Email.ToLower() == email.ToLower()).
                FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [Authorize]//Poniéndole seguridad al método, para poder llamarlo hay que enviarle el token
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            //Si el usuario tenía foto
            if (user.ImageArray != null && user.ImageArray.Length > 0)
            {
                //Convirtiendo un ImageArray en un stream
                var stream = new MemoryStream(user.ImageArray);

                //Un Guid es un código alfanumérico Random, que va a ser imposible que le de dos Guids iguales
                //Es muy utilizado para generar claves
                var guid = Guid.NewGuid().ToString();

                //Al archivo guid, le ponemos como nombre de archivo jpg
                var file = string.Format("{0}.jpg", guid);

                //Ese archivo se va a guardar en la carpeta Content/Image
                var folder = "~/Content/Images";
                var fullPath = string.Format("{0}/{1}", folder, file);
                //Obtengo el verdadero o falso, depende de si se creó el archivo
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                //Si se pudo crear, guarde el archivo en la propiedad del modelo
                if (response)
                {

                    user.ImagePath = fullPath;
                }

                db.Entry(user).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                
            }
            return Ok(user);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Si el usuario tenía foto
            if (model.ImageArray != null && model.ImageArray.Length > 0)
            {
                //Convirtiendo un ImageArray en un stream
                var stream = new MemoryStream(model.ImageArray);

                //Un Guid es un código alfanumérico Random, que va a ser imposible que le de dos Guids iguales
                //Es muy utilizado para generar claves
                var guid = Guid.NewGuid().ToString();

                //Al archivo guid, le ponemos como nombre de archivo jpg
                var file = string.Format("{0}.jpg", guid);

                //Ese archivo se va a guardar en la carpeta Content/Image
                var folder = "~/Content/Images";
                var fullPath = string.Format("{0}/{1}", folder, file);
                //Obtengo el verdadero o falso, depende de si se creó el archivo
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                //Si se pudo crear, guarde el archivo en la propiedad del modelo
                if (response)
                {

                    model.ImagePath = fullPath;
                }
            }
            

            db.Users.Add(model);
            //Aquí creamos el usuario con toda la información enviada desde el modelo, se guarda en la tabla usuarios de la base de datos
            await db.SaveChangesAsync();

            //Aquí estamos creando el usuario también en la tabla de seguridad
            UsersHelper.CreateUserASP(model.Email, "User", model.Password);

           

            return CreatedAtRoute("DefaultApi", new { id = model.UserId }, model);
        }

        

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}