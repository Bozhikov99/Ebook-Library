using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class BookUserConfiguration : IEntityTypeConfiguration<BookUser>
    {
        public void Configure(EntityTypeBuilder<BookUser> builder)
        {
            builder.HasKey(bu => new { bu.BookId, bu.UserId });

            builder.HasOne(bu => bu.Book)
                .WithMany(b => b.BookUsers)
                .HasForeignKey(bu => bu.BookId);
            
            builder.HasOne(bu => bu.User)
                .WithMany(b => b.FavouriteBooks)
                .HasForeignKey(bu => bu.UserId);
        }
    }
}
