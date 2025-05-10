    using System;
    using System.Collections.Generic;

    namespace CustomUsableComponentsTelerik.Client.Components.CommandBar.Models
    {
        public class FilterCategory
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public string Icon { get; set; }
        }

        public class CommandAction
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Icon { get; set; }
            public string Type { get; set; }
            public List<string> Shortcut { get; set; }
            public Action Action { get; set; }
        }

        public class CommandSectionModel
        {
            public string Type { get; set; } = "section";
            public string Title { get; set; }
            public List<CommandAction> Items { get; set; } = new List<CommandAction>();
        }

        public static class CommandPaletteData
        {
            // Filter categories
            public static List<FilterCategory> FilterCategories = new List<FilterCategory>
        {
            new FilterCategory { Id = "invoices", Label = "Invoices", Icon = "bi-receipt" },
            new FilterCategory { Id = "orders", Label = "Orders", Icon = "bi-bag" },
            new FilterCategory { Id = "customers", Label = "Customers", Icon = "bi-people" },
            new FilterCategory { Id = "products", Label = "Products", Icon = "bi-box" },
            new FilterCategory { Id = "reports", Label = "Reports", Icon = "bi-graph-up" }
        };

            // Recent items
            public static List<CommandAction> RecentItems = new List<CommandAction>
        {
            new CommandAction
            {
                Id = "recent1",
                Title = "Assign to...",
                Icon = "bi-person",
                Shortcut = new List<string> { "A" },
                Type = "action",
                Action = () => Console.WriteLine("Assign to action")
            },
            new CommandAction
            {
                Id = "recent2",
                Title = "Add new...",
                Icon = "bi-plus-circle",
                Shortcut = new List<string> { "⌘", "N" },
                Type = "action",
                Action = () => Console.WriteLine("Add new action")
            },
            new CommandAction
            {
                Id = "recent3",
                Title = "Change status...",
                Icon = "bi-arrow-repeat",
                Shortcut = new List<string> { "S" },
                Type = "action",
                Action = () => Console.WriteLine("Change status action")
            }
        };

            // Quick actions
            public static List<CommandAction> QuickActions = new List<CommandAction>
        {
            new CommandAction
            {
                Id = "quick1",
                Title = "View recent items",
                Icon = "bi-clock-history",
                Shortcut = new List<string> { "⌘", "1" },
                Type = "view",
                Action = () => Console.WriteLine("View recent items")
            },
            new CommandAction
            {
                Id = "quick2",
                Title = "View saved searches",
                Icon = "bi-bookmark",
                Shortcut = new List<string> { "⌘", "2" },
                Type = "view",
                Action = () => Console.WriteLine("View saved searches")
            },
            new CommandAction
            {
                Id = "quick3",
                Title = "View notifications",
                Icon = "bi-bell",
                Shortcut = new List<string> { "⌘", "3" },
                Type = "view",
                Action = () => Console.WriteLine("View notifications")
            },
            new CommandAction
            {
                Id = "quick4",
                Title = "Create new document",
                Icon = "bi-file-earmark-plus",
                Shortcut = new List<string> { "⌘", "4" },
                Type = "action",
                Action = () => Console.WriteLine("Create new document")
            }
        };

            // All commands for search
            public static List<CommandAction> Commands = new List<CommandAction>
        {
            new CommandAction
            {
                Id = "cmd1",
                Title = "Assign to Simon Prusin",
                Subtitle = "simonprusin@gmail.com",
                Icon = "bi-person",
                Type = "action",
                Action = () => Console.WriteLine("Assign to Simon Prusin")
            },
            new CommandAction
            {
                Id = "cmd2",
                Title = "Sign up flow",
                Subtitle = "Notion Page",
                Icon = "bi-file-earmark-text",
                Type = "document",
                Action = () => Console.WriteLine("Open Notion page")
            },
            new CommandAction
            {
                Id = "cmd3",
                Title = "Sign up flow Design",
                Subtitle = "MSP-10",
                Icon = "bi-palette",
                Type = "design",
                Action = () => Console.WriteLine("Open design")
            },
            new CommandAction
            {
                Id = "cmd4",
                Title = "Sign up Error",
                Subtitle = "MSP-19",
                Icon = "bi-bug",
                Type = "issue",
                Action = () => Console.WriteLine("Open error ticket")
            },
            new CommandAction
            {
                Id = "cmd5",
                Title = "Dark mode",
                Subtitle = "Toggle theme",
                Icon = "bi-moon",
                Type = "setting",
                Action = () => Console.WriteLine("Toggle dark mode")
            },
            new CommandAction
            {
                Id = "cmd6",
                Title = "Settings",
                Subtitle = "View application settings",
                Icon = "bi-gear",
                Type = "setting",
                Action = () => Console.WriteLine("Open settings")
            },
            new CommandAction
            {
                Id = "cmd7",
                Title = "Help center",
                Subtitle = "Get help with the application",
                Icon = "bi-question-circle",
                Type = "help",
                Action = () => Console.WriteLine("Open help center")
            }
        };
        }
    }
