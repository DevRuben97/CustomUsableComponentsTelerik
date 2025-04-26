let tourGuideClient = null;
let dotnetHelper = null;

export function initialize(options, dotnetHelperInstance) {
    if (!tourGuideClient) {
        dotnetHelper = dotnetHelperInstance;
        tourGuideClient = new tourguide.TourGuideClient(options);

        tourGuideClient.onFinish(async () => {
            await dotnetHelper.invokeMethodAsync("Invoke_OnFinishTour");
        });
        tourGuideClient.onAfterExit(() => {
            dotnetHelper.invokeMethodAsync("Invoke_OnAfterExit");
        });
        tourGuideClient.onAfterStepChange(() => {
            dotnetHelper.invokeMethodAsync("Invoke_OnAfterStepChange");
        });
    }
}

export async function startTour(groupName) {
    await tourGuideClient.start(groupName);
}

export async function finishTour(groupName) {
    await tourGuideClient.finishTour(true, groupName);
}

export async function isFinished(groupName) {
    return await tourGuideClient.isFinished(groupName);
}

export async function deleteFinishedTour(groupName) {
    return await tourGuideClient.deleteFinishedTour(groupName);
}

export function addSteps(steps) {
    tourGuideClient.addSteps(steps);
}
