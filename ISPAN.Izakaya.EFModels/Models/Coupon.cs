﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ISPAN.Izakaya.EFModels.Models;

public partial class Coupon
{
    public int Id { get; set; }

    public int BranchId { get; set; }

    public string Name { get; set; }

    public int? ProductId { get; set; }

    public int? TypeId { get; set; }

    public string Condition { get; set; }

    public decimal? DiscountMethod { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsUsed { get; set; }

    public string Description { get; set; }

    public virtual Branch Branch { get; set; }

    public virtual Product Product { get; set; }

    public virtual ICollection<Reward> Rewards { get; set; } = new List<Reward>();

    public virtual CouponType Type { get; set; }
}