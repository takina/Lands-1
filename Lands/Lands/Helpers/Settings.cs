using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lands.Helpers
{
    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        //Guardando parámetros cuando el usuario se loguee
        const string token = "Token";
        const string tokenType = "TokenType";
        //Si no hay toquen, vamos a tener un valor por defecto
        static readonly string stringDefault = string.Empty;

        public static string Token
        {
            get
            {
                //Obtenemos el token y si no hay toquen, obtenemos el valor por default
                return AppSettings.GetValueOrDefault(token, stringDefault);
            }
            set
            {
                //Establecer el valor que tenga token
                AppSettings.AddOrUpdateValue(token, value);
            }
        }


        public static string TokenType
        {
            get
            {
                //Obtenemos el tokenType y si no hay toquenType, obtenemos el valor por default
                return AppSettings.GetValueOrDefault(tokenType, stringDefault);
            }
            set
            {
                //Establecer el valor que tenga tokenType
                AppSettings.AddOrUpdateValue(tokenType, value);
            }
        }
    }

}
