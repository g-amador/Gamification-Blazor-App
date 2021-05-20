using GamificationApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamificationApi.DTO
{
    /// <summary>
    /// The player is an user of the application. He can win points and badges when events happening.
    /// </summary>
    public class PlayerDTO
    {
		public long Id { get; set; }
		
		public string NickName { get; set; }
		
		[Required]
		public String FirstName { get; set; }

		[Required]
		public String LastName { get; set; }

		[EmailAddress]
		public String Email { get; set; }

		public int NumberOfPoints { get; set; }

		public int NumberOfCredits { get; set; }

		public ICollection<Badge> Badges { get; set; } = new List<Badge>();
	}
}
