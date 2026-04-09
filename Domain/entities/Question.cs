using System;

public class Question

{
    public class Question
    {
        public Guid Id { get; set; }

        public Guid QuizId { get; set; }

        public string Text { get; set; }
        public string Explanation { get; set; }

        public int OrderIndex { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Quiz Quiz { get; set; }
        public ICollection<Option> Options { get; set; }
    }
}
