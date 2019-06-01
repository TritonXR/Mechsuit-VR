using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Valve.VR;
using TensorFlow;

namespace MSVRDev {

public class GestureRecognition : MonoBehaviour {

    public Transform rightController;

    public List<Vector3> controllerData;

    private static readonly int DEGREES_OF_FREEDOM = 6;

    private TFGraph graph;
    private TFSession session;
    [SerializeField]
    private string tf_model_filename;

    public TFTensor CreateTensorFromFloat3D(float[,,] gesture) {
        var runner = session.GetRunner();
        TFTensor tensor = new TFTensor(gesture);
        runner.AddInput(graph["input"][0], tensor);
        runner.Fetch(graph["output"][0]);

        var output = runner.Run();

        // Fetch the results from output:
        TFTensor result = output[0];
        return result;
    }

    // Use this for initialization
    void Start() {
        graph = new TFGraph();
        string path_to_tf_model = Application.dataPath + "/ML_Custom/" + tf_model_filename;
        Debug.Log(path_to_tf_model);
        graph.Import(File.ReadAllBytes(path_to_tf_model));
        session = new TFSession(graph);
    }

    // Update is called once per frame
    void Update() {
        // If trackpad is pressed down, append translation and rotation data to controllerData
        if (InputManager.Instance.GetButtonInput(ButtonInput.SummonWeapon, Hand.RightHand)) {
            controllerData.Add(rightController.localPosition);
            controllerData.Add(rightController.localEulerAngles);
        }
        // If trackpad is not pressed down, check if there is enough data to classify gesture
        else {
            // If controllerData has data, convert it to a 3D array of floats
            if (controllerData.Count != 0) {
                float[,,] gesture = new float[1, controllerData.Count / 2, DEGREES_OF_FREEDOM];
                int idx = 0;
                while (idx < controllerData.Count) {
                    int gIdx = idx / 2;
                    // If current controllerData element is a translation
                    gesture[0, gIdx, 0] = controllerData[idx].x;
                    gesture[0, gIdx, 1] = controllerData[idx].y;
                    gesture[0, gIdx, 2] = controllerData[idx].z;
                    // If current controllerData element is a rotation
                    gesture[0, gIdx, 3] = controllerData[idx + 1].x;
                    gesture[0, gIdx, 4] = controllerData[idx + 1].y;
                    gesture[0, gIdx, 5] = controllerData[idx + 1].z;
                    idx += 2;
                }
                // Create tensor from the 3D array
                TFTensor classification = CreateTensorFromFloat3D(gesture);
                // Flush controllerData
                controllerData.Clear();
                // Log result of gesture classification
                Debug.Log("Gesture classified as " + classification);
            }
        }
    }
}

}