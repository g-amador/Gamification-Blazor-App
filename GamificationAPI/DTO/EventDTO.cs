using System;
using System.ComponentModel.DataAnnotations;

namespace GamificationApi.DTO
{
    /// <summary>
    /// When something happening to a player or when a player do something, an event should be created. An event is link to a player and is of a type. 
    /// Multiple events of the same type and player can be create. An event can't be deleted.
    /// </summary>
    public class EventDTO
    {
        public long Id { get; set; }
        
        public String Type { get; set; }

        [Timestamp]
        public DateTime Timestamp { get; set; }

        public long PlayerId { get; set; }
    }
}
