using FreschGames.Core.Misc;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony.Buildings
{
    public class LinkPreview : MonoBehaviour
    {
        [field: SerializeField, Header("Components")] private LinkCheckComponent LinkCheck { get; set; }

        [field: SerializeField, Header("References")] private LineRenderer LineRenderer { get; set; }

        [field: SerializeField, Header("Data")] private LinkPreviewData Data { get; set; }


        List<Building> Available { get; set; } = new List<Building>();

        public Transform Origin { get; private set; }
        public Building Target { get; private set; }

        public int Count { get => this.Available.Count; }

        int SelectionIndex { get; set; }

        public bool RenderPreview { get; private set; } = true;


        private void Awake()
        {
            this.LinkCheck.ProxyCollider.OnTriggerEnter2DEvent += this.OnLinkProxyTriggerEnter2D;
            this.LinkCheck.ProxyCollider.OnTriggerExit2DEvent += this.OnLinkProxyTriggerExit2D;

            this.LineRenderer = GameObject.Instantiate(this.Data.ConnectionRenderer);
            this.LineRenderer.transform.SetParent(this.transform);

            this.Origin = this.transform;
        }

        private void Update()
        {
            int max = this.Available.Count;

            if (max == 0)
            {
                this.LineRenderer.enabled = false;
                this.Target = null;
                return;
            }

            this.LineRenderer.enabled = true;

            //Vector2 scrollValue = this.ScrollValue.Read<Vector2>();

            //if (scrollValue.y > 0)
            //    this.SelectionIndex++;
            //else if (scrollValue.y < 0)
            //    this.SelectionIndex--;

            this.SelectionIndex = this.SelectionIndex.Loop(max);

            this.Target = this.Available[this.SelectionIndex];
            this.UpdateLine();
        }

        public bool Contains(Building building)
        {
            return this.Available.Contains(building);
        }

        public void SetOrigin(Transform transform)
        {
            this.Origin = transform;
        }

        public void ChangeIndex(int change)
        {
            this.SelectionIndex += change;
        }

        public void SetRenderPreview(bool value)
        {
            this.RenderPreview = value;
        }

        private void OnLinkProxyTriggerEnter2D(Collider2D collision)
        {
            Building building = collision.transform.GetProxyComponent<Building>();

            if (building == null)
                return;

            this.Available.Add(building);

            //Debug.Log($"(+) Link: {this.LinkList.Count}");
        }

        private void OnLinkProxyTriggerExit2D(Collider2D collision)
        {
            Building building = collision.transform.GetProxyComponent<Building>();

            if (building == null)
                return;

            this.Available.Remove(building);

            //Debug.Log($"(-) Link: {this.LinkList.Count}");
        }

        private void UpdateLine()
        {
            if (this.RenderPreview)
            {
                this.LineRenderer.enabled = true;

                this.LineRenderer.SetPosition(0, this.Origin.position);
                this.LineRenderer.SetPosition(1, (this.Origin.position + this.Target.transform.position) / 2f);
                this.LineRenderer.SetPosition(2, this.Target.transform.position);
            }
            else
            {
                this.LineRenderer.enabled = false;
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

    //public class BuildingPreview : MonoBehaviour
    //{
    //    [field: SerializeField, Header("References")] private GameData GameData { get; set; }
    //    [field: SerializeField] private BuildingPreviewData BuildingPreviewData { get; set; }
    //    [field: SerializeField] private MeshRenderer PreviewRenderer { get; set; }

    //    [field: SerializeField] private InputValue ScrollValue { get; set; }


    //    [field: SerializeField, Header("Components")] private BlockCheckComponent BlockCheck { get; set; }
    //    [field: SerializeField] private LinkCheckComponent LinkCheck { get; set; }



    //    List<Transform> BlockList { get; set; } = new List<Transform>();
    //    List<Building> LinkList { get; set; } = new List<Building>();

    //    public Building DesiredLink { get; private set; }

    //    private LineRenderer LinkRenderer { get; set; }
    //    int LinkIndex { get; set; } = 0;

    //    public bool IsValid { get; private set; }


    //    private void Awake()
    //    {
    //        this.BlockCheck.ProxyCollider.OnTriggerEnter2DEvent += this.OnBlockProxyTriggerEnter2D;
    //        this.BlockCheck.ProxyCollider.OnTriggerExit2DEvent += this.OnBlockProxyTriggerExit2D;

    //        this.LinkCheck.ProxyCollider.OnTriggerEnter2DEvent += this.OnLinkProxyTriggerEnter2D;
    //        this.LinkCheck.ProxyCollider.OnTriggerExit2DEvent += this.OnLinkProxyTriggerExit2D;

    //        this.LinkRenderer = GameObject.Instantiate(this.BuildingPreviewData.ConnectionRenderer);
    //        this.LinkRenderer.transform.SetParent(this.transform);
    //    }

    //    private void Update()
    //    {
    //        this.UpdateIsValid();

    //        int max = this.LinkList.Count;

    //        if (max == 0)
    //        {
    //            this.LinkRenderer.enabled = false;
    //            this.DesiredLink = null;
    //            return;
    //        }

    //        this.LinkRenderer.enabled = true;
    //        Vector2 scrollValue = this.ScrollValue.Read<Vector2>();

    //        if (scrollValue.y > 0)
    //            this.LinkIndex++;
    //        else if (scrollValue.y < 0)
    //            this.LinkIndex--;

    //        this.LinkIndex = this.LinkIndex.Loop(max);

    //        this.DesiredLink = this.LinkList[this.LinkIndex];
    //        this.UpdateLine(this.LinkRenderer);
    //    }

    //    private void UpdateIsValid()
    //    {
    //        bool isBlocked = this.BlockList.Count > 0;
    //        bool isLinked = this.LinkList.Count > 0;

    //        this.IsValid = !isBlocked && isLinked;

    //        this.BlockCheck.Renderer.material.color = isBlocked ? this.BuildingPreviewData.InvalidColor : this.BuildingPreviewData.ValidColor;
    //        this.BlockCheck.SetVisible(isBlocked || this.GameData.ShowValidBuildingPreviewBlockedCheck);

    //        this.PreviewRenderer.material.color = this.IsValid ? this.BuildingPreviewData.ValidColor : this.BuildingPreviewData.InvalidColor;
    //    }


    //    #region Proxy Trigger
    //    private void OnBlockProxyTriggerEnter2D(Collider2D collision)
    //    {
    //        this.BlockList.Add(collision.transform);

    //        //Debug.Log($"(+) Block: {this.BlockList.Count}");
    //    }

    //    private void OnBlockProxyTriggerExit2D(Collider2D collision)
    //    {
    //        this.BlockList.Remove(collision.transform);

    //        //Debug.Log($"(-) Block: {this.BlockList.Count}");
    //    }

    //    private void OnLinkProxyTriggerEnter2D(Collider2D collision)
    //    {
    //        Building building = collision.transform.GetProxyComponent<Building>();

    //        if (building == null)
    //            return;

    //        this.LinkList.Add(building);

    //        //Debug.Log($"(+) Link: {this.LinkList.Count}");
    //    }

    //    private void OnLinkProxyTriggerExit2D(Collider2D collision)
    //    {
    //        Building building = collision.transform.GetProxyComponent<Building>();

    //        if (building == null)
    //            return;

    //        this.LinkList.Remove(building);

    //        //Debug.Log($"(-) Link: {this.LinkList.Count}");
    //    }
    //    #endregion

    //    private void UpdateLine(LineRenderer renderer)
    //    {
    //        renderer.SetPosition(0, this.transform.position);
    //        renderer.SetPosition(1, (this.transform.position + this.DesiredLink.transform.position) / 2f);
    //        renderer.SetPosition(2, this.DesiredLink.transform.position);
    //    }

    //    public void SetPosition(Vector2 worldPosition)
    //    {
    //        this.transform.position = worldPosition;
    //    }

    //    public void Destroy()
    //    {
    //        GameObject.Destroy(this.gameObject);
    //    }

    //    [System.Serializable]
    //    class BlockCheckComponent
    //    {
    //        [field: SerializeField] public MeshRenderer Renderer { get; private set; }

    //        [field: SerializeField] public ProxyCollider ProxyCollider { get; private set; }

    //        public void SetColor(Color color)
    //        {
    //            this.Renderer.material.color = color;
    //        }

    //        public void SetVisible(bool visible)
    //        {
    //            this.Renderer.enabled = visible;
    //        }
    //    }

    //    [System.Serializable]
    //    class LinkCheckComponent
    //    {
    //        [field: SerializeField] public MeshRenderer Renderer { get; private set; }

    //        [field: SerializeField] public ProxyCollider ProxyCollider { get; private set; }

    //        public void SetColor(Color color)
    //        {
    //            this.Renderer.material.color = color;
    //        }

    //        public void SetVisible(bool visible)
    //        {
    //            this.Renderer.enabled = visible;
    //        }
    //    }
    //}
}