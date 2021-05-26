using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamificationAPI.Models
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
        public String ApiPassword { get; set; }

        public ICollection<Player> Players { get; set; } = new List<Player>();

        public ICollection<Badge> Badges { get; set; } = new List<Badge>();

        public ICollection<Event> Events { get; set; } = new List<Event>();

        public ICollection<Rule> Rules { get; set; } = new List<Rule>();

        public bool IsNotAuthToModify(Application application, string apiKey, string apiPassword)
        {
            return (application.ApiKey != apiKey || application.ApiPassword != apiPassword);
        }
    }
}
