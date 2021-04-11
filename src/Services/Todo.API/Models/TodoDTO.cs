namespace Todo.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class TodoDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
