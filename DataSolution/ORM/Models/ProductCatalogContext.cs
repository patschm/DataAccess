﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ORM.Models
{
    public partial class ProductCatalogContext : DbContext
    {
         public ProductCatalogContext(DbContextOptions<ProductCatalogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Price> Prices { get; set; } = null!;
        public virtual DbSet<PriceHistory> PriceHistories { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductGroup> ProductGroups { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Reviewer> Reviewers { get; set; } = null!;
        public virtual DbSet<Specification> Specifications { get; set; } = null!;
        public virtual DbSet<SpecificationDefinition> SpecificationDefinitions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Core");
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brands", "Core");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Website).HasMaxLength(1024);
            });

            modelBuilder.Entity<Price>(entity =>
            {
                entity.ToTable("Prices", "Core");

                entity.HasIndex(e => e.ProductId, "IX_Prices_ProductId");

                entity.Property(e => e.ShopName).HasMaxLength(255);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Prices)
                    .HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<PriceHistory>(entity =>
            {
                entity.ToTable("PriceHistory", "Core");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ShopName).HasMaxLength(255);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                });

            modelBuilder.Entity<ProductGroup>(entity =>
            {              
                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews", "Core");

                entity.HasIndex(e => e.ProductId, "IX_Reviews_ProductId");

                entity.HasIndex(e => e.ReviewerId, "IX_Reviews_ReviewerId");

                entity.Property(e => e.Organization).HasMaxLength(512);

                entity.Property(e => e.ReviewUrl).HasMaxLength(1024);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId);

                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ReviewerId);
            });

            modelBuilder.Entity<Reviewer>(entity =>
            {
                entity.ToTable("Reviewers", "Core");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.UserName).HasMaxLength(255);
            });

            modelBuilder.Entity<Specification>(entity =>
            {
                entity.ToTable("Specifications", "Core");

                entity.HasIndex(e => e.ProductId, "IX_Specifications_ProductId");

                entity.Property(e => e.Key).HasMaxLength(255);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Specifications)
                    .HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<SpecificationDefinition>(entity =>
            {
                entity.ToTable("SpecificationDefinitions", "Core");

                entity.HasIndex(e => e.ProductGroupId, "IX_SpecificationDefinitions_ProductGroupId");

                entity.Property(e => e.Key).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Timestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.Unit).HasMaxLength(127);

                entity.HasOne(d => d.ProductGroup)
                    .WithMany(p => p.SpecificationDefinitions)
                    .HasForeignKey(d => d.ProductGroupId);
            });

        }

    }
}
