using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

class DataManager
{
    private const string JsonFilePath = "Data.json";

    public static ReservationHandler LoadReservationsFromJson()
    {
        if (File.Exists(JsonFilePath))
        {
            string json = File.ReadAllText(JsonFilePath);
            return JsonConvert.DeserializeObject<ReservationHandler>(json);
        }
        else
        {
            return new ReservationHandler();
        }
    }

    public static void SaveReservationsToJson(ReservationHandler handler)
    {
        string json = JsonConvert.SerializeObject(handler, Formatting.Indented);
        File.WriteAllText(JsonFilePath, json);
    }
}

public class Room
{
    public string? RoomId { get; set; } // '?' ekleyerek null atanabilir olarak belirtiyorum
    public string? RoomName { get; set; } 
    public string? Capacity { get; set; } 
}

public class ReservationHandler
{
    public List<Reservation> Reservations { get; set; }

    public ReservationHandler()
    {
        Reservations = new List<Reservation>();
    }

    public void AddReservation(Reservation reservation)
    {
        Reservations.Add(reservation);
    }

    public void DeleteReservation(Reservation reservation)
    {
        Reservations.Remove(reservation);
    }

    public void DisplayWeeklySchedule()
    {
        Console.WriteLine("Weekly schedule:");
        foreach (var reservation in Reservations)
        {
            Console.WriteLine(reservation);
        }
    }
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

    public override string ToString()
    {
        return $"{DateTime}: {Room.RoomName} - {ReserverName}";
    }
}

class Program
{
    static void Main()
    {
        ReservationHandler handler = DataManager.LoadReservationsFromJson();

        while (true)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add reservation");
            Console.WriteLine("2. Delete reservation");
            Console.WriteLine("3. Display weekly schedule");
            Console.WriteLine("4. Exit");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    AddReservation(handler);
                    break;
                case "2":
                    DeleteReservation(handler);
                    break;
                case "3":
                    handler.DisplayWeeklySchedule();
                    break;
                case "4":
                    DataManager.SaveReservationsToJson(handler);
                    return;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option.");
                    break;
            }
        }
    }

    static void AddReservation(ReservationHandler handler)
    {
        Console.WriteLine("Enter room ID:");
        string roomId = Console.ReadLine();

        Console.WriteLine("Enter room name:");
        string roomName = Console.ReadLine();

        Console.WriteLine("Enter room capacity:");
        string capacity = Console.ReadLine();

        Room room = new Room { RoomId = roomId, RoomName = roomName, Capacity = capacity };

        Console.WriteLine("Enter reservation date and time (yyyy-MM-dd HH:mm):");
        DateTime dateTime;
        while (!DateTime.TryParse(Console.ReadLine(), out dateTime))
        {
            Console.WriteLine("Invalid date and time format. Please enter in the correct format (yyyy-MM-dd HH:mm):");
        }

        Console.WriteLine("Enter reserver name:");
        string reserverName = Console.ReadLine();

        handler.AddReservation(new Reservation(room, dateTime, reserverName));

        Console.WriteLine("Reservation added successfully.");
    }

    static void DeleteReservation(ReservationHandler handler)
    {
        Console.WriteLine("Enter reservation index to delete:");
        int index;
        while (!int.TryParse(Console.ReadLine(), out index) || index < 0 || index >= handler.Reservations.Count)
        {
            Console.WriteLine("Invalid reservation index. Please enter a valid index:");
        }

        handler.DeleteReservation(handler.Reservations[index]);
        Console.WriteLine("Reservation deleted successfully.");
    }
}
