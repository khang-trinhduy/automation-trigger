﻿// <auto-generated />
using System;
using Automation.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Automation.API.Migrations
{
    [DbContext(typeof(AutoContext))]
    [Migration("20190510112125_change-metadata")]
    partial class changemetadata
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Automation.API.Models.Action", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MetaDataId");

                    b.Property<int?>("TriggerId");

                    b.Property<string>("Type");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("MetaDataId");

                    b.HasIndex("TriggerId");

                    b.ToTable("Action");
                });

            modelBuilder.Entity("Automation.API.Models.Condition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MetaDataId");

                    b.Property<string>("Operator");

                    b.Property<string>("Threshold");

                    b.Property<int?>("TriggerId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("MetaDataId");

                    b.HasIndex("TriggerId");

                    b.ToTable("Condition");
                });

            modelBuilder.Entity("Automation.API.Models.MetaData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Field");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("MetaData");
                });

            modelBuilder.Entity("Automation.API.Models.Trigger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<bool>("IsNotActive");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<int>("Position");

                    b.Property<string>("Table");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Trigger");
                });

            modelBuilder.Entity("Automation.API.Models.Action", b =>
                {
                    b.HasOne("Automation.API.Models.MetaData", "MetaData")
                        .WithMany()
                        .HasForeignKey("MetaDataId");

                    b.HasOne("Automation.API.Models.Trigger")
                        .WithMany("Actions")
                        .HasForeignKey("TriggerId");
                });

            modelBuilder.Entity("Automation.API.Models.Condition", b =>
                {
                    b.HasOne("Automation.API.Models.MetaData", "MetaData")
                        .WithMany()
                        .HasForeignKey("MetaDataId");

                    b.HasOne("Automation.API.Models.Trigger")
                        .WithMany("Conditions")
                        .HasForeignKey("TriggerId");
                });
#pragma warning restore 612, 618
        }
    }
}
