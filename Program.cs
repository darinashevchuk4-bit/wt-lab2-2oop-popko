using System.Text;
using MusicStreaming.Interfaces;
using MusicStreaming.Models;
using MusicStreaming.Services;

namespace MusicStreaming;

/// <summary>
/// Лабораторная работа №2 «Классы. Свойства. Методы. Экземпляры объектов».
/// Вариант 13 — Музыкальный стриминг / Медиатека (Track, Album, Artist, Playlist).
/// Выполнила: Попко Д.А., группа 1д2401ГУП.
/// </summary>
public class Program
{
    private static TrackService trackService = null!;
    private static List<Artist> artists = null!;
    private static List<Album> albums = null!;
    private static List<Playlist> playlists = null!;
    private static readonly MediaLibraryService mediaService = new();

    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Начальные данные
        List<Track> tracks = DataSeeder.CreateTracks();
        trackService = new TrackService(tracks);
        artists = DataSeeder.CreateArtists();
        albums = DataSeeder.CreateAlbums(tracks);
        playlists = DataSeeder.CreatePlaylists(tracks);

        PrintWelcome();

        while (true)
        {
            PrintMenu();
            Console.Write("Выберите действие: ");
            string? choice = Console.ReadLine();
            if (choice == null) break; // конец ввода

            Console.WriteLine();
            switch (choice.Trim())
            {
                case "1": Block1Level1(); break;
                case "2": Block1Level2(); break;
                case "3": Block1Level3(); break;
                case "4": Block2Level1(); break;
                case "5": Block2Level2(); break;
                case "6": Block2Level3Queries(); break;
                case "7": Block2Level3Statistics(); break;
                case "8": await Block3Level1(); break;
                case "9": await Block3Level2(); break;
                case "10": await Block3Level3Parallel(); break;
                case "11": await Block3Level3Cancel(); break;
                case "0":
                    Console.WriteLine("Спасибо за использование музыкального сервиса! До свидания.");
                    return;
                default:
                    Console.WriteLine("Неизвестная команда. Введите число от 0 до 11.");
                    break;
            }

            Console.WriteLine();
            Console.Write("Нажмите Enter, чтобы вернуться в меню...");
            if (Console.ReadLine() == null) break;
        }
    }

    // ======================= ВВОДНОЕ ЗАДАНИЕ =======================

    private static void PrintWelcome()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("   ♫  МУЗЫКАЛЬНЫЙ СТРИМИНГ «MUSICBOX»  ♫");
        Console.WriteLine("     Медиатека: треки, альбомы, плейлисты");
        Console.WriteLine("==============================================");
        Console.WriteLine("Добро пожаловать! ЛР №2, вариант 13. Выполнила: Попко Д.А.");
        Console.WriteLine();
    }

    private static void PrintMenu()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("                ГЛАВНОЕ МЕНЮ");
        Console.WriteLine("==============================================");
        Console.WriteLine(" 1. Создать трек и вывести информацию   [Б1 У1]");
        Console.WriteLine(" 2. Инкапсуляция и полиморфизм          [Б1 У2]");
        Console.WriteLine(" 3. Сервис, перегрузка, protected       [Б1 У3]");
        Console.WriteLine(" 4. Показать все треки (List + foreach) [Б2 У1]");
        Console.WriteLine(" 5. Найти трек по ID (Dictionary)       [Б2 У2]");
        Console.WriteLine(" 6. LINQ: фильтр, сортировка, проекция  [Б2 У3]");
        Console.WriteLine(" 7. LINQ: статистика и группировка      [Б2 У3]");
        Console.WriteLine(" 8. Синхронизировать медиатеку (async)  [Б3 У1]");
        Console.WriteLine(" 9. Загрузить треки из API              [Б3 У2]");
        Console.WriteLine("10. Параллельный анализ плейлистов      [Б3 У3]");
        Console.WriteLine("11. Перекодирование с отменой           [Б3 У3]");
        Console.WriteLine(" 0. Выход");
        Console.WriteLine("==============================================");
    }

    private static void Title(string text)
    {
        Console.WriteLine($"=== {text} ===");
        Console.WriteLine();
    }

    // ======================= БЛОК 1 =======================

    // Уровень 1: класс с автосвойствами, конструктор, PrintInfo, new + инициализатор
    private static void Block1Level1()
    {
        Title("Б1, У1. Создание экземпляров класса Track");

        // Экземпляр через конструктор + синтаксис инициализатора объектов
        Track track1 = new Track(201, "Numb", "Linkin Park", "Alternative rock", 185, 1.49m, 2_000_000_000)
        {
            AlbumTitle = "Meteora",
            ReleaseYear = 2003,
            IsExplicit = false
        };

        Track track2 = new Track(202, "Someone Like You", "Adele", "Soul", 285, 1.99m)
        {
            AlbumTitle = "21",
            ReleaseYear = 2011
        };

        // Вызов методов объектов
        track1.PrintInfo();
        track2.PrintInfo();

        track1.Play();
        track2.Play();
        Console.WriteLine($"Прослушиваний «{track2.Title}» после воспроизведения: {track2.PlayCount}");
    }

    // Уровень 2: модификаторы доступа, абстрактный класс/интерфейс, полиморфизм
    private static void Block1Level2()
    {
        Title("Б1, У2. Инкапсуляция: изменение цены через UpdatePrice()");

        trackService.TryGetById(2, out Track? track);
        Console.WriteLine($"Трек: {track!.Title}, текущая цена: {track.Price:0.00} BYN");
        // track.Price = 0;  // ошибка компиляции CS0272: set-метод недоступен (private set)

        track.UpdatePrice(2.49m);   // корректное значение
        track.UpdatePrice(-5m);     // отрицательная цена — отклоняется
        track.UpdatePrice(100m);    // слишком высокая цена — отклоняется
        Console.WriteLine($"Итоговая цена: {track.Price:0.00} BYN");
        Console.WriteLine();

        Title("Б1, У2. Полиморфизм: массив базового типа BaseEntity[]");

        BaseEntity[] entities =
        {
            trackService.GetAll()[0],
            albums[1],
            artists[2],
            playlists[0]
        };

        // Один и тот же вызов PrintInfo() — разное поведение у каждого класса
        foreach (BaseEntity entity in entities)
            entity.PrintInfo();

        Title("Полиморфизм через интерфейс IPlayable");
        List<IPlayable> playables = new() { trackService.GetAll()[3], playlists[1] };
        foreach (IPlayable item in playables)
            item.Play();
    }

    // Уровень 3: интерфейс сервиса, полиморфный вызов, перегрузка, protected
    private static void Block1Level3()
    {
        Title("Б1, У3. Работа с сервисом через интерфейс IItemService<Track>");

        // Полиморфный вызов: переменная интерфейсного типа, объект — TrackService
        IItemService<Track> service = trackService;
        Console.WriteLine($"Тип объекта: {service.GetType().Name}, тип ссылки: IItemService<Track>");
        Console.WriteLine($"Всего треков в сервисе: {service.Count}");

        Track newTrack = new Track(8, "Yellow", "Coldplay", "Alternative", 269, 1.49m, 1_800_000_000) { ReleaseYear = 2000 };
        service.Add(newTrack);
        Console.WriteLine($"Добавлен трек «{newTrack.Title}». Теперь треков: {service.Count}");

        Console.WriteLine("Поиск через интерфейс: Search(\"cold\")");
        foreach (Track t in service.Search("cold"))
            Console.WriteLine($"   {t}");

        service.Remove(8);
        Console.WriteLine($"Трек с Id = 8 удалён. Треков: {service.Count}");
        Console.WriteLine();

        Title("Перегрузка методов Search() в TrackService");

        Console.WriteLine("1) Search(string query) — \"the\":");
        foreach (Track t in trackService.Search("the")) Console.WriteLine($"   {t}");

        Console.WriteLine("2) Search(string genre, long minPlays) — Pop, от 3 млрд:");
        foreach (Track t in trackService.Search("Pop", 3_000_000_000)) Console.WriteLine($"   {t}");

        Console.WriteLine("3) Search(TimeSpan maxDuration) — не длиннее 3:30:");
        foreach (Track t in trackService.Search(TimeSpan.FromSeconds(210))) Console.WriteLine($"   {t}");

        Console.WriteLine("4) Search(int fromYear, int toYear) — 2010–2020:");
        foreach (Track t in trackService.Search(2010, 2020)) Console.WriteLine($"   {t}");
        Console.WriteLine();

        Title("protected-члены в иерархии классов");
        Console.WriteLine("• Id { get; protected set; } — задаётся только в конструкторе BaseEntity/наследников;");
        Console.WriteLine("• protected FormatDuration() — используется в Track, Album, Playlist для вывода длительности;");
        Console.WriteLine("• protected viewCount — счётчик просмотров карточки, увеличивается в PrintInfo().");
        Console.WriteLine();

        Album album = albums[0];
        album.PrintInfo();
        album.PrintInfo();
        Console.WriteLine($"Карточку альбома «{album.Title}» просмотрели {album.GetViewCount()} раз(а) (protected viewCount).");
    }

    // ======================= БЛОК 2 =======================

    // Уровень 1: List<T> + foreach
    private static void Block2Level1()
    {
        Title("Б2, У1. Коллекция List<Track> — перебор foreach");

        IReadOnlyList<Track> tracks = trackService.GetAll();
        Console.WriteLine($"В медиатеке {tracks.Count} треков:");
        Console.WriteLine();
        Console.WriteLine($"{"ID",-4}{"Исполнитель",-18}{"Название",-20}{"Жанр",-13}{"Время",6}");
        Console.WriteLine(new string('-', 61));

        foreach (Track track in tracks)
        {
            Console.WriteLine($"{track.Id,-4}{track.ArtistName,-18}{track.Title,-20}{track.Genre,-13}" +
                              $"{track.Duration.ToString(@"m\:ss"),6}");
        }
    }

    // Уровень 2: Dictionary + TryGetValue
    private static void Block2Level2()
    {
        Title("Б2, У2. Dictionary<int, Track> — поиск по ID");

        IReadOnlyDictionary<int, Track> dict = trackService.GetDictionary();
        Console.WriteLine($"Ключи словаря: {string.Join(", ", dict.Keys)}");
        Console.Write("Введите ID трека: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Ошибка: ID должен быть целым числом.");
            return;
        }

        if (trackService.TryGetById(id, out Track? track))
        {
            Console.WriteLine("Трек найден:");
            track!.PrintInfo();
        }
        else
        {
            Console.WriteLine($"Трек с ID = {id} не найден в медиатеке.");
        }
    }

    // Уровень 3: LINQ Where, OrderBy, Select
    private static void Block2Level3Queries()
    {
        Title("Б2, У3. LINQ — Where (фильтрация)");
        IReadOnlyList<Track> tracks = trackService.GetAll();

        List<Track> hits = tracks
            .Where(t => t.PlayCount >= 2_800_000_000)
            .ToList();
        Console.WriteLine("Хиты (от 2,8 млрд прослушиваний):");
        foreach (Track t in hits)
            Console.WriteLine($"   {t.ArtistName} — {t.Title}: {t.PlayCount:N0}");
        Console.WriteLine();

        Title("LINQ — OrderByDescending / ThenBy (сортировка)");
        var sorted = tracks
            .OrderByDescending(t => t.PlayCount)
            .ThenBy(t => t.Title)
            .ToList();
        int place = 1;
        foreach (Track t in sorted)
            Console.WriteLine($"   {place++}. {t.Title} ({t.ArtistName}) — {t.PlayCount:N0}");
        Console.WriteLine();

        Title("LINQ — OrderBy + Select (проекция полей)");
        var shortInfo = tracks
            .OrderBy(t => t.ReleaseYear)
            .Select(t => new
            {
                t.ReleaseYear,
                Name = $"{t.ArtistName} — {t.Title}",
                Minutes = Math.Round(t.Duration.TotalMinutes, 1)
            });
        foreach (var item in shortInfo)
            Console.WriteLine($"   {item.ReleaseYear} | {item.Name} | {item.Minutes} мин");
    }

    // Уровень 3: агрегаты и GroupBy
    private static void Block2Level3Statistics()
    {
        Title("Б2, У3. LINQ — агрегатные функции");
        IReadOnlyList<Track> tracks = trackService.GetAll();

        int count = tracks.Count();
        long totalPlays = tracks.Sum(t => t.PlayCount);
        double avgDuration = tracks.Average(t => t.DurationSeconds);
        decimal minPrice = tracks.Min(t => t.Price);
        decimal maxPrice = tracks.Max(t => t.Price);
        Track longest = tracks.MaxBy(t => t.DurationSeconds)!;

        Console.WriteLine($"Количество треков (Count):            {count}");
        Console.WriteLine($"Всего прослушиваний (Sum):            {totalPlays:N0}");
        Console.WriteLine($"Средняя длительность (Average):       {TimeSpan.FromSeconds(avgDuration):m\\:ss}");
        Console.WriteLine($"Минимальная цена (Min):               {minPrice:0.00} BYN");
        Console.WriteLine($"Максимальная цена (Max):              {maxPrice:0.00} BYN");
        Console.WriteLine($"Самый длинный трек (MaxBy):           {longest.Title} ({longest.Duration:m\\:ss})");
        Console.WriteLine($"Треков дольше 4 минут (Count + фильтр): {tracks.Count(t => t.DurationSeconds > 240)}");
        Console.WriteLine();

        Title("LINQ — GroupBy (группировка по жанрам)");
        var byGenre = tracks
            .GroupBy(t => t.Genre)
            .Select(g => new
            {
                Genre = g.Key,
                Count = g.Count(),
                Plays = g.Sum(t => t.PlayCount),
                AvgPrice = g.Average(t => t.Price)
            })
            .OrderByDescending(g => g.Plays);

        Console.WriteLine($"{"Жанр",-13}{"Треков",7}{"Прослушиваний",18}{"Ср. цена",11}");
        Console.WriteLine(new string('-', 49));
        foreach (var g in byGenre)
            Console.WriteLine($"{g.Genre,-13}{g.Count,7}{g.Plays,18:N0}{g.AvgPrice,11:0.00}");
        Console.WriteLine();

        Title("GroupBy — треки по десятилетиям");
        foreach (var decade in tracks.GroupBy(t => t.ReleaseYear / 10 * 10).OrderBy(g => g.Key))
        {
            Console.WriteLine($"{decade.Key}-е ({decade.Count()}): " +
                              string.Join(", ", decade.Select(t => t.Title)));
        }
    }

    // ======================= БЛОК 3 =======================

    // Уровень 1: async Task + await Task.Delay
    private static async Task Block3Level1()
    {
        Title("Б3, У1. Асинхронный метод async Task");
        DateTime start = DateTime.Now;
        await mediaService.SyncLibraryAsync();   // await без .Result / .Wait()
        Console.WriteLine($"Время выполнения: {(DateTime.Now - start).TotalMilliseconds:0} мс");
    }

    // Уровень 2: async Task<List<T>> + try-catch-finally
    private static async Task Block3Level2()
    {
        Title("Б3, У2. Успешная загрузка: async Task<List<Track>>");
        List<Track> loaded = await mediaService.LoadTracksFromApiAsync();
        foreach (Track t in loaded)
            Console.WriteLine($"   {t}");
        Console.WriteLine();

        Title("Б3, У2. Загрузка с ошибкой (обработка try-catch-finally)");
        List<Track> failed = await mediaService.LoadTracksFromApiAsync(simulateError: true);
        Console.WriteLine($"Получено треков: {failed.Count}");
    }

    // Уровень 3: Task.WhenAll
    private static async Task Block3Level3Parallel()
    {
        Title("Б3, У3. Параллельный анализ плейлистов — Task.WhenAll()");

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        DateTime start = DateTime.Now;

        Task<TimeSpan> task1 = mediaService.AnalyzePlaylistAsync(playlists[0], cts.Token);
        Task<TimeSpan> task2 = mediaService.AnalyzePlaylistAsync(playlists[1], cts.Token);
        Task<TimeSpan> task3 = mediaService.AnalyzePlaylistAsync(playlists[2], cts.Token);

        TimeSpan[] results = await Task.WhenAll(task1, task2, task3);

        TimeSpan total = results.Aggregate(TimeSpan.Zero, (sum, d) => sum + d);
        Console.WriteLine();
        Console.WriteLine("Все параллельные задачи завершены.");
        Console.WriteLine($"Суммарная длительность трёх плейлистов: {total:h\\:mm\\:ss}");
        Console.WriteLine($"Общее время обработки: {(DateTime.Now - start).TotalMilliseconds:0} мс " +
                          "(меньше суммы задержек — задачи шли параллельно)");
    }

    // Уровень 3: CancellationToken
    private static async Task Block3Level3Cancel()
    {
        Title("Б3, У3. Отмена длительной операции — CancellationToken");

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(1500); // автоматическая отмена через 1,5 секунды
        Console.WriteLine("Операция будет отменена через 1,5 с (CancelAfter).");

        try
        {
            await mediaService.ConvertLibraryAsync(trackService.GetAll(), cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("[ОТМЕНА] Операция перекодирования отменена по токену (OperationCanceledException).");
        }
        finally
        {
            Console.WriteLine($"Статус токена: IsCancellationRequested = {cts.IsCancellationRequested}");
        }
    }
}
