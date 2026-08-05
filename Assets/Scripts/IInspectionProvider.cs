public interface IInspectionProvider
{
    bool CanInspect { get; }

    InspectionData GetInspectionData();
}