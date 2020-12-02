using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.Interaction.Toolkit
{

    public class TeleportationProvider : LocomotionProvider
    {

        // the current teleportation request
        TeleportRequest m_CurrentRequest;
        // whether the current teleportation request is valid.
        bool m_ValidRequest = false;


        public int type = 0;
        bool done = false;

        /// <summary>
        /// This function will queue a teleportation request within the provider. 
        /// </summary>
        /// <param name="teleportRequest">The teleportation request</param>
        /// <returns>true if successful.</returns>
        public bool QueueTeleportRequest(TeleportRequest teleportRequest)
        {
            m_CurrentRequest = teleportRequest;
            m_ValidRequest = true;
            return true;
        }

        public void changeType()
        {
            if (type == 1)
            {
                type = 0;
            }
            else
            {
                type = 1;
            }
        }

        public void setTeleportaion()
        {
            type = 0;
        }

        public void setDash()
        {
            type = 1;
        }

        /// <summary>
        /// Update function for the Teleportation Provider
        /// </summary>
        private void Update()
        {
            if(m_ValidRequest && BeginLocomotion())
            {
                var xrRig = system.xrRig;
                if (xrRig != null)
                {
                    switch (m_CurrentRequest.matchOrientation)
                    {
                        case MatchOrientation.None:
                            xrRig.MatchRigUp(m_CurrentRequest.destinationUpVector);
                            break;
                        case MatchOrientation.Camera:
                            xrRig.MatchRigUpCameraForward(m_CurrentRequest.destinationUpVector, m_CurrentRequest.destinationForwardVector);
                            break;
                        //case MatchOrientation.Rig:
                        //    xrRig.MatchRigUpRigForward(m_CurrentRequest.destinationUpVector, m_CurrentRequest.destinationForwardVector);
                        //    break;
                    }

                    Vector3 heightAdjustment = xrRig.rig.transform.up * xrRig.cameraInRigSpaceHeight;

                    Vector3 cameraDestination = m_CurrentRequest.destinationPosition + heightAdjustment;
                    done = false;
                    if (type == 1 && transform.position != cameraDestination)
                    {
                        done = xrRig.GlideCameraToWorldLocation(cameraDestination);
                    }
                    else if(type == 0)
                    {
                        xrRig.MoveCameraToWorldLocation(cameraDestination);
                    }
                    
                }
                EndLocomotion();
                if(type == 1 && done)
                {
                    m_ValidRequest = false;
                }else if(type == 0)
                {
                    m_ValidRequest = false;
                }
               
            }          
        }
    }
}
 
 