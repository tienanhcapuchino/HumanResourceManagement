namespace HRM.Domain.Models;

public class TotalVacationDaysTakenModel
{
    public string Name { get; set; }

    public List<TotalVacationDaysItemModel> Items { get; set; }
}