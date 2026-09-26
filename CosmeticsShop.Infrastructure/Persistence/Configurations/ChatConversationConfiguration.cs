using CosmeticsShop.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public sealed class ChatConversationConfiguration : IEntityTypeConfiguration<ChatConversation>
{
    public void Configure(EntityTypeBuilder<ChatConversation> builder)
    {
        builder.ToTable("ChatConversations");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();   // ← ajouté

        builder.Property(c => c.CustomerId).IsRequired();
        builder.Property(c => c.LastMessageAt).IsRequired();

        builder.HasIndex(c => c.CustomerId).IsUnique();

        builder.Navigation(c => c.Messages)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(c => c.Messages, message =>
        {
            message.ToTable("ChatMessages");
            message.WithOwner().HasForeignKey("ChatConversationId");
            message.HasKey(m => m.Id);
            message.Property(m => m.Id).ValueGeneratedNever();   // ← ajouté, le vrai correctif

            message.Property(m => m.Role).IsRequired().HasMaxLength(20);
            message.Property(m => m.Content).IsRequired().HasMaxLength(4000);
            message.Property(m => m.SentAt).IsRequired();
        });
    }
}