using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MYChamp.Models
{
    public class CDHoliday

    {
        [Key]
        public int Id { get; set; }

        //[Column(TypeName = "timestamp with time zone")]
        // public DateTime Date { get; set; }

        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set => _date = DateTime.SpecifyKind(value, DateTimeKind.Utc); 
        }

        public string  Name { get; set; }
        
        public string Country { get; set; }


    }
}
