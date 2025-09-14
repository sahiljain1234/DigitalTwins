using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PixyzPlugin4Unity.UI;

public class NewCustomAction_759574155 : ActionInOut<IList<GameObject>, IList<GameObject>> {

    [UserParameter]
    public float aFloatParameter = 1f;

    [UserParameter]
    public string aStringParameter = "Text";

    [HelperMethod]
    public void resetParameters() {
        aFloatParameter = 1f;
        aStringParameter = "Text";
    }

    public override int id { get { return 759574155;} }
    public override string menuPathRuleEngine { get { return "Custom/New Custom Action";} }
    public override string menuPathToolbox { get { return null;} }
    public override string tooltip { get { return "A Custom Action";} }

    public override IList<GameObject> run(IList<GameObject> input) {
        /// Your code here
        /// 


        return input;
    }
}