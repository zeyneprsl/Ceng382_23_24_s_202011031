using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Room
{
    public string RoomId { get; set; } = "";
    public string RoomName { get; set; }="";

    public int Capacity { get; set; }

    public static implicit operator Room(string v)
    {
        throw new NotImplementedException();
    }
}

public class Reservation
{
    public Room Room { get; set; }="";
    public DateTime DateTime { get; set; }
    public string ReserverName { get; set; }="";
}

public class ReservationHandler
{
    private List<Reservation> reservations;

    public ReservationHandler()
    {
        reservations = new List<Reservation>();
    }

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
            Console.WriteLine($"Room: {reservation.Room.RoomName}, Date: {reservation.DateTime}, Reserved by: {reservation.ReserverName}");
        }
    }
}

public partial class Program
{
    static void Main(string[] args)
    {
        List<Room> rooms = LoadRoomsFromJson("Data.json");

        ReservationHandler handler = new ReservationHandler();

        Room room = rooms.FirstOrDefault();
        if (room != null)
        {
            Reservation reservation = new Reservation
            {
                Room = room,
                DateTime = DateTime.Now.AddDays(1),
                ReserverName = "John Doe"
            };
            handler.AddReservation(reservation);
        }

        handler.DisplayWeeklySchedule();
    }

    static List<Room> LoadRoomsFromJson(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath);
            var rooms = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Room>>(json);
            return rooms ?? new List<Room>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading rooms from JSON: {ex.Message}");
            return new List<Room>();
        }
    }
}