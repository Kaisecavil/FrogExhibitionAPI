namespace FrogExhibitionDAL.Models.Base
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime? DeletedAt { get; set; } = null;
    }
}
