using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphPatternScript : MonoBehaviour
{
    public enum Patterns
    {
        Triangle, Square, Pentagon
    }
    [HideInInspector] public Vector3[] triangleCorners = new Vector3[4] { new Vector3(0f, 0f), new Vector3(-1f, -1f), new Vector3(1f, -1f), new Vector3(0f, 0f) };
    [HideInInspector] public Vector3[] squareCorners = new Vector3[5] { new Vector3(1f, 0f), new Vector3(1f, -1f), new Vector3(-1f, -1f), new Vector3(-1f, 0f), new Vector3(1f, 0f) };
    [HideInInspector] public Vector3[] pentagonCorners = new Vector3[6] { new Vector3(0f, 0f), new Vector3(-1f, -0.5f), new Vector3(-0.5f, -1f), new Vector3(0.5f, -1f), new Vector3(1f, -0.5f), new Vector3(0f, 0f) };
    private List<Patterns> takenPatterns = new List<Patterns>();
    public DrawingScript drawingScript;
    public Transform[] stepTransforms = new Transform[3];
    // Start is called before the first frame update
    void Start()
    {
        SetupGame();
    }

    void SetupGame()
    {
        for (int i = 0; i < 3; i++)
        {
            Patterns pattern = (Patterns)UnityEngine.Random.Range(0, 3);
            while (takenPatterns.Contains(pattern)) pattern = (Patterns)UnityEngine.Random.Range(0, 3);
            CheckPattern(stepTransforms[i], pattern);
            takenPatterns.Add(pattern);
        }

    }

    void CheckPattern(Transform step, Patterns pattern)
    {
        Debug.Log("Checking pattern");
        switch(pattern)
        {
            case Patterns.Triangle: 
                CreateShape(step, CreateTriangle);
                break;
            case Patterns.Square:
                CreateShape(step, CreateSquare);
                break;
            case Patterns.Pentagon:
                CreateShape(step, CreatePentagon);
                break;
        }
    }
    public void CreateShape(Transform step, Action<Vector3, LineRenderer> shapeCreator)
    {
        GameObject currDrawingBrush = Instantiate(drawingScript.drawingBrushPrefab, step);
        LineRenderer currLineRenderer = currDrawingBrush.GetComponent<LineRenderer>();
        Vector3 stepPosition = new Vector3(step.position.x, step.position.y + 0.5f, 0);
        shapeCreator(stepPosition, currLineRenderer);
    }

    public void CreateTriangle(Vector3 stepPosition, LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 4;
        for(int vector = 0; vector < 4; vector++) triangleCorners[vector] = triangleCorners[vector] + stepPosition;
        lineRenderer.SetPositions(triangleCorners);

    }
    public void CreateSquare(Vector3 stepPosition, LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 5;
        for (int vector = 0; vector < 5; vector++) squareCorners[vector] = squareCorners[vector] + stepPosition;
        lineRenderer.SetPositions(squareCorners);
    }

    public void CreatePentagon(Vector3 stepPosition, LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 6;
        for (int vector = 0; vector < 6; vector++) pentagonCorners[vector] = pentagonCorners[vector] + stepPosition;
        lineRenderer.SetPositions(pentagonCorners);
    }
}
