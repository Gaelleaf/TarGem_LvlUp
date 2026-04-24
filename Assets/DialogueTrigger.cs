using System;
using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    

    void Start()
    {
        StartCoroutine(StartDialogueAfterDelay());
    }
    IEnumerator StartDialogueAfterDelay()
    {
        yield return null;
        
        Dialogue initDialogue = new Dialogue();
        initDialogue.name = "Uninteresting Character";
        initDialogue.sentences = new String[]
        {
            "Очень интересное сообщение в начале уровня",
            "Почему бы тебе не пропустить этот диалог?",
            "Тебе интересно все это читать?"
        };
        
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        if (manager != null)
        {
            manager.StartDialogue(initDialogue);
        }
        else
        {
            Debug.LogError("DialogueManager не найден!");
        }
    }
    public void TriggerDialoge()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
