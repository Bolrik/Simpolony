using Misc;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Simpolony
{
    public class HealBolt : MonoBehaviour
    {
        IHealthObject HealthObject { get; set; }
        Vector3 Origin { get; set; }
        Vector3 Target { get; set; }

        float Timer { get; set; }
        float TimerDefault { get; set; }

        int HealStrength { get; set; }

        public void SetUp(Vector3 origin, Vector3 target, IHealthObject healthObject, float timer, int healStrength)
        {
            this.HealthObject = healthObject;

            this.Origin = origin;
            this.Target = target;

            Vector3 delta = (this.Target - this.Origin);

            Vector3 euler = Vector3.forward * (Random.value * 180);
            this.transform.eulerAngles = euler;

            this.Timer = 0;
            this.TimerDefault = timer;

            this.HealStrength = healStrength;

            this.transform.position = this.Origin;
            this.transform.right = delta.normalized;
        }

        private void Update()
        {
            this.Timer += Time.deltaTime;

            float percent = this.Timer / this.TimerDefault;
            float scale = percent.PingPong(.5f) / .5f;

            float sin = scale * Mathf.PI;
            float offsetPercent = Mathf.Sin(sin);


            this.transform.localScale = Vector3.one * (.1f + scale * .9f);
            Vector3 position = Vector3.Lerp(this.Origin, this.Target, percent);
            Vector3 offset = Vector3.up * offsetPercent * .5f;

            Vector3 euler = this.transform.eulerAngles;
            euler.z += Time.deltaTime * 180;
            this.transform.eulerAngles = euler;

            this.transform.position = position; // + offset;

            if (this.Timer >= this.TimerDefault)
            {
                this.HealthObject.Health.Heal(this.HealStrength);
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}