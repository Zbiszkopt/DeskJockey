using System;
using System.ComponentModel.DataAnnotations;

namespace DeskJockey.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        [Display(Name = "ID użytkownika")]
        public int UserId { get; set; }
        [Display(Name = "Numer biurka")]
        public int DeskId { get; set; }

        [Display(Name = "Data rozpoczęcia")] 
        public DateTime StartDate { get; set; }
        [Display(Name = "Data zakończenia")]
        public DateTime EndDate { get; set; }

        public string Status { get; set; }
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
    }
}