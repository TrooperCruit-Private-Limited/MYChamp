namespace MYChamp.AuthModel
{
    public class ForcefulLogout
    {
        public int id {  get; set; }    
        public string UserName { get; set;}
        public string ipAddress { get; set;}
        public DateTime logoutTime{ get; set;}
        public string forceful_logout_by { get; set;}

    }
}
