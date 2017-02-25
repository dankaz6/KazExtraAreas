using ICities;
using UnityEngine;
using System.IO;

namespace kazextraareas
{
    /* 
        The purpose of this mod is to set the maximum number of playable areas to 25
        and to quickly unlock all areas.  It would be best if we could unlock all areas
        immediately, but it gets complicated to do that while the map is initializing.
        Instead, this mod requires the user to purchase (unlock) one area, and when they 
        do that, any other locked areas are also unlocked at the same time.  This approach 
        keeps it simple because we can use the published AreasExtensionBase class.
    */

    public class MyIAreasExtension : AreasExtensionBase
    {
        private IAreas myAreas = null;

        // define constants for the maximum area size
        private static int MaxAreaX = 5;
        private static int MaxAreaZ = 5;
        private static int MaxAreas = MaxAreaX * MaxAreaZ;

        public override void OnCreated(IAreas areas)
        {
            base.OnCreated(areas);

            // save the reference to the IAreas object for future use
            myAreas = areas;

            // set the maximum number of areas that can be played
            areas.maxAreaCount = MaxAreas;
        }

        public override void OnUnlockArea(int x, int z)
        {
            // define variables to keep track of which area is still locked
            int lockedX = -1;
            int lockedZ = -1;
            bool foundLockedArea = false;

            // first, do what this function is meant to do and continue unlocking area x,z
            base.OnUnlockArea(x, z);

            // next, search for another area which is still locked
            // (note that we purposely don't want to unlock all remaining areas at once inside the loop - we'll unlock one new area for each call to this function) 
            if (myAreas.unlockedAreaCount < myAreas.maxAreaCount)
            {
                for (int ix = 0; ix < MaxAreaX; ix++)
                {
                    for (int iz = 0; iz < MaxAreaZ; iz++)
                    {
                        // if we find a locked area, save the info so that we can unlock it
                        if (myAreas.CanUnlockArea(ix, iz))
                        {
                            lockedX = ix;
                            lockedZ = iz;
                            foundLockedArea = true;
                        }
                    }
                }
            }

            // if we found a locked area, unlock it
            if (foundLockedArea)
            {
                // note that unlocking this area will call this function again; on the next call we'll unlock another area and keep recursing one area at a time until all areas are unlocked
                myAreas.UnlockArea(lockedX, lockedZ, false);
            }
        }
    }
}