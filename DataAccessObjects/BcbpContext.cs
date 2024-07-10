using System;
using System.Collections.Generic;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessObjects;

public partial class BcbpContext : DbContext
{
    public BcbpContext()
    {
    }

    public BcbpContext(DbContextOptions<BcbpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AvailableBookingType> AvailableBookingTypes { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingDetail> BookingDetails { get; set; }

    public virtual DbSet<BookingType> BookingTypes { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Court> Courts { get; set; }

    public virtual DbSet<CourtType> CourtTypes { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
            optionsBuilder.EnableSensitiveDataLogging(); // Thêm dòng này để bật ghi nhật ký dữ liệu nhạy cảm
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Account__1788CC4C22C2012D");

            entity.ToTable("Account");

            entity.Property(e => e.AvatarLink).IsUnicode(false);
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.Role).IsUnicode(false);
            entity.Property(e => e.UserPhone).IsUnicode(false);
            entity.Property(e => e.Username).IsUnicode(false);

            entity.HasOne(d => d.ClubManage).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.ClubManageId)
                .HasConstraintName("FK_User_ClubManageId");
        });

        modelBuilder.Entity<AvailableBookingType>(entity =>
        {
            entity.HasKey(e => e.AvailableBookingTypeId).HasName("PK__Availabl__EA1E51E351EC4870");

            entity.ToTable("AvailableBookingType");

            entity.HasOne(d => d.BookingType).WithMany(p => p.AvailableBookingTypes)
                .HasForeignKey(d => d.BookingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AvailableBookingType_BookingTypeId");

            entity.HasOne(d => d.Club).WithMany(p => p.AvailableBookingTypes)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AvailableBookingType_ClubId");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951AEDDE1D2943");

            entity.ToTable("Booking");

            entity.HasOne(d => d.BookingType).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.BookingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_BookingTypeId");

            entity.HasOne(d => d.Club).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_ClubId");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Booking_UserId");
        });

        modelBuilder.Entity<BookingDetail>(entity =>
        {
            entity.HasKey(e => e.BookingDetailId).HasName("PK__BookingD__8136D45AF8080A27");

            entity.ToTable("BookingDetail");

            entity.Property(e => e.EndTime).HasColumnName("EndTIme");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingDetail_BookingId");

            entity.HasOne(d => d.Court).WithMany(p => p.BookingDetails)
                .HasForeignKey(d => d.CourtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingDetail_CourtId");
        });

        modelBuilder.Entity<BookingType>(entity =>
        {
            entity.HasKey(e => e.BookingTypeId).HasName("PK__BookingT__649EC4914E954916");

            entity.ToTable("BookingType");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__City__F2D21B76A2F7CA00");

            entity.ToTable("City");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.ClubId).HasName("PK__Club__D35058E73B7EC9D8");

            entity.ToTable("Club");

            entity.Property(e => e.ApiKey).IsUnicode(false);
            entity.Property(e => e.AvatarLink).IsUnicode(false);
            entity.Property(e => e.ChecksumKey).IsUnicode(false);
            entity.Property(e => e.ClientId).IsUnicode(false);
            entity.Property(e => e.ClubEmail).IsUnicode(false);
            entity.Property(e => e.ClubPhone).IsUnicode(false);
            entity.Property(e => e.FanpageLink).IsUnicode(false);

            entity.HasOne(d => d.District).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Club_DistrictId");
        });

        modelBuilder.Entity<Court>(entity =>
        {
            entity.HasKey(e => e.CourtId).HasName("PK__Court__C3A67C9A19384575");

            entity.ToTable("Court");

            entity.HasOne(d => d.Club).WithMany(p => p.Courts)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Court_ClubId");

            entity.HasOne(d => d.CourtType).WithMany(p => p.Courts)
                .HasForeignKey(d => d.CourtTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Court_CourtTypeId");
        });

        modelBuilder.Entity<CourtType>(entity =>
        {
            entity.HasKey(e => e.CourtTypeId).HasName("PK__CourtTyp__FF339CB5DAE67B74");

            entity.ToTable("CourtType");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PK__District__85FDA4C6A0FB4FC7");

            entity.ToTable("District");

            entity.HasOne(d => d.City).WithMany(p => p.Districts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_District_CityId");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.MatchId).HasName("PK__Match__4218C81712C29E55");

            entity.ToTable("Match");

            entity.HasIndex(e => e.BookingId, "UQ__Match__73951AEC68563390").IsUnique();

            entity.HasOne(d => d.Booking).WithOne(p => p.Match)
                .HasForeignKey<Match>(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Match_BookingId");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79CE79E9E0BB");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Club).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_ClubId");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_UserId");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Slot__0A124AAF4C7A8509");

            entity.ToTable("Slot");

            entity.HasOne(d => d.Club).WithMany(p => p.Slots)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Slot_ClubId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
