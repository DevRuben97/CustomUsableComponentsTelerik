using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Telerik.SvgIcons;
using Telerik.Blazor.Components;
using System;

namespace CustomUsableComponentsTelerik.Client.Components.CodeInput
{
    public partial class CodeInput
    {
        [Parameter] public int Count { get; set; } = 6;
        [Parameter] public string? Value { get; set; }
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public bool Enabled { get; set; } = true;
        [Parameter] public bool IsReadOnly { get; set; } = false;
        [Parameter] public int Gap { get; set; } = 5;

        private string[] _values;

        private List<TelerikTextBox?> _elementReferences = new List<TelerikTextBox?>();

        private int _lastFocusedIndex = 0;

        bool _skipInputEvent = false;
        bool _skipRefocus = false;

        private string _theValue => string.Join("", _values.Take(Count).Where(s => !string.IsNullOrEmpty(s)));

        protected override void OnInitialized()
        {
            _values = new string[Count];
            _elementReferences = new List<TelerikTextBox?>(new TelerikTextBox?[Count+1]);
        }

        protected override void OnParametersSet()
        {
            UpdateValuesFromOutside(Value);
        }

        void UpdateValuesFromOutside(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (value.Length > Count)
            {
                value = value.Substring(0, Count);
            }
            for (int i = 0; i < value.Length; i++)
            {
                _values[i] = value[i].ToString();
            }
        }

        private async Task HandleInput(string value)
        {
            _values[_lastFocusedIndex] = value;
            if (_skipInputEvent)
            {
                _skipInputEvent = false;
                return;
            }
            await FocusNext();
        }

        async Task HandleKeyDown(KeyboardEventArgs arg)
        {
            if (!Enabled || IsReadOnly)
            {
                return;
            }

            if (arg.Key == "Backspace" || arg.Key == "ArrowLeft" || arg.Key == "Delete")
            {
                _skipInputEvent = true;
                _skipRefocus = true;
                if (arg.Key == "Delete")
                {
                    _values[_lastFocusedIndex] = string.Empty;
                    _skipInputEvent = false;
                }

                await Task.Delay(10);

                await FocusPrevious();
                return;
            }

            if (arg.Key == "ArrowRight")
            {
                await Task.Delay(10);
                await FocusNext();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        protected async Task CheckFocus(int count)
        {
            _lastFocusedIndex = count;
            if (_skipRefocus == true)
            {
                _skipRefocus = false;
                return;
            }
            string str = _theValue ?? string.Empty;


            if (_elementReferences[str.Length] == null)
            {
                return;
            }

            await _elementReferences[str.Length].FocusAsync();
        }


        async Task UpdateValue()
        {
            await ValueChanged.InvokeAsync(_theValue);
        }

        /// <summary>
        /// Focuses next input box.
        /// </summary>
        /// <returns></returns>
        public async Task FocusNext()
        {
            if (_lastFocusedIndex >= Count - 1)
            {
                _elementReferences[_lastFocusedIndex].TextBoxBase_Blur();
                await _elementReferences[_lastFocusedIndex].FocusAsync();
                return;
            }
            await _elementReferences[_lastFocusedIndex + 1].FocusAsync();
        }

        /// <summary>
        /// Focuses previous input box.
        /// </summary>
        /// <returns></returns>
        public async Task FocusPrevious()
        {
            if (_lastFocusedIndex == 0)
            {
                _elementReferences[_lastFocusedIndex].TextBoxBase_Blur();
                await _elementReferences[_lastFocusedIndex].FocusAsync();
                return;
            }
            await _elementReferences[_lastFocusedIndex - 1].FocusAsync();
        }
    }
}
