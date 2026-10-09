# wt-lab2-2oop-popko

Лабораторная работа №2 «Классы. Свойства. Методы. Экземпляры объектов»
Дисциплина «Web-технологии», вариант 13 — **Музыкальный стриминг / Медиатека**.

Выполнила: Попко Д.А., группа 1д2401ГУП.

## Сущности
`Track`, `Album`, `Artist`, `Playlist` (наследуются от абстрактного `BaseEntity`).

## Структура
```
Interfaces/  IEntity, IPlayable, IItemService<T>
Models/      BaseEntity, Track, Album, Artist, Playlist
Services/    TrackService, MediaLibraryService, DataSeeder
Program.cs   приветствие, меню, демонстрация блоков 1–3
```

## Запуск
```
dotnet run
```
