using System.ComponentModel.DataAnnotations;


namespace GamificationAPI.Models
{
    public class Employee
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public int Age { get; set; }

		[Required]
		public string Designation { get; set; }

		[Required]
		public int MobileNumber { get; set; }
	}
}
