﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SubscriptionProvider.Data.Contexts;

#nullable disable

namespace SubscriptionProvider.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240517123222_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SubscriptionProvider.Data.Entities.SubscribeEntity", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsSubscribed")
                        .HasColumnType("bit");

                    b.HasKey("Email");

                    b.ToTable("Subscribers");
                });
#pragma warning restore 612, 618
        }
    }
}
