namespace MYChamp.Models
{
    public class SalaryDispatch
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public decimal MonthlySalary { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime DispatchDate { get; set; }
    }
}
