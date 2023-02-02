using FreschGames.Core.Input;
using Simpolony.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simpolony
{
    public class GameCamera : MonoBehaviour
    {
        [field: SerializeField, Header("References")] private Camera Camera { get; set; }
        [field: SerializeField] private Transform Pivot { get; set; }

        [field: SerializeField, Header("Data")] private GameData GameData { get; set; }

        private GameCameraData Data { get => this.GameData.GameCameraData; }
        private GameDataInput Input { get => this.GameData.Input; }
        
        InputValue ResetCamera { get => this.Input.ResetCamera; }
        InputValue CameraMovement { get => this.Input.CameraMovement; }
        InputValue ScrollValue { get => this.Input.ScrollValue; }
        InputValue SecondaryButton { get => this.Input.SecondaryButton; }
        InputValue ViewPosition { get => this.Input.ViewPosition; }

        Vector3 CurrentViewPosition { get; set; }
        Vector3 PreviousViewPosition { get; set; }

        // Game State
        private GameStateManager StateManager { get => this.GameData.GameStateManager; }
        private GameStateManagerData StateData { get => this.GameData.GameStateManagerData; }
        private GameState State { get => this.StateData.State; }


        public float ZoomValue
        {
            get { return this.Camera.orthographicSize; }
            set { this.Camera.orthographicSize = value; }
        }

        public void Shake()
        {
            this.Data.SetShakeIntensity(1f);
        }

        void Update()
        {
            this.UpdateValues();
            this.UpdateState();


            this.Pivot.position = Vector3.zero;
            this.Data.WorldPosition = this.Camera.ScreenToWorldPoint(this.Input.ViewPosition.Read<Vector2>());
            this.UpdateScreenShake();

        }

        private void UpdateScreenShake()
        {
            if (!this.Data.IsScreenShakeActive)
                return;

            float shakeIntensity = this.Data.ShakeIntensity - (this.Data.ShakeIntensity * Time.deltaTime * this.Data.CameraShakeDropSpeed);

            if (this.Data.ShakeIntensity <= float.Epsilon)
                shakeIntensity = 0;

            this.Data.SetShakeIntensity(shakeIntensity);

            Vector2 shake = Random.insideUnitCircle * this.Data.ShakeIntensity;
            this.Pivot.position = new Vector3(this.Pivot.position.x + shake.x, this.Pivot.position.y + shake.y, this.Pivot.position.z);
        }

        private void UpdateValues()
        {
            this.PreviousViewPosition = this.CurrentViewPosition;
            this.CurrentViewPosition = this.ViewPosition.Read<Vector2>();


            if (this.SecondaryButton.WasPressed)
            {
                this.PreviousViewPosition = this.CurrentViewPosition;
            }

        }

        private void UpdateState()
        {
            if (
                this.State != GameState.Idle &&
                this.State != GameState.Building &&
                this.State != GameState.Connecting)
            {
                return;
            }

            this.UpdateMouseMovement();
            this.UpdateMovement();

            if (this.State != GameState.Building)
                this.UpdateZoom();
        }

        private void UpdateZoom()
        {
            if (this.ScrollValue.IsPressed)
            {
                float zoom = this.ScrollValue.Read<Vector2>().y * this.Data.ZoomSpeed;

                float zoomFactor = 1;

                float zoomDelta = this.Data.ZoomConstraints.Max - this.Data.ZoomConstraints.Min;

                if (this.ZoomValue > zoomDelta * .66f)
                {
                    zoomFactor = 2;
                }
                else if (this.ZoomValue < zoomDelta * .33f)
                {
                    zoomFactor = 0.5f;
                }

                float newOrthoSize = this.ZoomValue - zoom * zoomFactor * Time.deltaTime;
                this.ZoomValue = Mathf.Clamp(newOrthoSize, this.Data.ZoomConstraints.Min, this.Data.ZoomConstraints.Max);
            }
        }

        private void UpdateMouseMovement()
        {
            // this.State == GameState.Idle &&
            if (this.SecondaryButton.IsPressed)
            {
                Vector2 read = this.PreviousViewPosition - this.CurrentViewPosition;
                Vector3 change = new Vector3(read.x, read.y) * this.Data.MouseCameraSpeed * this.ZoomValue * Time.deltaTime;
                if (change.magnitude > this.Data.CameraMouseFrameRange)
                {
                    change = change.normalized * this.Data.CameraMouseFrameRange;
                }
                this.transform.position += change;
            }
        }

        private void UpdateMovement()
        {
            if (this.GameData.BuildingManager.Buildings.Count > 0 && this.ResetCamera.WasPressed)
            {
                Vector3 position = Vector3.zero;
                foreach (var building in this.GameData.BuildingManager.Buildings)
                {
                    position = building.Value.transform.position;
                    break;

                }

                position.z = this.transform.position.z;

                this.transform.position = position;
            }
            else if (this.CameraMovement.IsPressed)
            {
                Vector2 read = this.Input.CameraMovement.Read<Vector2>();
                this.transform.position += new Vector3(read.x, read.y) * this.Data.CameraSpeed * this.ZoomValue * Time.deltaTime;
            }
        }
    }
}