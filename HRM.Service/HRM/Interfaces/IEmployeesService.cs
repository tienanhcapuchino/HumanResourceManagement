using HRM.Domain.Enums;
using HRM.Domain.HRM.Entities;
using HRM.Domain.Models;

namespace HRM.Service.HRM.Interfaces;

public interface IEmployeesService
{
    Task<List<TotalEarningModel>> GetTotalEarningFilter(HrmFilterType filterType);

    decimal GetTotalEarning();
    
    Task<(int, int)> GetVacationDays();
}