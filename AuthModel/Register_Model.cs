using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYChamp.AuthModel
{
    public class Register_Model
    {
        [Column("id")] 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("middlename")]
        public string MiddleName {  get; set; }

        [Required]
        [Column("lastname")]
        public string LastName { get;set; }


        [Required]

        [Column("emailid")]
        public string EmailId{ get; set; }
        [Required]
        [Column("phonenumber")]
        public string PhoneNumber { get; set; }
        [Required]
        [Column("password")]
        public string Password { get; set; }
       
        [Compare("Password",ErrorMessage ="Passwords are not matching")]
        public string confirmpassword {  get; set; }
       public DateTime passwordvalidity { get; set; }
        [Column("otp")]
       public int Otp { get; set;}
        [Column("otpvalidity")]
        public DateTime OtpValiditity { get; set;}

        [Column("isemployee")]
        public Boolean IsEmployee {  get; set; }

        public Boolean active { get; set; }
      
    }
}
