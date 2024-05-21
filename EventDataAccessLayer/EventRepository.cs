using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using EventDataAccessLayer.Models;
using Microsoft.Extensions.Logging;

namespace EventDataAccessLayer
{
    public class EventRepository
    {

        private readonly EventDBContext _context;
        public EventRepository(EventDBContext context)
        {
            _context = context;
        }

        #region Library for MVC/Services Demo

        #region-To get all event details
        public List<Events> GetEvent()
        {
            List<Events> lstEvent = null;
            try
            {
                lstEvent = (from p in _context.Event
                               orderby p.EventId
                               select p).ToList();
                //logger.LogInformation("API-get event");
            }
            catch (Exception e)
            {
                lstEvent = null;
                //logger.LogInformation("Error in fetching event");
            }
            return lstEvent;
        }
        #endregion


        #region- To add a new event
        public bool AddEvent(Events eve)
        {
            bool status = false;
            try
            {
                Events eventObj = new Events();
                eventObj.EventId = eve.EventId;
                eventObj.CompanyId = eve.CompanyId;
                eventObj.CompanyName = eve.CompanyName;
                eventObj.EventType = eve.EventType;
                eventObj.PaymentStatus = eve.PaymentStatus;
                eventObj.JobId = eve.JobId;
                eventObj.JobName = eve.JobName;
                eventObj.RefundStatus = eve.RefundStatus;
                eventObj.FundValue = eve.FundValue;
                eventObj.EventTriggeredBy = eve.EventTriggeredBy;
                eventObj.EventTriggerType = eve.EventTriggerType;
                eventObj.UserComments = eve.UserComments;
                eventObj.TimeStampValue = eve.TimeStampValue;
                _context.Event.Add(eventObj);
                _context.SaveChanges();
                status = true;
                //logger.LogInformation("API-create event, to add a new event into db");
            }
            catch (Exception ex)
            {
                status = false;
                //logger.LogInformation("Error in adding event");
            }
            return status;
        }
        #endregion

        #region- To update the existing event details
        public bool UpdateEvent(Events eve)
        {
            bool status = false;
            try
            {
                var eventObj = (from ev in _context.Event
                               where ev.EventId == eve.EventId
                               select ev).FirstOrDefault<Events>();
                if (eventObj != null)
                {
                    eventObj.CompanyName = eve.CompanyName;
                    eventObj.JobName = eve.JobName;
                    eventObj.PaymentStatus = eve.PaymentStatus;
                    eventObj.RefundStatus = eve.RefundStatus;
                    _context.SaveChanges();
                    status = true;
                    //logger.LogInformation("API-update event, to update an event into db");
                }
            }
            catch (Exception ex)
            {
                status = false;
                //logger.LogInformation("Error in updating event");
            }
            return status;
        }
        #endregion

        #region-To delete a existing event
        public bool DeleteEvent(long eventId)
        {
            bool status = false;
            try
            {
                var eventObj = (from eve in _context.Event
                               where eve.EventId == eventId
                               select eve).FirstOrDefault<Events>();
                _context.Event.Remove(eventObj);
                _context.SaveChanges();
                status = true;
                //logger.LogInformation("API-delete event, to delete the event from db");
            }
            catch (Exception)
            {
                status = false;
                //logger.LogInformation("Error in deleting the event");
            }
            return status;
        }
        #endregion

        

        #endregion
    }
}
