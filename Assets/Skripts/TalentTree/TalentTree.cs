using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentTree : MonoBehaviour
{
    public string subClassMain="Alchemist", subClassLeft="Dummy", subClassRight="Dummy";
    enum subClassSlot { Main, Left, Right };

    Transform classTrees;
    Transform myTalentTree;

    [SerializeField] List<Transform> talentsMain, talentsLeft, talentsRight;
    float turningDuration = 2f;
    bool isTurning = false;

    void Start()
    {
        classTrees = GameObject.Find("SkillTreeCollection").transform.Find("CanvasAllSkillTrees").Find("TalentTree");
        myTalentTree = transform.Find("MainBody").Find("TalentTree");

        subClassMain = "Alchemist";
        subClassLeft = "Berserker";
        subClassRight = "Dummy";
    }

    public void UpdateSkillTree()
    {
        RemoveAllActiveTalents();

        for (int n = 0; n < 3; n++)
        {
            string cName;

            if (n == 0)
            { cName = subClassMain; }
            else if (n == 1)
            { cName = subClassLeft; }
            else
            { cName = subClassRight; }

            for (int i = 0; i < 4; i++)
            {
                Transform myCurrentTier = myTalentTree.Find("Tier" + (i + 1).ToString());
                for (int k = 0; k < classTrees.Find(cName).Find("Tier" + (i + 1).ToString()).childCount; k++)
                {
                    GameObject cTalent = classTrees.Find(cName).Find("Tier" + (i + 1).ToString()).GetChild(k).gameObject;
                    GameObject cTalentNew = Instantiate(cTalent, myCurrentTier);
                    cTalentNew.transform.rotation = Quaternion.identity;

                    if (n == 1)
                    { cTalentNew.transform.localPosition = Quaternion.Euler(0, 0, 120) * cTalentNew.transform.localPosition; }

                    if (n == 2)
                    { cTalentNew.transform.localPosition = Quaternion.Euler(0, 0, -120) * cTalentNew.transform.localPosition; }
                }
            }
        }
    }

    public void ShowClassWindow(string subClassPosition)
    {
        myTalentTree.parent.parent.Find("ClassWindow").gameObject.SetActive(true);
        myTalentTree.parent.parent.Find("ClassWindow").GetComponent<TalentClassWindow>().subClassPosition = subClassPosition;
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

            talentsToDestroy.ForEach(k => GameObject.Destroy(k));
            talentsToDestroy.Clear();
        }
    }

    public void ResetRingTuning()
    {
        if (isTurning)
        {
            return;
        }
        
        for (int i = 0; i < 4; i++)
        {
            Transform detunedRing = myTalentTree.Find("Tier" + (i+1).ToString());

            if ((int)detunedRing.rotation.eulerAngles.z == 120 || (int)detunedRing.rotation.eulerAngles.z == -240)
            {
                StartCoroutine(turningRingCoroutine(detunedRing, detunedRing.rotation.eulerAngles.z, false));
            }
            else if ((int)detunedRing.rotation.eulerAngles.z == 240 || (int)detunedRing.rotation.eulerAngles.z == -120)
            {
                StartCoroutine(turningRingCoroutine(detunedRing, detunedRing.rotation.eulerAngles.z, true));
            }
        }
        isTurning = true;
    }

    public void TurnRing(int ringNr)
    {
        if (isTurning)
        {
            return;
        }
        Transform movingRing = myTalentTree.Find("Tier" + ringNr.ToString());
        float startAngle = movingRing.rotation.eulerAngles.z;
        bool directionLeft = false;
        StartCoroutine(turningRingCoroutine(movingRing, startAngle, directionLeft));
        isTurning = true;
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
            Transform talent = currentRing.GetChild(i);
            Vector2 globalDirection = Quaternion.Euler(0, 0, currentRing.rotation.eulerAngles.z) * talent.localPosition.normalized;
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
        }
    }
}