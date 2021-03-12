using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    [HideInInspector] public GesturePattern[] gesturePatterns;
    public Transform[] patternReferences = new Transform[3];
    // Start is called before the first frame update
    void Start()
    {
        CreateDrawingSteps();
    }

    void CreateDrawingSteps()
    {
        gesturePatterns = Resources.LoadAll<GesturePattern>("Gestures");
        gesturePatterns = ShuffleList(gesturePatterns);
        for (int i = 0; i < patternReferences.Length; i++)
        patternReferences[i].GetComponent<GesturePatternDraw>().pattern = gesturePatterns[i];
    }

    private T[] ShuffleList<T>(T[] ts)
    {
        var count = ts.Length;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
        return ts;
    }
}
