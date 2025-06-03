using System;
using BepInEx;
using Bepinject;
using GorillaPronouns.ComputerInterface.Models;
using UnityEngine;
using Utilla;

namespace GorillaPronouns.ComputerInterface
{
    [BepInDependency("tonimacaroni.computerinterface", "1.7.5")]
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public void Awake()
        {
            Zenjector.Install<MainInstaller>().OnProject();
        }
    }
}
