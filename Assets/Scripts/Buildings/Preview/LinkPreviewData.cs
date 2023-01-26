using UnityEngine;

namespace Simpolony.Buildings
{
    [CreateAssetMenu(fileName = "LinkPreviewData", menuName = "Data/Buildings/new Link Preview Data")]
    public class LinkPreviewData : ScriptableObject
    {
        [field: SerializeField] public LineRenderer ConnectionRenderer { get; private set; }
    }
}