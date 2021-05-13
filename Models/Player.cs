using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GamificationApi.Models
{
    public class Player
    {
		public long Id { get; set; }

		public Application Application { get; set; }
		
		public string NickName { get; set; }
		
		[Required]
		public String FirstName { get; set; }

		[Required]
		public String LastName { get; set; }

		[EmailAddress]
		public String Email { get; set; }

		public int NumberOfPoints { get; set; }

		public int NumberOfCredits { get; set; }

		public HashSet<Badge> Badges { get; set; }
	}
}
