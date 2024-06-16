﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ISPAN.Izakaya.EFModels.Models;

public partial class Branch
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string Tel { get; set; }

    public int SeatingCapacity { get; set; }

    public TimeOnly OpeningTime { get; set; }

    public TimeOnly ClosingTime { get; set; }

    public DateOnly RestDay { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual ICollection<PurchaseRecord> PurchaseRecords { get; set; } = new List<PurchaseRecord>();

    public virtual ICollection<ReservationStatus> ReservationStatuses { get; set; } = new List<ReservationStatus>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}