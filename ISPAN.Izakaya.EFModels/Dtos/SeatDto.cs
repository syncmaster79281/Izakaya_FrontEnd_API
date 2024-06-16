using ISPAN.Izakaya.EFModels.Models;

namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class SeatDto
    {
        public int Id { get; set; }

        public int BranchId { get; set; }

        public string Name { get; set; }

        public string QrcodeLink { get; set; }

        public int Capacity { get; set; }

        public bool Status { get; set; }

    }
    public static class SeatTransferExtensions
    {
        public static Seat ToEntity(this SeatDto dto)
        {
            if (dto.Id < 0) throw new ArgumentException("Id 不可小於0");

            if (dto.BranchId < 0) throw new ArgumentException("BranchID 不可小於0");

            if (string.IsNullOrEmpty(dto.Name)) throw new ArgumentException("Name 不可以空白");
            if (dto.Name.Length > 5) throw new ArgumentException("Name 長度不可以超過5");

            if (string.IsNullOrEmpty(dto.QrcodeLink)) throw new ArgumentException("QRCodeLink 不可以空白");
            if (dto.QrcodeLink.Length > 50) throw new ArgumentException("QRCodeLink 長度不可以超過50");

            if (dto.Capacity < 0) throw new ArgumentException("Capacity 不可小於0");

            return new Seat
            {
                Id = dto.Id,
                BranchId = dto.BranchId,
                Name = dto.Name,
                QrcodeLink = dto.QrcodeLink,
                Capacity = dto.Capacity,
                Status = dto.Status
            };
        }
        public static SeatDto ToDto(this Seat entity)
        {
            return new SeatDto
            {
                Id = entity.Id,
                BranchId = entity.BranchId,
                Name = entity.Name,
                QrcodeLink = entity.QrcodeLink,
                Capacity = entity.Capacity,
                Status = entity.Status
            };
        }
    }

}
