using System;
using System.ComponentModel.DataAnnotations;

namespace LabsWebApp5.Models.Domain.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase() => DateAdded = DateTime.UtcNow.Date;

        [Required]
        public Guid Id { get;set; }

        [Display(Name = "Название / заголовок"), MaxLength(50)]
        public virtual string Title { get; set; }

        [Display(Name = "Краткое описание"), MaxLength(256)]
        public virtual string Subtitle { get; set; }

        [Display(Name = "Полное описание")]
        public virtual string Text { get; set; }

        [Display(Name = "Титульная картинка")]
        public virtual string TitleImagePath { get; set; }

        [Display(Name = "SEO метатег Title"), MaxLength(50)]
        public string MetaTitle { get; set; }

        [Display(Name = "SEO метатег Description"), MaxLength(256)]
        public string MetaDescription { get; set; }

        [Display(Name = "SEO метатег Keywords"), MaxLength(50)]
        public string MetaKeywords { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Дата и время")]
        public DateTime DateAdded { get; set; }
    }
}
