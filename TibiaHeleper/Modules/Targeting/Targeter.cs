using System;
using System.Collections.Generic;
using System.Threading;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Simulators;
using TibiaHeleper.Storage;

namespace TibiaHeleper.Modules.Targeting
{
    [Serializable]
    public class Targeter : Module
    {
        public bool working { get; set; }
        public bool stopped { get; set; }
        public List<Target> targetSettingsList { get; set; } //has to be sorted by priority

        public Targeter()
        {
            stopped = true;
            targetSettingsList = new List<Target>();
        }

        public void Run()
        {
            List<Creature> battleList;
            bool gotTarget;
            Creature target;
            Target Settings;
            while (working)
            {
                battleList = GetData.GetBattleList();
                gotTarget = false;
                target = null;
                Settings = null;
                lock (targetSettingsList)
                {
                    foreach (Target settings in targetSettingsList)
                    {
                        foreach (Creature creature in battleList)
                        {
                            if (settings.name == creature.name)
                            {
                                if (isTargetBetter(target, creature))
                                {
                                    gotTarget = true;
                                    target = creature;
                                    Settings = settings;
                                }
                            }
                        }
                        if (gotTarget) break;
                    }
                }                
                if (gotTarget)
                {
                    lock (targetSettingsList)
                    {
                        attack(target, Settings);
                    }
                }
                else Thread.Sleep(500);
            }
            stopped = true;
        }

        private void attack(Creature creature, Target settings)
        {
            MouseSimulator.clickOnField(creature.XPosition, creature.YPosition, true);
            while (working && GetData.getTargetID() == creature.id && creature.HPPercent > settings.minHP && creature.HPPercent <= settings.maxHP)//waiting until creature is dead or target is not reachable
            {
                KeyboardSimulator.Simulate(settings.action);
                Thread.Sleep(500);
            }
        }

        private bool isTargetBetter(Creature oldOne, Creature newOne)
        {
            if (newOne.HPPercent == 0)//if creature is dead
            {
                return false;
            }
            else if(newOne.Floor != GetData.MyFloor)
            {
                return false;
            }
            else if (oldOne == null)
            {
                return true;
            }
            else if (oldOne.HPPercent < newOne.HPPercent) //get the lowest hp target
            {
                return true;
            }
            else if (GetData.GetDistance(oldOne) > GetData.GetDistance(newOne)) // get the nearest target
            {
                return true;
            }
            return false;
        }

        public List<Target> getTargetListCopy()
        {
            List<Target> result = new List<Target>();
            lock (targetSettingsList)
            {
                foreach (Target target in targetSettingsList)
                {
                    result.Add(target);
                }
            }
            return result;
        }
        public void setTargetList(List<Target> list)
        {
            lock (targetSettingsList)
            {
                targetSettingsList = list;
            }
        }
    }
}
