using System;
using System.Collections.Generic;
using System.Threading;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Simulators;
using TibiaHeleper.Storage;
using System.Linq;
using TibiaHeleper.Modules.WalkerModule;
using TibiaHeleper.Modules.TargetingModule;

namespace TibiaHeleper.Modules.Targeting
{
    [Serializable]
    public class Targeter : Module
    {
        public bool working { get; set; }
        public bool stopped { get; set; }
        public List<Target> targetSettingsList { get; set; } //has to be sorted by priority
        public bool attacking { get; set; }
        public Item nextContainer { get; set; }
        public bool openNextContainer { get; set; }

        public Targeter()
        {
            stopped = true;
            targetSettingsList = new List<Target>();
            foodList = new List<Item>();
            lootList = new List<LootItem>();
            nextContainer = ItemList.Items.First(item => item.name == "Backpack");
        }
        static Targeter()
        {
            rand = new Random();
        }

        public List<Item> foodList;
        public List<LootItem> lootList;
        private static Random rand;

        public void Run()
        {
            attacking = false;
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
                            if (settings.name.ToUpper() == creature.name.ToUpper())
                            {
                                if (isTargetBetter(target, creature) && settings.maxDistance >= GetData.GetDistance(creature))
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
                if (gotTarget && !GetData.AmInPZ)
                {
                    lock (targetSettingsList)
                    {
                        attacking = true;
                        attack(target, Settings);
                        attacking = false;
                    }
                }
                else Thread.Sleep(500);
            }
            stopped = true;
        }

        private void attack(Creature creature, Target settings)
        {
            //  GetData.FollowTarget = settings.followTarget;
            MouseSimulator.clickOnField(creature.XPosition, creature.YPosition, true);

            int x, y; //setting follow mode
            GetData.getItemFromEQWindowPosition(out x, out y, Constants.FollowTargetModeXOffset, Constants.FollowTargetModeYOffset[settings.followTarget ? 1 : 0]);
            

            Thread.Sleep(200);
            while (working && GetData.getTargetID == creature.id && creature.HPPercent > settings.minHP && creature.HPPercent <= settings.maxHP)//waiting until creature is dead or target is not reachable
            {
                MouseSimulator.click(x, y);
                KeyboardSimulator.Simulate(settings.action);
                if(settings.diagonal)
                    tryToStandDiagonal();
                Thread.Sleep(500);
                if (settings.diagonal)
                    tryToStandDiagonal();
                Thread.Sleep(500);
            }
            if(creature.HPPercent == 0 && (settings.lookForFood || settings.loot))
            {
                if (GetData.isAnyWindowOpened)
                    GetData.closeAllOpenedWindows(true);
                else
                    GetData.openBackpack();
                
                if (settings.loot)
                {
                    bool itemDropped;

                    if (settings.lookForFood && GetData.HungryTime < 180)
                        itemDropped = GetData.CheckLoot(lootList, foodList);
                    else
                        itemDropped = GetData.CheckLoot(lootList);

                    if (itemDropped)
                    {
                        MouseSimulator.clickOnField(creature.XPosition, creature.YPosition, true);
                        GetData.waitForWindowOpen(GetData.secondOpenedWindow);
                        LootAndEat(creature, settings.lookForFood);

                    }
                }
                
                
            }
           
        }


