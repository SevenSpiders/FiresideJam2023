
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Clock : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (SceneView.lastActiveSceneView.camera == null)
            return;

        Camera cam = SceneView.lastActiveSceneView.camera;
        float dist = Vector3.Distance(transform.position, cam.transform.position);

        Vector3 center = transform.position;
        float TAU = 6.28318548f;

        Color timeCol = Color.black;
        float timeSize = 80f;

        GUIStyle digitalStyle = new GUIStyle();
        digitalStyle.normal.textColor = timeCol;
        digitalStyle.fontSize = Mathf.FloorToInt(timeSize / dist);


        DateTime time = DateTime.Now;
        Handles.Label(center, time.ToString("HH:mm:ss"),digitalStyle);

        Color clockCol = Color.yellow;
        Handles.color = clockCol;
        Handles.DrawWireDisc(center, transform.forward, 1f);

        float sec = time.Second;
        float secLenght = 1f;
        float secThickness = .1f;
        Color secCol = Color.red;
        float secX = Mathf.Sin(sec / 60f * TAU);
        float secY = Mathf.Cos(sec / 60f * TAU) ;
        Vector3 secVec = new(secX, secY, 0f);
        Handles.color = secCol;
        Handles.DrawLine(center, center + secVec * secLenght, secThickness);

        float min = time.Minute;
        float minLenght = .75f;
        float minThickness = 2f;
        Color minCol = Color.white;
        float minX = Mathf.Sin(min / 60f * TAU);
        float minY = Mathf.Cos(min / 60f * TAU);
        Vector3 minVec = new(minX, minY, 0f);
        Handles.color = minCol;
        Handles.DrawLine(center, center + minVec * minLenght , minThickness);

        float hour = time.Hour;
        float hourLenght = .5f;
        float hourThickness = 4f;
        Color hourCol = Color.white;
        float hourX = Mathf.Sin(hour / 12f * TAU);
        float hourY = Mathf.Cos(hour / 12f * TAU);
        Vector3 hourVec = new(hourX, hourY, 0f);
        Handles.color = hourCol;
        Handles.DrawLine(center, center + hourVec * hourLenght, hourThickness);


        float hourPointerLenght = .1f;
        float hourPointerThickness= 2f;
        float hourSize = 80f;
        Color hourPointerCol = Color.black;

        GUIStyle pointerStyle = new GUIStyle();
        pointerStyle.normal.textColor = hourPointerCol;
        pointerStyle.fontSize = Mathf.FloorToInt(hourSize / dist);

        Handles.color = hourPointerCol;
        for (int i = 0; i<12; i++)
        {
            float curX = Mathf.Sin((float)i / 12f * TAU);
            float curY = Mathf.Cos((float)i / 12f * TAU);
            Vector3 curVec = new(curX, curY, 0f);

            Handles.DrawLine(center + curVec * (1- hourPointerLenght/2) , center + curVec * (1 + hourPointerLenght / 2), hourPointerThickness);

            if (i != 0)
                Handles.Label(center + curVec * (1 + hourPointerLenght), i.ToString(), pointerStyle);
            else
                Handles.Label(center + curVec * (1 + hourPointerLenght),"12", pointerStyle);
        }
    }
#endif
}

