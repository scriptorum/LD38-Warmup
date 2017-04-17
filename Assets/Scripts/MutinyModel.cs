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

    public int numPirates = 9;
    public int mission = 0;
    public Task captainsTable, galley; // special tasks
    public List<Pirate> pirates = new List<Pirate>();
    public List<Task> tasks = new List<Task>();

    void Awake()
    {
    }

    // Reset for a new game
    public void reset()
    {
        captainsTable = new Task();
        galley = new Task();

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
            pirate.skills.Sort();

            onPirateCreated.Invoke(pirate);
        }

        // Choose the mutineer
        Pirate mutineer = pirates.Rnd();
        mutineer.mutiny = Pirate.maxMutiny;
        mutineer.mutineer = true;
    }

    public void startMission()
    {
        mission++;
        createTasks();
        onStartMission.Invoke(this);
    }

    // Determine the effects of assignment pirates to each group
    public void assignPirates(Dictionary<Task,List<Pirate>> assignments)
    {
        Pirate p;

        foreach(Task t in assignments.Keys)
        {
            // Captain's Table:
            if(t == captainsTable && assignments[captainsTable].Count > 0)
            {
                bool positiveReview = false;
                p = assignments[captainsTable][0];
                Pirate otherPirate = getOtherPirate(p);

                if(p.mutiny < Pirate.maxMutiny)
                {
                    p.mutiny = 0;
                    if(otherPirate.mutiny <= 2)
                        positiveReview = true;
                }

                // TODO give each pirate a name
                // TODO customize message
                // TODO add message variety
                // TODO fire review event
                // TODO display review and highlight pirate
                // TODO add name to pirate card display
                if(positiveReview)
                    Debug.Log("Pirate XXX has naught a bad word to say about ye.");
                else Debug.Log("Pirate XXX is a scurvy dog who speaks ill of ye.");
            }

            // Galley
            else if(t == galley && assignments[galley].Count > 0)
            {
                if(includesMutineer(assignments[galley]))
                    addMutinyToGroup(assignments[galley], 1);
            }

            // Task
            else
            {
                bool mutineerPresent = includesMutineer(assignments[t]);
                bool success = true;
                if(mutineerPresent)
                {
                    addMutinyToGroup(assignments[t], 1);
                    success = false;
                }
                else if(assignments[t].Count < t.crew)
                    success = false;
                else
                {               
                    // Determine how many success we have for each skill
                    Dictionary<SkillEnum, int> totals = new Dictionary<SkillEnum, int>();
                    foreach(SkillEnum skill in getAllSkills())
                        totals[skill] = 0;
                    foreach(Pirate member in assignments[t])
                    {
                        foreach(SkillEnum skill in member.skills)
                        {
                            int roll = Random.Range(1, 7) + member.mutiny;
                            if(roll <= 5)
                                totals[skill]++;
                        }
                    }

                    // Compare skill successes to requirements
                    foreach(SkillEnum req in t.skills)
                    {
                        if(totals[req] < 1)
                        {
                            success = false;
                            break;
                        }
                        totals[req]--; // decrement in case same skill/req appears more than once
                    }
                }

                // TODO notify
                // TODO respond to notification in view
                // TODO give names to tasks
                // TODO display name next to task?
                if(success)
                    Debug.Log("Aye aye, cap'n! Add " + t.gold + " to your chest!");
                else Debug.Log("Sorry cap'n, we tried!");
            }
        }
    }

    private void addMutinyToGroup(List<Pirate> group, int amount)
    {
        foreach(Pirate p in group)
            if(!p.mutineer && p.mutiny < 5)
                p.mutiny = System.Math.Min(p.mutiny + amount, Pirate.maxMutiny);
    }

    private bool includesMutineer(List<Pirate> group)
    {
        foreach(Pirate p in group)
            if(p.mutineer)
                return true;
        return false;
    }

    private Pirate getOtherPirate(Pirate p)
    {
        pirates.Shuffle();
        if(pirates[0] == p)
            return pirates[1];
        return pirates[0];
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
            task.skills.Sort();

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
    public bool mutineer = false;

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
