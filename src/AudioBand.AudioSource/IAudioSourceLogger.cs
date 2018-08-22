namespace AudioBand.AudioSource
{
    public interface IAudioSourceLogger
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}
