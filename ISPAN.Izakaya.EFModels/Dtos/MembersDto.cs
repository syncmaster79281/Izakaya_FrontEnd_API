using ISPAN.Izakaya.EFModels.Models;

namespace ISPAN.Izakaya.EFModels.Dtos
{
    public class MembersDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }


        public string? Email { get; set; }

        public string? Password { get; set; }


        public string? Phone { get; set; }

        public string? Salt { get; set; }

        public int? Point { get; set; }

        public DateTime? Birthday { get; set; }
        public string? VerificationCode { get; set; }
    }

    public static class MemberTransferExtensions
    {
        public static Member ToEntity(this MembersDto dto)
        {
            return new Member
            {
                Id = dto.Id.Value,
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Salt = dto.Salt,
                Points = dto.Point.Value,
                Birthday = dto.Birthday,
            };
        }

        public static MembersDto ToDto(this Member entity)
        {
            return new MembersDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Password = entity.Password,
                Salt = entity.Salt,
                Point = entity.Points,
                Birthday = entity.Birthday,
            };
        }
        public class UpdatePhoneDto
        {
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }
}
