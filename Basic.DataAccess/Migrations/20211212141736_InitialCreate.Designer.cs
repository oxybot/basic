﻿// <auto-generated />
using System;
using Basic.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Basic.DataAccess.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20211212141736_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Basic.Model.Client", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Identifier");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("Basic.Model.ClientContract", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Identifier");

                    b.HasIndex("ClientIdentifier");

                    b.ToTable("ClientContract");
                });

            modelBuilder.Entity("Basic.Model.Invoice", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClientContractIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Identifier");

                    b.HasIndex("ClientContractIdentifier");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("Basic.Model.Product", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DefaultPrice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DefaultQuantity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DefaultUnitPrice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Identifier");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Basic.Model.Service", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ContractIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProductIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,6)");

                    b.HasKey("Identifier");

                    b.HasIndex("ContractIdentifier");

                    b.HasIndex("ProductIdentifier");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("Basic.Model.User", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Identifier");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Basic.Model.ClientContract", b =>
                {
                    b.HasOne("Basic.Model.Client", "Client")
                        .WithMany("Contracts")
                        .HasForeignKey("ClientIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Basic.Model.Invoice", b =>
                {
                    b.HasOne("Basic.Model.ClientContract", null)
                        .WithMany("Invoices")
                        .HasForeignKey("ClientContractIdentifier");
                });

            modelBuilder.Entity("Basic.Model.Service", b =>
                {
                    b.HasOne("Basic.Model.ClientContract", "Contract")
                        .WithMany("Services")
                        .HasForeignKey("ContractIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Basic.Model.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductIdentifier");

                    b.Navigation("Contract");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Basic.Model.Client", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("Basic.Model.ClientContract", b =>
                {
                    b.Navigation("Invoices");

                    b.Navigation("Services");
                });
#pragma warning restore 612, 618
        }
    }
}
