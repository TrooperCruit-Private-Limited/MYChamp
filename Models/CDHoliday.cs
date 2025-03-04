using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MYChamp.Models
{
    public class CDHoliday

    {
        [Key]
        public int Id { get; set; }

        //[Column(TypeName = "timestamp with time zone")]
        public DateTime Date { get; set; }
     
        public  string  Name { get; set; }
        
        public string Country { get; set; }


    }
}
