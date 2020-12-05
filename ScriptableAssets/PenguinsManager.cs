using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace _Scripts.ScriptableAssets
{
    [TypeInfoBox("Uchovává data penguinů, která má hráč ve svém stádu.")]
    [CreateAssetMenu(fileName = "NewPenguinsManager", menuName = "Penguins/Create Penguins Manager")]
    public class PenguinsManager : SerializedScriptableObject
    {
	    [OdinSerialize]
	    public List<PenguinData> Penguins { get; private set; }
	    

	    // nastaví testovací penguiny
	    [Button, PropertyOrder(-1)]
	    public void ResetPenguins()
	    {
		    Penguins = new List<PenguinData>();
		    Penguins.Add(new PenguinData("Red", Color.red));
		    Penguins.Add(new PenguinData("Green", Color.green));
		    Penguins.Add(new PenguinData("Blue", Color.blue));
	    }

    }
    
    public struct PenguinData
    {
	    [OdinSerialize]
	    public string Name { get; private set; }

	    [OdinSerialize]
	    public Color Color { get; private set; }

	    public PenguinData(string pName, Color pColor)
	    {
		    Name = pName;
		    Color = pColor;
	    }
    }
}
