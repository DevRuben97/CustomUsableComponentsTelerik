using CustomUsableComponentsTelerik.Client.Components.CommandBar.Data;
using CustomUsableComponentsTelerik.Client.Components.CommandBar.Models;

namespace CustomUsableComponentsTelerik.Client.Components.CommandBar.Events
{
    public class OnReadCommandsEventArgs
    {
        public OnReadCommandsEventArgs(string? query, HashSet<string> filterCategories, int pageSize=20, int pageIndex=1)
        {
            Request = new ReadCommandsRequest()
            {
                Query = query,
                FilterCategories = filterCategories,
                PageNumber = pageIndex,
                PageSize = pageSize
            };
        }

        public ReadCommandsRequest Request { get; private set; } = new ReadCommandsRequest();

        public IEnumerable<CommandAction>? Actions { get; set; }

        public int TotalCount { get; set; } = 0;


        /// <summary>
        /// Sets the action results.
        /// </summary>
        /// <param name="actions">The list of command actions.</param>
        /// <param name="totalCount">The total count of available items.</param>
        public void SetResult(IEnumerable<CommandAction> actions, int totalCount)
        {
            Actions = actions;
            TotalCount = totalCount;
        }
    }
}
