using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AutoService.API.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<OrderPart> OrderParts { get; set; }

    public virtual DbSet<OrderService> OrderServices { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<RepairBay> RepairBays { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkOrder> WorkOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("appointments_pkey");

            entity.ToTable("appointments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DesiredDate).HasColumnName("desired_date");
            entity.Property(e => e.DesiredTime).HasColumnName("desired_time");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Car).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("appointments_car_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("appointments_client_id_fkey");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("appointments_service_id_fkey");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cars_pkey");

            entity.ToTable("cars");

            entity.HasIndex(e => e.Vin, "cars_vin_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .HasColumnName("brand");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(20)
                .HasColumnName("license_plate");
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .HasColumnName("model");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Vin)
                .HasMaxLength(17)
                .HasColumnName("vin");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.User).WithMany(p => p.Cars)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("cars_user_id_fkey");
        });

        modelBuilder.Entity<OrderPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_parts_pkey");

            entity.ToTable("order_parts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PartId).HasColumnName("part_id");
            entity.Property(e => e.PriceAtMoment)
                .HasPrecision(10, 2)
                .HasColumnName("price_at_moment");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.WorkOrderId).HasColumnName("work_order_id");

            entity.HasOne(d => d.Part).WithMany(p => p.OrderParts)
                .HasForeignKey(d => d.PartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_parts_part_id_fkey");

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.OrderParts)
                .HasForeignKey(d => d.WorkOrderId)
                .HasConstraintName("order_parts_work_order_id_fkey");
        });

        modelBuilder.Entity<OrderService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_services_pkey");

            entity.ToTable("order_services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PriceAtMoment)
                .HasPrecision(10, 2)
                .HasColumnName("price_at_moment");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.WorkOrderId).HasColumnName("work_order_id");

            entity.HasOne(d => d.Service).WithMany(p => p.OrderServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_services_service_id_fkey");

            entity.HasOne(d => d.WorkOrder).WithMany(p => p.OrderServices)
                .HasForeignKey(d => d.WorkOrderId)
                .HasConstraintName("order_services_work_order_id_fkey");
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("parts_pkey");

            entity.ToTable("parts");

            entity.HasIndex(e => e.Article, "parts_article_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Article)
                .HasMaxLength(100)
                .HasColumnName("article");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
        });

        modelBuilder.Entity<RepairBay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_bays_pkey");

            entity.ToTable("repair_bays");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("services_pkey");

            entity.ToTable("services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DurationMin)
                .HasDefaultValue(60)
                .HasColumnName("duration_min");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValueSql("'client'::character varying")
                .HasColumnName("role");
            entity.Property(e => e.Specialization)
                .HasMaxLength(100)
                .HasColumnName("specialization");
        });

        modelBuilder.Entity<WorkOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("work_orders_pkey");

            entity.ToTable("work_orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BayId).HasColumnName("bay_id");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.ClosedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("closed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.MechanicId).HasColumnName("mechanic_id");
            entity.Property(e => e.ScheduledEnd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("scheduled_end");
            entity.Property(e => e.ScheduledStart)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("scheduled_start");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'draft'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(12, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Bay).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.BayId)
                .HasConstraintName("work_orders_bay_id_fkey");

            entity.HasOne(d => d.Car).WithMany(p => p.WorkOrders)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("work_orders_car_id_fkey");

            entity.HasOne(d => d.Client).WithMany(p => p.WorkOrderClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("work_orders_client_id_fkey");

            entity.HasOne(d => d.Manager).WithMany(p => p.WorkOrderManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("work_orders_manager_id_fkey");

            entity.HasOne(d => d.Mechanic).WithMany(p => p.WorkOrderMechanics)
                .HasForeignKey(d => d.MechanicId)
                .HasConstraintName("work_orders_mechanic_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
