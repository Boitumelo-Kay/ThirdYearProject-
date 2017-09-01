using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dial_A_Chef_WebApi;

namespace Dail_a_chef_service.Models
{
    public class ADMIN : USER
    {

        private DateTime startDate;
        private DateTime endDate;
        private int id;

        public ADMIN()
        {

        }

        public ADMIN(string Name, string Surname, string password, string Email, string Address, string Contact,
            string UserType, string DateOfBirth, string ImagePath, DateTime startDate, DateTime endDate)
            : base(Name, Surname, password, Email, Address, Contact,
                UserType, DateOfBirth, ImagePath)
        {

            this.startDate = startDate;
            this.endDate = endDate;
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public static bool RegisterAdmin(ADMIN admin)
        {
            bool isRegistered = false;

            ubDatabaseDataContext data = new ubDatabaseDataContext();

            Account newUser = new Account()
            {
                U_Name = admin.getName(),
                U_Surname = admin.getSurname(),
                U_Email = admin.getEmail(),
                U_Password = Hash.ComputeHash(admin.getPassword()),
                U_DOB = admin.getDateOfBirth(),
                U_ContactNo = admin.getContact(),
                U_Image = null,
                U_Type = admin.getUserType().ToUpper()
            };

            data.Accounts.InsertOnSubmit(newUser);
            data.SubmitChanges();

            try
            {

                var userId = from a in data.Accounts
                    where a.U_Email == newUser.U_Email
                    select a.U_ID;


                int adminID = userId.FirstOrDefault<Int32>();

                if (adminID>0)
                {
                    admin.Id = adminID;

                    Administrator newAdmin = new Administrator()
                    {
                        Admin_ID = admin.Id,
                        Contract_Start = admin.startDate,
                        Contract_End = admin.endDate
                    };

                    data.Administrators.InsertOnSubmit(newAdmin);
                    data.SubmitChanges();
                    isRegistered = true;
                }
                  
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                throw;
            }
            return isRegistered;
        }
    }
}