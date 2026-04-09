using System;

public class PasswordResetToken
{
    public class PasswordResetToken
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public string TokenHash { get; set; }

        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public User User { get; set; }
    }
}
