namespace HRM.Domain.Models;

public class TotalEarningModel
{
    public string Name { get; set; }

    public List<TotalEarningItemModel> Items { get; set; }
}