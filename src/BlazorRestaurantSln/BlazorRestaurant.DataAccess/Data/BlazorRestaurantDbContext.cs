﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BlazorRestaurant.DataAccess.Models;

#nullable disable

namespace BlazorRestaurant.DataAccess.Data
{
    public partial class BlazorRestaurantDbContext : DbContext
    {
        public BlazorRestaurantDbContext()
        {
        }

        public BlazorRestaurantDbContext(DbContextOptions<BlazorRestaurantDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Scaffolding:ConnectionString", "Data Source=(local);Initial Catalog=BlazorRestaurantDatabase;Integrated Security=true");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}