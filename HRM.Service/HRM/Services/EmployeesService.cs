using HRM.Domain;
using HRM.Domain.Constants;
using HRM.Domain.Entities;
using HRM.Domain.Enums;
using HRM.Domain.HRM.Entities;
using HRM.Domain.Models;
using HRM.Service.HRM.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HRM.Service.HRM.Services;

public class EmployeesService : IEmployeesService
{
    private readonly MydbContext _context;
    private readonly HrmContext _hrmContext;

    public EmployeesService(MydbContext context,
        HrmContext hrmContext)
    {
        _context = context;
        _hrmContext = hrmContext;
    }

    public async Task<List<TotalEarningModel>> GetTotalEarningFilter(HrmFilterType filterType)
    {
        var result = new List<TotalEarningModel>();

        var employees = await _context.Employees.ToListAsync();

        var employments = await _hrmContext.Employments
            .Include(e => e.EmploymentWorkingTimes)
            .Where(e => e.EmploymentWorkingTimes.Any(x => x.MonthWorking == DateTime.Now.Month))
            .ToListAsync();

        if (employments is not null && employments.Count > 0)
        {
            switch (filterType)
            {
                case HrmFilterType.Shareholder:
                    var employmentGroupByShareholderStat = employments
                        .GroupBy(
                            e => e.Personal.ShareholderStatus,
                            e => e.EmploymentId,
                            (shareholderStat, ids) => new
                            {
                                ShareHolder = shareholderStat,
                                EmploymentIds = ids.ToList()
                            })
                        .ToList();
                    foreach (var item in employmentGroupByShareholderStat)
                    {
                        var totalEarningToDate = employees
                            .Where(e => item.EmploymentIds.Contains(e.IdEmployee))
                            .Sum(e => e.PaidToDate);
                        var totalEarningLastYear = employees
                            .Where(e => item.EmploymentIds.Contains(e.IdEmployee))
                            .Sum(e => e.PaidLastYear);

                        result.Add(new TotalEarningModel
                        {
                            Name = item.ShareHolder == 1 ? "Shareholder" : "Non-shareholder",
                            ToDateValue = (decimal)totalEarningToDate,
                            PrevYearValue = (decimal)totalEarningLastYear
                        });
                    }

                    break;
                case HrmFilterType.Gender:
                    var employmentGroupByGender = employments
                        .GroupBy(
                            e => e.Personal.CurrentGender,
                            e => e.EmploymentId,
                            (gender, ids) => new
                            {
                                Gender = gender,
                                EmploymentIds = ids.ToList()
                            })
                        .ToList();
                    foreach (var item in employmentGroupByGender)
                    {
                        var totalEarningToDate = employees
                            .Where(e => item.EmploymentIds.Contains(e.IdEmployee))
                            .Sum(e => e.PaidToDate);
                        var totalEarningLastYear = employees
                            .Where(e => item.EmploymentIds.Contains(e.IdEmployee))
                            .Sum(e => e.PaidLastYear);

                        result.Add(new TotalEarningModel
                        {
                            Name = item.Gender,
                            ToDateValue = (decimal)totalEarningToDate,
                            PrevYearValue = (decimal)totalEarningLastYear
                        });
                    }

                    break;
                case HrmFilterType.Ethnicity:
                    var employmentGroupByEthnicity = employments
                        .GroupBy(
                            e => e.Personal.Ethnicity,
                            e => e.EmploymentId,
                            (ethnicity, ids) => new
                            {
                                Ethnicity = ethnicity,
                                EmploymentIds = ids.ToList()
                            })
                        .ToList();
                    foreach (var item in employmentGroupByEthnicity)
                    {
                        var totalEarningToDate = employees
                            .Where(e => item.EmploymentIds.Contains(e.IdEmployee))
                            .Sum(e => e.PaidToDate);
                        var totalEarningLastYear = employees
                            .Where(e => item.EmploymentIds.Contains(e.IdEmployee))
                            .Sum(e => e.PaidLastYear);

                        result.Add(new TotalEarningModel
                        {
                            Name = item.Ethnicity,
                            ToDateValue = (decimal)totalEarningToDate,
                            PrevYearValue = (decimal)totalEarningLastYear
                        });
                    }

                    break;
            }

        }

        return result;
    }

    public decimal GetTotalEarning()
    {
        var employees = _context.Employees.ToList();
        var result = employees.Sum(e => e.PaidToDate);
        return result ?? 0;
    }

    public async Task<(int, int)> GetVacationDays()
    {
        var totalVacationDaysTaken = 0;
        var excessVacationDays = 0;
        var employees = await _context.Employees.ToListAsync();

        var employments = await _hrmContext.Employments
            .Include(e => e.EmploymentWorkingTimes)
            .Where(e => e.EmploymentWorkingTimes.Any(x => x.MonthWorking == DateTime.Now.Month))
            .ToListAsync();

        if (employments is not null && employments.Count > 0)
        {
            var employmentIds = employments.Select(e => e.EmploymentId).ToList();
            totalVacationDaysTaken = employees
                .Where(e => employmentIds.Contains(e.IdEmployee))
                .Sum(e => e.VacationDays) ?? 0;
            var totalVacationDays = employments
                .SelectMany(e => e.EmploymentWorkingTimes)
                .Sum(e => e.TotalNumberVacationWorkingDaysPerMonth) ?? 0;
            excessVacationDays = (int)totalVacationDays - totalVacationDaysTaken;
        }

        return (totalVacationDaysTaken, excessVacationDays);
    }
}