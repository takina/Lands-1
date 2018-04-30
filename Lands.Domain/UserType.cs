using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lands.Domain
{
    public class UserType
    {
        [Key]
        public int UserTypeId { get; set; }

        [Required(ErrorMessage = "The field {0} is requiered.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [Index("User_Name_Index", IsUnique = true)]//Un índice, para evitar que hayan 2 User Types iguales
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }//Definiendo la relación 1 a muchos, un UserType, puede tener varios Users
    }
}
