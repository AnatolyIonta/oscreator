﻿// <auto-generated />
using System;
using Ionta.OSC.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ionta.OSC.Storage.Migrations
{
    [DbContext(typeof(OscStorage))]
    [Migration("20230302064738_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ionta.OSC.Domain.AssemblyFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AssemblyName")
                        .HasColumnType("text");

                    b.Property<long?>("AssemblyPackageId")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Data")
                        .HasColumnType("bytea");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AssemblyPackageId");

                    b.ToTable("AssemblyFiles");
                });

            modelBuilder.Entity("Ionta.OSC.Domain.AssemblyPackage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Author")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AssemblyPackages");
                });

            modelBuilder.Entity("Ionta.OSC.Domain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Email = "Admin@OSC.ru",
                            Name = "Admin",
                            Password = "E6C83B282AEB2E022844595721CC00BBDA47CB24537C1779F9BB84F04039E1676E6BA8573E588DA1052510E3AA0A32A9E55879AE22B0C2D62136FC0A3E85F8BB"
                        });
                });

            modelBuilder.Entity("Ionta.OSC.Domain.AssemblyFile", b =>
                {
                    b.HasOne("Ionta.OSC.Domain.AssemblyPackage", null)
                        .WithMany("Assembly")
                        .HasForeignKey("AssemblyPackageId");
                });

            modelBuilder.Entity("Ionta.OSC.Domain.AssemblyPackage", b =>
                {
                    b.Navigation("Assembly");
                });
#pragma warning restore 612, 618
        }
    }
}
