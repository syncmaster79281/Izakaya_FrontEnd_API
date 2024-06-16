using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Extensions;
using ISPAN.Izakaya.EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISPAN.Izakaya.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IzakayaContext db;
        private readonly Random random = new Random();

        public ReservationsController(IzakayaContext context)
        {
            db = context;
        }

        // 取得符合資料的比數
        // GET: api/<ReservationsController/GetSeatIsused>
        [HttpGet("GetSeatIsused")]
        public int GetSeatIsused([FromQuery] SearchReservationDto dto)
        {


            var reservations = db.Reservations
                .AsNoTracking()
                .Include(r => r.Seat)
                .Include(r => r.Branch)
                .IsMemberId(dto.MemberId)
                .IsBranchId(dto.BranchId)
                .InStatus(dto.Status)
                .TimeBetween(dto.ReservationTime)
                .Select(r => new ReservationDto
                {
                    BranchId = r.BranchId,
                    BranchName = r.Branch.Name,
                    SeatId = r.SeatId,
                    SeatName = r.Seat.Name,
                    MemberId = r.MemberId,
                    ReservationTime = r.ReservationTime,
                    Tel = r.Tel,
                    Status = r.Status,
                    Qty = r.Qty,
                    Name = r.Name,
                    FillUp = r.FillUp,
                    Email = r.Email,
                    Adult = r.Adult,
                    Child = r.Child,
                    Message = r.Message,
                    Random = r.Random,
                })
                .ToList().Count();
            return reservations;
        }
        // 將符合資料筆數的數據全數顯現
        [HttpGet("GetAll")]
        public IEnumerable<ReservationDto> GetAll([FromQuery] SearchReservationDto dto)
        {
            var reservations = db.Reservations
               .AsNoTracking()
               .Include(r => r.Seat)
               .Include(r => r.Branch)
               .IsMemberId(dto.MemberId)
               .IsBranchId(dto.BranchId)
               .InStatus(dto.Status)
               .TimeBetween(dto.ReservationTime)
               .Select(r => new ReservationDto
               {
                   BranchId = r.BranchId,
                   BranchName = r.Branch.Name,
                   SeatId = r.SeatId,
                   SeatName = r.Seat.Name,
                   MemberId = r.MemberId,
                   ReservationTime = r.ReservationTime,
                   Tel = r.Tel,
                   Status = r.Status,
                   Qty = r.Qty,
                   Name = r.Name,
                   FillUp = r.FillUp,
                   Email = r.Email,
                   Adult = r.Adult,
                   Child = r.Child,
                   Message = r.Message,
               })
               .ToList();
            return reservations;
        }



        // GET api/<ReservationsController>
        [HttpGet]
        public string Get()
        {
            return "value";
        }

        // GET api/<ReservationsController>/5
        [HttpGet("{random}")]
        public ActionResult<Object> Get(string random)
        {
            try
            {
                var reservation = db.Reservations.AsNoTracking().FirstOrDefault(r => r.Random == random);
                if (reservation == null)
                {
                    return NotFound("訂位資料未找到");
                }

                return reservation;
            }
            catch (Exception e)
            {
                return StatusCode(500, "更新訂位資料時發生錯誤：" + e.Message);
            }
        }




        // 接收資訊並創建資料庫且同時寄信
        // POST api/<ReservationsController>
        [HttpPost]
        public string Post([FromBody] ReservationDto dto)
        {
            try
            {


                var count = db.Reservations
                       .AsNoTracking()
                       .Include(r => r.Seat)
                       .Include(r => r.Branch)
                       .IsBranchId(dto.BranchId)
                       .TimeBetween(dto.ReservationTime)
                       .Count();

                if (count > 0)
                {
                    throw new Exception("該時段訂位已額滿，請重新選擇時段");
                }



                // 生成 隨機數
                var random = Guid.NewGuid().ToString().Replace("-", "");

                Reservation reservations = new Reservation
                {
                    BranchId = dto.BranchId,
                    SeatId = dto.SeatId,
                    MemberId = dto.MemberId,
                    OrderTime = dto.ReservationTime,
                    ReservationTime = dto.ReservationTime,
                    Tel = dto.Tel,
                    Status = dto.Status,
                    Qty = dto.Qty,
                    Name = dto.Name,
                    FillUp = dto.FillUp,
                    Email = dto.Email,
                    Adult = dto.Adult,
                    Child = dto.Child,
                    Message = dto.Message,
                    Random = random,
                };



                //dto.Random = randomNumber;
                db.Reservations.Add(reservations);
                db.SaveChanges();
                int id = reservations.Id;
                // 寄信功能
                string BranchName = reservations.BranchId == 1 ? "台北店" : "台中店";
                var email = new EmailHelper();
                var info = new ReservationInfo()
                {
                    Phone = reservations.Tel,
                    ReservationTime = reservations.ReservationTime,
                    Adult = reservations.Adult,
                    Child = reservations.Child
                };
                // 寄信的按鈕導航到指定頁面(更改訂位)
                bool isSend = email.SendReservationInformationEmail($"https://localhost:8080/ChangeReservation?random={random}", reservations.Name, reservations.Email, BranchName, reservations.Message, info);
                //bool isSend = email.SendReservationInformationEmail($"https://localhost:8080/ChangeReservation?branchId={dto.BranchId}&dateTime={dto.ReservationTime.ToString("yyyy/MM/dd+HH:mm")}&name={reservations.Name}&email={reservations.Email}&phone={reservations.Tel}&adult={reservations.Adult}&child={reservations.Child}&message={reservations.Message}&id={id}", reservations.Name, reservations.Email, BranchName, reservations.Message, info);

                if (isSend) return "恭喜你! 訂位成功!  即將回到首頁";
                else return "抱歉!!訂位失敗!";

            }
            catch (Exception e)
            {
                return $"{e.Message}";
            }
        }


        // 從信箱連結更改訂位資訊 更改資訊並更新資料庫 同時寄信
        // PUT api/<ReservationsController>/5
        [HttpPut]
        public ActionResult<object> Put([FromForm] ReservationPutDto dto)
        {
            try
            {
                var reservationToUpdate = db.Reservations.FirstOrDefault(r => r.Id == dto.Id && r.Random == dto.Random);
                if (reservationToUpdate == null)
                {
                    return NotFound("訂位資料未找到");
                }
                // 生成 隨機數
                var random = Guid.NewGuid().ToString().Replace("-", "");

                // 更新訂位資料的属性
                reservationToUpdate.Qty = dto.Child + dto.Adult;
                reservationToUpdate.Adult = dto.Adult;
                reservationToUpdate.Child = dto.Child;
                reservationToUpdate.Message = dto.Message;
                reservationToUpdate.Random = random;

                db.SaveChanges();

                // 寄信功能
                string BranchName = reservationToUpdate.BranchId == 1 ? "台北店" : "台中店";
                var email = new EmailHelper();
                var info = new ReservationInfo()
                {
                    Phone = reservationToUpdate.Tel,
                    ReservationTime = reservationToUpdate.ReservationTime,
                    Adult = reservationToUpdate.Adult,
                    Child = reservationToUpdate.Child
                };
                // 寄信的按鈕導航到指定頁面(更改訂位)
                bool isSend = email.SendChangeEmail($"https://localhost:8080/ChangeReservation?random={random}", reservationToUpdate.Name, reservationToUpdate.Email, BranchName, reservationToUpdate.Message, info);

                //if (isSend) return Ok("訂位資料已成功更新，並已發送郵件通知！  即將回到首頁");
                //else return Ok("訂位資料已成功更新，但发送邮件通知失败。");

                return new
                {
                    Success = true,
                    Sended = isSend,
                    Content = isSend ? "訂位資料已成功更新，並已發送郵件通知！  即將回到首頁" : "訂位資料已成功更新，但发送邮件通知失败。",
                };
            }
            catch (Exception e)
            {
                return StatusCode(500, "更新訂位資料時發生錯誤：" + e.Message);
            }
        }

        // DELETE api/<ReservationsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
