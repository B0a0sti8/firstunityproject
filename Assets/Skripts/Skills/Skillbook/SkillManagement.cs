














						// Kann gelöscht werden





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Die Idee ist, dass in dem Dictionary SkillDict alle aktuellen Skills gespeichert werden, die der Charakter momentan hat.
// Der Key dient zudem als Nummerierung, dass die Skills richtig im Skillbook angezeigt werden: ZZZ_XXX_XX
// ZZZ Sind Buchstaben, die eine Aussage über die Klasse machen und zwischen Talenten und 'normalen' Skills unterscheiden.
// Die ersten Ziffern (XXX) sind das Level auf dem es gelernt wird, die zweiten (XX) dienen der Nummerierung innerhalb eines Levels.
// Also z.B. WaN_007_02 Für den zweiten 'normalen' Krieger-Skill auf Level 7. Bei Talenten können die letzten beiden Ziffern vermutlich auf 0 gesetzt werden.

public class SkillManagement : MonoBehaviour
{
	#region Singleton

	public static SkillManagement instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<SkillManagement>();
			}
			return _instance;
		}
	}
	static SkillManagement _instance;

	void Awake()
	{
		_instance = this;
	}

	#endregion

	public Dictionary<string, string> SkillsDict { get; private set; } // Alle Skills die der Charakter im Moment hat
	GameObject PLAYER;
	GameObject skillBook;
	int nrOfSkillBookButtons;

	void Start() // Start is called before the first frame update
	{
		PLAYER = gameObject.transform.parent.gameObject;
		//skillBook = PLAYER.transform.Find("Own Canvases").transform.Find("Canvas Skillbook").transform.Find("Skillbook").transform.Find("BlueMageSkills").gameObject;
		SkillsDict = new Dictionary<string, string>();
		//nrOfSkillBookButtons = skillBook.transform.childCount;
	}

	public void ClearSkillsDict() // Löscht das gesamte Dictionary, z.B. wenn Klasse gewechselt wird.
    {
		SkillsDict.Clear();
	}

	public void SkillsDictAdd(string SkillNr, string SkillName) // Fügt einen Eintrag zum Dictionary hinzu
    {
		SkillsDict.Add(SkillNr, SkillName);
    }

	public void UpdateSkillBook() // Hier werden zunächst alle Skills aus dem Skillbook entfernt und anschließend alle aktuell im Dictionary vorhandenen Skills ins Skillbook hinzugefügt.
	{
        for (int i = 0; i < nrOfSkillBookButtons; i++)
        {

        }
    }

}
