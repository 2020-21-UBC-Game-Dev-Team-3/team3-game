using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RandomNumberGenerator : MonoBehaviour
{
    private TMP_Text m_textComponent;
    // Start is called before the first frame update
    private void Awake()
    {
        
        // Text numbers = GameObject.Find("Canvas/Text").GetComponent<Text>();
        // numbers.text = "1 2 3 4 5";
        int[] nums = randomNumberGenerator();
        string word = string.Join(", ", nums.Select(i => i.ToString()).ToArray());
        Debug.Log(word);
        m_textComponent = GetComponent<TMP_Text>();
        m_textComponent.text = word;


        
    }

    private int[] randomNumberGenerator() {
        int[] nums = new int[] { 1,2,3,4,5 };
        for(int i = 0; i < nums.Length; i++){
            int tmp = nums[i];
            int r = Random.Range(i, nums.Length);
            nums[i] = nums[r];
            nums[r] = tmp;
        }

        return nums;

    }

   

    
}
