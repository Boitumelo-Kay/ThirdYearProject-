using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dial_A_Chef_WebApi;

namespace Dail_a_chef_service.Models
{
    public class USER
    {

        private string name;
        private string surname;
        private string email;
        private string password;
        private string address;
        private string contact;
        private string userType;
        private string dateOfBirth; // format dd/mm/yyyy
        private string imagePath;

        
        public USER()
        {
            name = null;
            surname = null;
            email = null;
            password = null;
            address = null;
            contact = null;
            userType = null;
            dateOfBirth = null;
            imagePath = null;
        }

        public USER(string Name, string Surname,string password, string Email,string Address, string Contact,string UserType,string DateOfBirth,string ImagePath)
        { 
            name = Name;
            surname = Surname;
            email = Email;
            this.password = password;
            address = Address;
            contact = Contact;
            userType = UserType;
            dateOfBirth = DateOfBirth;
            imagePath = ImagePath;
        }



        public string getName()
        {
            return name;

        }

        public void setName(string name)
        {
            this.name = name;
        }

        public string getSurname()
        {
            return surname;
        }

        public void setSurname(string surname)
        {
            this.surname = surname;
        }

        public string getEmail()
        {
            return email;
        }

        public void setEmail(string email)
        {
            this.email = email;
        }

        public string getContact()
        {
            return contact;
        }

        public void setContact(string contact)
        {
            this.contact = contact;
        }

        public string getPassword()
        {
            return password;
        }

        public void setPassword(string password)
        {
            this.password = password;
        }

        public string getAddress()
        {
            return address;
        }

        public void setAddress(string address)
        {
            this.address = address;
        }



        public string getUserType()
        {
            return userType;
        }

        public void setUserType(string usertype)
        {
            this.userType = usertype;
        }


        public string getDateOfBirth()
        {
            return dateOfBirth;
        }

        public void setDateOfBirth(string date)
        {
            this.dateOfBirth = date;
        }


        public string getImage()
        {
            return imagePath;
        }

        public void setImage(string image)
        {
            this.imagePath = image;
        }


        /**
         * Method registers new user 
         * @return - boolean value indicating successful or failed registration
         */
        public bool register()
        {
            bool isRegistered = false;
            Account newUser = new Account()
            {
                U_Name = getName(),
                U_Surname = getSurname(),
                U_ContactNo = getContact(),
                U_Email = getEmail(),
                U_Password = Hash.ComputeHash(getPassword()),
                U_DOB = getDateOfBirth(),
                U_Type = getUserType(),
                U_Image = getImage(),
            };
           
            ubDatabaseDataContext content = new ubDatabaseDataContext();

            content.Accounts.InsertOnSubmit(newUser);
            content.SubmitChanges();
            isRegistered = true;

            return isRegistered;
        }

        /**
         * Login for registered users
         * @param email - email used during registration
         * @param password - password given during registration
         * @return - boolean value indicating successful or failed login 
         */
        public bool Login(string email, string password)
        {

            bool exists = false;
            string hashPassword = Hash.ComputeHash(password);
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var checkEmail = from k in data.Accounts select k.U_Email;
            var checkPass = from k in data.Accounts select k.U_Password;

            if (checkEmail.Contains<String>(email))
            {
                if (checkPass.Contains<String>(hashPassword))
                {
                    exists = true;
                }
            }

            return exists;
        }

        /**
         * @param strEmail - email address for user updating account 
         * @param strNam - updated name 
         * @param strSurname - updated surname
         * @param strDob - updated date of birth
         * @param strCont - updated contact number
         * @param strAdd - updated home address 
         */ 
        public static bool UpdateAccount(String strEmail, String strName, String strSurname, String strDob, String strCont, String strAdd)
        {
            bool updated = false;

            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            //var uEmail = from k in connect.Accounts where k.U_Email == user.email select k.U_Email;
            var updateUser = connect.Accounts.FirstOrDefault(e => e.U_Email.Equals(strEmail));

            if (!(updateUser.Equals(null)))
            {

                updated = true;

                updateUser.U_Name = strName;
                updateUser.U_Surname = strSurname;
                updateUser.U_DOB = strDob;
                updateUser.U_ContactNo = strCont;
                
             
                connect.SubmitChanges();
            }

            return updated;
        }
    }
}