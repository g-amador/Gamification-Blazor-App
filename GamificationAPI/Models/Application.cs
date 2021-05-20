using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamificationApi.Models
{
    /// <summary>
    /// The application is the entry point of the API. All others resources are link to an application. Each application has a unique apiKey and an apiPassword. 
    /// Those are use for authentification and must be pass on every request (in the http header).
    /// </summary>
    public class Application
    {
        public long Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public String ApiKey { get; set; }

        [DataType(DataType.Password)]
        public String ApiSecret { get; set; }

        public ICollection<Player> Players { get; set; }

        public ICollection<Badge> Badges { get; set; }

        public ICollection<Event> Events { get; set; }

        public ICollection<Rule> Rules { get; set; }
    }
}
