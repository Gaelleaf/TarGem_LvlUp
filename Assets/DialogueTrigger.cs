using System;
using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GridManager gridManager;

    void Start()
    {
        StartCoroutine(StartDialogueAfterDelay());
    }
    IEnumerator StartDialogueAfterDelay()
    {
        yield return null;
        
        Dialogue initDialogue1 = new Dialogue();
        initDialogue1.name = "Uninteresting Character";
        initDialogue1.sentences = new String[]
        {
            "Очень интересное сообщение в начале уровня",
            "Почему бы тебе не пропустить этот диалог?",
            "Тебе интересно все это читать?"
        };
        Dialogue initDialogue2 = new Dialogue();
        initDialogue2.name = "Uninteresting Character №2";
        initDialogue2.sentences = new String[]
        {
            "Вообще неинтересное сообщение в начале 2 уровня",
            "Почему бы тебе не пропустить этот диалог?",
            "Тебе интересно все это читать?"
        };
        gridManager = FindObjectOfType<GridManager>();
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        switch (gridManager.levelData.levelNumber)
        {
            case 1:
                manager.StartDialogue(initDialogue1);
                break;
            case 2:
                manager.StartDialogue(initDialogue2);
                break;
        }
    }
    public void TriggerDialoge()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
