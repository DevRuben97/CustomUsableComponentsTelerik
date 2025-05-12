using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomUsableComponentsTelerik.Client.Components.CommandBar.Events;
using CustomUsableComponentsTelerik.Client.Components.CommandBar.Models;
using CustomUsableComponentsTelerik.Client.Pages;
using Microsoft.JSInterop;

namespace CustomUsableComponentsTelerik.Client.Components.CommandBar.StateContainers
{
    public class CommandPaletteStateManager : ICommandPaletteStateManager
    {
        private readonly IJSRuntime _jsRuntime;
        private HashSet<string> _activeFilters = new HashSet<string>();
        private List<CommandAction> _allCommandItems { get; set; } = new List<CommandAction>();
        private List<CommandSectionModel> _filteredItems = new List<CommandSectionModel>();
        private List<FilterCategory> _filterCategories { get; set; } = new List<FilterCategory>();
        private int _selectedItemIndex = -1;
        private int _pageSize = 20;
        private int _pageIndex = 1;
        private bool _isLoading = false;
        private bool _isLoadingMoreItems = false;
        private bool _hasMoreData = true;
        private string _currentQuery = string.Empty;

        public event Action OnStateChanged;
        public event Action OnOpened;
        public event Action OnLoadingStarted;
        public event Action OnLoadingFinished;

        Func<OnReadCommandsEventArgs, Task>? ServerSearchFunction;

        public CommandPaletteStateManager(IJSRuntime jsRuntime, int pageSize = 20)
        {
            _jsRuntime = jsRuntime;
            _pageSize = pageSize;
        }

        public bool IsOpen { get; private set; }

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                // Notificar solo si hay un cambio de estado
                if (_isLoading != value)
                {
                    _isLoading = value;
                    if (_isLoading)
                    {
                        OnLoadingStarted?.Invoke();
                    }
                    else
                    {
                        OnLoadingFinished?.Invoke();
                    }
                }
            }
        }

        public bool IsLoadingMoreItems
        {
            get => _isLoadingMoreItems;
            private set
            {
                _isLoadingMoreItems = value;
            }
        }

        public bool HasMoreData => _hasMoreData;
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

        public List<FilterCategory> FilterCategories
        {
            get => _filterCategories;
            set
            {
                _filterCategories = value;
                NotifyStateChanged();
            }
        }

        public void Open()
        {
            if (IsOpen) return;

            IsOpen = true;
            _pageIndex = 1;
            _hasMoreData = true;
            _=FilterItems(string.Empty);
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

            // Resetear la paginación cuando se cambia un filtro
            _pageIndex = 1;
            _hasMoreData = true;
            _ = FilterItems(_currentQuery);
            NotifyStateChanged();
        }

        public async Task FilterItems(string query)
        {
            query = query?.ToLowerInvariant().Trim() ?? string.Empty;
            _currentQuery = query;

            // Resetear la paginación cuando cambia la consulta
            _pageIndex = 1;
            _hasMoreData = true;
            _filteredItems.Clear();

            await LoadMoreItems(true);
        }

        public async Task LoadMoreItems(bool isNewSearch = false)
        {
            if (IsLoading || (!_hasMoreData && !isNewSearch)) return;

            try
            {
                IsLoading = true;
                IsLoadingMoreItems = !isNewSearch;
                NotifyStateChanged();

                var allItems = new List<CommandAction>();

                if (ServerSearchFunction != null)
                {
                    var args = new OnReadCommandsEventArgs(_currentQuery, _activeFilters, _pageSize, _pageIndex);
                    await ServerSearchFunction.Invoke(args);
                    allItems = args.Actions?.ToList() ?? new List<CommandAction>();

                    // Determinar si hay más datos disponibles
                    _hasMoreData = allItems.Count >= _pageSize;
                }
                else
                {
                    // Filtrar los elementos localmente
                    allItems = _allCommandItems.ToList();

                    // Aplicar filtros de búsqueda y categoría
                    if (!string.IsNullOrWhiteSpace(_currentQuery))
                    {
                        allItems = allItems.Where(item =>
                            item.Title.ToLowerInvariant().Contains(_currentQuery) ||
                            (item.Subtitle != null && item.Subtitle.ToLowerInvariant().Contains(_currentQuery))
                        ).ToList();
                    }

                    if (_activeFilters.Count > 0)
                    {
                        allItems = allItems.Where(item => _activeFilters.Contains(item.Type ?? "other")).ToList();
                    }

                    // Aplicar paginación en memoria
                    int totalItems = allItems.Count;
                    int skip = (_pageIndex - 1) * _pageSize;
                    allItems = allItems.Skip(skip).Take(_pageSize).ToList();

                    // Determinar si hay más datos disponibles
                    _hasMoreData = skip + _pageSize < totalItems;
                }

                // Incrementar el índice de página para la próxima carga
                _pageIndex++;

                // Agrupar elementos por tipo
                var groupedItems = allItems
                    .GroupBy(item => item.Type ?? "other")
                    .ToDictionary(g => g.Key, g => g.ToList());

                // Crear nuevas secciones o actualizar existentes
                var newSections = groupedItems
                    .Select(group => new CommandSectionModel
                    {
                        Type = "section",
                        Title = char.ToUpper(group.Key[0]) + group.Key.Substring(1),
                        Items = group.Value
                    })
                    .ToList();

                // Si es una nueva búsqueda, reemplazar completamente
                if (isNewSearch)
                {
                    _filteredItems = newSections;
                }
                else
                {
                    // Para cargas posteriores, combinar con elementos existentes
                    foreach (var newSection in newSections)
                    {
                        var existingSection = _filteredItems.FirstOrDefault(s => s.Title == newSection.Title);
                        if (existingSection != null)
                        {
                            // Añadir elementos a la sección existente
                            existingSection.Items.AddRange(newSection.Items);
                        }
                        else
                        {
                            // Añadir nueva sección
                            _filteredItems.Add(newSection);
                        }
                    }
                }

                IsLoading = false;
                IsLoadingMoreItems = false;
                NotifyStateChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading items: {ex.Message}");
                IsLoading = false;
                NotifyStateChanged();
            }
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

        public void SetServerSearchFunction(Func<OnReadCommandsEventArgs, Task> serverSearchFunction)
        {
            this.ServerSearchFunction = serverSearchFunction;
        }
    }
}