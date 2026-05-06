using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyRoute.Domain.Entities;
using SkyRoute.Domain.ValueObjects;

namespace SkyRoute.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Reference);

        builder.Property(b => b.Reference)
            .HasConversion(r => r.Value, s => BookingReference.FromString(s))
            .HasMaxLength(50);

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(b => b.CreatedAt).IsRequired();

        builder.OwnsOne(b => b.PricePerPassenger, m =>
        {
            m.Property(x => x.Amount).HasColumnName("PricePerPassenger_Amount").HasPrecision(18, 2);
            m.Property(x => x.Currency).HasColumnName("PricePerPassenger_Currency").HasMaxLength(3);
        });

        builder.OwnsOne(b => b.TotalPrice, m =>
        {
            m.Property(x => x.Amount).HasColumnName("TotalPrice_Amount").HasPrecision(18, 2);
            m.Property(x => x.Currency).HasColumnName("TotalPrice_Currency").HasMaxLength(3);
        });

        builder.OwnsOne(b => b.Flight, f =>
        {
            f.Property(x => x.FlightId).HasColumnName("Flight_FlightId").HasMaxLength(50).IsRequired();
            f.Property(x => x.Provider).HasColumnName("Flight_Provider").HasMaxLength(50).IsRequired();
            f.Property(x => x.FlightNumber).HasColumnName("Flight_FlightNumber").HasMaxLength(20).IsRequired();
            f.Property(x => x.DepartureTime).HasColumnName("Flight_DepartureTime").IsRequired();
            f.Property(x => x.ArrivalTime).HasColumnName("Flight_ArrivalTime").IsRequired();
            f.Property(x => x.CabinClass).HasConversion<string>().HasColumnName("Flight_CabinClass").HasMaxLength(20).IsRequired();

            f.OwnsOne(x => x.Origin, o =>
            {
                o.Property(a => a.Code).HasColumnName("Flight_Origin_Code").HasMaxLength(3).IsRequired();
                o.Property(a => a.Name).HasColumnName("Flight_Origin_Name").HasMaxLength(200).IsRequired();
                o.Property(a => a.City).HasColumnName("Flight_Origin_City").HasMaxLength(100).IsRequired();
                o.Property(a => a.Country).HasColumnName("Flight_Origin_Country").HasMaxLength(2).IsRequired();
                o.Property(a => a.CountryName).HasColumnName("Flight_Origin_CountryName").HasMaxLength(100).IsRequired();
            });

            f.OwnsOne(x => x.Destination, d =>
            {
                d.Property(a => a.Code).HasColumnName("Flight_Destination_Code").HasMaxLength(3).IsRequired();
                d.Property(a => a.Name).HasColumnName("Flight_Destination_Name").HasMaxLength(200).IsRequired();
                d.Property(a => a.City).HasColumnName("Flight_Destination_City").HasMaxLength(100).IsRequired();
                d.Property(a => a.Country).HasColumnName("Flight_Destination_Country").HasMaxLength(2).IsRequired();
                d.Property(a => a.CountryName).HasColumnName("Flight_Destination_CountryName").HasMaxLength(100).IsRequired();
            });

            f.OwnsOne(x => x.BaseFare, bf =>
            {
                bf.Property(x => x.Amount).HasColumnName("Flight_BaseFare_Amount").HasPrecision(18, 2);
                bf.Property(x => x.Currency).HasColumnName("Flight_BaseFare_Currency").HasMaxLength(3);
            });
        });

        builder.OwnsMany(b => b.Passengers, p =>
        {
            p.ToTable("BookingPassengers");
            p.WithOwner().HasForeignKey("BookingReference");
            p.Property<long>("Id").ValueGeneratedOnAdd();
            p.HasKey("Id");
            p.Property(x => x.FullName).HasMaxLength(100).IsRequired();
            p.Property(x => x.Email).HasMaxLength(200).IsRequired();
            p.Property(x => x.DocumentNumber).HasMaxLength(20).IsRequired();
        });
    }
}
