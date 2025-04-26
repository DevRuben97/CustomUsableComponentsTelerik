using CustomUsableComponentsTelerik.Client.Services.TourGuide.Models;
using CustomUsableComponentsTelerik.Client.Services.TourGuide.Models.MiAplicacion.TourGuide;
using Microsoft.JSInterop;
using System.Text.Json;

namespace CustomUsableComponentsTelerik.Client.Services.TourGuide
{
    public class TourGuideService: ITourGuideService
    {
        private IJSRuntime _jsRuntime;
        private IJSObjectReference? _module;
        private DotNetObjectReference<TourGuideService>? _dotNetRef;
        private bool _isInitialized = false;

        public event Action OnFinish;
        public event Action OnAfterExit;
        public event Action OnAfterStepChange;

        public TourGuideService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeOptionsAsync(Action<TourGuideOptions> optionsCtr)
        {
            var options = new TourGuideOptions();
            optionsCtr(options);

            if (!_isInitialized)
            {
                _module ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/tourguideInterop.js?v=12");
                _dotNetRef ??= DotNetObjectReference.Create(this);

                var jsonOptions = JsonSerializer.Serialize(options);
                await _module.InvokeVoidAsync("initialize", jsonOptions, _dotNetRef);

                _isInitialized = true;
            }
        }

        public async Task DeleteFinishedTourAsync(string? group = null)
        {
           await _module.InvokeVoidAsync("deleteFinishedTour", group);
        }

        public async Task<bool> IsFinishedAsync(string? group = null)
        {
           return await _module.InvokeAsync<bool>("isFinished", group);
        }

        public async Task StartTourAsync(string? group)
        {
            await _module.InvokeVoidAsync("startTour", group);
        }

        public async Task FinishTourAsync(string? group = null)
        {
          await _module.InvokeVoidAsync("finishTour", group);
        }

        public async Task AddStepsAsync(IEnumerable<TourGuideStep> steps)
        {
           await _module.InvokeVoidAsync("addSteps", steps);
        }

        #region Métodos - Captura de Eventos

        [JSInvokable]
        public void Invoke_OnFinishTour()
        {
            OnFinish?.Invoke();
        }
        [JSInvokable]
        public void Invoke_OnAfterExit()
        {
            OnAfterExit?.Invoke();
        }
        [JSInvokable]
        public void Invoke_OnAfterStepChange()
        {
            OnAfterStepChange?.Invoke();
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null)
            {
                await _module.DisposeAsync();
            }
            _dotNetRef?.Dispose();
            GC.SuppressFinalize(this);

        }

        #endregion
    }
}
