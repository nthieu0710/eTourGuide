﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using eTourGuide.Data.Entity;

#nullable disable

namespace eTourGuide.Data.Context
{
    public partial class eTourGuideContext : DbContext
    {
        public eTourGuideContext()
        {
        }

        public eTourGuideContext(DbContextOptions<eTourGuideContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Edge> Edges { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Exhibit> Exhibits { get; set; }
        public virtual DbSet<ExhibitInEvent> ExhibitInEvents { get; set; }
        public virtual DbSet<ExhibitInTopic> ExhibitInTopics { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<Map> Maps { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("Account");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Edge>(entity =>
            {
                entity.ToTable("Edge");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.FromPositionNavigation)
                    .WithMany(p => p.EdgeFromPositionNavigations)
                    .HasForeignKey(d => d.FromPosition)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Edge_Positions");

                entity.HasOne(d => d.ToPositionNavigation)
                    .WithMany(p => p.EdgeToPositionNavigations)
                    .HasForeignKey(d => d.ToPosition)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Edge_Positions1");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameEng).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Event_Room");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK_Event_Account");
            });

            modelBuilder.Entity<Exhibit>(entity =>
            {
                entity.ToTable("Exhibit");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameEng).HasMaxLength(50);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExhibitInEvent>(entity =>
            {
                entity.HasKey(e => new { e.ExhibitId, e.EventId, e.CreateDate });

                entity.ToTable("ExhibitInEvent");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.ExhibitInEvents)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_ExhibitInEvent_Event");

                entity.HasOne(d => d.Exhibit)
                    .WithMany(p => p.ExhibitInEvents)
                    .HasForeignKey(d => d.ExhibitId)
                    .HasConstraintName("FK_ExhibitInEvent_Exhibit");
            });

            modelBuilder.Entity<ExhibitInTopic>(entity =>
            {
                entity.HasKey(e => new { e.ExhibitId, e.TopicId, e.CreateDate });

                entity.ToTable("ExhibitInTopic");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Exhibit)
                    .WithMany(p => p.ExhibitInTopics)
                    .HasForeignKey(d => d.ExhibitId)
                    .HasConstraintName("FK_ExhibitInTopic_Exhibit");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.ExhibitInTopics)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK_ExhibitInTopic_Topic");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.VisitorName).HasMaxLength(50);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_Feedback_Event");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK_Feedback_Topic");
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.ToTable("Floor");

                entity.Property(e => e.FloorId).ValueGeneratedNever();

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.Floors)
                    .HasForeignKey(d => d.MapId)
                    .HasConstraintName("FK_Floor_Map");
            });

            modelBuilder.Entity<Map>(entity =>
            {
                entity.ToTable("Map");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DescriptionEng)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DescriptionVie).HasMaxLength(100);

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Positions_Map");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Positions_Room");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topic");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameEng).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Topic_Room");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK_Topic_Account");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
