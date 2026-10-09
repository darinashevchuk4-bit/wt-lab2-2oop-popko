namespace MusicStreaming.Models;

/// <summary>
/// Исполнитель (артист или группа).
/// </summary>
public class Artist : BaseEntity
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string MainGenre { get; set; }

    // Число подписчиков меняется только через Subscribe/Unsubscribe
    public long Followers { get; private set; }

    public Artist(int id, string name, string country, string mainGenre, long followers = 0)
        : base(id)
    {
        Name = name;
        Country = country;
        MainGenre = mainGenre;
        Followers = followers;
    }

    public override string GetEntityType() => "Исполнитель";

    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"Имя: {Name}");
        Console.WriteLine($"Страна: {Country}");
        Console.WriteLine($"Основной жанр: {MainGenre}");
        Console.WriteLine($"Подписчики: {Followers:N0}");
        Console.WriteLine();
    }

    public void Subscribe()
    {
        Followers++;
        Console.WriteLine($"Вы подписались на {Name}. Подписчиков: {Followers:N0}");
    }

    public void Unsubscribe()
    {
        if (Followers == 0)
        {
            Console.WriteLine("Ошибка: у исполнителя нет подписчиков.");
            return;
        }
        Followers--;
        Console.WriteLine($"Вы отписались от {Name}. Подписчиков: {Followers:N0}");
    }
}
