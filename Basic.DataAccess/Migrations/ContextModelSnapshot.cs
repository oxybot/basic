﻿// <auto-generated />
using System;
using Basic.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Basic.DataAccess.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Basic.Model.Agreement", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InternalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OwnerIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PrivateNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SignatureDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Identifier");

                    b.HasIndex("ClientIdentifier");

                    b.HasIndex("OwnerIdentifier");

                    b.ToTable("Agreement");
                });

            modelBuilder.Entity("Basic.Model.AgreementItem", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AgreementIdentifier")
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

                    b.HasIndex("AgreementIdentifier");

                    b.HasIndex("ProductIdentifier");

                    b.ToTable("AgreementItem");
                });

            modelBuilder.Entity("Basic.Model.Balance", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Allowed")
                        .HasColumnType("int");

                    b.Property<Guid>("CategoryIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Transfered")
                        .HasColumnType("int");

                    b.Property<Guid>("UserIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Identifier");

                    b.HasIndex("CategoryIdentifier");

                    b.HasIndex("UserIdentifier");

                    b.ToTable("Balance");
                });

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

            modelBuilder.Entity("Basic.Model.Event", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DurationFirstDay")
                        .HasColumnType("int");

                    b.Property<int>("DurationLastDay")
                        .HasColumnType("int");

                    b.Property<int>("DurationTotal")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Identifier");

                    b.HasIndex("CategoryIdentifier");

                    b.HasIndex("UserIdentifier");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("Basic.Model.EventCategory", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mapping")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.Property<bool>("RequireBalance")
                        .HasColumnType("bit");

                    b.HasKey("Identifier");

                    b.ToTable("EventCategory");
                });

            modelBuilder.Entity("Basic.Model.Invoice", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AgreementIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Identifier");

                    b.HasIndex("AgreementIdentifier");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("Basic.Model.Product", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DefaultDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DefaultQuantity")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("DefaultUnitPrice")
                        .HasColumnType("decimal(18,6)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Identifier");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Basic.Model.Role", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Identifier");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Identifier = new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"),
                            Code = "ClientRO"
                        },
                        new
                        {
                            Identifier = new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"),
                            Code = "Client"
                        });
                });

            modelBuilder.Entity("Basic.Model.User", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Identifier");

                    b.ToTable("User");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("RolesIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RolesIdentifier", "UsersIdentifier");

                    b.HasIndex("UsersIdentifier");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("Basic.Model.Agreement", b =>
                {
                    b.HasOne("Basic.Model.Client", "Client")
                        .WithMany("Agreements")
                        .HasForeignKey("ClientIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Basic.Model.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerIdentifier");

                    b.Navigation("Client");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Basic.Model.AgreementItem", b =>
                {
                    b.HasOne("Basic.Model.Agreement", "Agreement")
                        .WithMany("Items")
                        .HasForeignKey("AgreementIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Basic.Model.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductIdentifier");

                    b.Navigation("Agreement");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Basic.Model.Balance", b =>
                {
                    b.HasOne("Basic.Model.EventCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Basic.Model.User", "User")
                        .WithMany("Balances")
                        .HasForeignKey("UserIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Basic.Model.Client", b =>
                {
                    b.OwnsOne("Basic.Model.StreetAddress", "Address", b1 =>
                        {
                            b1.Property<Guid>("ClientIdentifier")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Line1")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Line2")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PostalCode")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ClientIdentifier");

                            b1.ToTable("Client");

                            b1.WithOwner()
                                .HasForeignKey("ClientIdentifier");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("Basic.Model.Event", b =>
                {
                    b.HasOne("Basic.Model.EventCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Basic.Model.User", "User")
                        .WithMany("Events")
                        .HasForeignKey("UserIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Basic.Model.Invoice", b =>
                {
                    b.HasOne("Basic.Model.Agreement", null)
                        .WithMany("Invoices")
                        .HasForeignKey("AgreementIdentifier");
                });

            modelBuilder.Entity("Basic.Model.User", b =>
                {
                    b.OwnsOne("Basic.Model.TypedFile", "Avatar", b1 =>
                        {
                            b1.Property<Guid>("UserIdentifier")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<byte[]>("Data")
                                .IsRequired()
                                .HasColumnType("varbinary(max)");

                            b1.Property<string>("MimeType")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserIdentifier");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserIdentifier");
                        });

                    b.Navigation("Avatar");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("Basic.Model.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Basic.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UsersIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Basic.Model.Agreement", b =>
                {
                    b.Navigation("Invoices");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Basic.Model.Client", b =>
                {
                    b.Navigation("Agreements");
                });

            modelBuilder.Entity("Basic.Model.User", b =>
                {
                    b.Navigation("Balances");

                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
