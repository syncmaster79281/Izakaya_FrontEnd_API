using ISPAN.Izakaya.EFModels.Models;

namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class SeatProductCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
    public static class ProductCategoryTransferExtensions
    {
        public static ProductCategory ToEntity(this SeatProductCategoryDto dto)
        {
            //欄位驗證
            //if (dto.Id < 0) throw new ArgumentException("Id 不可小於0");

            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentException("Name 不可以空白");
            if (dto.Name.Length > 20) throw new ArgumentException("Name 長度不可以超過20");


            return new ProductCategory
            {
                //Id = dto.Id,
                Name = dto.Name,
            };
        }
        public static SeatProductCategoryDto ToDto(this ProductCategory entity)
        {
            return new SeatProductCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
