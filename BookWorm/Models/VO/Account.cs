using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookWorm.Models.VO
{
    public class Account
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _username;
        private string _password;
        private bool _isAdmin = false;

        public Account() { }

        public Account(int id, string firstName, string lastName, string email, string username, string password, bool isAdmin)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
        }

        public int Id 
        { 
            get { return _id; }
            set { _id = value; }
        }

        public string FirstName 
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName 
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string Email 
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Username 
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password 
        {
            get { return _password; }
            set { _password = value; }
        }

        public bool IsAdmin 
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }

        public override string ToString()
        {
            return String.Format("ID: {0}, First name: {1}, Last name: {2}, E-mail: {3}, IsAdmin: {4}",
                Id.ToString(), FirstName, LastName, Email, IsAdmin.ToString());
        }
    }
}
