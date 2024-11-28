using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.UrdfImporter.Control;
using UnityEngine;

public class movejoint : MonoBehaviour
{
    [SerializeField] float q1 = 0;
    [SerializeField] float q2 = 0;
    [SerializeField] float q3 = 0;
    [SerializeField] float joint_speed = 10;
    private ArticulationBody[] articulationChain;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<FKRobot>();
        articulationChain = this.GetComponentsInChildren<ArticulationBody>();
    }
    public IEnumerator MoveJointToAngle(int jointIndex, float targetAngle, float speed)
    {
        if (jointIndex < 0 || jointIndex >= articulationChain.Length)
        {
            Debug.LogError("Invalid joint index.");
            yield break;
        }

        ArticulationBody joint = articulationChain[jointIndex];
        ArticulationDrive drive = joint.xDrive;

        float currentAngle = drive.target; // Start from the current target angle
        float step = speed * Time.deltaTime; // Increment based on speed and delta time

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, step); // Gradually move to the target
            drive.target = currentAngle;
            joint.xDrive = drive;

            yield return null; // Wait for the next frame
        }

        drive.target = targetAngle; // Ensure the final target is set precisely
        joint.xDrive = drive;
    }
    public IEnumerator MoveThroughJointAngleSequences(float[][] angleSequences, float speed)
    {
        // Validate the input
        if (angleSequences.Length != 3)
        {
            Debug.LogError("The angleSequences array must contain exactly 3 subarrays, one for each joint.");
            yield break;
        }

        int maxSteps = Mathf.Max(angleSequences[0].Length, angleSequences[1].Length, angleSequences[2].Length);

        for (int step = 0; step < maxSteps; step++)
        {
            for (int jointIndex = 0; jointIndex < angleSequences.Length; jointIndex++)
            {
                // Check if the current step exists for the current joint
                if (step < angleSequences[jointIndex].Length)
                {
                    float targetAngle = angleSequences[jointIndex][step];
                    StartCoroutine(MoveJointToAngle(jointIndex + 1, targetAngle, speed));
                }
            }

            // Wait for all joints to finish moving to this step
            yield return new WaitForSeconds(1.0f / speed); // Adjust duration as needed
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            StartCoroutine(MoveJointToAngle(1, q1, joint_speed));
            StartCoroutine(MoveJointToAngle(2, q2, joint_speed));
            StartCoroutine(MoveJointToAngle(3, q3, joint_speed));
        }

        if (Input.GetKeyDown("t")) // Example key to test the sequence
        {
            float[][] angleSequences = new float[][]
            {
                new float[] { 0.00000000e+00f,  3.20465081e-04f,  1.26013388e-03f,  2.78641672e-03f,
   4.86672395e-03f,  7.46846588e-03f,  1.05590529e-02f,  1.41058952e-02f,
   1.80764032e-02f,  2.24379873e-02f,  2.71580577e-02f,  3.22040249e-02f,
   3.75432990e-02f,  4.31432905e-02f,  4.89714097e-02f,  5.49950669e-02f,
   6.11816725e-02f,  6.74986367e-02f,  7.39133700e-02f,  8.03932825e-02f,
   8.69057848e-02f,  9.34182870e-02f,  9.98981996e-02f,  1.06312933e-01f,
   1.12629897e-01f,  1.18816503e-01f,  1.24840160e-01f,  1.30668279e-01f,
   1.36268271e-01f,  1.41607545e-01f,  1.46653512e-01f,  1.51373582e-01f,
   1.55735166e-01f,  1.59705674e-01f,  1.63252517e-01f,  1.66343104e-01f,
   1.68944846e-01f,  1.71025153e-01f,  1.72551436e-01f,  1.73491104e-01f,
   1.73811570e-01f}, // Angles for q1
                new float[] {
    0.00000000e+00f, 9.98566289e-02f, 3.92656575e-01f, 8.68244926e-01f,
    1.51646677e+00f, 2.32716720e+00f, 3.29019130e+00f, 4.39538416e+00f,
    5.63259086e+00f, 6.99165651e+00f, 8.46242618e+00f, 1.00347450e+01f,
    1.16984579e+01f, 1.34434102e+01f, 1.52594469e+01f, 1.71364130e+01f,
    1.90641537e+01f, 2.10325140e+01f, 2.30313391e+01f, 2.50504740e+01f,
    2.70797638e+01f, 2.91090536e+01f, 3.11281884e+01f, 3.31270135e+01f,
    3.50953738e+01f, 3.70231145e+01f, 3.89000807e+01f, 4.07161173e+01f,
    4.24610696e+01f, 4.41247826e+01f, 4.56971014e+01f, 4.71678710e+01f,
    4.85269367e+01f, 4.97641434e+01f, 5.08693362e+01f, 5.18323603e+01f,
    5.26430608e+01f, 5.32912826e+01f, 5.37668710e+01f, 5.40596709e+01f,
    5.41595275e+01f
}, // Angles for q2
                new float[] {    0.00000000e+00f, -6.94355236e-03f, -2.73034601e-02f, -6.03735994e-02f,
    -1.05447846e-01f, -1.61820076e-01f, -2.28784166e-01f, -3.05633991e-01f,
    -3.91663428e-01f, -4.86166353e-01f, -5.88436641e-01f, -6.97768169e-01f,
    -8.13454812e-01f, -9.34790448e-01f, -1.06106895e+00f, -1.19158420e+00f,
    -1.32563006e+00f, -1.46250043e+00f, -1.60148916e+00f, -1.74189014e+00f,
    -1.88299725e+00f, -2.02410436e+00f, -2.16450534e+00f, -2.30349407e+00f,
    -2.44036444e+00f, -2.57441030e+00f, -2.70492555e+00f, -2.83120405e+00f,
    -2.95253969e+00f, -3.06822633e+00f, -3.17755786e+00f, -3.27982815e+00f,
    -3.37433107e+00f, -3.46036051e+00f, -3.53721034e+00f, -3.60417443e+00f,
    -3.66054666e+00f, -3.70562090e+00f, -3.73869104e+00f, -3.75905095e+00f,
    -3.76599450e+00f} // Angles for q3
            };

            StartCoroutine(MoveThroughJointAngleSequences(angleSequences, joint_speed));
        }
    }
}
