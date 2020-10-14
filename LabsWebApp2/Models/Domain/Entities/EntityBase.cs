using System;
using System.ComponentModel.DataAnnotations;

namespace LabsWebApp2.Models.Domain.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase() => DateAdded = DateTime.UtcNow.Date;

        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Название / заголовок")]
        [MinLength(1, ErrorMessage = "необходим хотя бы один символ")]
        public virtual string Title { get; set; }

        [Display(Name = "Краткое описание")]
        [MaxLength(256, ErrorMessage = "максимум 256 символов")]
        public virtual string Subtitle { get; set; }

        [Display(Name = "Полное описание")]
        public virtual string Text { get; set; }

        [Display(Name = "Титульная картинка")]
        public virtual string TitleImagePath { get; set; }

        [Display(Name = "SEO метатег Title")]
        [MaxLength(40, ErrorMessage = "максимум 50 символов")]
        public string MetaTitle { get; set; }

        [Display(Name = "SEO метатег Description")]
        [MaxLength(256, ErrorMessage = "максимум 256 символов")]
        public string MetaDescription { get; set; }

        [Display(Name = "SEO метатег Keywords")]
        [MaxLength(256, ErrorMessage = "максимум 256 символов")]
        public string MetaKeywords { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Дата и время")]
        public DateTime DateAdded { get; set; }
    }
}
