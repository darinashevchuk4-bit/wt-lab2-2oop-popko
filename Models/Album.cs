using MusicStreaming.Interfaces;

namespace MusicStreaming.Models;

/// <summary>
/// Альбом — набор треков одного исполнителя.
/// </summary>
public class Album : BaseEntity, IPlayable
{
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public int ReleaseYear { get; set; }

    // Список треков закрыт: снаружи только чтение
    private readonly List<Track> tracks = new();
    public IReadOnlyList<Track> Tracks => tracks;

    public TimeSpan Duration => TimeSpan.FromSeconds(tracks.Sum(t => t.DurationSeconds));

    public Album(int id, string title, string artistName, int releaseYear) : base(id)
    {
        Title = title;
        ArtistName = artistName;
        ReleaseYear = releaseYear;
    }

    public override string GetEntityType() => "Альбом";

    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"Название: {Title}");
        Console.WriteLine($"Исполнитель: {ArtistName}");
        Console.WriteLine($"Год выпуска: {ReleaseYear}");
        Console.WriteLine($"Количество треков: {tracks.Count}");
        Console.WriteLine($"Общая длительность: {FormatDuration(Duration)}");
        Console.WriteLine();
    }

    public void AddTrack(Track track)
    {
        if (tracks.Any(t => t.Id == track.Id))
        {
            Console.WriteLine($"Трек «{track.Title}» уже есть в альбоме.");
            return;
        }
        track.AlbumTitle = Title;
        track.ReleaseYear = ReleaseYear;
        tracks.Add(track);
    }

    public void Play()
    {
        Console.WriteLine($"► Альбом «{Title}» ({tracks.Count} тр., {FormatDuration(Duration)})");
        foreach (Track track in tracks)
            track.Play();
    }
}
