using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject line;
    public float lineLength = 5f;
    public Vector3 lineEnd;
    public float angleBuffer;
    public Vector3 pushStack;
    public float pushAngle;
    void Start()
    {
        StartCoroutine(stepThroughSentence());
    }

    IEnumerator stepThroughSentence()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        for (int i = 0; i < SentenceConstructor.instance.resultants.Count; i++)
        {
            string sentence = SentenceConstructor.instance.resultants[i];
            lineEnd = Vector3.zero;


            Debug.Log(sentence);
            foreach (char c in sentence)
            {
                if (c.ToString() == "F")
                {
                    Debug.Log("Create Line");
                    newLine();
                }
                else if (c.ToString() == "+")
                {
                    Debug.Log("Turn Right");
                    angleBuffer -= 25.7f;

                }
                else if (c.ToString() == "-")
                {
                    Debug.Log("Turn Left");
                    angleBuffer += 25.7f;
                    
                }
                else if (c.ToString() == "[")
                {
                    Debug.Log("Push Stack");
                    pushStack = lineEnd;
                    pushAngle = angleBuffer;
                }
                else if (c.ToString() == "]")
                {
                    Debug.Log("Pop Stack");
                    lineEnd = pushStack;
                    angleBuffer = pushAngle;
                }
                yield return new WaitForSeconds(.01f);
            }
            lineLength *= 0.5f;
        }

    }

    void newLine()
    {
        float x = Mathf.Sin(angleBuffer) * lineLength;
        float y = Mathf.Cos(angleBuffer) * lineLength;
        GameObject newLine = Instantiate(line, lineEnd,Quaternion.identity);
        newLine.GetComponent<LineRenderer>().SetPosition(0, lineEnd);
        newLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(lineEnd.x + x,lineEnd.y + y, 0));
        setLineEnd(new Vector3(lineEnd.x + x, lineEnd.y + y,0));
        
    }

    void setLineEnd(Vector3 pos)
    {
        lineEnd = pos;
    }
}
