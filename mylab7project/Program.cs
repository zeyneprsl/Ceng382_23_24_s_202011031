using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class DataManager:ReservationHandler
{
    private const string JsonFilePath = "Data.json";
    
    public static ReservationHandler LoadReservationsFromJson()
{
    if (File.Exists(JsonFilePath))
    {
        string? json = File.ReadAllText(JsonFilePath);
        if (json != null)
        {
            return JsonConvert.DeserializeObject<ReservationHandler>(json)!;
        }
    }
    // If file doesn't exist or json is null, return a new instance of ReservationHandler
    return new ReservationHandler();
}

    public static void SaveReservationsToJson(ReservationHandler handler)
    {
        string json = JsonConvert.SerializeObject(handler, Formatting.Indented);
        File.WriteAllText(JsonFilePath, json);
    }
}

public interface IReservationRepository
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(Reservation reservation);
    List<Reservation> GetAllReservations();
}

public class RoomHandler
{
    private string _filePath;

    public RoomHandler(string filePath)
    {
        _filePath = filePath;
    }

   public List<Room> GetRooms()
{
    List<Room> rooms = new List<Room>();
    if (File.Exists(_filePath))
    {
        string? json = File.ReadAllText(_filePath);
        if (json != null)
        {
            rooms = JsonConvert.DeserializeObject<List<Room>>(json)!;
        }
    }
    return rooms;
}

    public void SaveRooms(List<Room> rooms)
    {
        string json = JsonConvert.SerializeObject(rooms);
        File.WriteAllText(_filePath, json);
    }
}

public class ReservationRepository : IReservationRepository
{
    private List<Reservation> _reservations;

    public ReservationRepository()
    {
        _reservations = new List<Reservation>();
    }

    public void AddReservation(Reservation reservation)
    {
        _reservations.Add(reservation);
    }

    public void DeleteReservation(Reservation reservation)
    {
        _reservations.Remove(reservation);
    }

    public List<Reservation> GetAllReservations()
    {
        return _reservations;
    }
}

public class ReservationHandler : Room
{
    public List<Reservation> Reservations { get; set; }
    public List<Room> Rooms { get; set; }
    private readonly IReservationRepository _reservationRepository;
    private readonly RoomHandler _roomHandler;
    private readonly LogHandler _logHandler;

    public ReservationHandler(IReservationRepository reservationRepository, RoomHandler roomHandler, LogHandler logHandler)
    {
        _reservationRepository = reservationRepository;
        _roomHandler = roomHandler;
        _logHandler = logHandler;
        Reservations = new List<Reservation>();
        Rooms = new List<Room>();
    }

    public void AddReservation(Reservation reservation)
    {
        Reservations.Add(reservation);
        _reservationRepository.AddReservation(reservation);
        _logHandler.AddLog(new LogRecord { DateTime.Now, Message = $"Reservation added: {reservation}" });
    }

    public void DeleteReservation(Reservation reservation)
    {
        Reservations.Remove(reservation);
        _reservationRepository.DeleteReservation(reservation);
    }

    public void DisplayWeeklySchedule()
    {
        Console.WriteLine("Weekly schedule:");
        foreach (var reservation in Reservations)
        {
            Console.WriteLine(reservation);
        }
    }

    public List<Reservation> GetAllReservations()
    {
        return Reservations;
    }

    public List<Room> GetRooms()
    {
        return Rooms;
    }

    public void SaveRooms(List<Room> rooms)
    {
        Rooms = rooms;
    }
}

public interface ILogger
{
    void Log(LogRecord log);
}

public class LogHandler
{
    private readonly ILogger _logger;

    public LogHandler(ILogger logger)
    {
        _logger = logger;
    }

    public void AddLog(LogRecord log)
    {
        _logger.Log(log);
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(LogRecord log)
    {
       Console.WriteLine(log.ToString());
    }
}

public class Reservation
{
    public Room Room { get; set; }
    public DateTime Time { get; private set; }
    public string ReserverName { get; private set; }

    public Reservation(Room room, DateTime time, string reserverName)
    {
        Room = room;
        Time = time;
        ReserverName = reserverName;
    }

    // Getters and setters for Time and ReserverName (optional)

    public DateTime GetTime()
    {
        return Time;
    }

    public void SetTime(DateTime time)
    {
        Time = time;
    }

    public string GetReserverName()
    {
        return ReserverName;
    }

    public void SetReserverName(string reserverName)
    {
        ReserverName = reserverName;
    }
}
public class ReservationService : IReservationService
{
    private ReservationHandler _ReservationHandler;

    public ReservationService(ReservationHandler reservationHandler)
    {
        _ReservationHandler = reservationHandler;
    }

    public void AddReservation(Reservation reservation)
    {
        _ReservationHandler.AddReservation(reservation);
    }

    public void DeleteReservation(Reservation reservation)
    {
        _ReservationHandler.DeleteReservation(reservation);
    }

    public void DisplayWeekSchedule()
    {
        _ReservationHandler.DisplayWeeklySchedule();
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

            string? userInput = Console.ReadLine();

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
        string? roomId = Console.ReadLine();

        Console.WriteLine("Enter room name:");
        string? roomName = Console.ReadLine();

        Console.WriteLine("Enter room capacity:");
        string? capacity = Console.ReadLine();
        Room room = new Room();
        room.SetRoomId(roomId);
        room.SetRoomName(roomName);
        room.SetCapacity(capacity);
        roomId=room.GetRoomId();
        roomName=room.GetRoomName();
        capacity=room.GetCapacity();
        Console.WriteLine("Enter reservation date and time (yyyy-MM-dd HH:mm):");
       DateTime dateTime;
    while (!DateTime.TryParse(Console.ReadLine(), out dateTime))
    {
        Console.WriteLine("Invalid date and time format. Please enter in the correct format (yyyy-MM-dd HH:mm):");
    }

    Console.WriteLine("Enter reserver name:");
    string? reserverName = Console.ReadLine();

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