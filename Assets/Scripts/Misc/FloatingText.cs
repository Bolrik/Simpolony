using System;
using TMPro;
using UnityEngine;

namespace Simpolony.Misc
{
    public class FloatingText : MonoBehaviour
    {
        [field: SerializeField] private TMP_Text Text { get; set; }
        
        private Vector3 Velocity { get; set; }

        private float TerminationTime { get; set; }
        float Alpha { get; set; }


        public void SetText(Vector3 origin, string text, Color color, Vector3 velocity, float duration)
        {
            this.transform.position = origin;

            this.Text.text = text;
            this.Text.color = color;
            this.Alpha = color.a;

            this.Velocity = velocity;

            this.TerminationTime = Time.time + duration;
        }

        private void Update()
        {
            this.transform.position += (
                (this.transform.up * this.Velocity.y) +
                (this.transform.right * this.Velocity.x) +
                (this.transform.forward * this.Velocity.z)) * Time.deltaTime;

            float delta = (this.TerminationTime - Time.time).ClampMax(1);

            Color color = this.Text.color;
            color.a = delta * this.Alpha;
            this.Text.color = color;

            if (this.TerminationTime <= Time.time)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
