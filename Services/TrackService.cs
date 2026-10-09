using MusicStreaming.Interfaces;
using MusicStreaming.Models;

namespace MusicStreaming.Services;

/// <summary>
/// Сервис управления треками медиатеки.
/// Реализует IItemService&lt;Track&gt;, хранит данные в List и Dictionary.
/// </summary>
public class TrackService : IItemService<Track>
{
    // List<T> — для перебора и LINQ-запросов
    private readonly List<Track> tracks = new();

    // Dictionary<TKey, TValue> — для быстрого доступа по Id
    private readonly Dictionary<int, Track> tracksById = new();

    public int Count => tracks.Count;

    public TrackService() { }

    public TrackService(IEnumerable<Track> initialTracks)
    {
        foreach (Track track in initialTracks)
            Add(track);
    }

    public IReadOnlyList<Track> GetAll() => tracks;

    public IReadOnlyDictionary<int, Track> GetDictionary() => tracksById;

    // Безопасный поиск через TryGetValue
    public bool TryGetById(int id, out Track? item)
    {
        return tracksById.TryGetValue(id, out item);
    }

    public void Add(Track item)
    {
        if (tracksById.ContainsKey(item.Id))
            throw new InvalidOperationException($"Трек с Id = {item.Id} уже существует.");

        tracks.Add(item);
        tracksById.Add(item.Id, item);
    }

    public bool Remove(int id)
    {
        if (!tracksById.TryGetValue(id, out Track? track))
            return false;

        tracksById.Remove(id);
        return tracks.Remove(track);
    }

    // ===== Перегрузка методов (method overloading) =====

    // 1) Поиск по строке (название или исполнитель)
    public IEnumerable<Track> Search(string query)
    {
        return tracks.Where(t =>
            t.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            t.ArtistName.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    // 2) Поиск по жанру и минимальному числу прослушиваний
    public IEnumerable<Track> Search(string genre, long minPlays)
    {
        return tracks.Where(t =>
            t.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase) &&
            t.PlayCount >= minPlays);
    }

    // 3) Поиск по максимальной длительности
    public IEnumerable<Track> Search(TimeSpan maxDuration)
    {
        return tracks.Where(t => t.Duration <= maxDuration);
    }

    // 4) Поиск по диапазону годов выпуска
    public IEnumerable<Track> Search(int fromYear, int toYear)
    {
        return tracks.Where(t => t.ReleaseYear >= fromYear && t.ReleaseYear <= toYear);
    }
}
