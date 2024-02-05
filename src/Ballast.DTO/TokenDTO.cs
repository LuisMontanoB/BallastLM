namespace Ballast.DTO
{
    public class TokenDTO
    {
        public int UserTokenId { get; set; }
        public int UserId { get; set; }
        public Guid TokenCode { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
