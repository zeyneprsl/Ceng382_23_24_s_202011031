using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class DataManager : ReservationHandler
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

    public static List<Reservation> DisplayReservationByReserver(string name)
    {
        return _ReservationHandler.Reservations.Where(r => r.ReserverName == name).ToList();
    }

    public static List<Reservation> DisplayReservationByRoomId(string Id)
    {
        return _ReservationHandler.Reservations.Where(r => r.Room.GetRoomId() == Id).ToList();
    }
}

public class LogService
{
    private List<LogRecord> _logs;

    public LogService(List<LogRecord> logs)
    {
        _logs = logs;
    }

    public static List<LogRecord> DisplayLogsByName(string name)
    {
        return _logs.Where(log => log.Message.Contains(name)).ToList();
    }

    public static List<LogRecord> DisplayLogs(DateTime start, DateTime end)
    {
        return _logs.Where(log => log.DateTime >= start && log.DateTime <= end).ToList();
    }
}

class Program
{
    static void Main()
    {
        ReservationHandler handler = DataManager.LoadReservationsFromJson();
        LogHandler logHandler = new LogHandler(new ConsoleLogger());

        while (true)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add reservation");
            Console.WriteLine("2. Delete reservation");
            Console.WriteLine("3. Display weekly schedule");
            Console.WriteLine("4. Display reservations by reserver name");
            Console.WriteLine("5. Display reservations by room ID");
            Console.WriteLine("6. Display logs by name");
            Console.WriteLine("7. Display logs by time interval");
            Console.WriteLine("8. Exit");

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
                    Console.WriteLine("Enter reserver name:");
                    string? reserverName = Console.ReadLine();
                    DisplayReservationsByReserverName(reserverName);
                    break;
                case "5":
                    Console.WriteLine("Enter room ID:");
                    string? roomId = Console.ReadLine();
                    DisplayReservationsByRoomId(roomId);
                    break;
                case "6":
                    Console.WriteLine("Enter username to search logs:");
                    string? username = Console.ReadLine();
                    DisplayLogsByUsername(username);
                    break;
                case "7":
                    Console.WriteLine("Enter start date and end date (yyyy-MM-dd HH:mm):");
                    DateTime start, end;
                    while (!DateTime.TryParse(Console.ReadLine(), out start))
                    {
                        Console.WriteLine("Invalid start date format. Please enter in the correct format (yyyy-MM-dd HH:mm):");
                    }
                    while (!DateTime.TryParse(Console.ReadLine(), out end))
                    {
                        Console.WriteLine("Invalid end date format. Please enter in the correct format (yyyy-MM-dd HH:mm):");
                    }
                    DisplayLogsByTimeInterval(start, end);
                    break;
                case "8":
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
        roomId = room.GetRoomId();
        roomName = room.GetRoomName();
        capacity = room.GetCapacity();

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

    static void DisplayReservationsByReserverName(string name)
    {
        List<Reservation> reservations = ReservationService.DisplayReservationByReserver(name);
        foreach (var reservation in reservations)
        {
            Console.WriteLine(reservation);
        }
    }

    static void DisplayReservationsByRoomId(string id)
    {
        List<Reservation> reservations = ReservationService.DisplayReservationByRoomId(id);
        foreach (var reservation in reservations)
        {
            Console.WriteLine(reservation);
        }
    }

    static void DisplayLogsByUsername(string name)
    {
        List<LogRecord> logs = LogService.DisplayLogsByName(name);
        foreach (var log in logs)
        {
            Console.WriteLine(log);
        }
    }

    static void DisplayLogsByTimeInterval(DateTime start, DateTime end)
    {
        List<LogRecord> logs = LogService.DisplayLogs(start, end);
        foreach (var log in logs)
        {
            Console.WriteLine(log);
        }
    }
}

