using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYChamp.Pages.PaySlip
{
    public class IndexModel : PageModel
    {
        private readonly MYChampDbContext _db;

        public IndexModel(MYChampDbContext db)
        {
            _db = db;
        }

        public List<PaySlipDetail> PaySlipDetails { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var employees = await _db.Employees.ToListAsync();

            foreach (var employee in employees)
            {
                var monthlySalary = (decimal)employee.Salary / 12;

                // Calculate deductions
                var epf = monthlySalary * 0.12m;
                var professionTax = monthlySalary > 15000 ? 200 : 0;
                var incomeTax = monthlySalary > 50000 ? monthlySalary * 0.10m : 0;
                var leaveDeductions = (employee.RemainingLeaves < 0 ? -employee.RemainingLeaves : 0) * ((double)monthlySalary / 30);

                var totalDeductions = (double)epf + (double)professionTax + (double)incomeTax + leaveDeductions;
                var netSalary = (double)monthlySalary - (double)totalDeductions;

                var dispatch = await _db.SalaryDispatches
                    .FirstOrDefaultAsync(s => s.EmployeeId == employee.EmployeeId);

                PaySlipDetails.Add(new PaySlipDetail
                {
                    EmployeeId = employee.EmployeeId,
                    Name = employee.Name,
                    MonthlySalary = Math.Round(monthlySalary, 2),
                    EPF = Math.Round(epf, 2),
                    ProfessionTax = Math.Round((decimal)professionTax, 2),
                    IncomeTax = Math.Round(incomeTax, 2),
                    LeaveDeductions = Math.Round((decimal)leaveDeductions, 2),
                    NetSalary = Math.Round((decimal)netSalary, 2),
                    DispatchDate = dispatch?.DispatchDate,
                });
            }

            return Page();
        }


        public async Task<IActionResult> OnPostDispatchAsync(int employeeId,decimal netSalary)
        {
            // Find the employee
            var employee = await _db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
            if (employee != null)
            {
              
                    // Record dispatch date
                    var salaryDispatch = new SalaryDispatch
                    {
                        EmployeeId = employeeId,
                        EmployeeName = employee.Name,
                        Email = employee.Email,
                        MonthlySalary = employee.Salary/12,
                        NetSalary = netSalary,
                        DispatchDate = DateTime.UtcNow
                    };

                    _db.SalaryDispatches.Add(salaryDispatch);
                    await _db.SaveChangesAsync();
                
            }

            return RedirectToPage();
        }

    }

    public class PaySlipDetail
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public decimal MonthlySalary { get; set; }
        public decimal EPF { get; set; }
        public decimal ProfessionTax { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal LeaveDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime? DispatchDate { get; set; } 
        public bool CanDispatch => !DispatchDate.HasValue || DispatchDate.Value.AddDays(1) < DateTime.UtcNow; 
    }

}
