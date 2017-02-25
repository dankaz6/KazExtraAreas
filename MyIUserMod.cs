using ICities;
using UnityEngine;

namespace kazextraareas
{
    public class MyIUserMod : IUserMod
    {
        public string Name
        {
            get { return "KazExtraAreasMod"; }
        }

        public string Description
        {
            get { return "Allow maximum of 25 areas and unlock all 25 areas when one area is purchased"; }
        }
    }
}