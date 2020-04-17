using MobileDevApp.RemoteProviders.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MobileDevApp.Helpers
{
    public class Validator
    {
        private Regex emailRegex { get; set; }
        private Regex phoneRegex { get; set; }
        private Regex hasNumber { get; set; }
        private Regex hasUpperChar { get; set; }
        private Regex hasMiniMaxChars { get; set; }
        private Regex hasLowerChar { get; set; }
        private Regex hasSymbols { get; set; }

        public Validator()
        {
            emailRegex = new Regex(@"^\w.+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$");
            phoneRegex = new Regex(@"^\+?[0-9]{3}-?[0-9]{6,12}$");
            hasNumber = new Regex(@"[0-9]+");
            hasUpperChar = new Regex(@"[A-Z]+");
            hasMiniMaxChars = new Regex(@".{8,15}");
            hasLowerChar = new Regex(@"[a-z]+");
            hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
        }

        public bool ValidateName(string name, out string exception)
        {
            exception = "";

            if (string.IsNullOrEmpty(name))
            {
                exception = "Name cannot be empty.";
                return false;
            }

            if (name.Length == 0)
            {
                exception = "Name must be at least 1 character";
                return false;
            }

            return true;
        }

        public bool ValidatePassword(string password, out string exception)
        {
            exception = "";

            if (string.IsNullOrEmpty(password))
            {
                exception = "Password cannot be empty.";
                return false;
            }

            if (password.Length < 8)
            {
                exception = "Password must be longer than 8 characters.";
                return false;
            }

            if (!hasLowerChar.IsMatch(password))
            {
                exception = "Password should contain at least one lower case letter.";
                return false;
            }
            else if (!hasUpperChar.IsMatch(password))
            {
                exception = "Password should contain at least one upper case letter.";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(password))
            {
                exception = "Password should not be lesser than 8 or greater than 15 characters.";
                return false;
            }
            else if (!hasNumber.IsMatch(password))
            {
                exception = "Password should contain at least one numeric value.";
                return false;
            }

            else if (!hasSymbols.IsMatch(password))
            {
                exception = "Password should contain at least one special case character.";
                return false;
            }

            return true;
        }

        public bool ValidatePasswordsEquals(string password, string confirmPassword, out string exception)
        {
            exception = "";

            if (string.IsNullOrEmpty(password))
            {
                exception = "Password cannot be empty.";
                return false;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                exception = "Password confirm cannot be empty.";
                return false;
            }

            if (!password.Equals(confirmPassword))
            {
                exception = "Password must be the same as password confirmation.";
                return false;
            }

            return true;
        }

        public bool ValidateLogin(string login, out string exception)
        {
            exception = "";

            if (string.IsNullOrEmpty(login))
            {
                exception = "Login cannot be empty.";
                return false;
            }

            if (login.Length < 8)
            {
                exception = "Login must be longer than 8 characters.";
                return false;
            }

            if (!emailRegex.IsMatch(login) && !phoneRegex.IsMatch(login))
            {
                exception = "Login must be phone number or email.";
                return false;
            }

            return true;
        }

        public LoginType GetLoginType(string login)
        {
            if (emailRegex.IsMatch(login))
            {
                return LoginType.Email;
            }
            else
            {
                return LoginType.PhoneNumber;
            }
        }

        public bool ValidateString(string str, out string exception)
        {
            exception = "";

            if (string.IsNullOrEmpty(str))
            {
                exception = "Login cannot be empty.";
                return false;
            }

            return true;
        }

        public bool ValidatePhoneNumber(string phoneNumber, out string exception)
        {
            exception = "";

            if (string.IsNullOrEmpty(phoneNumber))
            {
                exception = "Name cannot be empty.";
                return false;
            }

            Regex phoneRegex = new Regex(@"^\+?[0-9]{3}-?[0-9]{6,12}$");

            if (!phoneRegex.IsMatch(phoneNumber))
            {
                exception = "Phone number must be like +xx(xxx)xxxxxxx.";
                return false;
            }

            return true;
        }

        public bool ValidateEmail(string email, out string exception)
        {
            exception = "";

            if (string.IsNullOrEmpty(email))
            {
                exception = "Login cannot be empty.";
                return false;
            }

            Regex emailRegex = new Regex(@"^\w.+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$");

            if (!emailRegex.IsMatch(email))
            {
                exception = "Email must be valid.";
                return false;
            }

            return true;
        }
    }
}
