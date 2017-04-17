using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spewnity;

public class MutinyModel : MonoBehaviour 
{
    public MutinyEvent onReset;
    public MutinyEvent onStartMission;
    public PirateEvent onPirateCreated;
    public TaskEvent onTaskCreated;

    private int numPirates = 9;
    private int mission = 0;
    public List<Pirate> pirates = new List<Pirate>();
    public List<Task> tasks = new List<Task>();

    // Reset for a new game
    public void reset()
    {
        mission = 0;
        tasks.Clear();
        createPirates();
        onReset.Invoke(this);

        startMission();
    }

    private void createPirates()
    {
        SkillEnum[] skillList = new SkillEnum[] {};

        // Create pirates. Randomize pirate skills.
        for(int pi = 0; pi < numPirates; pi++)
        {
            Pirate pirate = new Pirate();
            pirates.Add(pirate);

            for(int si = 0; si < Pirate.numSkills; si++)
            {
                if(skillList.Length == 0)
                    skillList = getAllSkills();
                SkillEnum skill = skillList.Rnd();
                pirate.skills.Add(skill);
            }
            pirate.skills.Sort((a,b) => a < b ? -1 : 0);

            onPirateCreated.Invoke(pirate);
        }

        // Choose the mutineer
        pirates.Rnd().mutiny = Pirate.maxMutiny;
    }

    public void startMission()
    {
        mission++;
        createTasks();
        onStartMission.Invoke(this);
    }

    private void createTasks()
    {
        SkillEnum[] skillList = new SkillEnum[] {};

        for(int ti = 0; ti < 3; ti++)
        {
            Task task = new Task();
            tasks.Add(task);

            for(int si = 0; si < Task.numSkills; si++)
            {
                if(skillList.Length == 0)
                    skillList = getAllSkills();
                
                SkillEnum skill = skillList.Rnd();
                task.skills.Add(skill);
            }
            task.skills.Sort((a,b) => a < b ? -1 : 0);

            task.gold = Random.Range(Task.minGold, Task.maxGold + 1);
            float v = Random.value;
            task.crew = v < 0.5 ? 1 : (v < 0.9 ? 2 : 3);

            onTaskCreated.Invoke(task);
        }        
    }

    private SkillEnum[] getAllSkills()
    {
        return new SkillEnum[] { SkillEnum.Looting, 
            SkillEnum.Fighting, SkillEnum.Cooking, 
            SkillEnum.Boating, SkillEnum.Cleaning };
    }
}

public class Pirate
{
    public const int numSkills = 3;
    public  const int maxMutiny = 5;
    public List<SkillEnum> skills = new List<SkillEnum>();
    public int mutiny = 0;

    public Pirate()
    {
    }
}

public class Task
{
    public const int numSkills = 3;
    public const int maxGold = 5;
    public const int minGold = 1;
    public List<SkillEnum> skills = new List<SkillEnum>();
    public int gold;
    public int crew;

    public Task()
    {        
    }
}

[System.Serializable]
public class MutinyEvent: UnityEvent<MutinyModel> { }
[System.Serializable]
public class PirateEvent: UnityEvent<Pirate> { }
[System.Serializable]
public class TaskEvent: UnityEvent<Task> { }
