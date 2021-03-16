using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GestureHandler : MonoBehaviour
{
	private int currentStepNumber = 0;
	public float drawingAccuracyPercentage;

	public Text textResult;
	public GameObject winText;
	public Transform referenceRoot;

	public DrawingManager drawingManager;
	GesturePatternDraw[] references;

	void Start()
	{
		references = referenceRoot.GetComponentsInChildren<GesturePatternDraw>();
	}

	void ShowAll()
	{
		for (int i = 0; i < references.Length; i++)
		{
			references[i].gameObject.SetActive(true);
		}
	}

	public void OnRecognize(RecognitionResult result)
	{
		StopAllCoroutines();
		ShowAll();
		if (result != RecognitionResult.Empty)
		{
			if (IsCorrectGesture(result))
			{
				currentStepNumber++;
				if (currentStepNumber >= drawingManager.patternReferences.Length)
					WinGestureMinigame();
				StartCoroutine(Blink(result.gesture.id));
			}
		}
		GetComponent<DrawDetector>().ClearLines();
	}

	bool IsCorrectGesture(RecognitionResult result)
    {
		return result.gesture.id == drawingManager.gesturePatterns[currentStepNumber].id && Mathf.RoundToInt(result.score.score * 100) > drawingAccuracyPercentage;
    }

	void WinGestureMinigame()
    {
		winText.SetActive(true);
        SceneManager.UnloadSceneAsync("DrinkMixingMinigame");
    }

	IEnumerator Blink(string id)
	{
		GetComponent<DrawDetector>().ClearLines();
		var draw = references.Where(e => e.pattern.id == id).FirstOrDefault();
		if (draw != null)
		{
			var seconds = new WaitForSeconds(0.1f);
			for (int i = 0; i <= 20; i++)
			{
				draw.gameObject.SetActive(i % 2 == 0);
				yield return seconds;
			}
			draw.gameObject.SetActive(true);
		}
		GetComponent<DrawDetector>().ClearLines();
	}
}
