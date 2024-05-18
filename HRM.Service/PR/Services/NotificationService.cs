using HRM.Domain.Constants;
using HRM.Domain.Entities;
using HRM.Domain.Enums;
using HRM.Domain.Models;
using HRM.Service.HR.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRM.Service.HR.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HrmContext _dbContext;
        public NotificationService(HrmContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<NotificationModel> GetAllNotificationsAsync()
        {
            var result = new NotificationModel()
            {
                Datas = new List<NotificationData>()
            };
            #region get employee birth date today
            var employeeBirthDates = await GetAllEmployeesHaveDateBirthToday();
            if (employeeBirthDates != null && employeeBirthDates.Any())
            {
                result.Datas.AddRange(GenerateNotificationBirthDate(employeeBirthDates));
            }
            #endregion
            result.TotalCount = result.Datas.Count;
            return result;
        }

        public async Task<List<EmployeeBirthDateModel>> GetAllEmployeesHaveDateBirthToday()
        {
            var dateToday = DateTime.UtcNow.Day;
            var monthToday = DateTime.UtcNow.Month;
            var yearToday = DateTime.UtcNow.Year;
            var allEmployees = await _dbContext.Employments
                .Include(p => p.Personal)
                .Where(e => e.Personal.BirthDate.Value.Day == dateToday
                            && e.Personal.BirthDate.Value.Month == monthToday)
                .Select(e => new EmployeeBirthDateModel
                {
                    BirthDate = e.Personal.BirthDate.Value,
                    EmployeeName = e.Personal.CurrentFirstName + e.Personal.CurrentMiddleName + e.Personal.CurrentLastName,
                    Ages = yearToday - e.Personal.BirthDate.Value.Year
                })
                .ToListAsync();
            return allEmployees;
        }

        private List<NotificationData> GenerateNotificationBirthDate(List<EmployeeBirthDateModel> employees)
        {
            var result = new List<NotificationData>();
            foreach (var employee in employees)
            {
                var notificationData = new NotificationData()
                {
                    PublishedTime = DateTime.UtcNow,
                    Type = NotificationType.EmployeeBirthDay,
                };
                string content = string.Format(NotificationContent.EmployeeBirthDayTitle, employee.EmployeeName, employee.Ages);
                notificationData.Content = content;
                result.Add(notificationData);
            }
            return result;
        }

    }
}
