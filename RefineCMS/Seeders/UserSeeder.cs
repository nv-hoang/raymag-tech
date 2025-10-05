using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefineCMS.Models;

namespace RefineCMS.Seeders;

public class UserSeeder : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(new User
        {
            Id = 1,
            UserLogin = "admin",
            UserPass = "$2a$11$EoX1VnDnQ1s5.pd6pVprkOrxvgaoFaO9tiVSnwmT/w7LYZG3k5VOC", // Admin@123
            DisplayName = "Administration",
            UserStatus = 1
        });
    }
}
