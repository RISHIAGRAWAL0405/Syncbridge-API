namespace PhotographyPortfolioAPI.Core.Enums.Project
{
    // Named TaskItemStatus (not TaskStatus) to avoid clashing with System.Threading.Tasks.TaskStatus
    public enum TaskItemStatus
    {
        NEW,
        IN_PROGRESS,
        ON_HOLD,
        COMPLETED,
        CANCELLED
    }
}
