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

namespace EventServices.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventRepository repository, IMapper mapper, ILogger<EventController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("get-event")]
        public ActionResult<IEnumerable<Models.Event>> GetEvents()
        {
            try
            {
                var eventList = _repository.GetEvent();
                if (eventList == null)
                {
                    _logger.LogInformation("No events found.");
                    return NotFound();
                }

                var events = eventList.Select(item => _mapper.Map<Models.Event>(item)).ToList();
                _logger.LogInformation("API-get event");
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in fetching event");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create-event")]
        public ActionResult AddEvent(Models.Event eve)
        {
            if (eve == null)
            {
                return BadRequest("Event is null");
            }

            var status = _repository.AddEvent(_mapper.Map<Events>(eve));
            _logger.LogInformation("API:  create-event,Controller:Event");
            if (status)
            {
                return Ok(new { message = "Event created successfully" });
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update-event")]
        public ActionResult UpdateEvent(Models.Event eve)
        {
            if (eve == null)
            {
                return BadRequest("Event is null");
            }

            try
            {
                var status = _repository.UpdateEvent(_mapper.Map<Events>(eve));
                _logger.LogInformation("API-update event, to update an event into db");
                if (status)
                {
                    return Ok(new { message = "Event updated successfully" });
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in updating event");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete-event")]
        public ActionResult DeleteEvent(long eventId)
        {
            try
            {
                var status = _repository.DeleteEvent(eventId);
                _logger.LogInformation("API-delete event, to delete the event from db");
                if (status)
                {
                    return Ok(new { message = "Event deleted successfully" });
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting the event");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}