using CustomUsableComponentsTelerik.Client.Services.TourGuide.Models;
using CustomUsableComponentsTelerik.Client.Services.TourGuide.Models.MiAplicacion.TourGuide;

namespace CustomUsableComponentsTelerik.Client.Services
{
    public interface ITourGuideService: IAsyncDisposable
    {
        /// <summary>
        /// Evento que se dispara cuando se finaliza el recorrido de guia
        /// </summary>
        public event Action OnFinish;

        /// <summary>
        /// Evento que se dispara cuando la guia es cerrada
        /// </summary>
        public event Action OnAfterExit;

        /// <summary>
        /// Evento que se dispara cuando se cambia de un paso a otro
        /// </summary>
        public event Action OnAfterStepChange;

        /// <summary>
        /// Inicializa el servicio con las opciones suministradas
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task InitializeOptionsAsync(Action<TourGuideOptions> optionsCt);

        /// <summary>
        /// Agrega pasos al recorrido de guia
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        Task AddStepsAsync(IEnumerable<TourGuideStep> steps);

        /// <summary>
        /// Inicia el recorrido de guia del grupo especificado. Si un grupo no es especificado , se inicia el recorrido de guia de todos los pasos configurados
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task StartTourAsync(string? group= null);

        /// <summary>
        /// Valida si el grupo de pasos especificados ya fue finalizado. Dejar el grupo en blanco o nulo para validar si todos los pasos fueron finalizados
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>

        Task<bool> IsFinishedAsync(string? group = null);

        /// <summary>
        /// Finaliza el recorrido de guia del grupo especificado. Si un grupo no es especificado , se finalizan todos los recorridos de guia
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task FinishTourAsync(string? group = null);

        /// <summary>
        /// Elimina el recorrido de guia finalizado del grupo especificado. Si un grupo no es especificado , se eliminan todos los recorridos de guia finalizados
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task DeleteFinishedTourAsync(string? group = null);
    }
}
