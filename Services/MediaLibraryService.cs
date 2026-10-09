using MusicStreaming.Models;

namespace MusicStreaming.Services;

/// <summary>
/// Асинхронные операции медиатеки: имитация загрузки из БД / внешнего API,
/// обработка плейлистов, длительные операции с поддержкой отмены.
/// </summary>
public class MediaLibraryService
{
    private readonly Random random = new();

    /// <summary>
    /// Б3, У1. Простой асинхронный метод без возвращаемого значения.
    /// </summary>
    public async Task SyncLibraryAsync()
    {
        Console.WriteLine("Синхронизация медиатеки с сервером...");
        await Task.Delay(1000); // имитация длительной операции
        Console.WriteLine("Синхронизация завершена.");
    }

    /// <summary>
    /// Б3, У2. Асинхронный метод с возвращаемым значением Task&lt;List&lt;T&gt;&gt;,
    /// имитирующий получение треков из внешнего API. Содержит try-catch-finally.
    /// </summary>
    public async Task<List<Track>> LoadTracksFromApiAsync(bool simulateError = false,
                                                          CancellationToken token = default)
    {
        var result = new List<Track>();
        Console.WriteLine("Подключение к Music API...");

        try
        {
            await Task.Delay(1000, token);

            if (simulateError)
                throw new HttpRequestException("Сервер Music API вернул ошибку 503 (Service Unavailable).");

            result.Add(new Track(101, "Levitating", "Dua Lipa", "Pop", 203, 1.99m, 1_600_000_000) { ReleaseYear = 2020 });
            result.Add(new Track(102, "Smells Like Teen Spirit", "Nirvana", "Rock", 301, 1.49m, 1_900_000_000) { ReleaseYear = 1991 });
            result.Add(new Track(103, "Lose Yourself", "Eminem", "Hip-Hop", 326, 1.99m, 2_100_000_000) { ReleaseYear = 2002, IsExplicit = true });

            Console.WriteLine($"Получено треков из API: {result.Count}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            Console.WriteLine("Будет возвращён пустой список.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Загрузка из API отменена.");
            throw;
        }
        finally
        {
            // Выполняется всегда — и при успехе, и при ошибке
            Console.WriteLine("Соединение с Music API закрыто (finally).");
        }

        return result;
    }

    /// <summary>
    /// Б3, У3. Обработка плейлиста (анализ треков) — используется в Task.WhenAll.
    /// </summary>
    public async Task<TimeSpan> AnalyzePlaylistAsync(Playlist playlist, CancellationToken token)
    {
        Console.WriteLine($"  [старт] Анализ плейлиста «{playlist.Name}»...");
        int delay = 700 + random.Next(0, 900);
        await Task.Delay(delay, token);
        Console.WriteLine($"  [готово] «{playlist.Name}»: {playlist.Tracks.Count} тр., " +
                          $"{playlist.Duration:h\\:mm\\:ss} (обработано за {delay} мс)");
        return playlist.Duration;
    }

    /// <summary>
    /// Б3, У3. Длительная операция (перекодирование медиатеки) с поддержкой отмены.
    /// </summary>
    public async Task ConvertLibraryAsync(IReadOnlyList<Track> tracks, CancellationToken token)
    {
        Console.WriteLine("Перекодирование медиатеки в формат FLAC...");
        for (int i = 0; i < tracks.Count; i++)
        {
            token.ThrowIfCancellationRequested();
            await Task.Delay(400, token);
            Console.WriteLine($"  Перекодировано {i + 1}/{tracks.Count}: {tracks[i].Title}");
        }
        Console.WriteLine("Перекодирование завершено полностью.");
    }
}
