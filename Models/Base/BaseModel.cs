namespace Pigga.Models.Base
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public BaseModel()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
