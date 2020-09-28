/*
 * R e a d m e
 * -----------
 * 
 * In this file you can include any instructions or other comments you want to have injected onto the 
 * top of your final script. You can safely delete this file if you do not want any such comments.
 */


List<CargoItems> AmmoComps = new List<CargoItems>()
{
    new CargoItems("SteelPlate",0),
    new CargoItems("Motor",0),
    new CargoItems("PowerCell",0),
    new CargoItems("Construction",0),
    new CargoItems("Computer",0),
    new CargoItems("Superconductor",0),
    new CargoItems("LargeTube",0),
    new CargoItems("GravityGenerator",0),
};
//initialize variables
IMyTextPanel Comp;
IMyTextPanel AmmoCount;
CargoItems temp;
public Program()
{
    //updates every 100 ticks
    Runtime.UpdateFrequency=UpdateFrequency.Update100;

    AmmoCount = GridTerminalSystem.GetBlockWithName("AmmoCount") as IMyTextPanel;
    Comp = GridTerminalSystem.GetBlockWithName("Comp") as IMyTextPanel;
    temp = new CargoItems("name", 0);

}
//linearly searches through all containers in the grid for all items required to make a shell, gets the count of each item
public void CountItems()
{
    List<IMyTerminalBlock> Cargo = new List<IMyTerminalBlock>();
    List<MyInventoryItem> itemList = new List<MyInventoryItem>();
    GridTerminalSystem.GetBlocksOfType<IMyEntity>(Cargo, block => block.InventoryCount > 0);
    foreach (IMyTerminalBlock CBlock in Cargo)
    {

        itemList.Clear();
        CBlock.GetInventory(0).GetItems(itemList, item => item.Type.SubtypeId.Equals("SteelPlate") || item.Type.SubtypeId.Equals("Motor") ||
        item.Type.SubtypeId.Equals("PowerCell") || item.Type.SubtypeId.Equals("Computer") || item.Type.SubtypeId.Equals("Construction") ||
        item.Type.SubtypeId.Equals("LargeTube") || item.Type.SubtypeId.Equals("Superconductor") || item.Type.SubtypeId.Equals("GravityGenerator"));

        foreach (MyInventoryItem item in itemList)
        {
            for (int i = 0; i < AmmoComps.Count; i++)
            {
                AmmoComps[i].CompareAndAdd(item);
            }
        }

        if (CBlock.InventoryCount > 1)
        {
            itemList.Clear();
            CBlock.GetInventory(1).GetItems(itemList, item => item.Type.SubtypeId.Equals("SteelPlate") || item.Type.SubtypeId.Equals("Motor") ||
            item.Type.SubtypeId.Equals("PowerCell") || item.Type.SubtypeId.Equals("Computer") || item.Type.SubtypeId.Equals("Construction") ||
            item.Type.SubtypeId.Equals("LargeTube") || item.Type.SubtypeId.Equals("Superconductor") || item.Type.SubtypeId.Equals("GravityGenerator"));

            foreach (MyInventoryItem item in itemList)
            {
                for (int i = 0; i < AmmoComps.Count; i++)
                {
                    AmmoComps[i].CompareAndAdd(item);
                }
            }
        }
    }
}

public void ResetCount()
{
    for (int i = 0; i < AmmoComps.Count; i++)
    {
        AmmoComps[i].Clear();
    }
}
//calculates the average amount of shells you can make with the amount of items you have
public int ShellAmt(List<CargoItems> list)
{
    double sum = 0;

    foreach(CargoItems item in list)
    {
        if(item.GetName().Equals("SteelPlate"))
        {
            sum += item.GetAmount() / 182;
        }

        if (item.GetName().Equals("Motor"))
        {
            sum += item.GetAmount() / 2;
        }

        if (item.GetName().Equals("PowerCell"))
        {
            sum += item.GetAmount() / 80;
        }

        if (item.GetName().Equals("Construction"))
        {
            sum += item.GetAmount() / 75;
        }

        if (item.GetName().Equals("Computer"))
        {
            sum += item.GetAmount() / 47;
        }

        if (item.GetName().Equals("Superconductor"))
        {
            sum += item.GetAmount() / 20;
        }

        if (item.GetName().Equals("LargeTube"))
        {
            sum += item.GetAmount() / 6;
        }

        if (item.GetName().Equals("GravityGenerator"))
        {
            sum += item.GetAmount() / 9;
        }

        if (item.GetAmount() == 0)
        {
            return 0;
        }
    }

    return (int) Math.Floor(sum/8);
}
//prints out each item and its amount
public string ToString(List<CargoItems> list)
{
    string output="";
    foreach(CargoItems item in list)
    {
        output += item.GetName() + ": " + item.GetAmount() + "\n";
    }

    return output;
}



public void Main(string argument, UpdateType updateSource)
{
    ResetCount();
    CountItems();

    AmmoCount.WriteText("Ammo Remaining: " + ShellAmt(AmmoComps));
    Comp.WriteText(ToString(AmmoComps));
}

public class CargoItems
{
    public string ItemName;
    public double Quantity;

    public CargoItems(string name, double amount)
    {
        ItemName = name;

        Quantity = amount;
    }

    public double GetAmount()
    {
        return Quantity;
    }

    public String GetName()
    {
        return ItemName;
    }

    public void AddTo(double amt)
    {
        Quantity += amt;
    }

    public void Clear()
    {
        Quantity = 0;
    }

    public void CompareAndAdd(MyInventoryItem item)
    {
        if (item.Type.SubtypeId.Equals(ItemName))
        {
            Quantity += (double)item.Amount;
        }
    }


}