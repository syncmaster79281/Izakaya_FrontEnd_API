﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ISPAN.Izakaya.EFModels.Models;

public partial class Member
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public int Points { get; set; }

    public string AuthenticationCode { get; set; }

    public DateTime? Birthday { get; set; }

    public string Salt { get; set; }

    public DateTime? AuthenticationCodeGeneratedAt { get; set; }

    public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Reward> Rewards { get; set; } = new List<Reward>();
}