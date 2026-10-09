using MusicStreaming.Models;

namespace MusicStreaming.Services;

/// <summary>
/// Тестовые данные медиатеки.
/// </summary>
public static class DataSeeder
{
    public static List<Track> CreateTracks()
    {
        // Б2, У1. Коллекция List<T> с 7 тестовыми объектами
        return new List<Track>
        {
            new Track(1, "Bohemian Rhapsody", "Queen", "Rock", 355, 1.49m, 2_500_000_000) { ReleaseYear = 1975 },
            new Track(2, "Blinding Lights", "The Weeknd", "Synth-pop", 200, 1.99m, 4_700_000_000) { ReleaseYear = 2019 },
            new Track(3, "Believer", "Imagine Dragons", "Pop-rock", 204, 1.79m, 2_900_000_000) { ReleaseYear = 2017 },
            new Track(4, "Bad Guy", "Billie Eilish", "Pop", 194, 1.99m, 2_800_000_000) { ReleaseYear = 2019 },
            new Track(5, "Shape of You", "Ed Sheeran", "Pop", 233, 1.99m, 4_000_000_000) { ReleaseYear = 2017 },
            new Track(6, "Viva La Vida", "Coldplay", "Alternative", 242, 1.49m, 2_000_000_000) { ReleaseYear = 2008 },
            new Track(7, "Get Lucky", "Daft Punk", "Electronic", 369, 1.29m, 1_500_000_000) { ReleaseYear = 2013 },
        };
    }

    public static List<Artist> CreateArtists()
    {
        return new List<Artist>
        {
            new Artist(1, "Queen", "Великобритания", "Rock", 52_000_000),
            new Artist(2, "The Weeknd", "Канада", "R&B / Synth-pop", 98_000_000),
            new Artist(3, "Imagine Dragons", "США", "Pop-rock", 61_000_000),
        };
    }

    public static List<Album> CreateAlbums(IReadOnlyList<Track> tracks)
    {
        var opera = new Album(1, "A Night at the Opera", "Queen", 1975);
        opera.AddTrack(tracks[0]);

        var afterHours = new Album(2, "After Hours", "The Weeknd", 2020);
        afterHours.AddTrack(tracks[1]);

        var evolve = new Album(3, "Evolve", "Imagine Dragons", 2017);
        evolve.AddTrack(tracks[2]);

        return new List<Album> { opera, afterHours, evolve };
    }

    public static List<Playlist> CreatePlaylists(IReadOnlyList<Track> tracks)
    {
        var workout = new Playlist(1, "Для тренировки", "Дарья");
        workout.AddTrack(tracks[1], tracks[2], tracks[6]);          // перегрузка: params

        var classics = new Playlist(2, "Классика рока", "Дарья");
        classics.AddTrack(tracks[0]);                                // перегрузка: один трек
        classics.AddTrack(tracks[5]);

        var chill = new Playlist(3, "Спокойный вечер", "Дарья") { IsPublic = false };
        chill.AddTrack(tracks[3], tracks[4], tracks[5]);

        return new List<Playlist> { workout, classics, chill };
    }
}
