

using System;
using System.Collections.Generic;
using Disc0ver.PythonPlugin;
using Python.Runtime;
using UnityEngine;

namespace Disc0ver.Engine
{
    public class PyBehaviour: MonoBehaviour
    {
        [System.Serializable]
        public class ObjectReference
        {
            public string fieldName;
            public UnityEngine.Object referenced;
        }



        public string scriptPath = "";
        public string className = "";
        private PyObject _pyObject;

        public PyObject Env => _pyObject;
        
        private Action _pyAwake;
        private Action _pyStart;
        private Action _pyOnDestroy;
        private Action _pyUpdate;

        public ObjectReference[] References;
        public Dictionary<string, UnityEngine.Object> ReferenceDict;
        public Dictionary<string, object> Variables;

        public PyObject Invoke(string functionname)
        {

            using (Py.GIL())
            {
                var module = PythonModule.Import(scriptPath);
                var pyClass = module.GetAttr(className);
                _pyObject = pyClass.Invoke();
                if (Env.GetAttr(functionname) != PyObject.None)
                {

                    return Env.InvokeMethod(functionname);
                }

            }
            return null;
        }

        public PyObject InvokeArgs(string functionname, params PyObject[] args)
        {
            using (Py.GIL())
            {
                var module = PythonModule.Import(scriptPath);
                var pyClass = module.GetAttr(className);
                _pyObject = pyClass.Invoke();
             

                if (Env.GetAttr(functionname) != PyObject.None)
                {

                    return Env.InvokeMethod(functionname, args);
                }

            }
            return null;
        }

        private void Awake()
        {
            Variables = new Dictionary<string, object>();
            ReferenceDict = new Dictionary<string, UnityEngine.Object>();

            if (References != null)
            {

                foreach (ObjectReference or in References)
                {
                    ReferenceDict[or.fieldName] = or.referenced;
                }
            }

            PythonModule.RunString("import main\nmain.main()");
            using (Py.GIL())
            {
                var module = PythonModule.Import(scriptPath);
                var pyClass = module.GetAttr(className);
                _pyObject = pyClass.Invoke();
            
                Env.SetAttr("Controller", this.ToPython());

                
                if (Env.HasAttr("PassMBReference"))
                {
                    Env.InvokeMethod("PassMBReference", PyObject.FromManagedObject(this));
                }


                if (Env.HasAttr("Start"))
                {
                    Debug.Log("behaviour start");
                    _pyStart += () =>
                    {
                        Env.InvokeMethod("Start");
                    };
                }

                if (Env.HasAttr("OnDestroy"))
                {
                    Debug.Log("behaviour destroy");
                    _pyOnDestroy += () =>
                    {
                        Env.InvokeMethod("OnDestroy");
                    };
                }
            
                if (Env.HasAttr("Awake"))
                {
                    Env.InvokeMethod("Awake");
                }

                if (Env.HasAttr("Update"))
                {
                    Debug.Log("behaviour update");
                    _pyUpdate += () =>
                    {
                        Env.InvokeMethod("Update");
                    };
                }

            }
        }

        private void Start()
        {
            using (Py.GIL())
            {
                _pyStart.Invoke();
            }
        }

        private void Update()
        {
            using (Py.GIL())
            {
                _pyUpdate.Invoke();
            }
        }

        private void OnDestroy()
        {
            using (Py.GIL())
            {
                Env.DelAttr("Controller");
                if(PythonEngine.IsInitialized)
                    _pyOnDestroy.Invoke();
                _pyObject = null;
            }
        }
    }
}

