using CustomUsableComponentsTelerik.Client.Components.CommandBar.Models;

namespace CustomUsableComponentsTelerik.Client.Components.CommandBar.Data
{
    public class ReadCommandsResult
    {
        public IEnumerable<CommandAction> Actions { get; set; }

        public int TotalCount { get; set; }

        public ReadCommandsResult(IEnumerable<CommandAction> actions, int totalCount)
        {
            Actions = actions;
            TotalCount = totalCount;
        }

        public ReadCommandsResult()
        {
            Actions = new List<CommandAction>();
            TotalCount = 0;
        }
    }
}
