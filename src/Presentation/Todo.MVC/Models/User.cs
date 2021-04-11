namespace Todo.MVC.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser<string>
    {
        [Required]
        public string Name { get; set; }
    }
}
