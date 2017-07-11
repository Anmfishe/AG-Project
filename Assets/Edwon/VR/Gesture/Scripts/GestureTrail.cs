﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Edwon.VR.Gesture
{

    public class GestureTrail : MonoBehaviour
    {
        CaptureHand registeredHand;
        int lengthOfLineRenderer = 50;
        List<Vector3> displayLine;
        LineRenderer currentRenderer;
        Color initialColor, finalColor;

        public bool listening = false;

        bool currentlyInUse = false;

        // Use this for initialization
        void Start()
        {
            //Added to pass allow for passing of colors.
            if (initialColor == null) initialColor = Color.red;
            if (finalColor == null) finalColor = Color.blue;

            currentlyInUse = true;
            displayLine = new List<Vector3>();
            currentRenderer = CreateLineRenderer(initialColor, finalColor);
        }

        public void UpdateRenderer(Color color1, Color color2)
        {
            print(color1 + " " + color2);
            initialColor = color1;
            finalColor = color2;
            if (currentRenderer != null)
            {
                //currentRenderer.SetColors(initialColor, finalColor);
                currentRenderer.startColor = initialColor;
                currentRenderer.endColor = finalColor;
                print("WTF" + currentRenderer.startColor);
            }
        }

        void OnEnable()
        {
            if (registeredHand != null)
            {
                SubscribeToEvents();
            }
        }

        void SubscribeToEvents()
        {
            registeredHand.StartCaptureEvent += StartTrail;
            registeredHand.ContinueCaptureEvent += CapturePoint;
            registeredHand.StopCaptureEvent += StopTrail;
        }

        void OnDisable()
        {
            if (registeredHand != null)
            {
                UnsubscribeFromEvents();
            }
        }

        void UnsubscribeFromEvents()
        {
            registeredHand.StartCaptureEvent -= StartTrail;
            registeredHand.ContinueCaptureEvent -= CapturePoint;
            registeredHand.StopCaptureEvent -= StopTrail;
        }

        void UnsubscribeAll()
        {

        }

        void OnDestroy()
        {
            currentlyInUse = false;
        }

        LineRenderer CreateLineRenderer(Color c1, Color c2)
        {
            GameObject myGo = new GameObject("Trail Renderer");
            myGo.transform.parent = transform;
            myGo.transform.localPosition = Vector3.zero;

            LineRenderer lineRenderer = myGo.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            //lineRenderer.material = new Material(Shader.Find("VertexColor"));
            lineRenderer.SetColors(c1, c2);
            lineRenderer.SetWidth(0.01F, 0.05F);
            lineRenderer.SetVertexCount(0);
            return lineRenderer;
        }

        public void RenderTrail(LineRenderer lineRenderer, List<Vector3> capturedLine)
        {
            if (capturedLine.Count == lengthOfLineRenderer)
            {
                lineRenderer.SetVertexCount(lengthOfLineRenderer);
                lineRenderer.SetPositions(capturedLine.ToArray());
            }
        }

        public void StartTrail()
        {
            currentRenderer.SetColors(initialColor, finalColor);
            displayLine.Clear();
            listening = true;
        }

        public void CapturePoint(Vector3 rightHandPoint)
        {
            displayLine.Add(rightHandPoint);
            currentRenderer.SetVertexCount(displayLine.Count);
            currentRenderer.SetPositions(displayLine.ToArray());
        }

        public void CapturePoint(Vector3 myVector, List<Vector3> capturedLine, int maxLineLength)
        {
            if (capturedLine.Count >= maxLineLength)
            {
                capturedLine.RemoveAt(0);
            }
            capturedLine.Add(myVector);
        }

        public void StopTrail()
        {
            Color start = currentRenderer.startColor;
            Color end = currentRenderer.endColor;
            start.a = 0.1f;
            end.a = 0.1f;
            currentRenderer.SetColors(start, end);
            listening = false;
        }

        public void ClearTrail()
        {
            currentRenderer.SetVertexCount(0);
        }

        public bool UseCheck()
        {
            return currentlyInUse;
        }

        public void AssignHand(CaptureHand captureHand)
        {
            currentlyInUse = true;
            registeredHand = captureHand;
            SubscribeToEvents();

        }

    }
}
