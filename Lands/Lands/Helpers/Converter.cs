using System;
using System.Collections.Generic;
using System.Text;
using Lands.Domain;
using Lands.Models;

namespace Lands.Helpers
{
    public static class Converter
    {
        public static UserLocal ToUserLocal(User user)
        {
            return new UserLocal
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImagePath = user.ImagePath,
                Telephone = user.Telephone,
                UserId = user.UserId,
                UserTypeId = user.UserTypeId,
            };
        }
    }
}
