using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    public Transform Transform;
    public GlobalPersistantScript persistantScript;


    public int hours;
    public int minutes;

    // Start is called before the first frame update
    void Start()
    {
        Transform = GetComponent<RectTransform>();
        persistantScript = GameObject.Find("PersistantObject").GetComponent<GlobalPersistantScript>();
    }

    // Update is called once per frame
    void Update()
    {
        hours = (int)Mathf.Floor(persistantScript.worldtime / 60);
        float tempMinutes = persistantScript.worldtime % 60;

        minutes = Mathf.FloorToInt(tempMinutes);

        transform.eulerAngles = new Vector3(0, 0, persistantScript.globalTimeElapsed/4);
    }
}

