using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.UrdfImporter.Control;
using UnityEngine;
using UnityEngine.Serialization;

// [System.Serializable]
// public class AngleSequence
// {
//     public float[] angles;
// }

public class movejoint : MonoBehaviour
{
    [SerializeField] float q1 = 0;
    [SerializeField] float q2 = 0;
    [SerializeField] float q3 = 0;
    [SerializeField] float joint_speed = 10;
    [SerializeField] private float time = 1;

    // [SerializeField] public List<AngleSequence> AngleSequences;

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

        var joint = articulationChain[jointIndex];
        var drive = joint.xDrive;

        var currentAngle = drive.target; // Start from the current target angle
        var step = speed * Time.deltaTime; // Increment based on speed and delta time

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

    public IEnumerator MoveJointToAngleInTime(int jointIndex, float targetAngle, float totalTime)
    {
        if (jointIndex < 0 || jointIndex >= articulationChain.Length)
        {
            yield break;
        }

        var joint = articulationChain[jointIndex];
        var drive = joint.xDrive;

        var currentAngle = drive.target; // Start from the current target angle
        var angleDifference = Mathf.Abs(targetAngle - currentAngle);
        var speed = angleDifference / totalTime; // Calculate the speed required to reach the target in the given time

        var elapsedTime = 0f;


        while (elapsedTime < totalTime)
        {
            currentAngle =
                Mathf.MoveTowards(currentAngle, targetAngle, speed * Time.deltaTime); // Gradually move to the target
            drive.target = currentAngle;
            joint.xDrive = drive;

            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        drive.target = targetAngle; // Ensure the final target is set precisely
        joint.xDrive = drive;

    }

    // public IEnumerator MoveThroughJointAngleSequences(float[][] angleSequences, float speed)
    // {
    //     // Validate the input
    //     if (angleSequences.Length != 3)
    //     {
    //         Debug.LogError("The angleSequences array must contain exactly 3 subarrays, one for each joint.");
    //         yield break;
    //     }
    //
    //     var maxSteps = Mathf.Max(angleSequences[0].Length, angleSequences[1].Length, angleSequences[2].Length);
    //
    //     for (var step = 0; step < maxSteps; step++)
    //     {
    //         for (var jointIndex = 0; jointIndex < angleSequences.Length; jointIndex++)
    //         {
    //             // Check if the current step exists for the current joint
    //             if (step >= angleSequences[jointIndex].Length) continue;
    //             var targetAngle = angleSequences[jointIndex][step];
    //             StartCoroutine(MoveJointToAngle(jointIndex + 1, targetAngle, speed));
    //         }
    //
    //         // Wait for all joints to finish moving to this step
    //         yield return new WaitForSeconds(1.0f / speed); // Adjust duration as needed
    //     }
    // }

    public IEnumerator MoveThroughJointAngleSequences(float[][] angleSequences, float totalTime)
    {
        // Validate the input
        if (angleSequences.Length != 3)
        {
            Debug.LogError("The angleSequences array must contain exactly 3 subarrays, one for each joint.");
            yield break;
        }

        var maxSteps = Mathf.Max(angleSequences[0].Length, angleSequences[1].Length, angleSequences[2].Length);

        var timePerStep = totalTime / maxSteps;
        for (var step = 0; step < maxSteps; step++)
        {
            for (var jointIndex = 0; jointIndex < angleSequences.Length; jointIndex++)
            {
                // Check if the current step exists for the current joint
                if (step >= angleSequences[jointIndex].Length) continue;
                var targetAngle = angleSequences[jointIndex][step];
                StartCoroutine(MoveJointToAngleInTime(jointIndex + 1, targetAngle, timePerStep));
            }
            // Wait for all joints to finish moving to this step
            yield return new WaitForSeconds(timePerStep); // Adjust duration as needed
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(MoveJointToAngle(1, q1, joint_speed));
            StartCoroutine(MoveJointToAngle(2, q2, joint_speed));
            StartCoroutine(MoveJointToAngle(3, q3, joint_speed));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(MoveJointToAngleInTime(1, q1, time));
            StartCoroutine(MoveJointToAngleInTime(2, q2, time));
            StartCoroutine(MoveJointToAngleInTime(3, q3, time));
        }

        if (Input.GetKeyDown(KeyCode.T)) // Example key to test the sequence
        {
            float[][] angleSequences = new float[][]
            {
                new float[]
                {
                    0.0f, 0.16629248f, 0.65389587f, 1.44589906f, 2.52539095f, 3.87546045f,
                    5.47919644f, 7.31968784f, 9.38002354f, 11.64329244f, 14.09258345f,
                    16.71098545f, 19.48158736f, 22.38747806f, 25.41174647f, 28.53748148f,
                    31.74777199f, 35.0257069f, 38.35437511f, 41.71686552f, 45.09626703f,
                    48.47566854f, 51.83815895f, 55.16682716f, 58.44476207f, 61.65505257f,
                    64.78078758f, 67.80505599f, 70.7109467f, 73.4815486f, 76.09995061f,
                    78.54924161f, 80.81251051f, 82.87284621f, 84.71333761f, 86.3170736f,
                    87.6671431f, 88.74663499f, 89.53863818f, 90.02624157f, 90.19253405f
                }, // Angles for q1

                new float[]
                {
                    0.0f, 0.16634501f, 0.65410241f, 1.44635577f, 2.52618863f, 3.87668456f,
                    5.48092712f, 7.32199987f, 9.38298635f, 11.64697014f, 14.09703478f,
                    16.71626384f, 19.48774088f, 22.39454945f, 25.41977312f, 28.54649543f,
                    31.75779995f, 35.03677024f, 38.36648986f, 41.73004236f, 45.1105113f,
                    48.49098024f, 51.85453274f, 55.18425235f, 58.46322264f, 61.67452717f,
                    64.80124948f, 67.82647314f, 70.73328172f, 73.50475875f, 76.12398782f,
                    78.57405246f, 80.83803625f, 82.89902273f, 84.74009547f, 86.34433803f,
                    87.69483396f, 88.77466683f, 89.56692018f, 90.05467759f, 90.2210226f
                },
                new float[]
                {
                    0.0f, 0.16608764f, 0.65309039f, 1.44411798f, 2.52228013f, 3.87068659f,
                    5.47244707f, 7.31067132f, 9.36846906f, 11.62895002f, 14.07522395f,
                    16.69040055f, 19.45758958f, 22.35990076f, 25.38044382f, 28.50232849f,
                    31.7086645f, 34.98256159f, 38.30712949f, 41.66547792f, 45.04071663f,
                    48.41595533f, 51.77430376f, 55.09887166f, 58.37276875f, 61.57910476f,
                    64.70098943f, 67.72153249f, 70.62384367f, 73.3910327f, 76.00620931f,
                    78.45248323f, 80.71296419f, 82.77076193f, 84.60898618f, 86.21074667f,
                    87.55915312f, 88.63731527f, 89.42834286f, 89.91534561f, 90.08143325f
                }
            };

            StartCoroutine(MoveThroughJointAngleSequences(angleSequences, 4));
        }

        if (Input.GetKeyDown(KeyCode.Y)) // Example key to test the sequence
        {
            float[][] angleSequences = new float[][]
            {
                new float[]
                {
                    90.19253405f, 90.02631506f, 89.55858256f, 88.83572324f, 87.90412379f,
                    86.81017091f, 85.60025129f, 84.32075163f, 83.01805862f, 81.73855897f,
                    80.52863935f, 79.43468647f, 78.50308702f, 77.7802277f, 77.31249519f,
                    77.1462762f
                }, // Angles for q1

                new float[]
                {
                    90.2210226f, 89.81463636f, 88.67108439f, 86.90377679f, 84.62612369f,
                    81.95153519f, 78.99342141f, 75.86519246f, 72.68025846f, 69.55202951f,
                    66.59391573f, 63.91932723f, 61.64167413f, 59.87436653f, 58.73081456f,
                    58.32442832f
                },
                new float[]
                {
                    90.08143325f, 90.08292943f, 90.08713959f, 90.09364621f, 90.10203175f,
                    90.11187867f, 90.12276943f, 90.1342865f, 90.14601233f, 90.1575294f,
                    90.16842016f, 90.17826708f, 90.18665262f, 90.19315924f, 90.1973694f,
                    90.19886558f
                }
            };

            StartCoroutine(MoveThroughJointAngleSequences(angleSequences, 1.5f));
        }

        if (Input.GetKeyDown(KeyCode.U)) // Example key to test the sequence
        {
            float[][] angleSequences = new float[][]
            {
                new float[]
                {
                    77.1462762f, 77.31249519f, 77.7802277f, 78.50308702f, 79.43468647f,
                    80.52863935f, 81.73855897f, 83.01805862f, 84.32075163f, 85.60025129f,
                    86.81017091f, 87.90412379f, 88.83572324f, 89.55858256f, 90.02631506f,
                    90.19253405f
                }, // Angles for q1

                new float[]
                {
                    58.32442832f, 58.73081456f, 59.87436653f, 61.64167413f, 63.91932723f,
                    66.59391573f, 69.55202951f, 72.68025846f, 75.86519246f, 78.99342141f,
                    81.95153519f, 84.62612369f, 86.90377679f, 88.67108439f, 89.81463636f,
                    90.2210226f
                },
                new float[]
                {
                    90.19886558f, 90.1973694f, 90.19315924f, 90.18665262f, 90.17826708f,
                    90.16842016f, 90.1575294f, 90.14601233f, 90.1342865f, 90.12276943f,
                    90.11187867f, 90.10203175f, 90.09364621f, 90.08713959f, 90.08292943f,
                    90.08143325f
                }
            };

            StartCoroutine(MoveThroughJointAngleSequences(angleSequences, 1.5f));
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            float[][] angleSequences = new float[][]
            {
                new float[]
                {
                    90.19253405f, 90.04413902f, 89.60562335f, 88.88699122f, 87.89824678f,
                    86.64939419f, 85.15043761f, 83.41138119f, 81.44222911f, 79.25298551f,
                    76.85365457f, 74.25424043f, 71.46474726f, 68.49517921f, 65.35554045f,
                    62.05583514f, 58.60606743f, 55.01624149f, 51.29636148f, 47.45643154f,
                    43.50645586f, 39.45643857f, 35.31638385f, 31.09629586f, 26.80617874f,
                    22.45603667f, 18.0558738f, 13.61569429f, 9.1455023f, 4.655302f,
                    0.15509753f, -4.34510693f, -8.83530724f, -13.30549923f, -17.74567874f,
                    -22.14584161f, -26.49598368f, -30.78610079f, -35.00618879f, -39.14624351f,
                    -43.19626079f, -47.14623648f, -50.98616641f, -54.70604643f, -58.29587237f,
                    -61.74564008f, -65.04534539f, -68.18498415f, -71.15455219f, -73.94404537f,
                    -76.54345951f, -78.94279045f, -81.13203405f, -83.10118613f, -84.84024254f,
                    -86.33919913f, -87.58805172f, -88.57679616f, -89.29542829f, -89.73394396f,
                    -89.88233899f
                }, // Angles for q1

                new float[]
                {
                    90.2210226f, 90.18402479f, 90.07469419f, 89.89552503f, 89.64901155f,
                    89.33764798f, 88.96392856f, 88.53034751f, 88.03939908f, 87.4935775f,
                    86.89537699f, 86.24729181f, 85.55181617f, 84.81144432f, 84.02867048f,
                    83.2059889f, 82.34589381f, 81.45087943f, 80.52344002f, 79.56606979f,
                    78.58126298f, 77.57151384f, 76.53931658f, 75.48716546f, 74.41755469f,
                    73.33297852f, 72.23593117f, 71.12890689f, 70.01439991f, 68.89490446f,
                    67.77291477f, 66.65092509f, 65.53142964f, 64.41692265f, 63.30989837f,
                    62.21285103f, 61.12827486f, 60.05866409f, 59.00651296f, 57.97431571f,
                    56.96456656f, 55.97975976f, 55.02238953f, 54.09495011f, 53.19993574f,
                    52.33984065f, 51.51715906f, 50.73438523f, 49.99401338f, 49.29853774f,
                    48.65045255f, 48.05225205f, 47.50643047f, 47.01548203f, 46.58190099f,
                    46.20818156f, 45.89681799f, 45.65030451f, 45.47113536f, 45.36180476f,
                    45.32480695f
                },
                new float[]
                {
                    90.08143325f, 90.04462365f, 89.93584919f, 89.75759143f, 89.51233192f,
                    89.2025522f, 88.83073382f, 88.39935832f, 87.91090725f, 87.36786216f,
                    86.77270459f, 86.12791609f, 85.43597821f, 84.69937249f, 83.92058048f,
                    83.10208373f, 82.24636378f, 81.35590217f, 80.43318047f, 79.4806802f,
                    78.50088293f, 77.49627019f, 76.46932353f, 75.4225245f, 74.35835464f,
                    73.27929551f, 72.18782864f, 71.08643558f, 69.97759789f, 68.8637971f,
                    67.74751477f, 66.63123244f, 65.51743165f, 64.40859396f, 63.3072009f,
                    62.21573404f, 61.1366749f, 60.07250504f, 59.02570601f, 57.99875935f,
                    56.99414661f, 56.01434934f, 55.06184907f, 54.13912737f, 53.24866577f,
                    52.39294582f, 51.57444906f, 50.79565705f, 50.05905133f, 49.36711345f,
                    48.72232495f, 48.12716739f, 47.58412229f, 47.09567123f, 46.66429573f,
                    46.29247734f, 45.98269762f, 45.73743811f, 45.55918035f, 45.4504059f,
                    45.41359629f
                }
            };

            StartCoroutine(MoveThroughJointAngleSequences(angleSequences, 6));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            float[][] angleSequences = new float[][]
            {
                new float[]
                {
                    -89.882339f, -89.23069f, -87.36563f, -84.42199f, -80.534576f,
                    -75.83822f, -70.46775f, -64.55799f, -58.243755f, -51.659874f,
                    -44.94117f, -38.22246f, -31.638582f, -25.32435f, -19.414585f,
                    -14.044115f, -9.347763f, -5.460352f, -2.516705f, -0.651647f,
                    0.0f
                }, // Angles for q1

                new float[]
                {
                    45.324806f, 44.9962f, 44.05571f, 42.571327f, 40.611027f,
                    38.242805f, 35.53465f, 32.554543f, 29.370476f, 26.050432f,
                    22.662403f, 19.274374f, 15.954332f, 12.770264f, 9.790158f,
                    7.082001f, 4.71378f, 2.753482f, 1.269094f, 0.328604f,
                    0.0f
                },
                new float[]
                {
                    45.413597f, 45.084347f, 44.142014f, 42.65472f, 40.690582f,
                    38.317722f, 35.60426f, 32.618317f, 29.42801f, 26.101465f,
                    22.706797f, 19.312132f, 15.985586f, 12.795281f, 9.809337f,
                    7.095874f, 4.723014f, 2.758876f, 1.271581f, 0.329249f,
                    0.0f
                }
            };

            StartCoroutine(MoveThroughJointAngleSequences(angleSequences, 2));
        }
    }
}