using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ParticleGenerator))]
public class ParticleGeneratorEditor : Editor
{
    ParticleGenerator particleGenerator;
    Editor[] setting;
    delegate void Function(int i);
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (particleGenerator.mapParticleSetting == null)
            return;
        int cnt = particleGenerator.mapParticleSetting.Length;
        for(int i = 0; i < cnt; i++)
        {
            if (GUILayout.Button("Initialize" + i))
            {
                particleGenerator.InitializeParticle(i);
            }
        }
        
    }
    private void OnEnable()
    {
        particleGenerator = (ParticleGenerator)target;
    }
}
