//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class TalentTreeOld : MonoBehaviour
//{
//    [SerializeField]
//    private int talentPoints = 500;
//    private int talentPointsMax = 500;

//    [SerializeField]
//    private TextMeshProUGUI talentPointText;

//    [SerializeField]
//    private Talent[] talents;
//    [SerializeField]
//    private PassiveTalent[] passiveTalents;
//    [SerializeField]
//    private Talent[] unlockedByDefault;

//    [SerializeField]
//    private Talent[] tree1Talents;
//    [SerializeField]
//    private PassiveTalent[] tree1TalentsPassive;
//    private int tree1AbsPointCount;
    
//    [SerializeField]
//    private Talent[] tree2Talents;
//    [SerializeField]
//    private PassiveTalent[] tree2TalentsPassive;
//    private int tree2AbsPointCount;

//    [SerializeField]
//    private Talent[] tree3Talents;
//    [SerializeField]
//    private PassiveTalent[] tree3TalentsPassive;
//    private int tree3AbsPointCount;

//    Transform tree1;
//    Transform tree2;
//    Transform tree3;

//    Transform currentTier;
//    Talent currentTalent;
//    PassiveTalent currentPassive;

//    // Start is called before the first frame update
//    void Start()
//    {
//        tree1 = transform.Find("AdditionalLayer").Find("TalentTree1").Find("Active");
//        tree2 = transform.Find("AdditionalLayer").Find("TalentTree2").Find("Active");
//        tree3 = transform.Find("AdditionalLayer").Find("TalentTree3").Find("Active");
//        ResetTalents();
//        UpdateTalentPointText();
//    }

//    public void TryUseTalent(Talent talent)
//    {
//        Debug.Log("Passt 1");
//        if (talentPoints > 0 && talent.TryAllocateTalent())
//        {
//            talentPoints--;
//            CheckUnlock();
//            CheckUnlockPassive();
//        }
//        UpdateTalentPointText();
//    }

//    public void ResetTalents()
//    {
//        talentPoints = talentPointsMax;
//        UpdateTalentPointText();
//        foreach (Talent talent in talents)
//        {
//            talent.Lock();
//            talent.RemoveActiveTalentEffect();
//            talent.currentCount = 0;
//            talent.UpdatePointCounter();
//        }

//        foreach (PassiveTalent passiveTalent in passiveTalents)
//        {
//            passiveTalent.Lock();
//        }

//        foreach (Talent talent in unlockedByDefault)
//        {
//            talent.Unlock();
//        }
//    }

//    public int TalentPoints
//    {
//        get
//        {
//            return talentPoints;
//        }
//        set
//        {
//            talentPoints = value;
//            UpdateTalentPointText();
//        }
//    }

//    public void UpdateTalentPointText()
//    {
//        talentPointText.text = talentPoints.ToString() + " / " + talentPointsMax.ToString();
//    }

//    public void CheckUnlock()   // Funktion um neues Tier von Talenten zu unlocken, wenn genügend Skillpunkte in einem Baum investiert sind
//    {
//        tree1AbsPointCount = 0;
//        tree2AbsPointCount = 0;
//        tree3AbsPointCount = 0;

//        foreach (Talent talent in tree1Talents)     // Gesamtzahl aller Skillpunkte in Baum 1
//        { tree1AbsPointCount += talent.currentCount; }

//        foreach (Talent talent in tree2Talents)     // Gesamtzahl aller Skillpunkte in Baum 2
//        { tree2AbsPointCount += talent.currentCount; }

//        foreach (Talent talent in tree3Talents)     // Gesamtzahl aller Skillpunkte in Baum 3
//        { tree3AbsPointCount += talent.currentCount; }

//        for (int i = 0; i < tree1.childCount; i++) // Durchläuft alle Tiers von Skilltree 1
//        {
//            currentTier = tree1.GetChild(i);        // Momentanes Tier von Skills wird in Variable gepackt
//            if (i > 0 && tree1AbsPointCount >= 5 * i)   // Falls das Tierlevel höher als 0 ist (also ab der 2. Zeile von Skills)
//            {                                           // wird geschaut ob die Anzahl der investierten Punkte größer ist als 5 mal die Tierstufe
//                for (int j = 0; j < currentTier.childCount; j++)    // Wenn ja werden alle Talente durchlaufen
//                {
//                    currentTalent = currentTier.GetChild(j).GetComponent<Talent>();
//                    currentTalent.Unlock();             // Und alle Talente im entsprechenden Tier unlocked.
//                }
//            }
//        }

