using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LabsWebApp5.Models
{
    public class InfoModel
    {
        public string Area { get; } = "";
        [MaxLength(50)] public string Title { get; set; } = "ИНФО";
        [NotNull] public string Text { get; set; }
    }
}
