using MusicStreaming.Interfaces;

namespace MusicStreaming.Models;

/// <summary>
/// Пользовательский плейлист.
/// </summary>
public class Playlist : BaseEntity, IPlayable
{
    public string Name { get; set; }
    public string Owner { get; set; }
    public bool IsPublic { get; set; } = true;

    private readonly List<Track> tracks = new();
    public IReadOnlyList<Track> Tracks => tracks;

    public TimeSpan Duration => TimeSpan.FromSeconds(tracks.Sum(t => t.DurationSeconds));

    public Playlist(int id, string name, string owner) : base(id)
    {
        Name = name;
        Owner = owner;
    }

    public override string GetEntityType() => "Плейлист";

    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"Название: {Name}");
        Console.WriteLine($"Владелец: {Owner}");
        Console.WriteLine($"Доступ: {(IsPublic ? "публичный" : "приватный")}");
        Console.WriteLine($"Треков: {tracks.Count}, длительность: {FormatDuration(Duration)}");
        foreach (Track track in tracks)
            Console.WriteLine($"   • {track.ArtistName} — {track.Title}");
        Console.WriteLine();
    }

    // Перегрузка методов: добавление одного трека...
    public void AddTrack(Track track)
    {
        if (!tracks.Contains(track))
            tracks.Add(track);
    }

    // ...и добавление сразу нескольких треков
    public void AddTrack(params Track[] newTracks)
    {
        foreach (Track track in newTracks)
            AddTrack(track);
    }

    public bool RemoveTrack(int trackId)
    {
        Track? track = tracks.FirstOrDefault(t => t.Id == trackId);
        return track != null && tracks.Remove(track);
    }

    public void Play()
    {
        Console.WriteLine($"► Плейлист «{Name}» ({tracks.Count} тр.)");
        foreach (Track track in tracks)
            track.Play();
    }
}
