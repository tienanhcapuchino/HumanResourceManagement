﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Domain.Constants
{
    public static class NotificationTitle
    {
        public const string HiringAniveraryTitle = "Today is {0} years aniversary of {1}";
        public const string LimitedNumberOfDatysVacationTitle = "{0} has reached out to limitation of day of vacation";
        public const string ChangeBenefitPlanTitle = "{0} has changed benefit plan";
        public const string EmployeeBirthDayTitle = "Today is {0} 's birthday";
    }
    public static class NotificationContent
    {
        public const string HiringAniveraryContent = "";
        public const string LimitedNumberOfDatysVacationContent = "";
        public const string ChangeBenefitPlanContent = "";
        public const string EmployeeBirthDayContent = "";
    }
}