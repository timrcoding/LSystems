using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rules : MonoBehaviour
{
    //USE OF PUBLIC STATIC INSTANCE FOR EASY ACCESS
    public static Rules instance;

    //DEFINES WHICH RULE GETS PASSED TO THE SENTENCE BUILDER
    public int ruleSelected;
    [SerializeField]
    private TextMeshProUGUI ruleSelectedText;

    //RULES ARE ALL CONTAINED IN TEXTDOCUMENT
    public TextAsset rulesText;
    //LIST MADE OUT STREAMREADER OF TEXT DOCUMENT
    public List<string> rules;

    //SLIDER FOR ANGLES
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI angleSliderText;

    //SLIDER FOR LINES
    [SerializeField]
    private Slider lineSlider;
    [SerializeField]
    private TextMeshProUGUI lineSliderText;



    public string activeAxiom;
    public string activeXRule;
    public string activeFRule;
    public float activeAngle;
    public float activeLineLength;

    [SerializeField]
    private TMP_InputField axiomInput;
    [SerializeField]
    private TMP_InputField xRuleInput;
    [SerializeField]
    private TMP_InputField fRuleInput;

    //L SYSTEM COMPONENTS ACTIVE AS ACCORDING TO ACTIVE RULE
    public List<string> axiom;
    public List<string> xRules;
    public List<string> fRules;
    public List<float> angles;
    public List<float> lineLengths;
    void Start()
    {
        instance = this;
        addToList();
        setAngleSliderText();
        setLineSliderText();
        setActiveRules();
        ruleSelected = axiom.Count;
        incrementRule();
    }
    //READS TEXT DOCUMENT AND COMPILES TO LISTS
    void addToList()
    {
        int count = 0;
        rules = new List<string>(rulesText.text.Split('\n'));
        for(int i = 0; i < rules.Count; i++)
        {
            if(count >= 4) { count = 0; }
            if(count == 0) { axiom.Add(rules[i]); }
            if (count == 1) { xRules.Add(rules[i]);}
            if(count == 2) { fRules.Add(rules[i]); }
            if(count == 3) { angles.Add(float.Parse(rules[i])); }
            count++;
        }
    }
    //INCREMENTS RULE SELECTION, SETTING SLIDER VALUES AND TEXT AS APPROPRIATE
    public void incrementRule()
    {
        ruleSelected++;
        if(ruleSelected >= axiom.Count)
        {
            ruleSelected = 0;
        }
        setActiveRules();
        activeLineLength = lineLengths[ruleSelected];
        ruleSelectedText.text = "R: " +  (ruleSelected+1).ToString();
        slider.value = activeAngle;
        lineSlider.value = activeLineLength;
        setAngleSliderText();
        activeLineLength = lineLengths[ruleSelected];
        setLineSliderText();
        initaliseInputFields();
    }
    //MAKES A PARTICULAR REFERENCE OF VALUES THE ACTIVE VALUES
    void setActiveRules()
    {
        activeAxiom = axiom[ruleSelected];
        activeXRule = xRules[ruleSelected];
        activeFRule = fRules[ruleSelected];
        activeAngle = angles[ruleSelected];
    }
    //ANGLE SLIDER
    public void setSliderValue()
    {
        activeAngle = slider.value;
        LSystem.instance.setRules();
        LSystem.instance.destroyAll();
        LSystem.instance.createSentence();
        setAngleSliderText();
    }


    //LINE SLIDER
    public void setLineSliderValue()
    {
        activeLineLength = lineSlider.value;
        LSystem.instance.setRules();
        LSystem.instance.destroyAll();
        LSystem.instance.createSentence();
        setLineSliderText();
    }

    void setAngleSliderText()
    {
        float fl = (float)Mathf.Round(activeAngle * 100f) / 100f;
        angleSliderText.text = "Angle: " + fl.ToString();
    }

    void setLineSliderText()
    {
        float fl = (float)Mathf.Round(activeLineLength * 100f) / 100f;
        lineSliderText.text = "Length: " + fl.ToString();
    }

    public void initaliseInputFields()
    {
        axiomInput.text = activeAxiom;
        xRuleInput.text = activeXRule;
        fRuleInput.text = activeFRule;
    }

    public void setAxiomFromInput()
    {
         activeAxiom = axiomInput.text;
         setSliderValue();
    }

    public void setXRuleFromInput()
    {
        activeXRule = xRuleInput.text;
        setSliderValue();
    }

    public void setFRuleFromInput()
    {
        activeFRule = fRuleInput.text;
        setSliderValue();
    }
}
