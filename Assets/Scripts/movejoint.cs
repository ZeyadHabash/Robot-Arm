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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            StartCoroutine(MoveJointToAngle(1, q1, joint_speed));
            StartCoroutine(MoveJointToAngle(2, q2, joint_speed));
            StartCoroutine(MoveJointToAngle(3, q3, joint_speed));


        }
    }
}
