using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class textClusterHelpers : MonoBehaviour
{

    public GameObject textMessageClusterPrefab;

    public GameObject textMessagePrefab;

    private GameObject textMessageCluster;

    private GameObject currentTextMessage;

    private Vector3 clusterPosition = new Vector3(40, -150, 0);

    private int numOfMessages = 0;

    private string[] testArray = new string[2];
    private string[] testArray2 = new string[2];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createCluster(string[] texts)
    {
        if (textMessageCluster != null)
        {
            Destroy(textMessageCluster);
        }

        textMessageCluster = Instantiate(textMessageClusterPrefab, gameObject.transform);

        textMessageCluster.GetComponent<VerticalLayoutGroup>().spacing = 20;

        for (int i = 0; i < texts.Length; i++)
        {
            currentTextMessage = Instantiate(textMessagePrefab, textMessageCluster.transform);

            currentTextMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = texts[i];

            textMessageCluster.GetComponent<VerticalLayoutGroup>().spacing = 20;
        }
    }
}
