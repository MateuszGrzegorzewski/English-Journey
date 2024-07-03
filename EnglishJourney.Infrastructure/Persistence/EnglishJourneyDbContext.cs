using EnglishJourney.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnglishJourney.Infrastructure.Persistence
{
    public class EnglishJourneyDbContext(DbContextOptions<EnglishJourneyDbContext> options)
        : IdentityDbContext<User>(options)
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<ConnectionTopic> ConnectionTopics { get; set; }
        public DbSet<ConnectionAttribute> ConnectionAtrributes { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<FlashcardBox> FlashcardsBoxes { get; set; }
        public DbSet<FlashcardCategory> FlashcardsCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConnectionAttribute>()
                .HasOne(c => c.Topic)
                .WithMany(c => c.Attributes)
                .HasForeignKey(c => c.TopicId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<FlashcardBox>()
                .HasMany(f => f.Flashcards)
                .WithOne(f => f.FlashcardBox)
                .HasForeignKey(f => f.FlashcardBoxId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FlashcardBox>()
                .HasOne(f => f.FlashcardCategory)
                .WithMany(f => f.FlashcardBoxes)
                .HasForeignKey(f => f.FlashcardCategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                 .HasMany(u => u.ConnectionTopics)
                 .WithOne(c => c.User)
                 .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.FlashcardCategories)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Notes)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId);
        }
    }
}