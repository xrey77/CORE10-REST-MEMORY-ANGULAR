namespace core10_memorycache.Models
{

   public record ContactModel
    {
        public Int32 Id { get; init; } = 0; 
        public string Firstname { get; init; } = string.Empty;
        public string Lastname { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Mobile { get; init; } = string.Empty;
    }
}