using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class GameView : MonoBehaviour 
{
	[HideInInspector]
	public SkillView[] skills;
	
	void Start() 
	{
		foreach(PirateView pv in transform.GetComponentsInChildren<PirateView>())
		{
			pv.setSkill(0, (SkillEnum) Random.Range(1,6));
			pv.setSkill(1, (SkillEnum) Random.Range(1,6));
			pv.setSkill(2, (SkillEnum) Random.Range(1,6));
		}

		foreach(TaskView tv in transform.GetComponentsInChildren<TaskView>())
		{
			tv.setCoin(4);
			tv.setCrew(2);
			tv.setSkill(0, (SkillEnum) Random.Range(1,6));
			tv.setSkill(1, (SkillEnum) Random.Range(1,6));
			tv.setSkill(2, (SkillEnum) Random.Range(0,6));
		}
	}
	
	public Sprite getSpriteForSkill(SkillEnum skillEnum)
	{
		foreach(SkillView skill in skills)
			if(skill.type == skillEnum)
				return skill.sprite;
		Debug.Log("Skill not found:" + skillEnum);
		return null;
	}
}


[System.Serializable]
public struct SkillView
{
	public SkillEnum type;
	public Sprite sprite;
}