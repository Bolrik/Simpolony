using Misc;
using Simpolony.Projectiles;
using System;
using UnityEngine;

namespace Simpolony
{
    public class DisarmRay : MonoBehaviour
    {
        [field: SerializeField] private Transform Pivot { get; set; }

        Vector3 Origin { get; set; }
        Rocket Target { get; set; }

        float Timer { get; set; }
        float TimerDefault { get; set; }

        public void SetUp(Vector3 origin, Rocket target, float timer)
        {
            this.Origin = origin;
            this.Target = target;

            this.Timer = 0;
            this.TimerDefault = timer;

            this.transform.position = this.Origin;
        }

        private void Update()
        {
            if (this.Target.IsDestroyed)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            this.Timer += Time.deltaTime;

            float percent = this.Timer / this.TimerDefault;

            this.AimAtTarget(percent);

            Vector3 euler = this.Pivot.localEulerAngles;
            euler.x += Time.deltaTime * 100f;
            this.Pivot.localEulerAngles = euler;

            // this.Pivot.localScale = new Vector3(0f.Lerp(this.Distance, percent), 1, 1);


            if (this.Timer >= this.TimerDefault)
            {
                this.Target.Destroy();

                GameObject.Destroy(this.gameObject);
            }
        }

        private void AimAtTarget(float scale)
        {
            Vector3 delta = (this.Target.transform.position - this.Origin);
            this.Pivot.localScale = new Vector3(delta.magnitude, scale, scale);
            this.transform.right = delta.normalized;
        }
    }
}