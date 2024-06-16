using ISPAN.Izakaya.EFModels.Models;

namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class CouponDto
    {
        public int? Id { get; set; }
        public int? BranchId { get; set; }
        public string? Name { get; set; }

        public int? ProductId { get; set; }
        public int? TypeId { get; set; }
        public string? Condition { get; set; }
        public decimal? DiscountMethod { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? IsUsed { get; set; }
        public string? Description { get; set; }
    }
    public static class CouponTransferExtensions
    {
        public static Coupon ToEntity(this CouponDto dto)
        {
            //欄位驗證
            //if (dto.Id < 0) throw new ArgumentException("Id 不可小於0");

            if (dto.Id < 0) throw new ArgumentException("Id 不可小於0");

            if (dto.BranchId < 0) throw new ArgumentException("BranchId 不可小於0");

            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentException("Name 不可以空白");

            if (dto.ProductId < 0) throw new ArgumentException("ProductID 不可小於0");

            if (dto.TypeId < 0) throw new ArgumentException("TypedId 不可小於0");

            if (dto.Condition.Length > 50) throw new ArgumentException("Condition 長度不可以超過50");

            if (dto.DiscountMethod > 0) throw new ArgumentException("DiscountMethod 不可大於0");

            if (dto.StartTime < DateTime.Now) throw new ArgumentException("StartTime 不可以是過去時間");

            if (dto.EndTime < DateTime.Now) throw new ArgumentException("EndDate 不可以是過去時間");

            if (dto.IsUsed == null) throw new ArgumentException("IsUsed 不可以是空值");

            if (dto.Description.Length > 50) throw new ArgumentException("Description 長度不可以超過50");

            if (dto.Description.Length > 50) throw new ArgumentException("Description 長度不可以超過50");

            return new Coupon
            {
                Id = dto.Id.Value,
                BranchId = dto.BranchId.Value,
                Name = dto.Name,
                ProductId = dto.ProductId.Value,
                TypeId = dto.TypeId.Value,
                Condition = dto.Condition,
                DiscountMethod = dto.DiscountMethod.Value,
                StartTime = dto.StartTime.Value,
                EndTime = dto.EndTime.Value,
                IsUsed = dto.IsUsed.Value,
                Description = dto.Description,
            };
        }
        public static CouponDto ToDto(this Coupon entity)
        {
            return new CouponDto
            {
                Id = entity.Id,
                BranchId = entity.BranchId,
                Name = entity.Name,
                ProductId = entity.ProductId,
                TypeId = entity.TypeId,
                Condition = entity.Condition,
                DiscountMethod = entity.DiscountMethod,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                IsUsed = entity.IsUsed,
                Description = entity.Description,

            };
        }
    }


}
