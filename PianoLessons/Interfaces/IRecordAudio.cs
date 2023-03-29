namespace PianoLessons.Interfaces;

public interface IRecordAudio
{
    void StartRecord();
    string StopRecord();
    void PauseRecord();
    void ResetRecord();
}
