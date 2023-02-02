using UnityEngine.UIElements;

namespace Simpolony.UI
{
    public abstract class UIComponent
    {
        internal abstract void Initialize(UIElement root, VisualElement visualElement);
    }
}
