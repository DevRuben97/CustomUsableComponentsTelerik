namespace CustomUsableComponentsTelerik.Client.Services.TourGuide.Models
{
    namespace MiAplicacion.TourGuide
    {
        /// <summary>
        /// Representa las opciones de configuración de TourGuide JS.
        /// </summary>
        public class TourGuideOptions
        {

            /// <summary>
            /// Lista por defecto de los pasos del tour.
            /// </summary>
            public List<TourGuideStep> Steps { get; set; } = new List<TourGuideStep>();

            /// <summary>
            /// Desplaza automáticamente la ventana del navegador hasta el elemento objetivo.
            /// </summary>
            public bool AutoScroll { get; set; } = true;

            /// <summary>
            /// Usa un desplazamiento suave al mover la ventana hasta el elemento objetivo.
            /// </summary>
            public bool AutoScrollSmooth { get; set; } = true;

            /// <summary>
            /// Margen en píxeles alrededor del elemento objetivo cuando se realiza el desplazamiento automático.
            /// </summary>
            public int AutoScrollOffset { get; set; } = 20;

            /// <summary>
            /// Habilita transiciones en el fondo del tour para suavizar los cambios de tamaño y posición.
            /// </summary>
            public bool BackdropAnimate { get; set; } = true;

            /// <summary>
            /// Clase CSS personalizada para el fondo del tour.
            /// </summary>
            public string? BackdropClass { get; set; }

            /// <summary>
            /// Color del fondo del tour. Se define como un valor CSS (ej. rgba, hex, etc.).
            /// </summary>
            public string BackdropColor { get; set; } = "rgba(20,20,21,0.84)";

            /// <summary>
            /// Clase CSS para aplicar transiciones en el cuadro de diálogo del tour.
            /// </summary>
            public string? DialogAnimate { get; set; }

            /// <summary>
            /// Clase CSS personalizada para el cuadro de diálogo del tour.
            /// </summary>
            public string? DialogClass { get; set; }

            /// <summary>
            /// Z-index del cuadro de diálogo del tour.
            /// </summary>
            public int DialogZ { get; set; } = 999;

            /// <summary>
            /// Ancho fijo del cuadro de diálogo en píxeles.
            /// </summary>
            public int? DialogWidth { get; set; }

            /// <summary>
            /// Ancho máximo del cuadro de diálogo en píxeles.
            /// </summary>
            public int DialogMaxWidth { get; set; } = 340;

            /// <summary>
            /// Posición del cuadro de diálogo en relación con el elemento objetivo.
            /// Puede ser "top", "right", "bottom", "left" o null para auto-ajuste.
            /// </summary>
            public string? DialogPlacement { get; set; }

            /// <summary>
            /// Espacio alrededor del elemento resaltado en píxeles.
            /// </summary>
            public string? TargetPadding { get; set; }

            /// <summary>
            /// Oculta el botón "Siguiente".
            /// </summary>
            public bool HideNext { get; set; } = false;

            /// <summary>
            /// Oculta el botón "Atrás".
            /// </summary>
            public bool HidePrev { get; set; } = false;

            /// <summary>
            /// Texto para el botón "Siguiente".
            /// </summary>
            public string NextLabel { get; set; } = "Next";

            /// <summary>
            /// Texto para el botón "Atrás".
            /// </summary>
            public string PrevLabel { get; set; } = "Back";

            /// <summary>
            /// Texto para el botón "Finalizar" que aparece en el último paso.
            /// </summary>
            public string FinishLabel { get; set; } = "Finish";

            /// <summary>
            /// Guarda el estado del tour en localStorage cuando se completa, permitiendo detectar si ya fue finalizado.
            /// </summary>
            public bool CompleteOnFinish { get; set; } = true;

            /// <summary>
            /// Permite salir del tour presionando la tecla "Escape".
            /// </summary>
            public bool ExitOnEscape { get; set; } = true;

            /// <summary>
            /// Permite salir del tour al hacer clic fuera del cuadro de diálogo.
            /// </summary>
            public bool ExitOnClickOutside { get; set; } = true;

            /// <summary>
            /// Habilita el uso de controles de teclado (flechas adelante/atrás y escape).
            /// </summary>
            public bool KeyboardControls { get; set; } = true;

            /// <summary>
            /// Muestra puntos de progreso del tour.
            /// </summary>
            public bool ShowStepDots { get; set; } = true;

            /// <summary>
            /// Ubicación de los puntos de progreso del tour. Puede ser "footer" o "body".
            /// </summary>
            public string StepDotsPlacement { get; set; } = "footer";

            /// <summary>
            /// Muestra los botones "Siguiente" y "Atrás".
            /// Puede usarse junto con HideNext y HidePrev.
            /// </summary>
            public bool ShowButtons { get; set; } = true;

            /// <summary>
            /// Muestra la cantidad de pasos completados en formato "1/5".
            /// </summary>
            public bool ShowStepProgress { get; set; } = true;

            /// <summary>
            /// Color de la barra de progreso debajo del encabezado del cuadro de diálogo.
            /// </summary>
            public string ProgressBar { get; set; } = "";

            /// <summary>
            /// Muestra un botón de "Cerrar" en la esquina superior derecha del cuadro de diálogo.
            /// </summary>
            public bool CloseButton { get; set; } = true;

            /// <summary>
            /// Guarda el paso actual y permite continuar desde el último paso cuando se reabre el tour.
            /// </summary>
            public bool RememberStep { get; set; } = false;

            /// <summary>
            /// Permite hacer clic en elementos dentro del área resaltada.
            /// </summary>
            public bool PropagateEvents { get; set; } = false;

            /// <summary>
            /// Habilita mensajes de depuración en la consola del navegador.
            /// </summary>
            public bool Debug { get; set; } = true;
        }
    }

}
