using System;
using UnityEngine;

namespace Simpolony.Resources
{
    [CreateAssetMenu(fileName = "ResourceManager", menuName = "Data/Resources/new Resource Manager")]
    public class ResourceManager : ScriptableObject
    {
        [field: SerializeField] public int Available { get; private set; }

        public int GetResourceCount()
        {
            return this.Available;
        }

        public void SetResources(int amount)
        {
            this.Available = amount.Abs();
        }

        public void UseResources(int amount)
        {
            this.Available -= amount.Abs();
        }

        public void AddResources(int amount)
        {
            this.Available += amount.Abs();
        }
    }
}