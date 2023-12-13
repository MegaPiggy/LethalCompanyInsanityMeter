using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsanityMeter
{
    public static class PlayerExtensions
    {
        public static float GetInsanityPercentage(this PlayerControllerB player) => player.insanityLevel / player.maxInsanityLevel;
    }
}
