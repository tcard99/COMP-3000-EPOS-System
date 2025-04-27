using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Services
{
    public class EmailValidationService
    {
        //Method to check if input is valid email format
        public static bool isValidEmail(string email)
        {
            try
            {
                MailAddress mailAddr = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
