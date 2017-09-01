using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using OpenPop.Pop3;
using OpenPop.Mime;
using Dial_A_Chef_WebApi;

namespace Dail_a_chef_service.Models
{
    public class Mail
    {
        private String msgContent;
        private DateTime sentDate;
        private String sender;
        private String receiver;
        private String subject;

        private const string SERVER_HOST = "smtp.mailtrap.io";
        private const int PORT_NUM = 2525;
        private const string USERNAME = "bf0ea9ff40679a";
        private const string PASSWORD = "b2f4aa7b562fe0";

        public Mail(String msg, DateTime date, String sender, String receiver, String subject)
        {
            msgContent = msg;
            sentDate = date;
            this.sender = sender;
            this.receiver = receiver;
            this.subject = subject;
        }

        public String SendMail(Mail mail)
        {
            String retMsg = "";
            SmtpClient smtpClient = null;

            if (Mail.ValidateEmail(mail.sender) && Mail.ValidateEmail(mail.receiver))
            {
                smtpClient = new SmtpClient(SERVER_HOST, PORT_NUM)
                {
                    Credentials = new NetworkCredential(USERNAME, PASSWORD),
                    EnableSsl = true
                };
                try
                {
                    smtpClient.Send(mail.sender, mail.receiver, mail.subject, mail.msgContent);
                   
                    retMsg = "Email was sent successfully";

                }
                catch (Exception e)
                {

                    retMsg = e.Message;
           
                }

                if (retMsg.Equals("Email was sent successfully"))
                {
                    this.SaveMail(mail);
                    //this.SaveMailHistory(GetMsgId(mail.msgContent,mail.sender,mail.receiver,mail.sentDate),this.GetUserId(mail.sender));
                }

            }
      
            return retMsg;

        }

       // public Boolean SendMultipleMail()

        private static int GetMsgId(String msgCo, String se, String re, DateTime sendDate)
        {
            int id = -1;

            ubDatabaseDataContext msgContext = new ubDatabaseDataContext();

            var msgId = from m in msgContext.Messages
                where
                m.Msg_Content == msgCo && m.Sent_Date == sendDate && m.Msg_From == se && m.Msg_To == re
                select m;

            try
            {
                if (msgId.FirstOrDefault() != null)
                {
                    id = msgId.FirstOrDefault().Msg_ID;
                }
            }
            catch (NullReferenceException e)
            {
                id = -1;
                Console.Write(e);
            }

          
            return id;
        }

        public static bool ValidateEmail(string email)
        {
            bool isValid = false;
            if(email==null)
            {
                return false; 
            }
            try
            {
                 MailAddress m = new MailAddress(email);

                 isValid = true;
                
            }
            catch (FormatException format)
            {
                Console.WriteLine(format.Message);
            }

            return isValid;
        }

        private int GetUserId(string email)
        {
            int id = -1;
            if (Mail.ValidateEmail(email))
            {
                 ubDatabaseDataContext check = new ubDatabaseDataContext();
                 var user = from k in check.Accounts where k.U_Email == email select k.U_ID;
                 id =  user.First<Int32>();
            }
            return id;
        }

        public static String[]ReceiveMail(String serverName, String userEmail, String password, String port, bool isSSL)
        {
            ValidateEmail(userEmail);
            Pop3Client pop3Client = new Pop3Client();
            pop3Client.Connect(serverName, Int32.Parse(port), isSSL);
            pop3Client.Authenticate(userEmail, password);

            int msgCount = pop3Client.GetMessageCount();

            List<String> msgList = new List<string>();

            msgList.Add("Inbox(" + msgCount + ")");
            //int counter = 0;
            for(int i =0; i<msgCount; i++)
            {
                OpenPop.Mime.Message msg = pop3Client.GetMessage(i);
                String msgCont = i.ToString() + " " + msg.Headers.From.ToString() + " " + msg.Headers.Subject.ToString() + " " + msg.Headers.DateSent.ToString();//msgNumber msgFrom msgSubject msgDateSent   SANITIZE

                msgList.Add(msgCont);
            }

            return msgList.ToArray();
        }

        public static String ReadMail(String serverName, String userEmail, String password, String port, bool isSSL, int intMsgNum)
        {
           // ValidateEmail(userEmail);   
            Pop3Client pop3Client = new Pop3Client();
            pop3Client.Connect(serverName, Int32.Parse(port), isSSL);
            pop3Client.Authenticate("bf0ea9ff40679a", "b2f4aa7b562fe0");

            OpenPop.Mime.Message msg = pop3Client.GetMessage(intMsgNum);
            OpenPop.Mime.MessagePart msgPart = msg.MessagePart.MessageParts[0];

            String msgCont = msg.Headers.From.Address+" "+msg.Headers.Subject+" "+msgPart.BodyEncoding.GetString(msgPart.Body);//sanitize

            return msgCont;

      }

        public void SaveMail(Mail mail)
        {
            ubDatabaseDataContext ubDb = new ubDatabaseDataContext();

            Dial_A_Chef_WebApi.Message msg = new Dial_A_Chef_WebApi.Message()
            {
                Msg_Content = mail.msgContent,
                Sent_Date = mail.sentDate,
                Msg_From = mail.sender,
                Msg_To = mail.receiver
            };

            ubDb.Messages.InsertOnSubmit(msg);
            ubDb.SubmitChanges();

            int msgId = GetMsgId(mail.msgContent, mail.sender, mail.receiver, mail.sentDate);
            int uId = GetUserId(mail.sender);

            SaveMailHistory(msgId,uId);
             
        }

        private void SaveMailHistory(int MsgId, int UId)
        {
            ubDatabaseDataContext ubDb = new ubDatabaseDataContext();
            MessageHistory msgHistory = new MessageHistory()
            {
                Msg_ID = MsgId,
                U_ID = UId
            };

            ubDb.MessageHistories.InsertOnSubmit(msgHistory);
            ubDb.SubmitChanges();
        }

    }
}