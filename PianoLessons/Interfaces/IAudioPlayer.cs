
namespace PianoLessons.Interfaces;

public interface IAudioPlayer
{
    void PlayAudio(string filePath);
    void Pause();
    void Stop();
    string GetCurrentPlayTime();
    bool CheckFinishedPlayingAudio();
}
