using System;

namespace GamificationAPI.DTO
{
    /// <summary>
    /// The application is the entry point of the API. All others resources are link to an application. Each application has a unique apiKey and an apiPassword. 
    /// Those are use for authentification and must be pass on every request (in the http header).
    /// </summary>
    public class ApplicationDTO
    {        
        public String Name { get; set; }

        public String Description { get; set; }

        public String ApiKey { get; set; }
    }
}
