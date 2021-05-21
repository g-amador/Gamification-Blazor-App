using System;

namespace GamificationAPI.Models
{
    /// <summary>
    /// A rule determine how many points or/and which badge can be win by a player on an event type. Many rules can be create for the same event type.
    /// </summary>
    public class Rule
    {
        public long Id { get; set; }
                
        public String OnEventType { get; set; }
        
        public int NumberOfPoints { get; set; }

        public int NumberOfCredits { get; set; }

        public Badge Badge { get; set; }

        public Application Application { get; set; }
    }
}
