using ISPAN.Izakaya.EFModels.Models;

namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }

        public string Contents { get; set; }
        public DateOnly PublishDate { get; set; }
        public DateTime? HideTime { get; set; }
        public bool Status { get; set; }
        public string? ImageUrl { get; set; }
    }

    public static class ArticleTransferExtensions
    {
        public static Article ToEntity(this ArticleDto dto)
        {
            if (string.IsNullOrEmpty(dto.Title)) throw new ArgumentException("Title 不可以空白");
            if (dto.Title.Length > 50) throw new ArgumentException("Title 長度不可以超過50");

            if (string.IsNullOrEmpty(dto.Contents)) throw new ArgumentException("Contents 不可以空白");

            if (dto.PublishDate == null) throw new ArgumentException("PublishDate 不可以空白");

            if (dto.HideTime < DateTime.Now) throw new ArgumentException("HideTime 不可以小於現在時間");

            return new Article
            {
                EmployeeId = dto.EmployeeId,
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Contents = dto.Contents,
                PublishDate = dto.PublishDate,
                HideTime = dto.HideTime,
                Status = dto.Status,
                ImageUrl = dto.ImageUrl
            };
        }

        public static ArticleDto ToDto(this Article entity)
        {
            return new ArticleDto
            {
                Id = entity.Id,
                EmployeeId = entity.EmployeeId,
                CategoryId = entity.CategoryId,
                Title = entity.Title,
                Contents = entity.Contents,
                PublishDate = entity.PublishDate,
                HideTime = entity.HideTime,
                Status = entity.Status,
                ImageUrl = entity.ImageUrl
            };
        }
    }
}
