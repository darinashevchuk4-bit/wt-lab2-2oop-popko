namespace MusicStreaming.Interfaces;

/// <summary>
/// Обобщённый интерфейс сервиса управления данными.
/// T — любая сущность, реализующая IEntity.
/// </summary>
public interface IItemService<T> where T : IEntity
{
    int Count { get; }

    IReadOnlyList<T> GetAll();
    bool TryGetById(int id, out T? item);
    void Add(T item);
    bool Remove(int id);
    IEnumerable<T> Search(string query);
}
