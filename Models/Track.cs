using MusicStreaming.Interfaces;

namespace MusicStreaming.Models;

/// <summary>
/// Музыкальный трек — основной класс предметной области.
/// </summary>
public class Track : BaseEntity, IPlayable
{
    // Автосвойства с открытыми get/set
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string AlbumTitle { get; set; } = "Сингл";
    public string Genre { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsExplicit { get; set; }

    // Инкапсуляция: менять напрямую нельзя, только через методы с проверкой
    public int DurationSeconds { get; private set; }
    public long PlayCount { get; private set; }
    public decimal Price { get; private set; }

    public TimeSpan Duration => TimeSpan.FromSeconds(DurationSeconds);

    // Явный конструктор с параметрами
    public Track(int id, string title, string artistName, string genre,
                 int durationSeconds, decimal price, long playCount = 0)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название трека не может быть пустым.", nameof(title));
        if (durationSeconds <= 0)
            throw new ArgumentException("Длительность должна быть больше 0.", nameof(durationSeconds));
        if (price < 0)
            throw new ArgumentException("Цена не может быть отрицательной.", nameof(price));

        Title = title;
        ArtistName = artistName;
        Genre = genre;
        DurationSeconds = durationSeconds;
        Price = price;
        PlayCount = playCount;
    }

    public override string GetEntityType() => "Трек";

    // Переопределение виртуального метода базового класса
    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"Название: {Title}");
        Console.WriteLine($"Исполнитель: {ArtistName}");
        Console.WriteLine($"Альбом: {AlbumTitle} ({ReleaseYear})");
        Console.WriteLine($"Жанр: {Genre}");
        Console.WriteLine($"Длительность: {FormatDuration(Duration)}");
        Console.WriteLine($"Прослушивания: {PlayCount:N0}");
        Console.WriteLine($"Цена: {Price:0.00} BYN");
        Console.WriteLine($"Возрастная метка: {(IsExplicit ? "18+" : "нет")}");
        Console.WriteLine();
    }

    public void Play()
    {
        PlayCount++;
        Console.WriteLine($"► Воспроизводится: {ArtistName} — {Title} [{FormatDuration(Duration)}]");
    }

    /// <summary>
    /// Изменение цены с валидацией (цена закрыта private set).
    /// </summary>
    public bool UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
        {
            Console.WriteLine("Ошибка: цена не может быть отрицательной.");
            return false;
        }
        if (newPrice > 50)
        {
            Console.WriteLine("Ошибка: цена трека не может превышать 50 BYN.");
            return false;
        }

        decimal oldPrice = Price;
        Price = newPrice;
        Console.WriteLine($"Цена трека «{Title}» изменена: {oldPrice:0.00} → {Price:0.00} BYN");
        return true;
    }

    /// <summary>
    /// Добавление прослушиваний с валидацией.
    /// </summary>
    public void AddPlays(long count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Количество прослушиваний должно быть больше 0.");
        PlayCount += count;
    }

    public override string ToString() =>
        $"[{Id}] {ArtistName} — {Title} ({Genre}, {FormatDuration(Duration)}, {PlayCount:N0} просл.)";
}
