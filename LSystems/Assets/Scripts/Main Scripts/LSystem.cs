using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;

public class LSystem : MonoBehaviour
{
    //USE OF PUBLIC STATIC INSTANCE FOR EASY ACCESS
    public static LSystem instance;
    
    //THIS SCRIPT WORKS BY TRANSLATING A TRANSFORM ACCORDING THE TURTLE DRAWING DIRECTIONS
    //AS SET OUT IN THE L SYSTEM. IT THEN DRAWS A LINE BETWEEN ITS PREVIOUS POINT AND ITS CURRENT ONE
    //FINALLY, IT ADDS A 'LEAF' AT THE 'POP' POINT, WHERE IT RETURNS TO A PREVIOUSLY STORED POSITION.
    //THIS IS ALWAYS AT THE END OF A 'BRANCH'.

    //LINE DRAWING

    //INSTANTIATED LINE
    [SerializeField]
    private GameObject drawnLine;
    //INSTANTIATED LEAF
    [SerializeField]
    private GameObject leaf;
    [SerializeField]
    //SETS THE ANGLE OF THE LEAF AS THE VECTOR OF THE LINE 
    private float leafAngle;
    //ANGLE OF LINE
    [SerializeField]
    private float angle;

    //L SYSTEM
    [SerializeField]
    private string axiom;
    //SETS TOTAL AMOUNT OF GENERATIONS
    [SerializeField]
    private int generations;
    //SHOWS TEXT FOR WHICH STEP OF GENERATION IS BEING DRAWN
    [SerializeField]
    private TextMeshProUGUI generationText;
    //DEFINES WHICH STEP OF GENERATION IS BEING DRAWN
    [SerializeField]
    private int generationStep;
    //STACK FOR STORING TRANSFORM POSITIONAL DATA
    private Stack<PushInformation> pushStack;
    //DICTIONARY FOR RULESET, AS DEFINED BY TEXT DOCUMENT
    private Dictionary<char, string> ruleset;
    //L SYSTEM SENTENCE
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
        //INITIALISES STACK
        pushStack = new Stack<PushInformation>();
        //REMOVES ALL SENTENCES AS STORED IN LIST
        sentences.Clear();
        Rules r = Rules.instance;
        //DEFINES WHICH AXIOM AND ANGLE IS BEING USED
        axiom = r.activeAxiom;
        angle = r.activeAngle;
        //SETS RULES IN DICTIONARY
        ruleset = new Dictionary<char, string>
        {
            {'X',r.activeXRule },
            {'F',r.activeFRule }
        };
        createSentence();
    }

    public void createSentence()
    {
        //INITIALISES SENTENCE ACCORDING AXIOM
        sentence = axiom;
        //MAKES THIS THE FIRST ENTRY IN SENTENCES LIST
        sentences.Add(axiom);

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < generations; i++)
        {
            foreach (char c in sentence)
            {
                //IF SENTENCE CONTAINS A KEY IN RULE, THEN APPLY RULE, OTHERWISE JUST APPLY TO SENTENCE
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
        //SETS TRANSFORM TO BOTTOM CENTRE OF SCREEN
        transform.position = new Vector2(0, -9.5f);
        //RESETS ROTATION
        transform.rotation = Quaternion.Euler(0, 0, 0);
        foreach (char c in sentences[generationStep])
        {
            //TURTLE GRAPHICS INSTRUCTIONS
            if (c == 'F')
            {
                //SAVES INITIAL POSITION IN LOCAL VARIABLE
                Vector3 startingPosition = transform.position;
                //MOVES TRANSFORM TO NEW POINT
                transform.Translate(Vector3.up * Rules.instance.activeLineLength);
                //CREATES LINE
                GameObject line = Instantiate(drawnLine);
                //SETS LINE'S STARTING POINT AS SAVED VARIABLE AND END POINT AS CURRENT POSITION
                line.GetComponent<LineRenderer>().SetPosition(0, startingPosition);
                line.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                //CALCULATES LEAF ANGLE BY FINDING THE VECTOR BETWEEN THESE TWO POINTS
                leafAngle = returnAngle(startingPosition, transform.position);
               
            }
            else if (c == 'X')
            {

            }
            else if (c == '+')
            {
                //TURNS TRANSFORM
                transform.Rotate(Vector3.forward * angle);
            }
            else if (c == '-')
            {
                //TURNS TRANSFORM
                transform.Rotate(Vector3.back * angle);
            }
            else if (c == '[')
            {
                //SAVES POSITION TO STACK
                pushStack.Push(new PushInformation()
                {
                    pushPosition = transform.position,
                    pushRotation = transform.rotation
                });
            }
            else if (c == ']')
            {
                //CREATES A NEW LEAF
                GameObject newLeaf = Instantiate(leaf, transform.position, Quaternion.Euler(0,0,leafAngle));
                //SETS RANDOM LEAF SIZE
                float size = Random.Range(0.05f, 0.15f);
                newLeaf.transform.localScale = new Vector3(size, size, 1); 
                //RETURNS SAVED STACK INFORMATION AND MOVES/ROTATES TRANSFORM
                PushInformation returnPosition = pushStack.Pop();
                transform.position = returnPosition.pushPosition;
                transform.rotation = returnPosition.pushRotation;
            }
            
        }
    }
    //RETURNS THETA
    float returnAngle(Vector2 p1, Vector2 p2)
    {
        float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
        return angle;
    }
    //RESETS GENERATIONS
    public void resetGenerations()
    {
        generationStep = generations - 1;
        setGenerations();
    }
    //INCREMENTS GENERATIONS AND SETS TEXT
    public void setGenerations()
    {
        generationStep++;
        if(generationStep >= generations)
        {
            generationStep = 0;
            generationText.text = "Generate";
            //FOR EACH GENERATION, DESTROYS ALL INSTANTIATED OBJECTS ON SCREEN
            destroyAll();
        }
        generationText.text = "G: " + generationStep.ToString();
        drawSystem();
    }
    //DESTROYS ALL INSTANTIATED OBJECTS ON SCREEN
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


