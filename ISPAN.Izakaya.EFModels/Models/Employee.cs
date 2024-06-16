﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ISPAN.Izakaya.EFModels.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int BranchId { get; set; }

    public string Name { get; set; }

    public string Department { get; set; }

    public int Salary { get; set; }

    public DateTime HireDate { get; set; }

    public string Gender { get; set; }

    public string EmployeePassword { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual Branch Branch { get; set; }

    public virtual ICollection<CustomerFeedback> CustomerFeedbacks { get; set; } = new List<CustomerFeedback>();
}