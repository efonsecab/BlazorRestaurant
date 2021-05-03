﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BlazorRestaurant.DataAccess.Models
{
    [Table("OrderDetail", Schema = "orders")]
    [Index(nameof(OrderId), nameof(LineNumber), Name = "UI_OrderDetail_Line", IsUnique = true)]
    [Index(nameof(OrderId), nameof(ProductId), Name = "UI_OrderDetail_Product", IsUnique = true)]
    public partial class OrderDetail
    {
        [Key]
        public long OrderDetailId { get; set; }
        public long OrderId { get; set; }
        public int ProductId { get; set; }
        public int ProductQty { get; set; }
        public int LineNumber { get; set; }
        [Column(TypeName = "money")]
        public decimal LineTotal { get; set; }
        public DateTimeOffset RowCreationDateTime { get; set; }
        [Required]
        [StringLength(256)]
        public string RowCreationUser { get; set; }
        [Required]
        [StringLength(250)]
        public string SourceApplication { get; set; }
        [Required]
        [Column("OriginatorIPAddress")]
        [StringLength(100)]
        public string OriginatorIpaddress { get; set; }

        [ForeignKey(nameof(OrderId))]
        [InverseProperty("OrderDetail")]
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty("OrderDetail")]
        public virtual Product Product { get; set; }
    }
}