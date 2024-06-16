using ISPAN.Izakaya.EFModels.Models;

namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class OrderPaymentDto
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int CombinedOrderId { get; set; }

        public int PaymentMethodId { get; set; }

        public int PaymentStatusId { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentMethod { get; set; }

        public int TotalAmount { get; set; }

        public int Discount { get; set; }

        public int NetAmount { get; set; }

        public DateTime PaymentTime { get; set; } = DateTime.Now;
    }
    public static class OrderPaymentTransferExtensions
    {
        public static OrderPayment ToEntity(this OrderPaymentDto dto)
        {
            //欄位驗證
            //if (dto.Id < 0) throw new ArgumentException("Id 不可小於0");

            //if (dto.MemberId < 0) throw new ArgumentException("MemberID 不可小於0");

            if (dto.CombinedOrderId < 0) throw new ArgumentException("CombinedOrderID 不可小於0");

            //if (dto.PaymentMethodId < 0) throw new ArgumentException("PaymentMethodID 不可小於0");

            if (dto.PaymentStatusId < 0) throw new ArgumentException("PaymentStatusID 不可小於0");

            //if (dto.TotalAmount < 0) throw new ArgumentException("TotalAmount 不可小於0");

            //if (dto.Discount < 0) throw new ArgumentException("Discount 不可小於0");

            if (dto.NetAmount < 0) throw new ArgumentException("NetAmount 不可小於0");

            if (dto.PaymentTime > DateTime.Now) throw new ArgumentException("PaymentTime 不可以是未來時間");

            return new OrderPayment
            {
                //Id = dto.Id,
                MemberId = dto.MemberId,
                CombinedOrderId = dto.CombinedOrderId,
                PaymentMethodId = dto.PaymentMethodId,
                PaymentStatusId = dto.PaymentStatusId,
                TotalAmount = dto.TotalAmount,
                Discount = dto.Discount,
                NetAmount = dto.NetAmount,
                PaymentTime = dto.PaymentTime,
            };
        }
        public static OrderPaymentDto ToDto(this OrderPayment entity)
        {
            return new OrderPaymentDto
            {
                Id = entity.Id,
                MemberId = entity.MemberId,
                CombinedOrderId = entity.CombinedOrderId,
                PaymentMethodId = entity.PaymentMethodId,
                PaymentStatusId = entity.PaymentStatusId,
                TotalAmount = entity.TotalAmount,
                Discount = entity.Discount,
                NetAmount = entity.NetAmount,
                PaymentTime = entity.PaymentTime,
            };
        }
    }
}
