// Room class definition
public class Room
{
    private string _roomId;
    private string _roomName;
    private int _capacity;

    public string RoomId
    {
        get { return _roomId; }
        protected set
        {
            if (value != null)
                _roomId = value;
            else
                throw new ArgumentNullException(nameof(value), "RoomId cannot be null.");
        }
    }

    public string RoomName
    {
        get { return _roomName; }
        protected set
        {
            if (value != null)
                _roomName = value;
            else
                throw new ArgumentNullException(nameof(value), "RoomName cannot be null.");
        }
    }

    public int Capacity
    {
        get { return _capacity; }
        protected set { _capacity = value; }
    }

    public Room(string roomId, string roomName, int capacity)
    {
        RoomId = roomId;
        RoomName = roomName;
        Capacity = capacity;
    }
}

// Reservation class definition
public class Reservation
{
    private DateTime _dateTime;
    private string _reserverName;
    private Room _room;

    public DateTime DateTime
    {
        get { return _dateTime; }
        protected set { _dateTime = value; }
    }

    public string ReserverName
    {
        get { return _reserverName; }
        protected set
        {
            if (value != null)
                _reserverName = value;
            else
                throw new ArgumentNullException(nameof(value), "ReserverName cannot be null.");
        }
    }

    public Room Room
    {
        get { return _room; }
        protected set { _room = value; }
    }

    public Reservation(DateTime dateTime, string reserverName, Room room)
    {
        DateTime = dateTime;
        ReserverName = reserverName;
        Room = room;
    }
}
// LogRecord class definition
public class LogRecord
{
    private DateTime _timestamp;
    private string _reserverName;
    private string _roomName;

    public DateTime Timestamp
    {
        get { return _timestamp; }
        protected set { _timestamp = value; }
    }

    public string ReserverName
    {
        get { return _reserverName; }
        protected set
        {
            if (value != null)
                _reserverName = value;
            else
                throw new ArgumentNullException(nameof(value), "ReserverName cannot be null.");
        }
    }

    public string RoomName
    {
        get { return _roomName; }
        protected set
        {
            if (value != null)
                _roomName = value;
            else
                throw new ArgumentNullException(nameof(value), "RoomName cannot be null.");
        }
    }

    public LogRecord(DateTime timestamp, string reserverName, string roomName)
    {
        Timestamp = timestamp;
        ReserverName = reserverName;
        RoomName = roomName;
    }
}

// ILogger interface definition
public interface ILogger
{
    void Log(LogRecord log);
}

// FileLogger class definition implementing ILogger
public class FileLogger : ILogger
{
    public void Log(LogRecord log)
    {
        // Implementation to log log records into JSON files
        // You can implement this according to your specific requirements
    }
}

// LogHandler class definition
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

// IReservationRepository interface definition
public interface IReservationRepository
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(Reservation reservation);
    List<Reservation> GetAllReservations();
}

// ReservationRepository class definition implementing IReservationRepository
public class ReservationRepository : IReservationRepository
{
    private List<Reservation> _reservations = new List<Reservation>();

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

// RoomHandler class definition
public class RoomHandler
{
    public List<Room> GetRoomsFromJson(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath);
            var rooms = JsonConvert.DeserializeObject<List<Room>>(json);
            return rooms ?? new List<Room>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading rooms from JSON: {ex.Message}");
            return new List<Room>();
        }
    }
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


// IReservationService interface definition
public interface IReservationService
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(Reservation reservation);
    void DisplayWeeklySchedule();
}

// ReservationService class definition implementing IReservationService
public class ReservationService : IReservationService
{
    private readonly ReservationHandler _reservationHandler;

    public ReservationService(ReservationHandler reservationHandler)
    {
        _reservationHandler = reservationHandler;
    }

    // Implementing methods of IReservationService
    public void AddReservation(Reservation reservation)
    {
        _reservationHandler.AddReservation(reservation);
    }

    public void DeleteReservation(Reservation reservation)
    {
        _reservationHandler.DeleteReservation(reservation);
    }

    public void DisplayWeeklySchedule()
    {
        // Implementation to display weekly schedule
        // You can implement this according to your specific requirements
    }
}

// Program class definition
public partial class Program
{
   public static void Main(string[] args)
    {
        RoomHandler roomHandler = new RoomHandler();
        List<Room> rooms = roomHandler.GetRoomsFromJson("Data.json");

        Console.WriteLine("Rooms from data.json:");

        // Print the rooms
        foreach (var room in rooms)
        {
            Console.WriteLine($"Room ID: {room.RoomId}, Room Name: {room.RoomName}, Capacity: {room.Capacity}");
        }
    }
}
