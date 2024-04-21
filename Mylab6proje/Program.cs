using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class RoomData
{
    [JsonPropertyName("Room")]
    public Room[] Rooms{get; set;}
}
public class Room
{
    [JsonPropertyName("roomId")]
    public string roomId {get; set;}


    [JsonPropertyName("roomName")]
    public string roomName {get; set;}

    [JsonPropertyName("capacity")]
    public int capacity{get; set;}
}
public class Reservation
{
    public Room Room { get; set; }
    public DateTime DateTime { get; set; }
    public string ReserverName { get; set; }

    public Reservation(Room room, DateTime dateTime, string reserverName)
    {
        Room = room;
        DateTime = dateTime;
        ReserverName = reserverName;
    }
}

public class ReservationHandler
{
    private List<Reservation> reservations = new List<Reservation>();

    public void AddReservation(Reservation reservation)
    {
        reservations.Add(reservation);
    }

    public void DeleteReservation(Reservation reservation)
    {
        reservations.Remove(reservation);
    }

    public void DisplayWeeklySchedule()
    {
        var nextWeek = DateTime.Now.AddDays(7);

        var weeklySchedule = reservations
            .Where(r => r.DateTime <= nextWeek)
            .OrderBy(r => r.DateTime);

        Console.WriteLine("This week's schedule:");

        foreach (var reservation in weeklySchedule)
        {
            Console.WriteLine($"Room: {reservation.Room.roomName}, Date: {reservation.DateTime}, Reserved by: {reservation.ReserverName}");
        }
    }
}
class Program
{
    static void Main(String[]args)
    {
        //define file path
        string filePath = "Mylab6proje\\Data.json";

        //Read from json
        // 1 -> json to text // todo try catch 
        string jsonString = File.ReadAllText(filePath);
        // 2 -> decode text into meaningful classes
        var roomData = JsonSerializer.Deserialize<RoomData>(
                                jsonString, 
                                new JsonSerializerOptions()
        {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | 
                JsonNumberHandling.WriteAsString
        });

        //print
        if(roomData?.Rooms != null)
        {
            foreach (var room in roomData.Rooms)
            {
                Console.WriteLine($"Room ID : {room.roomId} RoomName : {room.roomName} Capacity : {room.capacity}");
            }
        }

    }
}
