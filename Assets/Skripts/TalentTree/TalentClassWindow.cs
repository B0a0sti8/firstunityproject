using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentClassWindow : MonoBehaviour
{
    public string subClassPosition;
    private TalentTree myTalentTree;
    void Start()
    {
        myTalentTree = transform.parent.GetComponent<TalentTree>();
    }
    // Start is called before the first frame update
    public void ChangeClassToButtonString(string newClassName)
    {
        if (subClassPosition == "Main")
        { myTalentTree.subClassMain = newClassName; }

        if (subClassPosition == "Left")
        { myTalentTree.subClassLeft = newClassName; }

        if (subClassPosition == "Right")
        { myTalentTree.subClassRight = newClassName; }

        CloseWindow();

        myTalentTree.UpdateSkillTree();
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
