using System;

public class ClOTPCodesass1
{
	public OTPCodes()
	{
		public int id { get; set; }

		public string Email { get; set; }

	public string CodeHash { get; set; }

    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool  IsIssued { get; set; }

	public int AttemptCount { get; set; }

	public int ResendCount { get; set; }

    public bool IsDeleted { get; set; }


}
}
