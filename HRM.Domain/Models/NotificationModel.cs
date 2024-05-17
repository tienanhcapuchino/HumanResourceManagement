using HRM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Domain.Models
{
    public class NotificationModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
    }
}
