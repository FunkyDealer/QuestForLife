using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplayer : MonoBehaviour
{
    [HideInInspector]
    public List<TextMessage> Messages;

    [SerializeField]
    GameObject messageBox;

    Vector2 MessageDisplayPos = new Vector2(200, -60);

    [SerializeField]
    BattleInterFaceManager manager;

    [SerializeField]
    GameObject textMessagePrefab;

    void Awake()
    {
        Messages = new List<TextMessage>();
        messageBox.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMessage(string text, TextMessage.MessageSpeed speed)
    {
        if (Messages.Count == 0)
        {
            messageBox.SetActive(true);

            GameObject o = Instantiate(textMessagePrefab, messageBox.transform);
            TextMessage t = o.GetComponent<TextMessage>();
            t.Init(text, new Vector2(200, 0), this, TextMessage.MessageStatus.READING, speed);
            Messages.Add(t);
        }
        else
        {
            GameObject o = Instantiate(textMessagePrefab, messageBox.transform);
            TextMessage t = o.GetComponent<TextMessage>();
            t.Init(text, new Vector2(200, 0), this, TextMessage.MessageStatus.WAITING, speed);
            Messages.Add(t);
            o.SetActive(false);
        }
    }

    public void loadNextMessage(TextMessage t)
    {
        Messages.Remove(t);

        if (Messages.Count > 0)
        {
            Messages[0].gameObject.SetActive(true);
        }
    }

}
