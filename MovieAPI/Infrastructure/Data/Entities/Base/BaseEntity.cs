namespace MovieAPI.Infrastructure.Data.Entities.Base;

public abstract class BaseEntity : IEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

