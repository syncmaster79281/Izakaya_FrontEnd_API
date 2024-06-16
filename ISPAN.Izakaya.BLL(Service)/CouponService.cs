using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using ISPAN.Izakaya.IBLL_IService_;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ISPAN.Izakaya.BLL_Service_
{
    public class CouponService : ICouponService
    {
        private readonly IzakayaContext db;
        //private readonly ICouponService _couponService;
        private readonly int _couponDiscountAmount; // 礼卷折扣金额
        private readonly TimeSpan _couponExpiration; // 礼卷有效期 
        public CouponService(IzakayaContext context)
        {
            db = context;
            //_couponService = new CouponService(context);
        }
        public CouponDto Get(int id)
        {
            if (id < 0) throw new ArgumentNullException("發生錯誤");
            var coupon = db.Coupons.Find(id);
            if (coupon == null) throw new ArgumentNullException("發生錯誤");

            return new CouponDto
            {
                Id = coupon.Id,
                BranchId = coupon.BranchId,
                Name = coupon.Name,
                ProductId = coupon.ProductId,
                TypeId = coupon.TypeId,
                Condition = coupon.Condition,
                DiscountMethod = coupon.DiscountMethod,
                StartTime = coupon.StartTime,
                EndTime = coupon.EndTime,
                IsUsed = coupon.IsUsed,
                Description = coupon.Description
            };
        }

        public IEnumerable<CouponDto> GetAll()
        {
            var coupons = db.Coupons.ToList();
            return coupons.Select(coupon => new CouponDto
            {
                Id = coupon.Id,
                BranchId = coupon.BranchId,
                Name = coupon.Name,
                ProductId = coupon.ProductId,
                TypeId = coupon.TypeId,
                Condition = coupon.Condition,
                DiscountMethod = coupon.DiscountMethod,
                StartTime = coupon.StartTime,
                EndTime = coupon.EndTime,
                IsUsed = coupon.IsUsed,
                Description = coupon.Description
            });
        }

        public IEnumerable<CouponDto> GetCoupon(int id)
        {
            var coupon = db.Coupons.Find(id);
            if (coupon == null) throw new ArgumentNullException("發生錯誤");
            return new List<CouponDto>
    {
        new CouponDto
        {
            Id = coupon.Id,
            BranchId = coupon.BranchId,
            Name = coupon.Name,
            ProductId = coupon.ProductId,
            TypeId = coupon.TypeId,
            Condition = coupon.Condition,
            DiscountMethod = coupon.DiscountMethod,
            StartTime = coupon.StartTime,
            EndTime = coupon.EndTime,
            IsUsed = coupon.IsUsed,
            Description = coupon.Description
        }
    };
        }

        public IEnumerable<CouponDto> CreateCoupon(CouponDto couponDto, RewardDto rewardDto)
        {
            var coupon = new Coupon
            {
                // Populate coupon properties from couponDto               
                BranchId = couponDto.BranchId.Value,
                Name = couponDto.Name,
                ProductId = couponDto.ProductId,
                TypeId = couponDto.TypeId,
                Condition = couponDto.Condition,
                DiscountMethod = couponDto.DiscountMethod,
                StartTime = couponDto.StartTime.Value,
                EndTime = couponDto.EndTime.Value,
                IsUsed = couponDto.IsUsed.Value,
                Description = couponDto.Description
            };

            var reward = new Reward
            {
                MemberId = rewardDto.MemberId,
                CouponId = rewardDto.Id,
                Qty = rewardDto.Qty
            };

            db.Coupons.Add(coupon);
            db.SaveChanges();

            return GetAll(); // Return all coupons including the newly created one
        }

        //public void DeleteRewards(int id)
        //{
        //    var rewards = db.Rewards.Where(r => r.CouponId == id);
        //    foreach (var reward in rewards)
        //    {
        //        db.Rewards.Remove(reward);
        //        db.SaveChanges();
        //    }

        //}

        public void DeleteExpire()
        {
            
            var now = DateTime.Now;
            var coupons = db.Coupons.Where(c => c.EndTime < DateTime.Now).Select(x=>x.Id).ToList();
            
            foreach(var coupon in coupons)
            {
                var rewards = db.Rewards.Where(r => r.CouponId == coupon).ToList();
                foreach (var reward in rewards)
                {
                    db.Rewards.Remove(reward);
                    db.SaveChanges();
                }
            }
        }
        public void IssueBirthdayCoupon(Member member)
        {   // 生成礼卷
            var reward = new Reward
            {
                MemberId = member.Id,
                CouponId = 11,
                Qty = 1, // 假设每个会员只发放一张优惠券

            };
            //var coupon = new Coupon
            //{
            //    BranchId = 1,
            //    Name = "貴賓生日優惠券",
            //    ProductId = null,
            //    TypeId = null,
            //    Condition = "全單8折",
            //    DiscountMethod = 0.80m,
            //    Description = "貴賓生日，全單8折",
            //    StartTime = DateTime.Now,
            //    EndTime = DateTime.Now.AddDays(60),
            //    IsUsed = false
            //};
            // 保存礼卷
            //db.Coupons.Add(coupon);
            db.Rewards.Add(reward);
            db.SaveChanges();

            // 发送礼卷通知给会员
            SendCouponNotification(member, reward);

        }
        public void Execute()
        {
            var today = DateTime.Today;
            var membersWithBirthdays = db.Members.ToList();
            var memberList = new List<Member>();
            foreach (var member in membersWithBirthdays)
            {
                var memberBirthday = member.Birthday;
                var memberMonth = memberBirthday?.Month;
                var memberDay = memberBirthday?.Day;
                if (memberMonth == today.Month && memberDay == today.Day)
                {
                    IssueBirthdayCoupon(member);
                }
            }


        }
        //public void DeleteExpire()
        //{
        //    var date = DateTime.Now;
        //    var expiredCouponIds = db.Coupons.Where(c => c.EndTime <= date).Select(x => x.Id).ToList();

        //    foreach (var id in expiredCouponIds)
        //    {
        //        DeleteCoupon(id);
        //    }
        //}
        private void SendCouponNotification(Member member, Reward reward)
        {
            // 实现发送通知的逻辑，例如发送电子邮件、短信等

        }
    }

       
        


        //public class BirthdayCouponJob
        //{
        //    private readonly IzakayaContext _db;
        //    private readonly ICouponService _couponService;

        //    public BirthdayCouponJob(IzakayaContext db, ICouponService couponService)
        //    {
        //        _db = db;
        //        _couponService = couponService;
        //    }
            
            

        //}
        
}
