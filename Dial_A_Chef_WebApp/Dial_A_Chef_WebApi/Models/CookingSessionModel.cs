using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial_A_Chef_WebApi.Models
{
    public class CookingSessionModel
    {
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
        public int SessionDuration { get; set; }

        public CookingSessionModel()
        {

        }

        public DateTime StartSession()
        {
            SessionStart = DateTime.Today;
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            return SessionStart;
        }

        public DateTime EndSession()
        {
            SessionEnd = DateTime.Today;
            return SessionEnd;
        }

        /*
            Returns the Duration of the cooking session in minutes(integer)
        */
        public int CalcTimeDuration()
        {
            int duration = 0;
            duration = SessionEnd.TimeOfDay.Minutes - SessionStart.TimeOfDay.Minutes;

            if (duration > 0)
            {
                SessionDuration = duration;
                return SessionDuration;
            }
                
            return duration;
        }

        public bool SaveSession()
        {
            bool isSaved = false;
            if(this!=null)
            {
                ubDatabaseDataContext data = new ubDatabaseDataContext();

                Dial_A_Chef_WebApi.CookingSession session = new Dial_A_Chef_WebApi.CookingSession()
                {
                    SessionStart = this.SessionStart,
                    SessionEnd = this.SessionEnd,
                    SessionDuration = this.CalcTimeDuration()
                };

                data.CookingSessions.InsertOnSubmit(session);
                data.SubmitChanges();

                var saved = from d in data.CookingSessions
                            where d.CS_Id == session.CS_Id
                            select d;

                if(saved.First()!=null)
                {
                    isSaved = true;
                }

            }
            return isSaved;
        }
    }
}