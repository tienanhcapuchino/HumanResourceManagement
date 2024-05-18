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

            #region get employees birth date today
            var employeeBirthDates = await GetAllEmployeesHaveDateBirthToday();
            if (employeeBirthDates != null && employeeBirthDates.Any())
            {
                result.Datas.AddRange(GenerateNotificationContent(employeeBirthDates, NotificationType.EmployeeBirthDay));
            }
            #endregion

            #region get employees aniversal today
            var employeesAniversal = await GetAllEmployeesAniveral();
            if (employeesAniversal != null && employeesAniversal.Any())
            {
                result.Datas.AddRange(GenerateNotificationContent(employeesAniversal, NotificationType.HiringAniverary));
            }
            #endregion

            result.Datas = result.Datas.OrderBy(o => o.PublishedTime).ToList();
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

        public async Task<List<EmployeeAniveralModel>> GetAllEmployeesAniveral()
        {
            var dateToday = DateTime.UtcNow.Day;
            var monthToday = DateTime.UtcNow.Month;
            var yearToday = DateTime.UtcNow.Year;
            var allEmps = await _dbContext.Employments
                                .Where(e => e.HireDateForWorking.Value.Day == dateToday
                                            && e.HireDateForWorking.Value.Month == monthToday)
                                .Include(p => p.Personal)
                                .Select(e => new EmployeeAniveralModel
                                {
                                    EmployeeName = e.Personal.CurrentFirstName + e.Personal.CurrentMiddleName + e.Personal.CurrentLastName,
                                    AniveralYears = yearToday - e.HireDateForWorking.Value.Year
                                }).ToListAsync();
            return allEmps;
        }

        public async Task<List<EmployeesLimitedVacationModel>> GetEmployeesLimitedVacation()
        {
            var result = new List<EmployeesLimitedVacationModel>();
            var allEmpsLimit = _dbContext.Employments.Include(p => p.Personal)
                               .Include(e => e.EmploymentWorkingTimes).AsQueryable();
            foreach (var emp in allEmpsLimit)
            {
                decimal? workingTimeActual = 0;
                if (emp.EmploymentWorkingTimes?.Count > 0)
                {
                    foreach(var empWork in emp.EmploymentWorkingTimes)
                    {
                        workingTimeActual += empWork.NumberDaysActualOfWorkingPerMonth;
                    }
                }
                if (workingTimeActual.HasValue && workingTimeActual >= emp.NumberDaysRequirementOfWorkingPerMonth)
                {
                    result.Add(new EmployeesLimitedVacationModel
                    {
                        EmployeeName = emp.Personal.CurrentFirstName + emp.Personal.CurrentMiddleName + emp.Personal.CurrentLastName,
                        NumberOfVacation = emp.NumberDaysRequirementOfWorkingPerMonth.Value
                    });
                }
            }
            return await Task.FromResult(result);
        }

        private List<NotificationData> GenerateNotificationContent<T>(List<T> employees, NotificationType notificationType)
        {
            var result = new List<NotificationData>();
            switch (notificationType)
            {
                case NotificationType.EmployeeBirthDay:
                    var employeesBirthDates = employees as List<EmployeeBirthDateModel>;
                    if (employeesBirthDates != null && employeesBirthDates.Count > 0)
                    {
                        foreach (var employee in employeesBirthDates)
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
                    }
                    break;
                case NotificationType.HiringAniverary:
                    var employeesAniveral = employees as List<EmployeeAniveralModel>;
                    if (employeesAniveral != null)
                    {
                        foreach(var employee in employeesAniveral)
                        {
                            var notificationData = new NotificationData()
                            {
                                PublishedTime = DateTime.UtcNow,
                                Type = NotificationType.HiringAniverary,
                            };
                            string content = string.Format(NotificationContent.HiringAniveraryTitle, employee.AniveralYears, employee.EmployeeName);
                            notificationData.Content = content;
                            result.Add(notificationData);
                        }
                    }
                    break;
                case NotificationType.LimitedNumberOfDatysVacation:
                    var employeesVacations = employees as List<EmployeesLimitedVacationModel>;
                    if (employeesVacations != null)
                    {
                        foreach (var employee in employeesVacations)
                        {
                            var notificationData = new NotificationData()
                            {
                                PublishedTime = DateTime.UtcNow,
                                Type = NotificationType.LimitedNumberOfDatysVacation,
                            };
                            string content = string.Format(NotificationContent.HiringAniveraryTitle, employee.EmployeeName, employee.NumberOfVacation);
                            notificationData.Content = content;
                            result.Add(notificationData);
                        }
                    }
                    break;
                default: 
                    break;
            }
            return result;
        }

    }
}
