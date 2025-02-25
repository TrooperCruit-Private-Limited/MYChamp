using System.ComponentModel.DataAnnotations;


namespace MYChamp.Models
{
    public class CDHoliday

    {
        [Key]
        public int Id { get; set; }
        
        public DateTime Date { get; set; }
     
        public  string  Name { get; set; }
        
        public string Country { get; set; }


    }
}
