using System;
using eTourGuide.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventInRoom> EventInRooms { get; set; }
        public virtual DbSet<Exhibit> Exhibits { get; set; }
        public virtual DbSet<ExhibitInEvent> ExhibitInEvents { get; set; }
        public virtual DbSet<ExhibitInTopic> ExhibitInTopics { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<TopicInRoom> TopicInRooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:etourguide.database.windows.net,1433;Database=eTourGuide;User ID=login;Password=Caps1234;Trusted_Connection=False;Encrypt=True;MultipleActiveResultSets=true;", x => x.UseNetTopologySuite());
            }
        }

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

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.HasIndex(e => e.Name, "IX_Event")
                    .IsUnique();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsDelete)
                    .HasColumnName("isDelete")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EventInRoom>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.RoomId })
                    .HasName("PK_EventInRoom_1");

                entity.ToTable("EventInRoom");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventInRooms)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventInRoom_Event1");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.EventInRooms)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventInRoom_Room1");
            });

            modelBuilder.Entity<Exhibit>(entity =>
            {
                entity.ToTable("Exhibit");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ExhibitInEvent>(entity =>
            {
                entity.HasKey(e => new { e.ExhibitId, e.EventId });

                entity.ToTable("ExhibitInEvent");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.ExhibitInEvents)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExhibitInEvent_Event");

                entity.HasOne(d => d.Exhibit)
                    .WithMany(p => p.ExhibitInEvents)
                    .HasForeignKey(d => d.ExhibitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExhibitInEvent_Exhibit");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.ExhibitInEvents)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_ObjectInEvent_Room");
            });

            modelBuilder.Entity<ExhibitInTopic>(entity =>
            {
                entity.HasKey(e => new { e.ExhibitId, e.TopicId });

                entity.ToTable("ExhibitInTopic");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Exhibit)
                    .WithMany(p => p.ExhibitInTopics)
                    .HasForeignKey(d => d.ExhibitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExhibitInTopic_Exhibit");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.ExhibitInTopics)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_ExhibitInTopic_Room");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.ExhibitInTopics)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

                entity.HasOne(d => d.Exhibitt)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.ExhibittId)
                    .HasConstraintName("FK_Feedback_Object");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK_Feedback_Topic");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.PointId);

                entity.ToTable("Position");

                entity.Property(e => e.Latitude).HasColumnType("decimal(8, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Position_Room");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topic");

                entity.HasIndex(e => e.Name, "IX_Topic")
                    .IsUnique();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.IsDelete)
                    .HasColumnName("isDelete")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TopicInRoom>(entity =>
            {
                entity.HasKey(e => new { e.TopicId, e.RoomId })
                    .HasName("PK_TopicInRoom_1");

                entity.ToTable("TopicInRoom");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.TopicInRooms)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TopicInRoom_Room1");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.TopicInRooms)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TopicInRoom_Topic1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
