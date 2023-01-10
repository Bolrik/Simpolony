using FreschGames.Core.Input;
using FreschGames.Core.Misc;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Simpolony.Buildings
{
    public class BuildingPreview : MonoBehaviour
    {
        [field: SerializeField, Header("References")] private GameData GameData { get; set; }
        [field: SerializeField] private BuildingPreviewData BuildingPreviewData { get; set; }
        [field: SerializeField] private MeshRenderer PreviewRenderer { get; set; }

        [field: SerializeField] private InputValue ScrollValue { get; set; }


        [field: SerializeField, Header("Components")] private BlockCheckComponent BlockCheck { get; set; }
        [field: SerializeField] private LinkCheckComponent LinkCheck { get; set; }



        List<Transform> BlockList { get; set; } = new List<Transform>();
        List<Building> LinkList { get; set; } = new List<Building>();

        public Building DesiredLink { get; private set; }

        private LineRenderer LinkRenderer { get; set; }
        int LinkIndex { get; set; } = 0;

        public bool IsValid { get; private set; }


        private void Awake()
        {
            this.BlockCheck.ProxyCollider.OnTriggerEnter2DEvent += this.OnBlockProxyTriggerEnter2D;
            this.BlockCheck.ProxyCollider.OnTriggerExit2DEvent += this.OnBlockProxyTriggerExit2D;

            this.LinkCheck.ProxyCollider.OnTriggerEnter2DEvent += this.OnLinkProxyTriggerEnter2D;
            this.LinkCheck.ProxyCollider.OnTriggerExit2DEvent += this.OnLinkProxyTriggerExit2D;

            this.LinkRenderer = GameObject.Instantiate(this.BuildingPreviewData.ConnectionRenderer);
            this.LinkRenderer.transform.SetParent(this.transform);
        }

        private void Update()
        {
            this.UpdateIsValid();

            int max = this.LinkList.Count;

            if (max == 0)
            {
                this.LinkRenderer.enabled = false;
                this.DesiredLink = null;
                return;
            }

            this.LinkRenderer.enabled = true;
            Vector2 scrollValue = this.ScrollValue.Read<Vector2>();

            if (scrollValue.y > 0)
                this.LinkIndex++;
            else if (scrollValue.y < 0)
                this.LinkIndex--;

            this.LinkIndex = this.LinkIndex.Loop(max);

            this.DesiredLink = this.LinkList[this.LinkIndex];
            this.UpdateLine(this.LinkRenderer);
        }

        private void UpdateIsValid()
        {
            bool isBlocked = this.BlockList.Count > 0;
            bool isLinked = this.LinkList.Count > 0;

            this.IsValid = !isBlocked && isLinked;

            this.BlockCheck.Renderer.material.color = isBlocked ? this.BuildingPreviewData.InvalidColor : this.BuildingPreviewData.ValidColor;
            this.BlockCheck.SetVisible(isBlocked || this.GameData.ShowValidBuildingPreviewBlockedCheck);

            this.PreviewRenderer.material.color = this.IsValid ? this.BuildingPreviewData.ValidColor : this.BuildingPreviewData.InvalidColor;
        }


        #region Proxy Trigger
        private void OnBlockProxyTriggerEnter2D(Collider2D collision)
        {
            this.BlockList.Add(collision.transform);

            Debug.Log($"(+) Block: {this.BlockList.Count}");
        }

        private void OnBlockProxyTriggerExit2D(Collider2D collision)
        {
            this.BlockList.Remove(collision.transform);

            Debug.Log($"(-) Block: {this.BlockList.Count}");
        }

        private void OnLinkProxyTriggerEnter2D(Collider2D collision)
        {
            Building building = this.GetComponent<Building>(collision.transform);

            if (building == null)
                return;

            this.LinkList.Add(building);

            Debug.Log($"(+) Link: {this.LinkList.Count}");
        }

        private void OnLinkProxyTriggerExit2D(Collider2D collision)
        {
            Building building = this.GetComponent<Building>(collision.transform);

            if (building == null)
                return;

            this.LinkList.Remove(building);

            Debug.Log($"(-) Link: {this.LinkList.Count}");
        }
        #endregion

        private void UpdateLine(LineRenderer renderer)
        {
            renderer.SetPosition(0, this.transform.position);
            renderer.SetPosition(1, (this.transform.position + this.DesiredLink.transform.position) / 2f);
            renderer.SetPosition(2, this.DesiredLink.transform.position);
        }

        public void SetPosition(Vector2 worldPosition)
        {
            this.transform.position = worldPosition;
        }

        public void Destroy()
        {
            GameObject.Destroy(this.gameObject);
        }

        private T GetComponent<T>(Transform transform)
            where T : Component
        {
            if (transform.GetComponent<T>() is T t1)
            {
                return t1;
            }

            if (transform.GetComponent<ProxyCollider>() is ProxyCollider proxy && proxy.Proxy.GetComponent<T>() is T t2)
            {
                return t2;
            }

            return null;
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