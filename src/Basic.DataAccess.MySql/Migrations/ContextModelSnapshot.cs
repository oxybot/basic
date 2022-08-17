﻿// <auto-generated />
using System;
using Basic.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Basic.DataAccess.MySql.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Basic.Model.Agreement", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ClientIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<string>("InternalCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("OwnerIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<string>("PrivateNotes")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("SignatureDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Identifier");

                    b.HasIndex("ClientIdentifier");

                    b.HasIndex("OwnerIdentifier");

                    b.ToTable("Agreement");
                });

            modelBuilder.Entity("Basic.Model.AgreementAttachment", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ParentIdentifier")
                        .HasColumnType("char(36)");

                    b.HasKey("Identifier");

                    b.HasIndex("ParentIdentifier");

                    b.ToTable("AgreementAttachment");
                });

            modelBuilder.Entity("Basic.Model.AgreementItem", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AgreementIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("ProductIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,6)");

                    b.HasKey("Identifier");

                    b.HasIndex("AgreementIdentifier");

                    b.HasIndex("ProductIdentifier");

                    b.ToTable("AgreementItem");
                });

            modelBuilder.Entity("Basic.Model.AgreementStatus", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AgreementIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("StatusIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UpdatedByIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Identifier");

                    b.HasIndex("AgreementIdentifier");

                    b.HasIndex("StatusIdentifier");

                    b.HasIndex("UpdatedByIdentifier");

                    b.ToTable("AgreementStatus");
                });

            modelBuilder.Entity("Basic.Model.Balance", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Allowed")
                        .HasColumnType("int");

                    b.Property<Guid>("CategoryIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<int>("Transfered")
                        .HasColumnType("int");

                    b.Property<Guid>("UserIdentifier")
                        .HasColumnType("char(36)");

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
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Identifier");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("Basic.Model.ClientAttachment", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ParentIdentifier")
                        .HasColumnType("char(36)");

                    b.HasKey("Identifier");

                    b.HasIndex("ParentIdentifier");

                    b.ToTable("ClientAttachment");
                });

            modelBuilder.Entity("Basic.Model.Event", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CategoryIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext");

                    b.Property<decimal>("DurationFirstDay")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("DurationLastDay")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("DurationTotal")
                        .HasColumnType("decimal(18,6)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<Guid>("UserIdentifier")
                        .HasColumnType("char(36)");

                    b.HasKey("Identifier");

                    b.HasIndex("CategoryIdentifier");

                    b.HasIndex("UserIdentifier");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("Basic.Model.EventAttachment", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ParentIdentifier")
                        .HasColumnType("char(36)");

                    b.HasKey("Identifier");

                    b.HasIndex("ParentIdentifier");

                    b.ToTable("EventAttachment");
                });

            modelBuilder.Entity("Basic.Model.EventCategory", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ColorClass")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Mapping")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.Property<bool>("RequireBalance")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Identifier");

                    b.ToTable("EventCategory");
                });

            modelBuilder.Entity("Basic.Model.EventStatus", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("EventIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("StatusIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UpdatedByIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Identifier");

                    b.HasIndex("EventIdentifier");

                    b.HasIndex("StatusIdentifier");

                    b.HasIndex("UpdatedByIdentifier");

                    b.ToTable("EventStatus");
                });

            modelBuilder.Entity("Basic.Model.GlobalDayOff", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.HasKey("Identifier");

                    b.HasIndex("Date")
                        .IsUnique();

                    b.ToTable("GlobalDayOff");
                });

            modelBuilder.Entity("Basic.Model.Invoice", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AgreementIdentifier")
                        .HasColumnType("char(36)");

                    b.HasKey("Identifier");

                    b.HasIndex("AgreementIdentifier");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("Basic.Model.Product", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DefaultDescription")
                        .HasColumnType("longtext");

                    b.Property<decimal>("DefaultQuantity")
                        .HasColumnType("decimal(18,6)");

                    b.Property<decimal>("DefaultUnitPrice")
                        .HasColumnType("decimal(18,6)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Identifier");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Basic.Model.Role", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Identifier");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Identifier = new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"),
                            Code = "client-ro"
                        },
                        new
                        {
                            Identifier = new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"),
                            Code = "client"
                        },
                        new
                        {
                            Identifier = new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"),
                            Code = "time-ro"
                        },
                        new
                        {
                            Identifier = new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"),
                            Code = "time"
                        },
                        new
                        {
                            Identifier = new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"),
                            Code = "user"
                        },
                        new
                        {
                            Identifier = new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"),
                            Code = "beta"
                        });
                });

            modelBuilder.Entity("Basic.Model.Schedule", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("ActiveFrom")
                        .HasColumnType("date");

                    b.Property<DateTime?>("ActiveTo")
                        .HasColumnType("date");

                    b.Property<Guid>("UserIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<string>("WorkingSchedule")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Identifier");

                    b.HasIndex("UserIdentifier");

                    b.ToTable("Schedule");
                });

            modelBuilder.Entity("Basic.Model.Status", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Identifier");

                    b.ToTable("Status");

                    b.HasData(
                        new
                        {
                            Identifier = new Guid("52bc6354-d8ef-44e2-87ca-c64deeeb22e8"),
                            Description = "The associated event has been created and is waiting for approval",
                            DisplayName = "Requested",
                            IsActive = true
                        },
                        new
                        {
                            Identifier = new Guid("4151c014-ddde-43e4-aa7e-b98a339bbe74"),
                            Description = "The associated event has been approved",
                            DisplayName = "Approved",
                            IsActive = true
                        },
                        new
                        {
                            Identifier = new Guid("e7f8dcc7-57d5-4e74-ac38-1fbd5153996c"),
                            Description = "The associated event has been rejected",
                            DisplayName = "Rejected",
                            IsActive = false
                        },
                        new
                        {
                            Identifier = new Guid("fdac7cc3-3fe0-4e59-ab16-aeaec008f940"),
                            Description = "The associated event has been canceled",
                            DisplayName = "Canceled",
                            IsActive = false
                        });
                });

            modelBuilder.Entity("Basic.Model.Token", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("UserIdentifier")
                        .HasColumnType("char(36)");

                    b.HasKey("Identifier");

                    b.HasIndex("UserIdentifier");

                    b.ToTable("Token");
                });

            modelBuilder.Entity("Basic.Model.User", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("ExternalIdentifier")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Salt")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Identifier");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Identifier = new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5"),
                            DisplayName = "John Doe",
                            IsActive = true,
                            Password = "QBG6AuURBMZ4wxp2pERIWzjzhl5QTYnDoKgLQ5uxojc=",
                            Salt = "demo",
                            Title = "User Group Evangelist",
                            Username = "demo"
                        });
                });

            modelBuilder.Entity("Basic.Model.UserAttachment", b =>
                {
                    b.Property<Guid>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("ParentIdentifier")
                        .HasColumnType("char(36)");

                    b.HasKey("Identifier");

                    b.HasIndex("ParentIdentifier");

                    b.ToTable("UserAttachment");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("RolesIdentifier")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UsersIdentifier")
                        .HasColumnType("char(36)");

                    b.HasKey("RolesIdentifier", "UsersIdentifier");

                    b.HasIndex("UsersIdentifier");

                    b.ToTable("RoleUser");

                    b.HasData(
                        new
                        {
                            RolesIdentifier = new Guid("8087c59d-7db0-4c40-aa35-742f6e11816f"),
                            UsersIdentifier = new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5")
                        },
                        new
                        {
                            RolesIdentifier = new Guid("7a42dca4-c92c-408b-af26-6ac2db418312"),
                            UsersIdentifier = new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5")
                        },
                        new
                        {
                            RolesIdentifier = new Guid("964afeec-f83b-4c98-b4a5-121d2a53985d"),
                            UsersIdentifier = new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5")
                        },
                        new
                        {
                            RolesIdentifier = new Guid("7e2d06c8-7f25-4ff4-8c21-1d0f365970a5"),
                            UsersIdentifier = new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5")
                        },
                        new
                        {
                            RolesIdentifier = new Guid("65726f0e-d856-47e1-8493-ced5ee7cba70"),
                            UsersIdentifier = new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5")
                        },
                        new
                        {
                            RolesIdentifier = new Guid("a0b62b59-6440-4031-ac22-0a74be98a409"),
                            UsersIdentifier = new Guid("d7467fee-1aec-4e72-9a29-72969c429ed5")
                        });
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

            modelBuilder.Entity("Basic.Model.AgreementAttachment", b =>
                {
                    b.HasOne("Basic.Model.Agreement", "Parent")
                        .WithMany("Attachments")
                        .HasForeignKey("ParentIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Basic.Model.TypedFile", "AttachmentContent", b1 =>
                        {
                            b1.Property<Guid>("AgreementAttachmentIdentifier")
                                .HasColumnType("char(36)");

                            b1.Property<byte[]>("Data")
                                .IsRequired()
                                .HasColumnType("longblob");

                            b1.Property<string>("MimeType")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("AgreementAttachmentIdentifier");

                            b1.ToTable("AgreementAttachment");

                            b1.WithOwner()
                                .HasForeignKey("AgreementAttachmentIdentifier");
                        });

                    b.Navigation("AttachmentContent")
                        .IsRequired();

                    b.Navigation("Parent");
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

            modelBuilder.Entity("Basic.Model.AgreementStatus", b =>
                {
                    b.HasOne("Basic.Model.Agreement", null)
                        .WithMany("Statuses")
                        .HasForeignKey("AgreementIdentifier");

                    b.HasOne("Basic.Model.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Basic.Model.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedByIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("UpdatedBy");
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
                                .HasColumnType("char(36)");

                            b1.Property<string>("City")
                                .HasColumnType("longtext");

                            b1.Property<string>("Country")
                                .HasColumnType("longtext");

                            b1.Property<string>("Line1")
                                .HasColumnType("longtext");

                            b1.Property<string>("Line2")
                                .HasColumnType("longtext");

                            b1.Property<string>("PostalCode")
                                .HasColumnType("longtext");

                            b1.HasKey("ClientIdentifier");

                            b1.ToTable("Client");

                            b1.WithOwner()
                                .HasForeignKey("ClientIdentifier");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("Basic.Model.ClientAttachment", b =>
                {
                    b.HasOne("Basic.Model.Client", "Parent")
                        .WithMany("Attachments")
                        .HasForeignKey("ParentIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Basic.Model.TypedFile", "AttachmentContent", b1 =>
                        {
                            b1.Property<Guid>("ClientAttachmentIdentifier")
                                .HasColumnType("char(36)");

                            b1.Property<byte[]>("Data")
                                .IsRequired()
                                .HasColumnType("longblob");

                            b1.Property<string>("MimeType")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("ClientAttachmentIdentifier");

                            b1.ToTable("ClientAttachment");

                            b1.WithOwner()
                                .HasForeignKey("ClientAttachmentIdentifier");
                        });

                    b.Navigation("AttachmentContent")
                        .IsRequired();

                    b.Navigation("Parent");
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

            modelBuilder.Entity("Basic.Model.EventAttachment", b =>
                {
                    b.HasOne("Basic.Model.Event", "Parent")
                        .WithMany("Attachments")
                        .HasForeignKey("ParentIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Basic.Model.TypedFile", "AttachmentContent", b1 =>
                        {
                            b1.Property<Guid>("EventAttachmentIdentifier")
                                .HasColumnType("char(36)");

                            b1.Property<byte[]>("Data")
                                .IsRequired()
                                .HasColumnType("longblob");

                            b1.Property<string>("MimeType")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("EventAttachmentIdentifier");

                            b1.ToTable("EventAttachment");

                            b1.WithOwner()
                                .HasForeignKey("EventAttachmentIdentifier");
                        });

                    b.Navigation("AttachmentContent")
                        .IsRequired();

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Basic.Model.EventStatus", b =>
                {
                    b.HasOne("Basic.Model.Event", null)
                        .WithMany("Statuses")
                        .HasForeignKey("EventIdentifier");

                    b.HasOne("Basic.Model.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Basic.Model.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedByIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("Basic.Model.Invoice", b =>
                {
                    b.HasOne("Basic.Model.Agreement", null)
                        .WithMany("Invoices")
                        .HasForeignKey("AgreementIdentifier");
                });

            modelBuilder.Entity("Basic.Model.Schedule", b =>
                {
                    b.HasOne("Basic.Model.User", "User")
                        .WithMany("Schedules")
                        .HasForeignKey("UserIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Basic.Model.Token", b =>
                {
                    b.HasOne("Basic.Model.User", null)
                        .WithMany("Tokens")
                        .HasForeignKey("UserIdentifier");
                });

            modelBuilder.Entity("Basic.Model.User", b =>
                {
                    b.OwnsOne("Basic.Model.TypedFile", "Avatar", b1 =>
                        {
                            b1.Property<Guid>("UserIdentifier")
                                .HasColumnType("char(36)");

                            b1.Property<byte[]>("Data")
                                .IsRequired()
                                .HasColumnType("longblob");

                            b1.Property<string>("MimeType")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("UserIdentifier");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserIdentifier");
                        });

                    b.Navigation("Avatar");
                });

            modelBuilder.Entity("Basic.Model.UserAttachment", b =>
                {
                    b.HasOne("Basic.Model.User", "Parent")
                        .WithMany("Attachments")
                        .HasForeignKey("ParentIdentifier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Basic.Model.TypedFile", "AttachmentContent", b1 =>
                        {
                            b1.Property<Guid>("UserAttachmentIdentifier")
                                .HasColumnType("char(36)");

                            b1.Property<byte[]>("Data")
                                .IsRequired()
                                .HasColumnType("longblob");

                            b1.Property<string>("MimeType")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("UserAttachmentIdentifier");

                            b1.ToTable("UserAttachment");

                            b1.WithOwner()
                                .HasForeignKey("UserAttachmentIdentifier");
                        });

                    b.Navigation("AttachmentContent")
                        .IsRequired();

                    b.Navigation("Parent");
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
                    b.Navigation("Attachments");

                    b.Navigation("Invoices");

                    b.Navigation("Items");

                    b.Navigation("Statuses");
                });

            modelBuilder.Entity("Basic.Model.Client", b =>
                {
                    b.Navigation("Agreements");

                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("Basic.Model.Event", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Statuses");
                });

            modelBuilder.Entity("Basic.Model.User", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Balances");

                    b.Navigation("Events");

                    b.Navigation("Schedules");

                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
