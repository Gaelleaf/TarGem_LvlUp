using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public void TriggerDialoge()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
