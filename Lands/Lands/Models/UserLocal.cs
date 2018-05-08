﻿namespace Lands.Models
{
    using SQLite.Net.Attributes;

    public class UserLocal
    {
        [PrimaryKey]//anotación usada en sqlLite
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string ImagePath { get; set; }

        public int? UserTypeId { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "noimage";
                }

                return string.Format(
                    "http://landsapi20180508021825.azurewebsites.net/{0}",
                    ImagePath.Substring(1));
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        public override int GetHashCode()
        {
            return UserId;
        }
    }
}

