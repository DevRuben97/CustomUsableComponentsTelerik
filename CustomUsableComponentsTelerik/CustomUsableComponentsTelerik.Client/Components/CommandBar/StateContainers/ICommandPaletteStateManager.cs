using CustomUsableComponentsTelerik.Client.Components.CommandBar.Models;
using System.Threading.Tasks;
using CustomUsableComponentsTelerik.Client.Components.CommandBar.Events;

namespace CustomUsableComponentsTelerik.Client.Components.CommandBar.StateContainers
{
    public interface ICommandPaletteStateManager
    {
        bool IsOpen { get; }
        bool IsLoading { get; }
        List<CommandAction> CommandItems { get; set;}
        void Open();
        void Close();
        Task FilterItems(string query);
        void SetServerSearchFunction(Func<OnReadCommandsEventArgs, Task> serverSearchFunction);
    }
}