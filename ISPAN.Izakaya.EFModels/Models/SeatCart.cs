﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ISPAN.Izakaya.EFModels.Models;

public partial class SeatCart
{
    public int Id { get; set; }

    public int SeatId { get; set; }

    public int ProductId { get; set; }

    public int CartStatusId { get; set; }

    public int UnitPrice { get; set; }

    public int Qty { get; set; }

    public string Notes { get; set; }

    public DateTime OrderTime { get; set; }

    public virtual CartStatus CartStatus { get; set; }

    public virtual Product Product { get; set; }

    public virtual Seat Seat { get; set; }
}