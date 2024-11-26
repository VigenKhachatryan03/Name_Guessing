using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public Text timeField;
    public Text WordToFindField;
    public GameObject[] hangMan;
    public GameObject winText;
    public GameObject loseText;
    public GameObject replayButton;
    private float time = 0f;
    //private string[] wordsLocal = { "HAKOB", "MARTIROS", "TIRAN", "SEVADA", "MARRY JANY" };
    private readonly string[] words = File.ReadAllLines(@"Assets/Texts/Words.txt");
    private string chosenWord;
    private string hiddenWord;
    private int fails;
    private bool gameEnd = false;

    void Start()
    {
        replayButton.SetActive(false);
        chosenWord = words[Random.Range(0, words.Length)];

        for (int i = 0; i < chosenWord.Length; i++)
        {
            char letter = chosenWord[i];
            if (char.IsWhiteSpace(letter))
            {
                hiddenWord += " ";
            }
            else
            {
                hiddenWord += "_";
            }

        }

        WordToFindField.text = hiddenWord;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnd == false)
        {
            time += Time.deltaTime;
            timeField.text = time.ToString();
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode.ToString().Length == 1)
        {
            string pressedKey = e.keyCode.ToString();
            Debug.Log("KeyDown - " + pressedKey);
            //Check
            if (chosenWord.Contains(pressedKey))
            {
                int i = chosenWord.IndexOf(pressedKey);
                while (i != -1)
                {
                    hiddenWord = hiddenWord.Substring(0, i) + pressedKey + hiddenWord.Substring(i + 1);
                    Debug.Log(hiddenWord);

                    chosenWord = chosenWord.Substring(0, i) + "_" + chosenWord.Substring(i + 1);
                    Debug.Log(chosenWord);

                    i = chosenWord.IndexOf(pressedKey);
                }
                WordToFindField.text = hiddenWord;
            }
            else
            {
                hangMan[fails].SetActive(true);
                fails++;
            }
            //case lost game
            if (fails == hangMan.Length)
            {
                loseText.SetActive(true);
                replayButton.SetActive(true);
                gameEnd = true;
            }
            // case won game
            if (hiddenWord.Contains("_") == false)
            {
                winText.SetActive(true);
                replayButton.SetActive(true);
                gameEnd = true;
            }

        }
    }
}
