﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ISPAN.Izakaya.EFModels.Models;

public partial class PurchaseRecord
{
    public int Id { get; set; }

    public int BranchId { get; set; }

    public DateTime OrderDate { get; set; }

    public int TotalCost { get; set; }

    public virtual Branch Branch { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}