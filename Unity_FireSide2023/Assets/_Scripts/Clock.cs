using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Clock : MonoBehaviour
{
    public int digitalClickSize = 100;
    public Vector2 digitalClockOffset = new(2.5f, -0.2f);
    public int notifierSize = 80;
    public Vector2 jamNotifierOffset = new(2.0f, -0.2f);

    public Color secPointerCol = Color.red;
    public Color minPointerCol = Color.white;
    public Color hourPointerCol = Color.white;
    public Color textCol = Color.black;
    public Color hourMarksCol = Color.black;



    private const float TAU = 6.28318548f;

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        // is scene camera active ? 
        if (SceneView.lastActiveSceneView.camera == null)
            return;

        Camera cam = SceneView.lastActiveSceneView.camera;

        // is cam looking on the clock from the front ?
        if (Vector3.Dot(cam.transform.forward, transform.forward) < 0f)
            return;

        // is camera behind the countdown ?
        if (cam.transform.position.z > transform.position.z)
            return;

        float dist = Vector3.Distance(transform.position, cam.transform.position);

        // is cam in close enough?
        if (dist > 30f)
            return;

        // set handles matrix to local
        Handles.matrix = transform.localToWorldMatrix;
        Vector3 center = Vector3.zero;
        Vector3 forward = Vector3.forward;
        Vector3 up = Vector3.up;
        Vector3 right = Vector3.right;

        // get the current time / start end dates of the jam and the time until start/ end
        DateTime time = DateTime.Now;
        DateTime jamBegin = new DateTime(2023, 5, 5, 20, 0, 0, 0);
        DateTime jamEnd = new DateTime(2023, 5, 14, 21, 0, 0, 0);
        TimeSpan timeTillJamBegin = jamBegin.Subtract(DateTime.Now);
        TimeSpan timeTillJamEnd = jamEnd.Subtract(DateTime.Now);

        bool hasJamStarted = timeTillJamBegin.TotalSeconds <= 0;

        // Jam Notifier 
        Vector3 jamNotifierPos = center + up * jamNotifierOffset.x + right * jamNotifierOffset.y;

        GUIStyle jamNotifierStyle = new GUIStyle();
        jamNotifierStyle.normal.textColor = textCol;
        jamNotifierStyle.fontSize = Mathf.FloorToInt(notifierSize / dist);

        if (hasJamStarted)
            Handles.Label(jamNotifierPos, "GameJam ENDS in: \n" +
                                          timeTillJamEnd.Days + " Day(s) \n" +
                                          timeTillJamEnd.Hours + " hour(s) \n" +
                                          timeTillJamEnd.Minutes + " mins(s) \n" +
                                          timeTillJamEnd.Seconds + " sec(s) \n",
                                          jamNotifierStyle);
        else
            Handles.Label(jamNotifierPos, "GameJam STARTS in: \n" +
                                          timeTillJamBegin.Days + " Day(s) \n" +
                                          timeTillJamBegin.Hours + " hour(s) \n" +
                                          timeTillJamBegin.Minutes + " mins(s) \n" +
                                          timeTillJamBegin.Seconds + " sec(s) \n",
                                          jamNotifierStyle);


        // Digital Clock
        GUIStyle digitalStyle = new GUIStyle();
        digitalStyle.normal.textColor = textCol;
        digitalStyle.fontSize = Mathf.FloorToInt(digitalClickSize / dist);
        Vector3 digitalClockPos = center + up * digitalClockOffset.x + right * digitalClockOffset.y;
        Handles.Label(digitalClockPos, time.ToString("HH:mm:ss"), digitalStyle);

        // Analogue Clock
        Color clockCol = Color.yellow;
        Handles.color = clockCol;
        Handles.DrawWireDisc(center, forward, 1f);

        // Seconds Pointer
        float sec = time.Second;
        float secLenght = 1f;
        float secThickness = .1f;
        float secX = Mathf.Sin(sec / 60f * TAU);
        float secY = Mathf.Cos(sec / 60f * TAU);
        Vector3 secVec = new(secX, secY, 0f);
        Handles.color = secPointerCol;
        Handles.DrawLine(center, center + secVec * secLenght, secThickness);

        // Minutes Pointer
        float min = time.Minute;
        float minLenght = .75f;
        float minThickness = 2f;
        float minX = Mathf.Sin(min / 60f * TAU);
        float minY = Mathf.Cos(min / 60f * TAU);
        Vector3 minVec = new(minX, minY, 0f);
        Handles.color = minPointerCol;
        Handles.DrawLine(center, center + minVec * minLenght, minThickness);

        // Hours Pointer
        float hour = time.Hour;
        float hourLenght = .5f;
        float hourThickness = 4f;
        float hourX = Mathf.Sin(hour / 12f * TAU);
        float hourY = Mathf.Cos(hour / 12f * TAU);
        Vector3 hourVec = new(hourX, hourY, 0f);
        Handles.color = hourPointerCol;
        Handles.DrawLine(center, center + hourVec * hourLenght, hourThickness);

        // Hour Marks
        float hourMarksLenght = .1f;
        float hourMarksThickness = 2f;
        float hourMarksSize = 80f;

        GUIStyle HourMarksStyle = new GUIStyle();
        HourMarksStyle.normal.textColor = textCol;
        HourMarksStyle.fontSize = Mathf.FloorToInt(hourMarksSize / dist);
        Handles.color = hourMarksCol;

        for (int i = 0; i < 12; i++)
        {
            float curX = Mathf.Sin((float)i / 12f * TAU);
            float curY = Mathf.Cos((float)i / 12f * TAU);
            Vector3 curVec = new(curX, curY, 0f);

            Handles.DrawLine(center + curVec * (1 - hourMarksLenght / 2), center + curVec * (1 + hourMarksLenght / 2), hourMarksThickness);

            if (i != 0)
                Handles.Label(center + curVec * (1 + hourMarksLenght), i.ToString(), HourMarksStyle);
            else
                Handles.Label(center + curVec * (1 + hourMarksLenght), "12", HourMarksStyle);
        }
    }

#endif

}