using FreschGames.Core.Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class BuildingPreview : MonoBehaviour
    {
        [field: SerializeField, Header("References")] private GameData GameData { get; set; }
        [field: SerializeField] private BuildingPreviewData BuildingPreviewData { get; set; }

        [field: SerializeField, Header("Components")] private BlockCheckComponent BlockCheck { get; set; }
        [field: SerializeField] private LinkCheckComponent LinkCheck { get; set; }



        List<Transform> BlockList { get; set; } = new List<Transform>();

        public bool IsValid { get; private set; }


        private void Awake()
        {
            this.BlockCheck.ProxyCollider.OnTriggerEnter2DEvent += this.OnProxyTriggerEnter2D;
            this.BlockCheck.ProxyCollider.OnTriggerExit2DEvent += this.OnProxyTriggerExit2D;
        }

        private void Update()
        {
            this.IsValid = this.BlockList.Count == 0;

            if (this.IsValid)
            {
                this.BlockCheck.Renderer.material.color = this.BuildingPreviewData.ValidColor;
                
                this.BlockCheck.SetVisible(this.GameData.ShowValidBuildingPreviewBlockedCheck);
            }
            else
            {
                this.BlockCheck.Renderer.material.color = this.BuildingPreviewData.InvalidColor;
                this.BlockCheck.SetVisible(true);
            }
        }

        #region Proxy Trigger
        private void OnProxyTriggerEnter2D(Collider2D collision)
        {
            this.BlockList.Add(collision.transform);

            Debug.Log($"(+) Collisions: {this.BlockList.Count}");

            //var collisionPoint = collision.ClosestPoint(transform.position);
            //DebugAssistant.Instance.ShowSphereAt(collisionPoint, Vector3.one * .1f, 4);
        }

        private void OnProxyTriggerExit2D(Collider2D collision)
        {
            this.BlockList.Remove(collision.transform);

            Debug.Log($"(-) Collisions: {this.BlockList.Count}");
        }
        #endregion

        public void SetPosition(Vector2 worldPosition)
        {
            this.transform.position = worldPosition;
        }

        [System.Serializable]
        class BlockCheckComponent
        {
            [field: SerializeField] public MeshRenderer Renderer { get; private set; }

            [field: SerializeField] public ProxyCollider ProxyCollider { get; private set; }

            public void SetColor(Color color)
            {
                this.Renderer.material.color = color;
            }

            public void SetVisible(bool visible)
            {
                this.Renderer.enabled = visible;
            }
        }

        [System.Serializable]
        class LinkCheckComponent
        {
            [field: SerializeField] public MeshRenderer Renderer { get; private set; }

            [field: SerializeField] public ProxyCollider ProxyCollider { get; private set; }

            public void SetColor(Color color)
            {
                this.Renderer.material.color = color;
            }

            public void SetVisible(bool visible)
            {
                this.Renderer.enabled = visible;
            }
        }
    }
}