namespace CustomUsableComponentsTelerik.Client.Components.CommandBar.Data
{
    public class ReadCommandsRequest
    {
        public string? Query { get; set; }

        public HashSet<string> FilterCategories { get; set; } = new HashSet<string>();

        public int PageSize { get; set; } = 20;

        public int PageNumber { get; set; } = 1;
    }
}
