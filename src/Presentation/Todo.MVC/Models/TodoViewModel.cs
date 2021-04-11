namespace Todo.MVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TodoViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }
    }
}

