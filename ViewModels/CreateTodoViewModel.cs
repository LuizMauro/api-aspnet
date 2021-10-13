using System.ComponentModel.DataAnnotations;

namespace MeuTodo.ViewModels
{
    public class SendEventModel
    {
        [Required]
        public string eventName {  get; set; }
        public string message {  get; set; }
    }
}
