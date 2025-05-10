using System;
using System.Collections.Generic;
using System.Linq;
using CustomUsableComponentsTelerik.Client.Components.CommandBar.Models;
using Microsoft.JSInterop;

namespace CustomUsableComponentsTelerik.Client.Components.CommandBar.StateContainers
{
    public class CommandPaletteStateContainer
    {
        private readonly IJSRuntime _jsRuntime;
        private HashSet<string> _activeFilters = new HashSet<string>();
        private List<CommandAction> _allCommandItems { get; set; } = new List<CommandAction>();
        private List<CommandSectionModel> _filteredItems = new List<CommandSectionModel>();
        private int _selectedItemIndex = -1;

        public event Action OnStateChanged;
        public event Action OnOpened;

        public CommandPaletteStateContainer(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public bool IsOpen { get; private set; }
        public HashSet<string> ActiveFilters => _activeFilters;
        public List<CommandSectionModel> FilteredItems => _filteredItems;
        public int SelectedItemIndex
        {
            get => _selectedItemIndex;
            set
            {
                _selectedItemIndex = value;
                NotifyStateChanged();
            }
        }

        public List<CommandAction> CommandItems
        {
            get => _allCommandItems;
            set
            {
                _allCommandItems = value;
                NotifyStateChanged();
            }
        }

        public void Open()
        {
            if (IsOpen) return;

            IsOpen = true;
            FilterItems(string.Empty);
            NotifyStateChanged();
            NotifyOpened();

            // Focus the input via JS
            _ = _jsRuntime.InvokeVoidAsync("commandPaletteInterop.focusInput", "commandPaletteInput");
            _ = _jsRuntime.InvokeVoidAsync("commandPaletteInterop.preventBodyScroll");
        }

        public void Close()
        {
            if (!IsOpen) return;

            IsOpen = false;
            _selectedItemIndex = -1;
            NotifyStateChanged();

            // Re-enable body scrolling
            _ = _jsRuntime.InvokeVoidAsync("commandPaletteInterop.enableBodyScroll");
        }

        public void ToggleFilter(string filterId)
        {
            if (_activeFilters.Contains(filterId))
            {
                _activeFilters.Remove(filterId);
            }
            else
            {
                _activeFilters.Add(filterId);
            }

            FilterItems(string.Empty);
            NotifyStateChanged();
        }

        public void FilterItems(string query)
        {
            query = query?.ToLowerInvariant().Trim() ?? string.Empty;

            // Get all items
            var allItems = _allCommandItems.ToList();

            // Filter by search query if present
            if (!string.IsNullOrWhiteSpace(query))
            {
                allItems = allItems.Where(item =>
                    item.Title.ToLowerInvariant().Contains(query) ||
                    (item.Subtitle != null && item.Subtitle.ToLowerInvariant().Contains(query))
                ).ToList();
            }

            // Apply active filters if any
            if (_activeFilters.Count > 0)
            {
                // In a real application, this would filter based on actual categories
                // For demonstration purposes, we'll just filter randomly
                var random = new Random();
                allItems = allItems.Where(item => random.Next(2) == 0 || _activeFilters.Count == 0).ToList();
            }

            // Group items by type
            var groupedItems = allItems
                .GroupBy(item => item.Type ?? "other")
                .ToDictionary(g => g.Key, g => g.ToList());

            // Convert grouped items to sections
            _filteredItems = groupedItems
                .Select(group => new CommandSectionModel
                {
                    Type = "section",
                    Title = char.ToUpper(group.Key[0]) + group.Key.Substring(1),
                    Items = group.Value
                })
                .ToList();

            // Reset selection
            _selectedItemIndex = -1;

            NotifyStateChanged();
        }

        public void SelectNextItem()
        {
            int totalItems = CountAllSelectableItems();
            if (totalItems == 0) return;

            _selectedItemIndex = (_selectedItemIndex + 1) % totalItems;
            NotifyStateChanged();
        }

        public void SelectPreviousItem()
        {
            int totalItems = CountAllSelectableItems();
            if (totalItems == 0) return;

            _selectedItemIndex = _selectedItemIndex <= 0
                ? totalItems - 1
                : _selectedItemIndex - 1;

            NotifyStateChanged();
        }

        public void ExecuteSelectedItem()
        {
            CommandAction selectedItem = GetSelectedItem();

            if (selectedItem != null && selectedItem.Action != null)
            {
                selectedItem.Action.Invoke();
                Close();
            }
        }

        public CommandAction GetSelectedItem()
        {
            int count = 0;
            foreach (var section in _filteredItems)
            {
                foreach (var item in section.Items)
                {
                    if (count == _selectedItemIndex)
                    {
                        return item;
                    }
                    count++;
                }
            }
            return null;
        }

        private int CountAllSelectableItems()
        {
            return _filteredItems.Sum(section => section.Items.Count);
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();

        public void NotifyOpened() => OnOpened?.Invoke();

    }
}