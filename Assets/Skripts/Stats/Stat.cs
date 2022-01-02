using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public float baseValue;

	private List<float> modifiersAdd = new List<float>();
	private List<float> modifiersMultiply = new List<float>();


	public float GetValue()
    {
		float finalValue = baseValue;
		modifiersAdd.ForEach(x => finalValue += x);
		modifiersMultiply.ForEach(x => finalValue *= x);
		return finalValue;
    }

	public void AddModifierAdd(float modifier)
	{
		if (modifier != 0)
			modifiersAdd.Add(modifier);
	}

	public void RemoveModifierAdd(float modifier)
	{
		if (modifier != 0)
			modifiersAdd.Remove(modifier);
	}

	public void AddModifierMultiply(float modifier)
	{
		if (modifier != 0)
			modifiersMultiply.Add(modifier);
	}

	public void RemoveModifierMultiply(float modifier)
	{
		if (modifier != 0)
			modifiersMultiply.Remove(modifier);
	}

}