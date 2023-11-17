namespace Domain.Common;

public interface IEntity : IHasDomainEvent
{
    public int Id { get; set; }
}
