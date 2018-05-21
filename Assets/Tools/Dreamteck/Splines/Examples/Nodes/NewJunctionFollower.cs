using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dreamteck.Splines
{
    public class NewJunctionFollower : MonoBehaviour
    {
        //This is a simplified version of the JunctionFollower class which uses the new API methods
        //for finding and connectiong SplineComputers in version 1.0.8.
        //Unlike JunctionFollower, NewJunctionFollower doesn't deal with Nodes and connections. It deals directly with SplineComputers and their points

        public SplineFollower follower;
        List<SplineComputer> computers = new List<SplineComputer>();
        List<int> connectionPoints = new List<int>();
        List<int> connectedPoints = new List<int>();

        // Use this for initialization
        void Start()
        {
            if (follower == null) follower = GetComponent<SplineFollower>();
            GetAvailableJunctions();
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        /// <summary>
        /// Used to get the available junctions. Call this to update the junction list on the GUI. 
        /// </summary>
        public void GetAvailableJunctions()
        {
            //Get the last SplineComputer in the junction address
            Spline.Direction currentDirection = Spline.Direction.Forward;
            //Get the last computer and the connected points in the address of the follower 
            SplineComputer computer = follower.address.elements[follower.address.depth - 1].computer;
            if (computer == null) return;
            double checkPercent = follower.address.elements[follower.address.depth - 1].startPercent;
            if (follower.address.elements[follower.address.depth - 1].startPercent > follower.address.elements[follower.address.depth - 1].endPercent) currentDirection = Spline.Direction.Backward;
            //The new overload of GetConnectedComputers will write the connected computers and their points into three arrays based on the current percent and direction
            computer.GetConnectedComputers(computers, connectionPoints, connectedPoints, checkPercent, currentDirection, false);
            //connectionPoints are the points of currentComputer and connectedPoints are the points of each connected computer from the computers list.
        }

        void OnGUI()
        {
            if (follower == null) return;
            float maxWidth = 250f;
            //if the address depth is bigger than 1 (at least one junction is entered), offer an option to exit the last junction
            if(follower.address.depth > 1) {
                if (GUI.Button(new Rect(5, 10, 35, 20), "< X"))
                {
                    follower.ExitAddress(1);
                    GetAvailableJunctions();
                }
            }
            for (int i = 0; i < computers.Count; i++)
            {
                GUI.Label(new Rect(0, 30 + 20 * i, maxWidth * 0.75f, 20), computers[i].name + " [at P" + connectionPoints[i] + "]");
                float x = maxWidth - 70f;
                if (connectedPoints[i] > 0)
                {
                    if (GUI.Button(new Rect(x, 30 + 20 * i, 35, 20), "◄─"))
                    {
                        follower.AddComputer(computers[i], connectionPoints[i], connectedPoints[i], Spline.Direction.Backward);
                        GetAvailableJunctions();
                        break;
                    }
                    x += 35f;
                }
                if (connectedPoints[i] < computers[i].pointCount - 1)
                {
                    if (GUI.Button(new Rect(x, 30 + 20 * i, 35, 20), "─►"))
                    {
                        follower.AddComputer(computers[i], connectionPoints[i], connectedPoints[i], Spline.Direction.Forward);
                        GetAvailableJunctions();
                        break;
                    }
                }
            }
        }
    }
}
