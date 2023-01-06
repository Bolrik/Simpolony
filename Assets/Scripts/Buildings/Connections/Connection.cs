using UnityEngine;

namespace Simpolony.Buildings
{
    public class Connection : MonoBehaviour
    {
        [field: SerializeField] public Building Origin { get; private set; }
        [field: SerializeField] public Building Target { get; private set; }

        public int FromNode { get; private set; }
        public int ToNode { get; private set; }

        public void SetNodes(int from, int to)
        {
            this.FromNode = from;
            this.ToNode = to;
        }
    }

}