namespace Simpolony.Selection
{
    public interface ISelectable
    {
        bool IsSelectable { get; }
        void SetSelected(bool state);
    }
}