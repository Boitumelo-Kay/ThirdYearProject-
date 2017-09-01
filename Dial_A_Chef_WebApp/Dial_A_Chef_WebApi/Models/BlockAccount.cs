using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dial_A_Chef_WebApi;

namespace Dail_a_chef_service.Models
{
    public class BlockAccount
    {
        private String blockedEmail;
        private String reasonBlocked;
        private int blockedId;
        private DateTime dateBlocked;
        private DateTime dateUnblocked;
        private Boolean isStillBlocked;

        public BlockAccount(String email, DateTime dBlocked, DateTime dUnBlocked, String reason, int id)
        {
            blockedEmail = email;
            dateBlocked = dBlocked;
            dateUnblocked = dUnBlocked;
            reasonBlocked= reason;
            blockedId = id;
            isStillBlocked = true;
        }

        public bool AddToBlocked()
        {
            bool isAdded = false;

            BlockedAccount blockedAccount = new BlockedAccount()
            {
                B_Email = this.blockedEmail,
                B_Reason = this.reasonBlocked,
                B_DateBlocked = this.dateBlocked,
                B_DateUnblocked = this.dateUnblocked,
                B_Id = this.blockedId,
                B_IsBlocked = this.isStillBlocked
            };
            
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            data.BlockedAccounts.InsertOnSubmit(blockedAccount);
            data.SubmitChanges();

            var inserted = from i in data.BlockedAccounts
                where i.B_Id == this.blockedId
                select i;
            try
            {
                if (inserted.FirstOrDefault() != null)
                 {
                     isAdded = true;
                 }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
                
            }
            

            return isAdded;
        }

        public static bool IsStillBlocked(String email)
        {
            bool isBlocked = true;
            Mail.ValidateEmail(email);
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var user = from t in data.BlockedAccounts
                where
                t.B_Email == email
                select t;

            try
            {
                if (user.FirstOrDefault() != null)
                {
                    //List<BlockedAccount> bUser = user.ToList();

                    //int id = user.FirstOrDefault().B_Id;
                    DateTime today = DateTime.Today;
                    foreach (BlockedAccount u in user.ToList())
                    {
                        if (u.B_DateUnblocked.Date.ToOADate() < today.ToOADate())
                        {
                            isBlocked = false;
                            u.B_IsBlocked = false;
                            data.SubmitChanges();
                        }
                    }

                }
               
            }
            catch (NullReferenceException ex)
            {
                isBlocked = false;
               Console.WriteLine(ex);
            }

            return isBlocked;
        }

        

    }
}