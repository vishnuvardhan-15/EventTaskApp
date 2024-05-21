using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EventDataAccessLayer;
using EventDataAccessLayer.Models;
using EventServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events = EventDataAccessLayer.Models.Events;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventServices.Controllers
{
    [Route("api/[controller]/")] 
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger ;

        public EventController(EventRepository repository, IMapper mapper, ILogger<EventController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get-event")]
        public JsonResult GetEvents()
        {
            List<Models.Event> events = new List<Models.Event>();
            try
            {
                List<Events> eventList = _repository.GetEvent();
                if(eventList != null)
                {
                    foreach(var item in eventList)
                    {
                        Models.Event eveObj = _mapper.Map<Models.Event>(item);
                        events.Add(eveObj);
                    }
                }
                _logger.LogInformation("API-get event");
            }
    
            catch(Exception ex)
            {
                events = null;
                _logger.LogInformation("Error in fetching event");
            }
            _logger.LogInformation("API:  get-event,Controller:Event");
            return new JsonResult(events);
        }

        [HttpPost("create-event")]
        public JsonResult AddEvent(Models.Event eve)
        {
            bool status = false;
            status = _repository.AddEvent(_mapper.Map<Events>(eve));
            _logger.LogInformation("API:  create-event,Controller:Event");
            _logger.LogInformation("API-create event, to add a new event into db");
            return new JsonResult(status);

        }

        [HttpPut("update-event")]
        public JsonResult UpdateEvent(Models.Event eve)
        {
            bool status = false;
            try
            {
                status = _repository.UpdateEvent(_mapper.Map<Events>(eve));
                _logger.LogInformation("API-update event, to update an event into db");
            }
            catch(Exception ex)
            {
                status = false;
                _logger.LogInformation("Error in updating event");
            }
            _logger.LogInformation("API:  update-event,Controller:Event");
            return new JsonResult(status);
        }

        [HttpDelete("delete-event")]
        public JsonResult DeleteEvent(long eventId)
        {
            bool status = false;
            try
            {
                status = _repository.DeleteEvent(eventId);
                _logger.LogInformation("API-delete event, to delete the event from db");
            }
            catch(Exception ex)
            {
                status = false;
                _logger.LogInformation("Error in deleting the event");
            }

            _logger.LogInformation("API:  delete-event,Controller:Event");
            return new JsonResult(status);
        }
    }
}
