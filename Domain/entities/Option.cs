using System;

public class Option

{
    public class Option
    {
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }

        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Question Question { get; set; }
    }
}
