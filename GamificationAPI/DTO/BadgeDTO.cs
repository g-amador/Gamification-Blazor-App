using System;
using System.ComponentModel.DataAnnotations;

namespace GamificationApi.DTO
{
    /// <summary>
    /// A badge is a distinction, win by a player when he does something significant (on event).
    /// </summary>
    public class BadgeDTO
    {
        public long Id { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }
        
        public String Icon { get; set; }
    }
}
