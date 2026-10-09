using MusicStreaming.Interfaces;

namespace MusicStreaming.Models;

/// <summary>
/// Абстрактный базовый класс для всех сущностей музыкального сервиса.
/// Содержит общие свойства (Id, CreatedAt) и protected-члены,
/// доступные только классам-наследникам.
/// </summary>
public abstract class BaseEntity : IEntity
{
    // Id можно задать только внутри иерархии классов (protected set)
    public int Id { get; protected set; }

    // Дату создания нельзя изменить снаружи (private set)
    public DateTime CreatedAt { get; private set; }

    // protected-поле: счётчик обращений, виден только наследникам
    protected int viewCount;

    protected BaseEntity(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id должен быть положительным числом.", nameof(id));

        Id = id;
        CreatedAt = DateTime.Now;
    }

    // Абстрактный метод — каждый наследник обязан его реализовать
    public abstract string GetEntityType();

    // Виртуальный метод — наследники переопределяют его через override
    public virtual void PrintInfo()
    {
        viewCount++;
        PrintHeader();
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Тип: {GetType().Name}");
        Console.WriteLine($"Дата создания: {CreatedAt:dd.MM.yyyy HH:mm:ss}");
    }

    // protected-метод: печать заголовка карточки
    protected void PrintHeader()
    {
        Console.WriteLine($"----- {GetEntityType().ToUpper()} -----");
    }

    // protected static-метод: форматирование длительности в мм:сс / чч:мм:сс
    protected static string FormatDuration(TimeSpan duration)
    {
        return duration.TotalHours >= 1
            ? duration.ToString(@"h\:mm\:ss")
            : duration.ToString(@"m\:ss");
    }

    public int GetViewCount() => viewCount;
}
