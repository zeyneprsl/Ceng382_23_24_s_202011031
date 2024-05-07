using System;
public class Room
{
    private string? _roomId;
    private string? _roomName;
    private string? _capacity;

    public Room(string? roomId, string? roomName, string? capacity)
    {
        _roomId = roomId;
        _roomName = roomName;
        _capacity = capacity;
    }

    public string? GetRoomId()
    {
        return _roomId;
    }

    public void SetRoomId(string? roomId)
    {
        _roomId = roomId;
    }

    public string? GetRoomName()
    {
        return _roomName;
    }

    public void SetRoomName(string? roomName)
    {
        _roomName = roomName;
    }

    public string? GetCapacity()
    {
        return _capacity;
    }

    public void SetCapacity(string? capacity)
    {
        _capacity = capacity;
    }
}

