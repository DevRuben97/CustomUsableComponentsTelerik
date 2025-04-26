using System.Text.RegularExpressions;
using System.Text;
using CronExpressionDescriptor;

namespace CustomUsableComponentsTelerik.Client.Components.CronExpression.Helpers
{
    public class CronExpressionHelper
    {
        public const string CronValidationPattern = @"^([\d/*,-]+)\s([\d/*,-]+)\s([\d/*,-]+)\s([\d/*,-]+)\s([\d/*,-]+)$";

        public static Match GetMatch(string cronExpression) => Regex.Match(cronExpression,CronValidationPattern);

        public static bool Validate(string cronExpression) => Regex.IsMatch(cronExpression, CronValidationPattern);

        public static ValueTuple<bool,string> GetCronDescription(string cronExpression)
        {
            Match match = Regex.Match(cronExpression, CronValidationPattern);

            if (!match.Success)
            {
                return (false,"Expresión CRON no válida.");
            }

            return (true, ExpressionDescriptor.GetDescription(cronExpression));
        }

    }
}
