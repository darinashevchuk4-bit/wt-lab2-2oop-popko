namespace MusicStreaming.Interfaces;

/// <summary>
/// Базовый контракт для любой сущности медиатеки:
/// у каждой сущности есть идентификатор, дата создания и вывод информации.
/// </summary>
public interface IEntity
{
    int Id { get; }
    DateTime CreatedAt { get; }

    string GetEntityType();
    void PrintInfo();
}
