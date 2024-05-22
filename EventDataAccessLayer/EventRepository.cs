using System;
using System.Collections.Generic;
using System.Linq;
using EventDataAccessLayer.Models;
using Microsoft.Extensions.Logging;

namespace EventDataAccessLayer
{
    public interface IEventRepository
    {
        List<Events> GetEvent();
        bool AddEvent(Events eve);
        bool UpdateEvent(Events eve);
        bool DeleteEvent(long eventId);
        void ClearAllEvents();
    }

    public class EventRepository : IEventRepository
    {
        private readonly EventDBContext _context;
        private readonly ILogger<EventRepository> _logger;

        public EventRepository(EventDBContext context, ILogger<EventRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Events> GetEvent()
        {
            try
            {
                return (from p in _context.EventTable
                        orderby p.EventId
                        select p).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in fetching event");
                return null;
            }
        }

        public bool AddEvent(Events eve)
        {
            try
            {
                _context.EventTable.Add(eve);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in adding event");
                return false;
            }
        }

        public bool UpdateEvent(Events eve)
        {
            try
            {
                var eventObj = _context.EventTable.FirstOrDefault(ev => ev.EventId == eve.EventId);
                if (eventObj != null)
                {
                    eventObj.CompanyName = eve.CompanyName;
                    eventObj.JobName = eve.JobName;
                    eventObj.PaymentStatus = eve.PaymentStatus;
                    eventObj.RefundStatus = eve.RefundStatus;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating event");
                return false;
            }
        }

        public bool DeleteEvent(long eventId)
        {
            try
            {
                var eventObj = _context.EventTable.FirstOrDefault(eve => eve.EventId == eventId);
                if (eventObj != null)
                {
                    _context.EventTable.Remove(eventObj);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting the event");
                return false;
            }
        }

        public void ClearAllEvents()
        {
            var allEvents = _context.EventTable.ToList();
            _context.EventTable.RemoveRange(allEvents);
            _context.SaveChanges();
        }
    }
}