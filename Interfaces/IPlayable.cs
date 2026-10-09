namespace MusicStreaming.Interfaces;

/// <summary>
/// Контракт для всего, что можно воспроизвести (трек, альбом, плейлист).
/// </summary>
public interface IPlayable
{
    TimeSpan Duration { get; }

    void Play();
}
