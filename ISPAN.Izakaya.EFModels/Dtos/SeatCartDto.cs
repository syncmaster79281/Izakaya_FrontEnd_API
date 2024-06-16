using ISPAN.Izakaya.EFModels.Models;

namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class SeatCartDto
    {
        public int? Id { get; set; }

        public int? SeatId { get; set; }

        public int? ProductId { get; set; }

        public int? CartStatusId { get; set; }

        public int? UnitPrice { get; set; }

        public int? Qty { get; set; }

        public string? Notes { get; set; }
        public string? ProductName { get; set; }
        public string? CartStatus { get; set; }

        public DateTime? OrderTime { get; set; } = DateTime.Now;
    }
    public static class SeatCartTransferExtensions
    {
        public static SeatCart ToEntity(this SeatCartDto dto)
        {
            //欄位驗證
            //if (dto.Id < 0) throw new ArgumentException("Id 不可小於0");

            if (dto.SeatId <= 0) throw new ArgumentException("SeatID 不可小於0");

            if (dto.ProductId <= 0) throw new ArgumentException("ProductID 不可小於0");

            if (dto.CartStatusId <= 0) throw new ArgumentException("CartStatusID 不可小於0");

            if (dto.UnitPrice < 0) throw new ArgumentException("UnitPrice 不可小於0");

            if (dto.Qty < 0) throw new ArgumentException("Qty 不可小於0");

            if (dto.Notes.Length > 50) throw new ArgumentException("Notes 長度不可以超過50");

            if (dto.OrderTime > DateTime.Now) throw new ArgumentException("OrderTime 不可以是未來時間");

            return new SeatCart
            {
                //Id = dto.Id.Value,
                SeatId = dto.SeatId.Value,
                ProductId = dto.ProductId.Value,
                CartStatusId = dto.CartStatusId.Value,
                UnitPrice = dto.UnitPrice.Value,
                Qty = dto.Qty.Value,
                Notes = dto.Notes,
                OrderTime = dto.OrderTime.Value,
            };
        }
        public static SeatCartDto ToDto(this SeatCart entity)
        {
            return new SeatCartDto
            {
                Id = entity.Id,
                SeatId = entity.SeatId,
                ProductId = entity.ProductId,
                CartStatusId = entity.CartStatusId,
                UnitPrice = entity.UnitPrice,
                Qty = entity.Qty,
                Notes = entity.Notes,
                OrderTime = entity.OrderTime,
            };
        }
    }
}
