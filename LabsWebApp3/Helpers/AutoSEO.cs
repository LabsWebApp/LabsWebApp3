using LabsWebApp3.Models.Domain.Entities;

namespace LabsWebApp3.Helpers
{ 
    public static class AutoSEO
    {
        public static void Set(EntityBase entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Title))
            {
                if (string.IsNullOrWhiteSpace(entity.MetaTitle))
                    entity.MetaTitle = entity.Title;
                if (string.IsNullOrWhiteSpace(entity.MetaKeywords))
                    entity.MetaKeywords = entity.Title;
            }

            if (!string.IsNullOrWhiteSpace(entity.Subtitle) && string.IsNullOrWhiteSpace(entity.MetaDescription))
                entity.MetaDescription = entity.Subtitle;
        }
    }
}
