using CustomUsableComponentsTelerik.Client.Components.CronExpression.Helpers;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace CustomUsableComponentsTelerik.Client.Components.CronExpression
{
    public partial class CronExpressionBuilder
    {

        #region Parameters

        [Parameter]
        public string? Value { get; set; }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        [Parameter]
        public bool EnableCustomInput { get; set; } = true;

        [Parameter]
        public bool ShowDescriptionOnConfiguration { get; set; } = true;

        #endregion

        private string CronExpression = "* * * * *";
        private CronFrequency SelectedFrequency = CronFrequency.Minute;
        private int MinuteInterval = 1;
        private int HourInterval = 1;
        private int DayInterval = 1;
        private int DayOfMonth = 1;
        private List<CronWeekDay> SelectedWeekDays = new();
        private List<CronMonth> SelectedMonths = new();
        private TimeSpan SelectedTime = new TimeSpan(8, 0, 0); // 08:00 AM
        private DateTime SelectedDateTime { get => DateTime.Today.Add(SelectedTime); set { SelectedTime = value.TimeOfDay; } }
        private string ManualCronExpression = "* * * * *";
        private bool IsValidCron = true;

        private List<CronFrequency> Frequencies = Enum.GetValues<CronFrequency>().ToList();
        private List<CronWeekDay> WeekDays = Enum.GetValues<CronWeekDay>().ToList();
        private List<CronMonth> Months = Enum.GetValues<CronMonth>().ToList();


        #region Métodos


        #region Métodos - Ciclo de Vida

        protected override void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                CronExpression = Value;
            }
        }

        #endregion

        void ParseCronExpression(string expression)
        {
            Match match = CronExpressionHelper.GetMatch(expression);

            if (!match.Success)
            {
                IsValidCron = false;
                return;
            }

            IsValidCron = true;
            string minutePart = match.Groups[1].Value;
            string hourPart = match.Groups[2].Value;
            string dayPart = match.Groups[3].Value;
            string monthPart = match.Groups[4].Value;
            string weekDayPart = match.Groups[5].Value;

            // Determinar la frecuencia
            if (minutePart.StartsWith("*/"))
            {
                SelectedFrequency = CronFrequency.Minute;
                MinuteInterval = int.Parse(minutePart.Replace("*/", ""));
            }
            else if (hourPart.StartsWith("*/"))
            {
                SelectedFrequency = CronFrequency.Hourly;
                HourInterval = int.Parse(hourPart.Replace("*/", ""));
            }
            else if (weekDayPart != "*" && dayPart == "*")
            {
                // Frecuencia semanal
                SelectedFrequency = CronFrequency.Weekly;
                SelectedWeekDays = weekDayPart.Split(',')
                                              .Where(d => !string.IsNullOrWhiteSpace(d))
                                              .Select(d => (CronWeekDay)int.Parse(d))
                                              .ToList();
            }
            else if (dayPart != "*" && monthPart == "*" && weekDayPart == "*")
            {
                // Frecuencia diaria
                SelectedFrequency = CronFrequency.Daily;
                DayInterval = int.Parse(dayPart);
            }
            else if (monthPart != "*" && dayPart != "*" && weekDayPart == "*")
            {
                // Frecuencia mensual basada en el día del mes
                SelectedFrequency = CronFrequency.Monthly;
                DayOfMonth = int.Parse(dayPart);
                SelectedMonths = monthPart.Split(',')
                                          .Where(m => !string.IsNullOrWhiteSpace(m))
                                          .Select(m => (CronMonth)int.Parse(m))
                                          .ToList();
            }
            else if (monthPart != "*" && weekDayPart != "*" && dayPart == "*")
            {
                // Frecuencia mensual basada en el día de la semana
                SelectedFrequency = CronFrequency.Monthly;
                SelectedWeekDays = weekDayPart.Split(',')
                                              .Where(d => !string.IsNullOrWhiteSpace(d))
                                              .Select(d => (CronWeekDay)int.Parse(d))
                                              .ToList();
                SelectedMonths = monthPart.Split(',')
                                          .Where(m => !string.IsNullOrWhiteSpace(m))
                                          .Select(m => (CronMonth)int.Parse(m))
                                          .ToList();
            }

            // Asignar Hora y Minuto
            int hour = hourPart != "*" ? int.Parse(hourPart) : 0;
            int minute = minutePart != "*" ? int.Parse(minutePart) : 0;
            SelectedTime = new TimeSpan(hour, minute, 0);
        }

        void OnFrecuencyChangedHandler()
        {
            this.SelectedTime = new TimeSpan(8, 0, 0);
            this.SelectedMonths = new List<CronMonth>();
            this.SelectedWeekDays = new List<CronWeekDay>();
            this.DayInterval = 1;
            this.HourInterval = 1;
            this.MinuteInterval = 1;
            this.DayOfMonth = 1;
            UpdateCronExpression();
        }

        private void UpdateCronExpression()
        {
            string hour = SelectedTime.Hours.ToString();
            string minute = SelectedTime.Minutes.ToString();
            string months = SelectedMonths.Any() ? string.Join(",", SelectedMonths.Select(m => (int)m)) : "*";
            string weekDays = SelectedWeekDays.Any() ? string.Join(",", SelectedWeekDays.Select(d => (int)d)) : "*";

            CronExpression = SelectedFrequency switch
            {
                CronFrequency.Minute => $"*/{MinuteInterval} * * * *",
                CronFrequency.Hourly => $"0 */{HourInterval} * * *",
                CronFrequency.Daily => $"{minute} {hour} */{DayInterval} * *",
                CronFrequency.Weekly => $"{minute} {hour} * * {weekDays}",
                CronFrequency.Monthly => $"{minute} {hour} {DayOfMonth} {months} {weekDays}",
                _ => "* * * * *"
            };

            ValueChanged.InvokeAsync(CronExpression);
        }

        private void ValidateManualExpression()
        {
            IsValidCron = CronExpressionHelper.Validate(ManualCronExpression);
            if (IsValidCron)
            {
                CronExpression = ManualCronExpression;
                ParseCronExpression(CronExpression);
            }
        }

        #endregion
    }
}
