namespace Managers
{
    public struct LogData
    {
        public long        _ID          { get; set; }
        public string      _Message     { get; set; }
        public OutputLevel _OutputLevel { get; set; }

        public LogData(OutputLevel outputLevel, string message)
        {
            _ID          = LogManager._Instance._LogIDCount;
            _Message     = message;
            _OutputLevel = outputLevel;
        }
    }
}