using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHK_INCHK_OUT.Model
{
    public class PropertiesOperations
    {
        //add all properties
        public static void RemoveProperties()
        {
            try
            {
                if (App.Current.Properties.ContainsKey("logged"))
                    App.Current.Properties.Remove("logged");

                if (App.Current.Properties.ContainsKey("latitudeCheckIn"))
                    App.Current.Properties.Remove("latitudeCheckIn");

                if (App.Current.Properties.ContainsKey("longitudeCheckIn"))
                    App.Current.Properties.Remove("longitudeCheckIn");

                if (App.Current.Properties.ContainsKey("dateCheckIn"))
                    App.Current.Properties.Remove("dateCheckIn");

                if (App.Current.Properties.ContainsKey("checkIn"))
                    App.Current.Properties.Remove("checkIn");

                if (App.Current.Properties.ContainsKey("AccessToken"))
                    App.Current.Properties.Remove("AccessToken");

                if (App.Current.Properties.ContainsKey("UserIDCRM"))
                    App.Current.Properties.Remove("UserIDCRM");

                if (App.Current.Properties.ContainsKey("Username"))
                    App.Current.Properties.Remove("Username");

                if (App.Current.Properties.ContainsKey("numProy"))
                    App.Current.Properties.Remove("numProy");
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Token GetTokenProperties()
        {
            try
            {
                Token token = new Token();
                if (App.Current.Properties.ContainsKey("AccessToken"))
                    token.AccessToken = (string)App.Current.Properties["AccessToken"];

                if (App.Current.Properties.ContainsKey("UserIDCRM"))
                    token.UserIDCRM = (string)App.Current.Properties["UserIDCRM"];

                if (App.Current.Properties.ContainsKey("Username"))
                    token.Username = (string)App.Current.Properties["Username"];

                return token;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetTokenProperties(Token token)
        {
            try
            {
                App.Current.Properties["AccessToken"] = token.AccessToken;
                App.Current.Properties["UserIDCRM"] = token.UserIDCRM;
                App.Current.Properties["Username"] = token.Username;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
