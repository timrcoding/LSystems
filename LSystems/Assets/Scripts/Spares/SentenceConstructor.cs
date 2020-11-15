using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceConstructor : MonoBehaviour
{
    public static SentenceConstructor instance;

    public char axiom;
    public string[] rule;
    public int chosenRule;
    public int generations;
    public List<string> resultants;
    void Start()
    {
        instance = this;
        resultants.Add(axiom.ToString());
        generate();
    }

    void generate()
    {
        for(int i = 0; i < generations; i++)
        {
            string temp = "";
            foreach (char c in resultants[i])
            {
                if (c == axiom)
                {
                    temp += rule[chosenRule];
                }
                else
                {
                    temp += c;
                }
            }
            resultants.Add(temp);
        }
    }

    
}
