using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TalentTree : MonoBehaviour
{
    public string subClassMain, subClassLeft, subClassRight;

    [SerializeField] int talentPoints = 500, talentPointsMax = 500;

    [SerializeField] List<Talent> talents, unlockedByDefault;
    [SerializeField] PassiveTalent[] passiveTalents;

    [SerializeField] List<GameObject> talentsMain, talentsLeft, talentsRight;
    //[SerializeField] PassiveTalent[] pTalentsMain, pTalentsLeft, pTalentsRight;
    int mainAbsPointCount, leftAbsPointCount, rightAbsPointCount;

    public bool checkAfterReset;

    Transform classTrees;
    Transform myTalentTree;

    float turningDuration = 2f;
    bool isTurning = false;
    GameObject PLAYER;

    TalentTreeUI myTalentTreeUI;

    List<int> talentCountForLoading;

    [SerializeField] TextMeshProUGUI talentPointText;

    void Awake()
    {
        PLAYER = transform.parent.parent.gameObject;
        classTrees = GameObject.Find("SkillTreeCollection").transform.Find("CanvasAllSkillTrees").Find("TalentTree");
        myTalentTree = transform.Find("TalentTreeWindow").Find("MainBody").Find("MaskLayer").Find("TalentTree");
        talentPointText = transform.Find("TalentTreeWindow").Find("MainBody").Find("TalentPointText").Find("TalentPointCount").GetComponent<TextMeshProUGUI>();

        checkAfterReset = false;

        myTalentTreeUI = transform.GetComponent<TalentTreeUI>();

        ResetSkillTree();
    }

    public void TryUseTalent(Talent talent)
    {
        //Debug.Log("Passt 1");
        if (talentPoints > 0 + talent.pointCost && talent.TryAllocateTalent())
        {
            talentPoints -= talent.pointCost;
            CheckUnlockTalent();
            //CheckUnlockPassive();
        }
        UpdateTalentPointText();
    }

    void CheckUnlockTalent()
    {
        mainAbsPointCount = 0;
        leftAbsPointCount = 0;
        rightAbsPointCount = 0;

        ClearTalentOrientation();
        for (int i = 0; i < 4; i++)
        {
            Transform myCurrentTier = myTalentTree.Find("Tier" + (i + 1).ToString());
            CheckTalentOrientation(myCurrentTier);
        }

        talentsMain.ForEach(k => mainAbsPointCount += k.GetComponent<Talent>().currentCount);
        talentsRight.ForEach(k => rightAbsPointCount += k.GetComponent<Talent>().currentCount);
        talentsLeft.ForEach(k => leftAbsPointCount += k.GetComponent<Talent>().currentCount);

        for (int i = 0; i < 4; i++)
        {
            Transform myCurrentTier = myTalentTree.Find("Tier" + (i + 1).ToString());

            for (int k = 0; k < myCurrentTier.childCount; k++)
            {
                if (talentsMain.Contains(myCurrentTier.GetChild(k).gameObject))
                {
                    if (mainAbsPointCount >= 10 * i)
                    {
                        if (myCurrentTier.GetChild(k).GetComponent<Talent>().myPredecessorTalent == null)
                        {
                            myCurrentTier.GetChild(k).GetComponent<Talent>().Unlock();
                        }
                        else
                        {
                            if (myCurrentTier.GetChild(k).GetComponent<Talent>().myPredecessorTalent.GetComponent<Talent>().currentCount > 0)
                            {
                                myCurrentTier.GetChild(k).GetComponent<Talent>().Unlock();
                            }
                        }
                    }
                }
                else if (talentsRight.Contains(myCurrentTier.GetChild(k).gameObject))
                {
                    if (rightAbsPointCount >= 10 * i)
                    {
                        if (myCurrentTier.GetChild(k).GetComponent<Talent>().myPredecessorTalent == null)
                        {
                            myCurrentTier.GetChild(k).GetComponent<Talent>().Unlock();
                        }
                        else if (myCurrentTier.GetChild(k).GetComponent<Talent>().myPredecessorTalent.GetComponent<Talent>().currentCount > 0)
                        {
                            myCurrentTier.GetChild(k).GetComponent<Talent>().Unlock();
                        }
                    }
                }
                else if (talentsLeft.Contains(myCurrentTier.GetChild(k).gameObject))
                {
                    if (leftAbsPointCount >= 10 * i)
                    {
                        if (myCurrentTier.GetChild(k).GetComponent<Talent>().myPredecessorTalent == null)
                        {
                            myCurrentTier.GetChild(k).GetComponent<Talent>().Unlock();
                        }
                        else if (myCurrentTier.GetChild(k).GetComponent<Talent>().myPredecessorTalent.GetComponent<Talent>().currentCount > 0)
                        {
                            myCurrentTier.GetChild(k).GetComponent<Talent>().Unlock();
                        }
                    }
                }
            }
        }
    }

    public void ResetSkillTree()
    {
        if (!PLAYER.GetComponent<NetworkObject>().IsOwner) return;

        myTalentTree = transform.Find("TalentTreeWindow").Find("MainBody").Find("MaskLayer").Find("TalentTree");

        ResetTalents();

        RemoveAllActiveTalents();

        ClearTalentOrientation();

        for (int n = 0; n < 3; n++)
        {
            string cName;

            if (n == 0)
            { cName = subClassMain; }
            else if (n == 1)
            { cName = subClassLeft; }
            else
            { cName = subClassRight; }

            if (cName == "") continue;

            for (int i = 0; i < 4; i++)
            {
                Transform myCurrentTier = myTalentTree.Find("Tier" + (i + 1).ToString());

                for (int k = 0; k < classTrees.Find(cName).Find("MaskLayer").Find("TalentTree").Find("Tier" + (i + 1).ToString()).childCount; k++)
                {
                    GameObject cTalent = classTrees.Find(cName).Find("MaskLayer").Find("TalentTree").Find("Tier" + (i + 1).ToString()).GetChild(k).gameObject;
                    GameObject cTalentNew = Instantiate(cTalent, myCurrentTier);
                    cTalentNew.transform.rotation = Quaternion.identity;

                    Color32 myNewColor = cTalent.transform.Find("TalentImage").GetComponent<Image>().color;
                    //Debug.Log("my color component: " + cTalent.GetComponent<Image>());
                    //Debug.Log("my color: " + myNewColor);
                    cTalentNew.transform.Find("TalentImage").GetComponent<Image>().color = myNewColor;

                    if (n == 1)
                    { cTalentNew.transform.localPosition = Quaternion.Euler(0, 0, 120) * cTalentNew.transform.localPosition; }

                    if (n == 2)
                    { cTalentNew.transform.localPosition = Quaternion.Euler(0, 0, -120) * cTalentNew.transform.localPosition; }
                }
            }
        }
        //checkAfterReset = true;
        StartCoroutine(CheckNextUpdate());
    }

    public void ResetTalents()
    {
        talentPoints = talentPointsMax;
        //UpdateTalentPointText();

        GetUnlockedByDefaultTalents();

        foreach (Talent talent in talents)
        {
            bool hasToDoEverything = true;
            if (talent != null)
            {
                if (talent.currentCount == 0) hasToDoEverything = false;

                talent.UpdateTalent();
                talent.Lock();
                if (hasToDoEverything) talent.RemoveActiveTalentEffect();
                talent.currentCount = 0;
                if (hasToDoEverything) talent.RemoveActiveTalentEffectAfterPointCountReduced();
                talent.UpdatePointCounterAndBackground();
                talent.FindMyPredecessor();
            }
        }

        foreach (PassiveTalent passiveTalent in passiveTalents)
        {
            passiveTalent.Lock();
            passiveTalent.RemoveActiveTalentEffect();
        }

        foreach (Talent talent in unlockedByDefault)
        {
            talent.Unlock();
        }
    }

    void GetUnlockedByDefaultTalents()
    {
        unlockedByDefault.Clear();
        unlockedByDefault = myTalentTree.Find("Tier1").GetComponentsInChildren<Talent>().ToList();
    }

    public void UpdateTalentPointText()
    {
        talentPointText.text = talentPoints.ToString() + " / " + talentPointsMax.ToString();
    }

    IEnumerator CheckNextUpdate()
    {
        yield return 1;
        HasToCheckAfterReset();
    }

    void LateUpdate()
    {
        if (checkAfterReset)
        {
            HasToCheckAfterReset();
        }
    }

    public void HasToCheckAfterReset()
    {
        Debug.Log("Doing HasToCheck");
        checkAfterReset = false;
        for (int i = 0; i < 4; i++)
        {
            Transform myCurrentTier = myTalentTree.Find("Tier" + (i + 1).ToString());
            CheckTalentOrientation(myCurrentTier);

            talentsMain = talentsMain.Where(k => k != null).ToList();
            talentsLeft = talentsLeft.Where(k => k != null).ToList();
            talentsRight = talentsRight.Where(k => k != null).ToList();
        }

        FetchAllTalents();
        ResetTalents();
    }

    void FetchAllTalents()
    {
        if (!PLAYER.GetComponent<NetworkObject>().IsOwner) return;

        talents.Clear();
        talents = myTalentTree.GetComponentsInChildren<Talent>().ToList();
        talents.RemoveAll(item => item == null);
    }

    public void ShowClassWindow(string subClassPosition)
    {
        myTalentTree.parent.parent.parent.Find("ClassWindow").gameObject.SetActive(true);
        myTalentTree.parent.parent.parent.Find("ClassWindow").GetComponent<TalentClassWindow>().subClassPosition = subClassPosition;
    }

    void RemoveAllActiveTalents()
    {
        for (int i = 0; i < 4; i++)
        {
            Transform tierToClear = myTalentTree.Find("Tier" + (i + 1).ToString());
            int clearCount = tierToClear.childCount;
            List<GameObject> talentsToDestroy = new List<GameObject>();
            for (int k = 0; k < clearCount; k++)
            {
                talentsToDestroy.Add(tierToClear.GetChild(k).gameObject);
            }

            talentsToDestroy.ForEach(k => talentsMain.Remove(k));
            talentsToDestroy.ForEach(k => talentsRight.Remove(k));
            talentsToDestroy.ForEach(k => talentsLeft.Remove(k));

            //talentsToDestroy.ForEach(k => talents.Remove(k.GetComponent<Talent>()));

            talentsToDestroy.ForEach(k => GameObject.Destroy(k));
            talentsToDestroy.Clear();
        }
    }

    public void ResetRingTuning()
    {
        if (isTurning) return;

        ResetSkillTree();
        //ResetTalents();

        for (int i = 0; i < 4; i++)
        {
            Transform detunedRing = myTalentTree.Find("Tier" + (i+1).ToString());

            if ((int)detunedRing.rotation.eulerAngles.z == 120 || (int)detunedRing.rotation.eulerAngles.z == -240)
            {
                StartCoroutine(turningRingCoroutine(detunedRing, detunedRing.rotation.eulerAngles.z, false));
                isTurning = true;
            }
            else if ((int)detunedRing.rotation.eulerAngles.z == 240 || (int)detunedRing.rotation.eulerAngles.z == -120)
            {
                StartCoroutine(turningRingCoroutine(detunedRing, detunedRing.rotation.eulerAngles.z, true));
                isTurning = true;
            }
        }
    }

    public void SetRingRotationWhenLoading(List<float> myRotAngles)
    {
        for (int i = 0; i < 4; i++)
        {
            Transform movingRing = myTalentTree.Find("Tier" + (i + 1).ToString());
            movingRing.rotation = Quaternion.Euler(0, 0, myRotAngles[i]);

            if (movingRing.rotation.eulerAngles.z == 360 || Mathf.Abs(movingRing.rotation.eulerAngles.z) < 0.01)
            {
                movingRing.rotation = Quaternion.Euler(0, 0, 0);
            }

            for (int k = 0; k < movingRing.childCount; k++)
            { movingRing.GetChild(k).rotation = Quaternion.Euler(0, 0, 0); }

            CheckTalentOrientation(movingRing);
        }
    }

    public void TurnRing(int ringNr)
    {
        if (isTurning) return;

        ResetSkillTree();
        ResetTalents();

        Transform movingRing = myTalentTree.Find("Tier" + ringNr.ToString());
        float startAngle = movingRing.rotation.eulerAngles.z;
        bool directionLeft = false;
        isTurning = true;
        StartCoroutine(turningRingCoroutine(movingRing, startAngle, directionLeft));
    }

    public IEnumerator turningRingCoroutine(Transform movingRing, float startAngle, bool directionLeft)
    {
        float elapsed = 0;
        float suffix;

        if (directionLeft)
        { suffix = 1; }
        else
        { suffix = -1; }

        while (elapsed <= turningDuration)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            float newAngle = suffix * 120 * Time.deltaTime / turningDuration;
            movingRing.Rotate(0, 0, newAngle);

            for (int i = 0; i < movingRing.childCount; i++)
            { movingRing.GetChild(i).Rotate(0, 0, -newAngle); }

            elapsed += Time.deltaTime;
        }

        movingRing.rotation = Quaternion.Euler(0, 0, startAngle + suffix * 120);    

        if (movingRing.rotation.eulerAngles.z == 360 || Mathf.Abs(movingRing.rotation.eulerAngles.z) < 0.01)
        {
            movingRing.rotation = Quaternion.Euler(0, 0, 0);
        }

        for (int i = 0; i < movingRing.childCount; i++)
        { movingRing.GetChild(i).rotation = Quaternion.Euler(0, 0, 0); }

        CheckTalentOrientation(movingRing);
        isTurning = false;
    }

    void CheckTalentOrientation(Transform currentRing)
    {
        for (int i = 0; i < currentRing.childCount; i++)
        {
            GameObject talent = currentRing.GetChild(i).gameObject;
            Vector2 globalDirection = Quaternion.Euler(0, 0, currentRing.rotation.eulerAngles.z) * talent.transform.localPosition.normalized;
            float angle = Vector2.SignedAngle(globalDirection, Vector2.up);

            talentsMain.Remove(talent);
            talentsLeft.Remove(talent);
            talentsRight.Remove(talent);

            if (angle > -60.00 && angle < 60.00)
            { talentsMain.Add(talent); }
            else if (angle < -60 && angle > -180)
            { talentsLeft.Add(talent); }
            else if (angle > 60 && angle < 180)
            { talentsRight.Add(talent); }

            talentsMain = talentsMain.Where(k => k != null).ToList();
            talentsLeft = talentsLeft.Where(k => k != null).ToList();
            talentsRight = talentsRight.Where(k => k != null).ToList();
        }
    }

    void ClearTalentOrientation()
    {
        talentsMain.Clear();
        talentsLeft.Clear();
        talentsRight.Clear();
    }

    public void BtnResetTalents()
    {
        UpdateTalentPointText();
        ResetSkillTree();
    }

    public List<Talent> GetTalentsForSaving()
    {
        FetchAllTalents();
        return talents;
    }

    public void AutoSkillWhenLoading(List<int> loadedTalentCount)
    {
        Debug.Log(loadedTalentCount.Count);
        Debug.Log(talents.Count);

        bool isWindowActive = transform.Find("TalentTreeWindow").gameObject.activeSelf;
        if (!isWindowActive) transform.Find("TalentTreeWindow").gameObject.SetActive(true);

        for (int i = 0; i < talents.Count-1; i++)
        {
            int myCurCount = loadedTalentCount[i];
            if (myCurCount == 0) continue;

            for (int k = 0; k < myCurCount; k++)
            {
                TryUseTalent(talents[i]);
            }
        }

        transform.Find("TalentTreeWindow").gameObject.SetActive(isWindowActive);
    }

    public void AutoSkillWhenLoading2()
    {
        ResetSkillTree();
        //talents.RemoveAll(item => item == null);
        Debug.Log(talentCountForLoading.Count);
        Debug.Log(talents.Count);
    }
}