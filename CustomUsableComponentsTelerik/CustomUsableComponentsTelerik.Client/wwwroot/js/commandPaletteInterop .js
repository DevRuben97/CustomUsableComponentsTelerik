// JavaScript interop functions for Command Palette
window.commandPaletteInterop = {
    focusInput: function (inputId) {
        setTimeout(() => {
            const input = document.getElementById(inputId);
            if (input) {
                input.focus();
            }
        }, 100);
    },

    preventBodyScroll: function () {
        document.body.style.overflow = 'hidden';
    },

    enableBodyScroll: function () {
        document.body.style.overflow = '';
    },

    scrollIntoView: function (elementId) {
        setTimeout(() => {
            const element = document.getElementById(elementId);
            if (element) {
                element.scrollIntoView({ block: 'nearest' });
            }
        }, 10);
    },

    addKeyboardShortcuts: function (dotNetReference) {
        document.addEventListener('keydown', function (e) {
            // Open with '/'
            if (e.key === '/' && !isInputField(e.target)) {
                e.preventDefault();
                dotNetReference.invokeMethodAsync('OpenCommandPalette');
            }

            // Open with Cmd+K or Ctrl+K
            if ((e.key === 'k' || e.key === 'K') && (e.metaKey || e.ctrlKey)) {
                e.preventDefault();
                dotNetReference.invokeMethodAsync('OpenCommandPalette');
            }
        });

        // Helper to check if the current focus is on an input field
        function isInputField(element) {
            const tagName = element.tagName.toLowerCase();
            return (
                tagName === 'input' ||
                tagName === 'textarea' ||
                element.isContentEditable
            );
        }
    },
    // Nueva función para detectar el scroll y cargar más datos
    setupInfiniteScroll: function (dotNetRef) {
        const resultsContainer = document.querySelector('.command-results');
        if (!resultsContainer) return;

        // Eliminar evento previo si existe
        resultsContainer.removeEventListener('scroll', resultsContainer._scrollHandler);

        // Función para manejar el evento de scroll
        resultsContainer._scrollHandler = function () {
            const scrollPosition = this.scrollTop + this.clientHeight;
            const scrollHeight = this.scrollHeight;

            // Si estamos cerca del final (a 100px del final)
            if (scrollHeight - scrollPosition < 100) {
                dotNetRef.invokeMethodAsync('LoadMoreItems');
            }
        };

        // Añadir el nuevo evento
        resultsContainer.addEventListener('scroll', resultsContainer._scrollHandler);
    },

    // Función para limpiar los event listeners
    cleanupInfiniteScroll: function () {
        const resultsContainer = document.querySelector('.command-results');
        if (resultsContainer && resultsContainer._scrollHandler) {
            resultsContainer.removeEventListener('scroll', resultsContainer._scrollHandler);
            delete resultsContainer._scrollHandler;
        }
    }
};