﻿// <auto-generated />
using System;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    [DbContext(typeof(IdentityContext))]
    [Migration("20240111021330_CreateCustomAttributeTable")]
    partial class CreateCustomAttributeTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Logitar.Identity.EntityFrameworkCore.Relational.Entities.ActorEntity", b =>
                {
                    b.Property<int>("ActorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ActorId"));

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PictureUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ActorId");

                    b.HasIndex("DisplayName");

                    b.HasIndex("EmailAddress");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("IsDeleted");

                    b.HasIndex("Type");

                    b.ToTable("Actors", (string)null);
                });

            modelBuilder.Entity("Logitar.Identity.EntityFrameworkCore.Relational.Entities.CustomAttributeEntity", b =>
                {
                    b.Property<int>("CustomAttributeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomAttributeId"));

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ValueShortened")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("CustomAttributeId");

                    b.HasIndex("Key");

                    b.HasIndex("ValueShortened");

                    b.HasIndex("EntityType", "EntityId");

                    b.HasIndex("EntityType", "EntityId", "Key")
                        .IsUnique();

                    b.ToTable("CustomAttributes", (string)null);
                });

            modelBuilder.Entity("Logitar.Identity.EntityFrameworkCore.Relational.Entities.SessionEntity", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SessionId"));

                    b.Property<string>("AggregateId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomAttributesSerialized")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CustomAttributes");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPersistent")
                        .HasColumnType("bit");

                    b.Property<string>("SecretHash")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("SignedOutBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("SignedOutOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("SessionId");

                    b.HasIndex("AggregateId")
                        .IsUnique();

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CreatedOn");

                    b.HasIndex("IsActive");

                    b.HasIndex("IsPersistent");

                    b.HasIndex("SignedOutBy");

                    b.HasIndex("SignedOutOn");

                    b.HasIndex("UpdatedBy");

                    b.HasIndex("UpdatedOn");

                    b.HasIndex("UserId");

                    b.HasIndex("Version");

                    b.ToTable("Sessions", (string)null);
                });

            modelBuilder.Entity("Logitar.Identity.EntityFrameworkCore.Relational.Entities.UserEntity", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("AddressCountry")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AddressFormatted")
                        .HasMaxLength(1279)
                        .HasColumnType("nvarchar(1279)");

                    b.Property<string>("AddressLocality")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AddressPostalCode")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AddressRegion")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AddressStreet")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AddressVerifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("AddressVerifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("AggregateId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("AuthenticatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomAttributesSerialized")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CustomAttributes");

                    b.Property<string>("DisabledBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("DisabledOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("EmailAddressNormalized")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("EmailVerifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("EmailVerifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FullName")
                        .HasMaxLength(767)
                        .HasColumnType("nvarchar(767)");

                    b.Property<string>("Gender")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("HasPassword")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAddressVerified")
                        .HasColumnType("bit");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPhoneVerified")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Locale")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Nickname")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PasswordChangedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("PasswordChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneCountryCode")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("PhoneE164Formatted")
                        .HasMaxLength(37)
                        .HasColumnType("nvarchar(37)");

                    b.Property<string>("PhoneExtension")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(17)
                        .HasColumnType("nvarchar(17)");

                    b.Property<string>("PhoneVerifiedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("PhoneVerifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Picture")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Profile")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("TenantId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TimeZone")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UniqueNameNormalized")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.Property<string>("Website")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.HasKey("UserId");

                    b.HasIndex("AddressCountry");

                    b.HasIndex("AddressFormatted");

                    b.HasIndex("AddressLocality");

                    b.HasIndex("AddressPostalCode");

                    b.HasIndex("AddressRegion");

                    b.HasIndex("AddressStreet");

                    b.HasIndex("AddressVerifiedBy");

                    b.HasIndex("AddressVerifiedOn");

                    b.HasIndex("AggregateId")
                        .IsUnique();

                    b.HasIndex("AuthenticatedOn");

                    b.HasIndex("Birthdate");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CreatedOn");

                    b.HasIndex("DisabledBy");

                    b.HasIndex("DisabledOn");

                    b.HasIndex("EmailAddress");

                    b.HasIndex("EmailVerifiedBy");

                    b.HasIndex("EmailVerifiedOn");

                    b.HasIndex("FirstName");

                    b.HasIndex("FullName");

                    b.HasIndex("Gender");

                    b.HasIndex("HasPassword");

                    b.HasIndex("IsAddressVerified");

                    b.HasIndex("IsConfirmed");

                    b.HasIndex("IsDisabled");

                    b.HasIndex("IsEmailVerified");

                    b.HasIndex("IsPhoneVerified");

                    b.HasIndex("LastName");

                    b.HasIndex("Locale");

                    b.HasIndex("MiddleName");

                    b.HasIndex("Nickname");

                    b.HasIndex("PasswordChangedBy");

                    b.HasIndex("PasswordChangedOn");

                    b.HasIndex("PhoneCountryCode");

                    b.HasIndex("PhoneE164Formatted");

                    b.HasIndex("PhoneExtension");

                    b.HasIndex("PhoneNumber");

                    b.HasIndex("PhoneVerifiedBy");

                    b.HasIndex("PhoneVerifiedOn");

                    b.HasIndex("TenantId");

                    b.HasIndex("TimeZone");

                    b.HasIndex("UniqueName");

                    b.HasIndex("UpdatedBy");

                    b.HasIndex("UpdatedOn");

                    b.HasIndex("Version");

                    b.HasIndex("TenantId", "EmailAddressNormalized");

                    b.HasIndex("TenantId", "UniqueNameNormalized")
                        .IsUnique()
                        .HasFilter("[TenantId] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Logitar.Identity.EntityFrameworkCore.Relational.Entities.SessionEntity", b =>
                {
                    b.HasOne("Logitar.Identity.EntityFrameworkCore.Relational.Entities.UserEntity", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Logitar.Identity.EntityFrameworkCore.Relational.Entities.UserEntity", b =>
                {
                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
