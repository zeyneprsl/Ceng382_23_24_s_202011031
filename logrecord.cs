public class LogRecord
{
    private DateTime _timestamp;
    private string _reserveName;
    private string _roomName;
    public string Message;

    public LogRecord(DateTime timestamp, string reserveName, string roomName,string message)
    {
        _timestamp = timestamp;
        _reserveName = reserveName;
        _roomName = roomName;
        Message=message;
    }

    public DateTime GetTimestamp()
    {
        return _timestamp;
    }

    public void SetTimestamp(DateTime timestamp)
    {
        _timestamp = timestamp;
    }

    public string GetReserveName()
    {
        return _reserveName;
    }

    public void SetReserveName(string reserveName)
    {
        _reserveName = reserveName;
    }

    public string GetRoomName()
    {
        return _roomName;
    }

    public void SetRoomName(string roomName)
    {
        _roomName = roomName;
    }

    public static LogRecord CreateLogRecord(DateTime timestamp, string reserveName, string roomName,string message)
    {
        return new LogRecord(timestamp, reserveName, roomName,message);
    }

    public override string ToString()
    {
        return $"[{_timestamp}] - Reservation by: {_reserveName}, Room: {_roomName}";
    }
}