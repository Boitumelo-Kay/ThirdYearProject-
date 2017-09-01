using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web;
using Dial_A_Chef;
using Dial_A_Chef.Models;
using Dial_A_Chef_WebApi;
using Dail_a_chef_service.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Dial_A_Chef.Models
{
    public class UserAccount
    {
        //[Required(ErrorMessage ="Please Enter Your Name")]
        public string U_Name { get; set; }
        //[Required(ErrorMessage = "Please Enter Your Surname")]
        public string U_Surname { get; set; }
        //[Required(ErrorMessage = "Please Enter Your Email Address")]
        [JsonProperty("email")]
        public string U_Email { get; set; }
        //[Required(ErrorMessage = "Please Enter Your Password")]
        [JsonProperty("password")]
        public string U_Password { get; set; }
        //[Required(ErrorMessage = "Please Enter Your Date of Birth")]
        public string U_DOB { get; set; }
        //[Required(ErrorMessage = "Please Enter Your Contact Number")]//@Html.ValidationMessageFor(model => model.Contact)
        public string U_ContactNo { get; set; }
        public string U_Image { get; set; }
        public string U_Type { get; set; }

        public int U_ID { get; set; }

        public UserAccount()
        { }

        public UserAccount(string name, string surname, string email, string password, string dob, string contact, string image, string utype)
        {
           
            U_Name = name;
            U_Surname = surname;
            U_Email = email;
            U_Password = password;
            U_DOB = dob;
            U_ContactNo = contact;
            U_Image = image;
            U_Type = utype;

        }

        public UserAccount(String email,String password) {


            U_Email = email;
            U_Password = password;

        }

        /*
            Retrieves a list of all of the users in the database
        */
        public static List<UserAccount> UsersList()
        {
            List<UserAccount> usersList = new List<UserAccount>();

            try
            {
                ubDatabaseDataContext data = new ubDatabaseDataContext();
                var users = from u in data.Accounts
                    select u;

                if (users.FirstOrDefault() != null)
                {
                    foreach (Account u in users.ToList())
                    {
                        UserAccount account = new UserAccount()
                        {
                            U_ID = u.U_ID,
                            U_Name = u.U_Name,
                            U_Surname = u.U_Surname,
                            U_Email = u.U_Email,
                            U_Password = u.U_Password,
                            U_DOB = u.U_DOB,
                            U_ContactNo = u.U_ContactNo,
                            U_Image = u.U_Image,
                            U_Type = u.U_Type
                        };
                        
                        usersList.Add(account);
                    }
                }

            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }
            return usersList;
        }

        /*
            Retrieves user information by the user id

        */
        public static UserAccount FindUserAccount(int id)
        {
            UserAccount user = null;
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            try
            {
                var userExists = from u in data.Accounts
                    where u.U_ID == id
                    select u;

                if (userExists.First() != null)
                {
                    user = new UserAccount()//save information from database to UserAccount object
                    {
                        U_ID = userExists.First().U_ID,
                        U_Name = userExists.First().U_Name,
                        U_Surname = userExists.First().U_Surname,
                        U_ContactNo = userExists.First().U_ContactNo,
                        U_DOB = userExists.First().U_DOB,
                        U_Email = userExists.First().U_Email,
                        U_Image = userExists.First().U_Image,
                        U_Password = userExists.First().U_Password,
                        U_Type = userExists.First().U_Type
                    };

                }


            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }

            return user;
        }
        /*
            Checks if the user exists in the database
            uses the email as the condition for checking if the user exists
        */
        public bool IsExistingUser()
        {
            bool exists = false;

            ubDatabaseDataContext data = new ubDatabaseDataContext();

            try
            {
                var user = from u in data.Accounts
                    where u.U_Email == this.U_Email
                    select u;

                if (user.FirstOrDefault() != null)
                {
                    exists = true;
                }

            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }

            return exists;
        }
        /*
            Registers a user according to the user type
            Base type goes to parent table
        */
        public bool RegisterUser(string name, string surname, string email, string password, string dob,
            string contact, string image, string utype)
        {
            bool isRegistered = false;
            Mail.ValidateEmail(email);
            password = new StringBuilder(Hash.ComputeHash(password)).ToString();
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            UserAccount newUser = new UserAccount()
            {
                U_Name = name,
                U_ContactNo = contact,
                U_DOB = dob,
                U_Email =  email,
                U_Image = image,
                U_Password = password,
                U_Surname = surname,
                U_Type = utype
            };

            bool exists = newUser.IsExistingUser();//checks if the email used exists in the database

            if (exists==false)//register the user if the email doesn't exist
            {
                Account regAccount = new Account()
                {
                    U_Name = newUser.U_Name,
                    U_ContactNo = newUser.U_ContactNo,
                    U_DOB = newUser.U_DOB,
                    U_Email = newUser.U_Email,
                    U_Image = newUser.U_Image,
                    U_Password = newUser.U_Password,
                    U_Surname = newUser.U_Surname,
                    U_Type = newUser.U_Type
                };

                data.Accounts.InsertOnSubmit(regAccount);
                data.SubmitChanges(ConflictMode.FailOnFirstConflict);//update the database if no conflict occurs

                //make sure that the new user exists in the database
                var idFound = from i in data.Accounts
                              where i.U_ID == regAccount.U_ID
                              select regAccount.U_ID;

                try
                {
                    if (idFound.First()>0)
                    {
                        isRegistered = true;//because you are certain that the user exists
                        switch (regAccount.U_Type.ToUpper())
                        {
                            case "CHEF":
                                //register user as a chef
                               isRegistered = Models.ChefAccount.RegisterChef(idFound.First());
                                break;
                            case "Customer":
                                //register user as a customer
                              isRegistered =  Models.CustomerAccount.RegisterCust(idFound.First());
                                break;
                            case "ADMIN":
                                //register user as an admin
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    throw new NullReferenceException();
                }
                
            }

            return isRegistered;
        }


        public bool RegisterUser()
        {
            bool isRegistered = false;
            Mail.ValidateEmail(this.U_Email);
            this.U_Password = new StringBuilder(Hash.ComputeHash(this.U_Password)).ToString();
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            UserAccount newUser = new UserAccount()
            {
                U_Name = this.U_Name,
                
                U_ContactNo = this.U_ContactNo,
                U_DOB = this.U_DOB,
                U_Email = this.U_Email,
                U_Image = this.U_Image,
                U_Password = this.U_Password,
                U_Surname = this.U_Surname,
                U_Type = this.U_Type
            };

            bool exists = newUser.IsExistingUser();//checks if the email used exists in the database

            if (exists == false)//register the user if the email doesn't exist
            {
                Account regAccount = new Account()
                {
                    U_Name = newUser.U_Name,
                    U_ContactNo = newUser.U_ContactNo,
                    U_DOB = newUser.U_DOB,
                    U_Email = newUser.U_Email,
                    U_Image = newUser.U_Image,
                    U_Password = newUser.U_Password,
                    U_Surname = newUser.U_Surname,
                    U_Type = newUser.U_Type
                };

                data.Accounts.InsertOnSubmit(regAccount);
                data.SubmitChanges(ConflictMode.FailOnFirstConflict);//update the database if no conflict occurs

                //make sure that the new user exists in the database
                var idFound = from i in data.Accounts
                              where i.U_ID == regAccount.U_ID
                              select regAccount.U_ID;

                try
                {
                    if (idFound.First() > 0)
                    {
                        isRegistered = true;//because you are certain that the user exists
                        switch (regAccount.U_Type.ToUpper())
                        {
                            case "CHEF":
                                //register user as a chef
                                isRegistered = Models.ChefAccount.RegisterChef(idFound.First());
                                break;
                            case "Customer":
                                //register user as a customer
                                isRegistered = Models.CustomerAccount.RegisterCust(idFound.First());
                                break;
                            case "ADMIN":
                                //register user as an admin
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    throw new NullReferenceException();
                }

            }

            return isRegistered;
        }






        //helper function that finds a user's email by querying the user's id
        public static string FindUserEmail(int id)
        {
            string strRet = "";
            ubDatabaseDataContext data = new ubDatabaseDataContext();
            var email = from d in data.Accounts
                where d.U_ID == id
                select d.U_Email;

            try
            {
                if (email.First() != null)
                {
                    strRet = email.First();
                }
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return strRet;
        }

        /*
            checks if a user with the specified email AND password exists in the database
        */
        public bool Login(string email, string password)
        {
            bool isLoggedIn = false;
            password = new StringBuilder(Hash.ComputeHash(password)).ToString();
            if (Mail.ValidateEmail(email))//validate the email address
            {
                UserAccount loginUser = new UserAccount();
                loginUser.U_Email = email;
                if (loginUser.IsExistingUser())//check if a user with such an email exists
                {
                    //check if the existing email and password are a matching combination
                    ubDatabaseDataContext data = new ubDatabaseDataContext();
                    var exists = from d in data.Accounts
                                 where d.U_Email == email && d.U_Password == password
                                 select d;

                    if (exists.First() != null)
                    {
                        
                        isLoggedIn = true;
                    }
                }
               
            }

            return isLoggedIn;
        }

        /*
            Updates the specfied user account
            On client side, turn user input into json object of UserAccount
            Then pass that through as paramenter
        */
        public void UpdateUserDetails()
        {
            ubDatabaseDataContext data = new ubDatabaseDataContext();
     
            try
            {
                if (Mail.ValidateEmail(this.U_Email))
                {
                    if (FindUserId(this.U_Email) <= 0) return;
                    var user = data.Accounts.FirstOrDefault(d => d.U_ID == FindUserId(this.U_Email));

                    if (user == null) return;
                    user.U_Name = this.U_Name;
                    user.U_Surname = this.U_Surname;
                    user.U_DOB = this.U_DOB;
                    user.U_ContactNo = this.U_ContactNo;
                    user.U_Image = this.U_Image;
                }
               
                data.SubmitChanges();

            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        //validate email function
        //validate contact function
        //validate date function

        /*
            Finds a user's ID by searching for the user's email address
        */  
        public int FindUserId(string email)
        {
            int id = -1;
            ubDatabaseDataContext data = new ubDatabaseDataContext();
            var userId = from d in data.Accounts
                where d.U_Email == email
                select d.U_ID;

            try
            {
                if (userId.First()>0)
                {
                    id = userId.First();
                }
            }
            catch (NullReferenceException)
            { 
                throw new NullReferenceException();
            }

            return id;
        }

        public string GetUserType(string email)
        {
            string strRet = "";
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var user = from d in data.Accounts
                       where d.U_Email == email
                       select d;

            if(user.FirstOrDefault()!=null)
            {
                Account userAcc = user.First();
                strRet = userAcc.U_Type;
            }

            return strRet;
        }

        public static string GetType(string email)
        {
            string strRet = "";
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var user = from d in data.Accounts
                       where d.U_Email == email
                       select d;

            if (user.FirstOrDefault() != null)
            {
                Account userAcc = user.First();
                strRet = userAcc.U_Type;
            }

            return strRet;
        }

    }
}