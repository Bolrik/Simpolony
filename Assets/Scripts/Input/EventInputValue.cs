using FreschGames.Core.Input;
using UnityEngine;

namespace Simpolony
{
    [CreateAssetMenu(fileName = "EventInputValue", menuName = "Data/Input/new Event Input Value")]
    public class EventInputValue : InputValue
    {
        public new EventValue<bool> WasPressed { get; set; } = new EventValue<bool>();
        public new EventValue<bool> WasReleased { get; set; } = new EventValue<bool>();
        public new EventValue<bool> IsPressed { get; set; } = new EventValue<bool>();

        protected override void OnLink(ref InputValueUpdate action)
        {
            action += this.Update;
        }

        void Update()
        {
            this.WasPressed.Reset(base.WasPressed);
            this.WasReleased.Reset(base.WasReleased);
            this.IsPressed.Reset(base.IsPressed);
        }
    }

    public class EventValue<T>
        where T : struct
    {
        public T Value { get; set; }
        public bool Handled { get; set; }

        public void Reset(T value)
        {
            this.Value = value;
            this.Handled = false;
        }

        public static implicit operator T(EventValue<T> eventValue) => eventValue.Value;
    }
}