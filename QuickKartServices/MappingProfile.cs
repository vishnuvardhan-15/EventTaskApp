using AutoMapper;
using EventDataAccessLayer.Models;
using EventServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events = EventDataAccessLayer.Models.Events;

namespace EventServices
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, Events>();
            CreateMap<Events, Event>();
        }
    }
}
