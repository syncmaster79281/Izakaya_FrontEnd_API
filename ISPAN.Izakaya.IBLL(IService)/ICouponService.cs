using ISPAN.Izakaya.EFModels.Dtos;
using ISPAN.Izakaya.EFModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISPAN.Izakaya.IBLL_IService_
{
    public interface ICouponService
    {
        IEnumerable<CouponDto> GetAll();
        IEnumerable<CouponDto> GetCoupon(int id);
        CouponDto Get(int id);
        IEnumerable<CouponDto> CreateCoupon(CouponDto couponDto, RewardDto rewardDto);
        
        //void DeleteRewards(int id);

        void IssueBirthdayCoupon(Member member);

        void Execute();
        //IEnumerable<int> DeleteExpire();
        void DeleteExpire();

    }
}
