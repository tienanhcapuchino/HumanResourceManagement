using HRM.Domain.Entities;
using HRM.Domain.Models;
using HRM.Service.HR.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Service.HR.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HrmContext _dbContext;
        public NotificationService(HrmContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<NotificationModel>> GetAllNotificationsAsync()
        {
            return new List<NotificationModel>();
        }
    }
}
