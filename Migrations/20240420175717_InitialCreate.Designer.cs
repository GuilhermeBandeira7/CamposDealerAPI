﻿// <auto-generated />
using System;
using CamposDealerApp.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CamposDealerApp.Migrations
{
    [DbContext(typeof(CampDContext))]
    [Migration("20240420175717_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CamposDealerApp.Model.Cliente", b =>
                {
                    b.Property<long>("IdCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IdCliente"));

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("NmCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdCliente");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("CamposDealerApp.Model.Produto", b =>
                {
                    b.Property<long>("IdProduto")
                        .HasColumnType("bigint");

                    b.Property<string>("DscProduto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("VlrUnitario")
                        .HasColumnType("real");

                    b.HasKey("IdProduto");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("CamposDealerApp.Model.Venda", b =>
                {
                    b.Property<long>("IdVenda")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("IdVenda"));

                    b.Property<DateTime>("DthVenda")
                        .HasColumnType("datetime2");

                    b.Property<long>("IdCliente")
                        .HasColumnType("bigint");

                    b.Property<int>("QtdVenda")
                        .HasColumnType("int");

                    b.Property<float>("VlrTotalVenda")
                        .HasColumnType("real");

                    b.Property<int>("VlrUnitarioVenda")
                        .HasColumnType("int");

                    b.HasKey("IdVenda");

                    b.HasIndex("IdCliente");

                    b.ToTable("Vendas");
                });

            modelBuilder.Entity("CamposDealerApp.Model.Produto", b =>
                {
                    b.HasOne("CamposDealerApp.Model.Venda", null)
                        .WithMany("Produtos")
                        .HasForeignKey("IdProduto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CamposDealerApp.Model.Venda", b =>
                {
                    b.HasOne("CamposDealerApp.Model.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("IdCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("CamposDealerApp.Model.Venda", b =>
                {
                    b.Navigation("Produtos");
                });
#pragma warning restore 612, 618
        }
    }
}
