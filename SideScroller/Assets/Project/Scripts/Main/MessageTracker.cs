using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTracker : MonoBehaviour {

    List<RectTransform> messageList = new List<RectTransform>();

    // NOTE: Always keep intro message to 0, upgrade message to 1 and first data to 2 and final data to 3 
    public RectTransform[] message;

    public GameObject[] messageGO;

    public GameObject data;

    public Pickups pickups;

    public LookAt lookAt;

    void Start()
    {
        MessageReceived(message[0]);
        SetMessagePosition();
        messageGO[0].SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Pickups.isUpgraded)
        {
            if (lookAt.controller)
            {
                if (!messageGO[4].activeSelf)
                {
                    MessageReceived(message[4]);
                    SetMessagePosition();
                    messageGO[4].SetActive(true);
                }
            }
            else
            {
                if (!messageGO[1].activeSelf)
                {
                    MessageReceived(message[1]);
                    SetMessagePosition();
                    messageGO[1].SetActive(true);
                }
            }
        }

        if (Pickups.dataCollected)
        {
            if (!messageGO[2].activeSelf)
            {
                MessageReceived(message[2]);
                SetMessagePosition();
                messageGO[2].SetActive(true);
            }

            if (!messageGO[3].activeSelf && data.transform.childCount == 0)
            {
                Debug.Log("Hello");
                MessageReceived(message[3]);
                SetMessagePosition();
                messageGO[3].SetActive(true);
            }
        }
	}

    void MessageReceived(RectTransform message)
    {
        messageList.Insert(0, message);
    }

    void SetMessagePosition()
    {
        if (messageList.Count == 1)
        {
            messageList[0].position = new Vector2(Screen.width / 2, (Screen.height / 2) - 180);
        }
        else if (messageList.Count == 2)
        {
            messageList[1].position = new Vector2(Screen.width / 2, (Screen.height / 2) - 60);
            messageList[0].position = new Vector2(Screen.width / 2, (Screen.height / 2) - 180);
        }
        else if (messageList.Count == 3)
        {
            messageList[2].position = new Vector2(Screen.width / 2, (Screen.height / 2) + 60);
            messageList[1].position = new Vector2(Screen.width / 2, (Screen.height / 2) - 60);
            messageList[0].position = new Vector2(Screen.width / 2, (Screen.height / 2) - 180);
        }
        else if (messageList.Count == 4)
        {
            messageList[3].position = new Vector2(Screen.width / 2, (Screen.height / 2) + 180);
            messageList[2].position = new Vector2(Screen.width / 2, (Screen.height / 2) + 60);
            messageList[1].position = new Vector2(Screen.width / 2, (Screen.height / 2) - 60);
            messageList[0].position = new Vector2(Screen.width / 2, (Screen.height / 2) - 180);
        }   
    }       
}
