using UnityEngine.UIElements;

namespace Simpolony.UI.SidePanelElements
{
    public abstract class UIComponent
    {
        internal abstract void Initialize(UIElement root, VisualElement visualElement);
    }
}