//        for (int i = 0; i < tree2.childCount; i++) // Durchläuft alle Tiers von Skilltree 2
//        {
//            currentTier = tree2.GetChild(i);        // Momentanes Tier von Skills wird in Variable gepackt
//            if (i > 0 && tree2AbsPointCount >= 5 * i)   // Falls das Tierlevel höher als 0 ist (also ab der 2. Zeile von Skills)
//            {                                           // wird geschaut ob die Anzahl der investierten Punkte größer ist als 5 mal die Tierstufe
//                for (int j = 0; j < currentTier.childCount; j++)    // Wenn ja werden alle Talente durchlaufen
//                {
//                    currentTalent = currentTier.GetChild(j).GetComponent<Talent>();
//                    currentTalent.Unlock();             // Und alle Talente im entsprechenden Tier unlocked.
//                }
//            }
//        }

//        for (int i = 0; i < tree3.childCount; i++) // Durchläuft alle Tiers von Skilltree 3
//        {
//            currentTier = tree3.GetChild(i);        // Momentanes Tier von Skills wird in Variable gepackt
//            if (i > 0 && tree3AbsPointCount >= 5 * i)   // Falls das Tierlevel höher als 0 ist (also ab der 2. Zeile von Skills)
//            {                                           // wird geschaut ob die Anzahl der investierten Punkte größer ist als 5 mal die Tierstufe
//                for (int j = 0; j < currentTier.childCount; j++)    // Wenn ja werden alle Talente durchlaufen
//                {
//                    currentTalent = currentTier.GetChild(j).GetComponent<Talent>();
//                    currentTalent.Unlock();             // Und alle Talente im entsprechenden Tier unlocked.
//                }
//            }
//        }
//    }

//    public void CheckUnlockPassive()   // Funktion um neues Tier von Talenten zu unlocken, wenn genügend Skillpunkte in einem Baum investiert sind
//    {
//        tree1AbsPointCount = 0;
//        tree2AbsPointCount = 0;
//        tree3AbsPointCount = 0;

//        foreach (Talent talent in tree1Talents)     // Gesamtzahl aller Skillpunkte in Baum 1
//        { tree1AbsPointCount += talent.currentCount; }

//        foreach (Talent talent in tree2Talents)     // Gesamtzahl aller Skillpunkte in Baum 2
//        { tree2AbsPointCount += talent.currentCount; }

//        foreach (Talent talent in tree3Talents)     // Gesamtzahl aller Skillpunkte in Baum 3
//        { tree3AbsPointCount += talent.currentCount; }

//        for (int i = 0; i < tree1.parent.Find("Passive").childCount; i++) // Durchläuft alle Tiers von Skilltree 1
//        {
//            currentPassive = tree1.parent.Find("Passive").GetChild(i).GetComponent<PassiveTalent>();        // Momentanes Tier von Skills wird in Variable gepackt
//            if (tree1AbsPointCount >= 5 * i + 5)   
//            {
//                currentPassive.Unlock();
//            }
//        }

//        for (int i = 0; i < tree2.parent.Find("Passive").childCount; i++) // Durchläuft alle Tiers von Skilltree 1
//        {
//            currentPassive = tree2.parent.Find("Passive").GetChild(i).GetComponent<PassiveTalent>();        // Momentanes Tier von Skills wird in Variable gepackt
//            if (tree2AbsPointCount >= 5 * i + 5)
//            {
//                currentPassive.Unlock();
//            }
//        }

//        for (int i = 0; i < tree3.parent.Find("Passive").childCount; i++) // Durchläuft alle Tiers von Skilltree 1
//        {
//            currentPassive = tree3.parent.Find("Passive").GetChild(i).GetComponent<PassiveTalent>();        // Momentanes Tier von Skills wird in Variable gepackt
//            if (tree3AbsPointCount >= 5 * i + 5)
//            {
//                currentPassive.Unlock();
//            }
//        }

//    }
//}
