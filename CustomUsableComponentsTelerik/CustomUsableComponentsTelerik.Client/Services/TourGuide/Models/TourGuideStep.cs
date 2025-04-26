using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace CustomUsableComponentsTelerik.Client.Services.TourGuide.Models
{
    public class TourGuideStep
    {
        [JsonIgnore]
        public string Id { get; set; } = System.Guid.NewGuid().ToString();

        /// <summary>
        /// Titutlo del paso
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Contenido del paso
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// Selector del elemento objetivo
        /// </summary>
        /// 
        [JsonPropertyName("target")]
        public string? TargetSelector { get; set; }
        /// <summary>
        /// Orden del paso
        /// </summary>
        public int? Order { get; set; }
        /// <summary>
        /// Sirve para agrupar los pasos que pertenecen a una misma pagina
        /// </summary>
        public string? Group { get; set; } = "guias";

        /// <summary>
        /// Indica si el elemento objetivo es fijo
        /// </summary>
        public bool Fixed { get; set; } = false;


  
    }
}
