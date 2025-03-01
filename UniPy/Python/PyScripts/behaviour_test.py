import UnityEngine

class Test():

    this = None

    def Awake(self):
        UnityEngine.Debug.Log("[Behaviour Test] Awake")
        
    def OnDestroy(self):
        UnityEngine.Debug.Log("[Behaviour Test] OnDestroy")
    
    def Start(self):
        UnityEngine.Debug.Log("[Behaviour Test] Start")
        UnityEngine.Debug.Log(f"{this.ReferenceDict['MC'].name} is this")


    def Update(self):
        UnityEngine.Debug.Log("[Behaviour Test] Update")

        if UnityEngine.Input.GetButtonDown("Submit"):
            sph = this.ReferenceDict["PB2"].InvokeArgs("SecondFunc", 2)
            sph.AddComponent(UnityEngine.Rigidbody)

        if UnityEngine.Input.GetButtonDown("Jump"):

            if "cube" not in this.Variables.keys():
                this.Variables["cube"] = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube)
                this.Variables["cube"].transform.position -= this.Variables["cube"].transform.up * 5
                this.Variables["cubecount"] = 1
            else:
                newcube = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube)
                newcube.transform.position = this.Variables["cube"].transform.position
                newcube.transform.position += newcube.transform.up * 1

                this.Variables["cube"] = newcube
                this.Variables["cubecount"] += 1

            UnityEngine.Debug.Log(f"The current number of cubes is {this.Variables['cubecount']}")

    def PassMBReference(self, mb):
        global this
        this = mb


class Test1():
    
    def Awake(self):
        UnityEngine.Debug.Log("[Behaviour Test1] Awake")
        
    def OnDestroy(self):
        UnityEngine.Debug.Log("[Behaviour Test1] OnDestroy")

    def Update(self):
        UnityEngine.Debug.Log("[Behaviour Test] Update")

    def Start(self):
        UnityEngine.Debug.Log("[Behaviour Test] Start")

    def SecondFunc(self,upoffset):
        sphere = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Sphere)
        sphere.transform.position += sphere.transform.up * upoffset
        return sphere