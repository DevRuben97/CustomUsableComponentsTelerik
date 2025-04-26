using CustomUsableComponentsTelerik.Client.Services.TourGuide.Models;

namespace CustomUsableComponentsTelerik.Client.Components.TourGuide.Interfaces
{
    public interface IPageTourGuideContainer: IAsyncDisposable
    {
        public string PageName { get; }

        public void AddStep(TourGuideStep step);

        public void RemoveStep(string targetSelector);
    }
}
