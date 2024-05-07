public class LogRecord
{
    public DateTime Timestamp { get; set; }
    public string ReserveName { get; set; }
    public string RoomName { get; set; }

    public LogRecord(DateTime timestamp, string reserveName, string roomName)
    {
        Timestamp = timestamp;
        ReserveName = reserveName;
        RoomName = roomName;
    }

    public override string ToString()
    {
        return $"[{Timestamp}] - Reservation by: {ReserveName}, Room: {RoomName}";
    }
}
