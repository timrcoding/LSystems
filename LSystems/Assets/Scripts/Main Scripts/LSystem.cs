using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

public class LSystem : MonoBehaviour
{
    public static LSystem instance;
    

    [SerializeField]
    private float generationTime;
    [SerializeField]
    private GameObject drawnLine;
    [SerializeField]
    private GameObject leaf;
    [SerializeField]
    private float leafAngle;
    [SerializeField]
    private float angle;
    [SerializeField]
    private string axiom;


    [SerializeField]
    private int generations;
    [SerializeField]
    private TextMeshProUGUI generationText;
    [SerializeField]
    private int generationStep;



    [SerializeField]
    private float lineLength = 1; 
    private Stack<PushInformation> pushStack;
    private Dictionary<char, string> ruleset;
    private string sentence;
    [SerializeField]
    private List<string> sentences;
    void Start()
    {
        instance = this;
        setRules();

    }

    public void setRules()
    {
        pushStack = new Stack<PushInformation>();
        sentences.Clear();
        Rules r = Rules.instance;
        axiom = r.activeAxiom;
        angle = r.activeAngle;
        ruleset = new Dictionary<char, string>
        {
            {'X',r.activeXRule },
            {'F',r.activeFRule }
        };
        createSentence();
    }

    public void createSentence()
    {
        sentence = axiom;
        sentences.Add(axiom);

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < generations; i++)
        {
            foreach (char c in sentence)
            {
                builder.Append(ruleset.ContainsKey(c) ? ruleset[c] : c.ToString());
            }
            sentence = builder.ToString();
            sentences.Add(sentence);
            builder = new StringBuilder();
        }
        
        drawSystem();

        
    }
    public void drawSystem()
    {
        transform.position = new Vector2(0, -9.5f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        foreach (char c in sentences[generationStep])
        {
            //yield return null;
            if (c == 'F')
            {
                Vector3 startingPosition = transform.position;
                transform.Translate(Vector3.up * Rules.instance.activeLineLength);
                GameObject line = Instantiate(drawnLine);
                line.GetComponent<LineRenderer>().SetPosition(0, startingPosition);
                line.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                

                leafAngle = returnAngle(startingPosition, transform.position);
               
            }
            else if (c == 'X')
            {

            }
            else if (c == '+')
            {
                transform.Rotate(Vector3.forward * angle);
            }
            else if (c == '-')
            {
                transform.Rotate(Vector3.back * angle);
            }
            else if (c == '[')
            {
                
                pushStack.Push(new PushInformation()
                {
                    pushPosition = transform.position,
                    pushRotation = transform.rotation
                });
            }
            else if (c == ']')
            {
                GameObject newLeaf = Instantiate(leaf, transform.position, Quaternion.Euler(0,0,leafAngle));
                float size = Random.Range(0.05f, 0.15f);
                newLeaf.transform.localScale = new Vector3(size, size, 1); 
                PushInformation returnPosition = pushStack.Pop();
                transform.position = returnPosition.pushPosition;
                transform.rotation = returnPosition.pushRotation;
            }
            
        }
    }
    float returnAngle(Vector2 p1, Vector2 p2)
    {
        float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
        return angle;
    }

    public void resetGenerations()
    {
        generationStep = generations - 1;
        setGenerations();
    }

    public void setGenerations()
    {
        generationStep++;
        if(generationStep >= generations)
        {
            generationStep = 0;
            generationText.text = "Generate";
            destroyAll();
        }
        generationText.text = "G: " + generationStep.ToString();
        drawSystem();
    }

    public void destroyAll()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("TreePart");
        foreach(GameObject o in obj)
        {
            Destroy(o);
        }
    }
    

}







public class PushInformation
{
    public Vector3 pushPosition;
    public Quaternion pushRotation;

}


