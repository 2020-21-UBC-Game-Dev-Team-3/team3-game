using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingScript : MonoBehaviour
{

    private LineRenderer currLineRenderer;
    private Vector2 lastPosition;
    [HideInInspector] public Vector2 startPosition;
    [HideInInspector] public List<Vector2> brushPositions = new List<Vector2>();
    [SerializeField] private Camera mainCamera;
    public GameObject drawingBrushPrefab;
    public Transform brushParent;

    private void Update()
    {
        DrawLine();
    }

    void DrawLine()
    {
        if (IsOnDrawingBoard())
        {
            if (Input.GetMouseButtonDown(0)) CreateNewBrush();

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mousePosition != lastPosition)
                {
                    AddNewPoint(mousePosition);
                    lastPosition = mousePosition;
                }
            }
            else currLineRenderer = null;
        } else EraseBrushes();
    }

    void CreateNewBrush()
    {
        GameObject currDrawingBrush = Instantiate(drawingBrushPrefab, brushParent);
        currLineRenderer = currDrawingBrush.GetComponent<LineRenderer>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startPosition = mousePosition;
        brushPositions.Add(mousePosition);
        currLineRenderer.SetPosition(0, mousePosition);
        currLineRenderer.SetPosition(1, mousePosition);
    }

    void AddNewPoint(Vector2 pointPosition)
    {
        brushPositions.Add(pointPosition);
        currLineRenderer.positionCount++;
        int positionIndex = currLineRenderer.positionCount - 1;
        currLineRenderer.SetPosition(positionIndex, pointPosition);
    }

    void EraseBrushes()
    {
        currLineRenderer = null;
        brushPositions.Clear();
        foreach (Transform child in brushParent)
        {
            Destroy(child.gameObject, 0.5f);
        }
    }

    bool IsOnDrawingBoard()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        try
        {
            return hit.collider.tag == "Drawing";
        }
        catch (NullReferenceException)
        {
            return false;
        }
    }
}