        private void LootAndEat(Creature creature, bool lookForFood)
        {
            

            int lastItemId=0, itemId, count=0;
            List<LootItem> removed = new List<LootItem>();


            lock (lootList)
            {

                int x, y;
                List<int> foodAndLootList = Item.ToIdList(lootList);


                if (lookForFood && GetData.HungryTime < 180)
                    foodAndLootList.AddRange(Item.ToIdList(foodList));
               

                
                    while (GetData.getItemCoordinatesFromOpenedWindow(out x, out y, out itemId, foodAndLootList, GetData.secondOpenedWindow))
                    {
                        if (lastItemId == itemId)
                            count++;
                        else
                            count = 0;

                        if (lootList.Any(li => li.ID == itemId)) //if it is loot item
                        {
                            lastItemId = itemId;
                            LootItem it = lootList.First(item => item.ID == itemId);


                            int toX, toY;
                            //find container to put it
                            if (GetData.getItemCoordinatesFromOpenedWindow(out toX, out toY, it.container.ID, GetData.firstOpenedWindow))
                            {
                                MouseSimulator.drag(x, y, toX, toY, true);
                                Thread.Sleep(200);
                                KeyboardSimulator.Press("Enter");
                            }
                            else
                            {
                                if (openNextContainer)
                                {
                                    if (GetData.UseItemFromOpenedWindow(nextContainer.ID, GetData.firstOpenedWindow))
                                        Thread.Sleep(1000);
                                    count = 5;
                                }
                            }

                            if (count > 5) // remove item which makes a problem
                            {
                                foodAndLootList.Remove(itemId);
                            }
                        }
                        else if (GetData.HungryTime < 180)  // if it is food
                            MouseSimulator.click(x, y, true);
                        else
                            foodAndLootList.Remove(itemId);
                    }
                
            }
        }


       
        private void tryToStandDiagonal()
        {
            List<Creature> CreaturesToStayDiagonal = new List<Creature>();
            List<Target> Names = new List<Target>();
            foreach (Target t in targetSettingsList)
            {
                if (t.diagonal)
                    Names.Add(t);
            }

            List<Creature> battleList = GetData.GetBattleList();
            int btc = battleList.Count;
            for (int i = 0; i < battleList.Count; i++) //removing targets too far away 
            {

                if (GetData.GetDistance(battleList[i]) > 1)
                {
                    battleList.RemoveAt(i);
                    i--;
                }
            }
            foreach (Creature c in battleList)
            {
                if (targetSettingsList.Any(t => t.name == c.name))
                    CreaturesToStayDiagonal.Add(c);
            }
            int[] direction = new int[10];
            int x = GetData.MyXPosition;
            int y = GetData.MyYPosition;
            int floor = GetData.MyFloor;
            foreach (Creature c in CreaturesToStayDiagonal) // get the best direction to stay diagonal
            {
                if (c.XPosition == x)
                {
                    direction[3]++;
                    direction[6]++;
                    direction[9]++;
                    direction[1]++;
                    direction[4]++;
                    direction[7]++;
                    direction[5]--;

                    if (c.YPosition > y)
                    {
                        direction[1]--;
                        direction[3]--;
                    }
                    else
                    {
                        direction[7]--;
                        direction[9]--;
                    }
                }
                if (c.YPosition == y)
                {
                    direction[1]++;
                    direction[2]++;
                    direction[3]++;
                    direction[7]++;
                    direction[8]++;
                    direction[9]++;
                    direction[5]--;

                    if (c.XPosition > x)
                    {
                        direction[3]--;
                        direction[9]--;
                    }
                    else
                    {
                        direction[1]--;
                        direction[7]--;
                    }
                }
                if(c.XPosition < x && c.YPosition < y) //left top corner
                {
                    direction[1]--;
                    direction[4]--;
                    direction[8]--;
                    direction[9]--;
                }
                else if (c.XPosition > x && c.YPosition < y) //right top corner
                {
                    direction[3]--;
                    direction[6]--;
                    direction[7]--;
                    direction[8]--;
                }
                else if (c.XPosition > x && c.YPosition < y) //right bottom corner
                {
                    direction[6]--;
                    direction[7]--;
                    direction[1]--;
                    direction[2]--;
                }
                else if (c.XPosition > x && c.YPosition < y) //left bottom corner
                {
                    direction[2]--;
                    direction[3]--;
                    direction[4]--;
                    direction[7]--;
                }
            }
            Creature target=null;
            foreach (Creature c in battleList) //deleting directions where can not go
            {
                if (c.XPosition < x && c.YPosition > y) direction[1] = 0;
                if (c.XPosition == x && c.YPosition > y) direction[2] = 0;
                if (c.XPosition > x && c.YPosition > y) direction[3] = 0;
                if (c.XPosition < x && c.YPosition == y) direction[4] = 0;
                if (c.XPosition == x && c.YPosition == y) direction[5] = 0;
                if (c.XPosition > x && c.YPosition == y) direction[6] = 0;
                if (c.XPosition < x && c.YPosition < y) direction[7] = 0;
                if (c.XPosition == x && c.YPosition < y) direction[8] = 0;
                if (c.XPosition > x && c.YPosition < y) direction[9] = 0;
                if (GetData.getTargetID == c.id) // getting target
                    target = c;
            }

            if (target != null) //to not go away of target
            {
                if(target.XPosition> x)
                {
                    direction[7] = direction[4] = direction[1] = -10;
                }
                else if(target.XPosition < x)
                {
                    direction[9] = direction[6] = direction[3] = -10;
                }

                if (target.YPosition > y)
                {
                    direction[7] = direction[8] = direction[9] = -10;
                }
                else if (target.YPosition < y)
                {
                    direction[1] = direction[2] = direction[3] = -10;
                }
            }

            direction[1]--; direction[3]--; direction[7]--; direction[9]--; // those movments are more expensive

            int max = 0, count = 0;
            for (int i = 0; i < direction.Length; i++)
            {
                if (direction[i] == max)
                    count++;

                if (direction[i] > max)
                {
                    max = direction[i];
                    count = 1;
                }
            }
            int dir=5;
            for (int i = 0; i < direction.Length; i++)
            {
                if (max > direction[5])
                {
                    if (dir == 5 || (direction[i] == max && rand.Next(0, count) == 0))
                    {
                        dir = i;
                    }
                }
               
            }

            if (dir == 1 || dir == 2 || dir == 3)
                y++;
            else if (dir == 7 || dir == 8 || dir == 9)
                y--;
            if (dir == 1 || dir == 4 || dir == 7)
                x--;
            else if (dir == 3 || dir == 6 || dir == 9)
                x++;

            if (dir!=5)
                ModulesManager.walker.go(new Waypoint(x, y, floor), dir);     

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
            else if(Math.Abs(newOne.XPosition - GetData.MyXPosition) > 7 || Math.Abs(newOne.YPosition - GetData.MyYPosition) >5)
            {
                return false;
            }
            else if (oldOne == null)
            {
                return true;
            }
            else if (oldOne.HPPercent > newOne.HPPercent) //get the lowest hp target
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
            return (List<Target>)Storage.Storage.Copy(targetSettingsList);
        }
        public void setLootList(List<LootItem> list)
        {
            lock (lootList)
            {
                lootList = list;
            }
        }
        public void setFoodList(List<Item> list)
        {
            lock (foodList)
            {
                foodList = list;
            }
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
