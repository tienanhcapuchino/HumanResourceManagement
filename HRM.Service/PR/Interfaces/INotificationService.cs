using HRM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Service.HR.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationModel>> GetAllNotificationsAsync();
    }
}
