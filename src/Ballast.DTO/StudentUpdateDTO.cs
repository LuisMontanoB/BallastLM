namespace Ballast.DTO
{
    public class StudentUpdateDTO
    {
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Enabled { get; set; }
    }
}
