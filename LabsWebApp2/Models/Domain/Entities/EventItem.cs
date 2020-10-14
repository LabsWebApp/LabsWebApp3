using System.ComponentModel.DataAnnotations;

namespace LabsWebApp2.Models.Domain.Entities
{
    public class EventItem : EntityBase
    {
        [Required(ErrorMessage = "Заполните название события или новости")]
        [Display(Name = "Название события или новости")]
        public override string Title { get; set; }
        [Display(Name = "Краткое описание события")]
        public override string Subtitle { get; set; }

        [Display(Name = "Полное описание события")]
        public override string Text { get; set; }
    }
}
