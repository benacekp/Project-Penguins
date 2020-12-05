using System;
using System.Collections.Generic;
using BlackFramework.ScriptableArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.ScriptableAssets.SpinWheelWins
{
    [CreateAssetMenu(fileName = "NewSpinSheelWinsManager", menuName = "Penguins/SpinWheel/Create New SpinWheel Manager")]
    public class SpinWheelWinsManager : SingletonScriptableObject<SpinWheelWinsManager>
    {
        public List<SpinWheelWinData> Wins = new List<SpinWheelWinData>();

        public SpinWheelWinData GetData(string pId)
        {
            foreach (SpinWheelWinData data in Wins)
            {
                if (data.Id.Contains(pId))
                    return data;
            }

            return null;
        }
    }
}
