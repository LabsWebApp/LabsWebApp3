using System.ComponentModel.DataAnnotations;

namespace LabsWebApp3.Models.Domain.Entities
{
    public class EventItem : EntityBase
    {
        [Required(ErrorMessage = "Заполните название события")]
        [Display(Name = "Название события")]
        public override string Title { get; set; }
        [Display(Name = "Краткое описание события")]
        public override string Subtitle { get; set; }

        [Display(Name = "Полное описание события")]
        public override string Text { get; set; }
    }
}
