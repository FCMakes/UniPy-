using System;
using UnityEngine;
using Disc0ver.Engine;
namespace Disc0ver.PythonPlugin
{
    public class Test: MonoBehaviour
    {
       
        
        private void Start()
        {

            PyBehaviour[] pbs = FindObjectsOfType<PyBehaviour>(true);
            foreach (PyBehaviour pb in pbs)
            {
                pb.enabled = false;
            }


            PythonModule.RunString("import main\nmain.main()");
            foreach (PyBehaviour pb in pbs)
            {
                pb.enabled = true;
            }
        }
    }
}