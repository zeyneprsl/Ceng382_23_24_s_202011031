using System;
public class Room
{
    private string? RoomId;
    private string? RoomName;
    private string? Capacity;

    public Room(string? roomId, string? roomName, string? capacity)
    {
        RoomId = roomId;
        RoomName = roomName;
        Capacity = capacity;
    }

    public string? GetRoomId()
    {
        return  RoomId;
    }

    public void SetRoomId(string? roomId)
    {
        RoomId = roomId;
    }

    public string? GetRoomName()
    {
        return RoomName;
    }

    public void SetRoomName(string? roomName)
    {
        RoomName = roomName;
    }

    public string? GetCapacity()
    {
        return Capacity;
    }

    public void SetCapacity(string? capacity)
    {
        Capacity = capacity;
    }
}

