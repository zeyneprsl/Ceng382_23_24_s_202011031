using System;
using System.Collections.Generic;

public interface IReservationService
{
    // Rezervasyon ekleme işlemi
    void AddReservation(Reservation reservation);

    // Rezervasyon silme işlemi
    void DeleteReservation(Reservation reservation);

    // Haftalık programı görüntüleme işlemi
    void DisplayWeekSchedule();
}