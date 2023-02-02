namespace Simpolony.Selection
{
    public class SelectionEvent
    {
        private ISelectable Selectable { get; set; }

        public SelectionEvent(ISelectable selectable)
        {
            this.Selectable = selectable;
        }

        public bool Get<T>(out T value)
            where T : class
        {
            if (this.Selectable is T toReturn)
            {
                value = toReturn;
                return true;
            }

            value = null;
            return false;
        }
    }
}