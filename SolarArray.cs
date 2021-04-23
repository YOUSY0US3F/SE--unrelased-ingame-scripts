

//this script takes two rotors and a solor panel and ajusts the solar panel to face the sun
IMySolarPanel panel;

IMyMotorStator pivot, rot , rotslave , pivotslave;

bool pivotIsAligned, pivotIsMoving, pivotRightWay = false;

bool rotIsAligned, rotIsMoving, rotRightWay = false;

double lastOutput,finalOutput = 0;
List<IMyTerminalBlock> pivots;
List<IMyTerminalBlock> rots;
IMyBlockGroup rotors;

public Program()
{
    //updates every tick
    Runtime.UpdateFrequency = UpdateFrequency.Update1;
    pivots = new List<IMyTerminalBlock>();
    rots = new List<IMyTerminalBlock>();
    rotors = GridTerminalSystem.GetBlockGroupWithName("pivots");
    if (rotors != null)
    {
        rotors.GetBlocks(pivots);
    }
    //if there are no detected rotors to slave the script will only use the one solar panel
    else
    {
        Echo(" no pivots group found, going solo");
    }
    rotors = GridTerminalSystem.GetBlockGroupWithName("rots");
    if (rotors != null)
    {
        rotors.GetBlocks(rots);
    }
    else
    {
        Echo(" no rots group found, going solo");
    }

    panel = GridTerminalSystem.GetBlockWithName("guide") as IMySolarPanel;
    pivot = GridTerminalSystem.GetBlockWithName("pivot") as IMyMotorStator;
    rot = GridTerminalSystem.GetBlockWithName("rot") as IMyMotorStator;
}

public void Main(string argument, UpdateType updateSource)
{
    //checks if the panel is aligned
    if (!pivotIsAligned)
    {
        //if the panel is not moving it will move in the opposite direction of its last movement
        if (!pivotIsMoving)
        {
            pivotIsMoving = true;
            pivot.TargetVelocityRPM = -1;
        }
        //checks if the panels output is lower this time around
        if (panel.MaxOutput < lastOutput)
        {
            pivot.TargetVelocityRPM = -pivot.TargetVelocityRPM;
            //if it is not facing the right way it will go through the main method from the start
            if (pivotRightWay)
            {
                pivotIsAligned = true;
                pivotIsMoving = false;
                pivot.TargetVelocityRPM = 0;
                lastOutput = 0;
                finalOutput = panel.MaxOutput;
            }
            else
            {
                pivotRightWay = true;
            }
        }
    }
    //this does the samething the pivot part did but for the rotational (aka Azimuth) axis
    if (!rotIsAligned)
    {
        if (!rotIsMoving)
        {
            rotIsMoving = true;
            rot.TargetVelocityRPM = -1;
        }

        if (panel.MaxOutput < lastOutput)
        {
            rot.TargetVelocityRPM = -rot.TargetVelocityRPM;

            if (rotRightWay)
            {
                rotIsAligned = true;
                rotIsMoving = false;
                rot.TargetVelocityRPM = 0;
                lastOutput = 0;
                pivotIsAligned = false;
            }
            else
            {
                rotRightWay = true;
            }
        }
    }

    //if the maxoutput is less than 90% of the current output we go through the whole main method again
    if (rotIsAligned && pivotIsAligned && panel.MaxOutput < finalOutput*0.9 )
    {
        rotIsAligned = false;
        pivotIsAligned = false;
        pivotRightWay = false;
        pivotRightWay = false;



    }
    //once everything is aligned and the slaved solar panels exist, we align the slaved solar panels with the guide
    if (rotIsAligned && pivotIsAligned && rotors!= null)
    {
        for (int i = 0; i < pivots.Count(); i++)
        {
            pivotslave = (IMyMotorStator)pivots[i];
            rotslave = (IMyMotorStator)rots[i];
            pivotslave.TargetVelocityRad = pivot.Angle - pivotslave.Angle;
            rotslave.TargetVelocityRad = rot.Angle - rotslave.Angle;
        }
    }
    //store current output
    lastOutput = panel.MaxOutput;
}
