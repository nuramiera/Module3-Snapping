using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

[PunRPC]
public class Hand : MonoBehaviourPunCallbacks
{
    private Socket socket = null;
    private SteamVR_Behaviour_Pose pose = null;

    public SteamVR_Action_Boolean m_GrabAction = null;
    private FixedJoint m_Joint = null;

    private Interactable m_CurrentInteractable = null;
    public List<Interactable> contactInteractables = new List<Interactable>();

    private void Awake()
    {
        if (photonView.IsMine)
        {
            socket = GetComponent<Socket>();
            pose = GetComponent<SteamVR_Behaviour_Pose>();
            m_Joint = GetComponent<FixedJoint>();
        }
    }

    public void Pickup()
    {
        //Get nearest
        m_CurrentInteractable = GetNearestInteractable();

        //Null Check
        if (!m_CurrentInteractable)
            return;

        // Already held,check
        if (m_CurrentInteractable.m_ActiveHand)
            m_CurrentInteractable.m_ActiveHand.Drop();

        //Position
        m_CurrentInteractable.transform.position = transform.position;

        //Attach
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;

        //Set active hand
        m_CurrentInteractable.m_ActiveHand = this;

    }

    
    public void Drop()
    {
        //Null check
        if (!m_CurrentInteractable)
            return;

        //Apply velocity
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();

        targetBody.velocity = pose.GetVelocity();
        targetBody.angularVelocity = pose.GetAngularVelocity();

        //Detach
        m_Joint.connectedBody = null;

        //clear
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Interactable interactable in contactInteractables)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }

        return nearest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            //function interact dengan interactable script
            AddInteractable(other.gameObject);

           // if (!other.gameObject.CompareTag("Interactable") && gameObject.GetComponent<Interactable>())
            //    return;
        }
    }

    private void AddInteractable(GameObject newObject)
    {
        if (photonView.IsMine)
        {
            //layer "interactable"
            Interactable newInteractable = newObject.GetComponent<Interactable>();

            //gameObject.CompareTag("Interactable");

            // add list
            contactInteractables.Add(newInteractable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine)
        {
            //function interact dgn interactable script
            RemoveInteractable(other.gameObject);
          //  if (!other.gameObject.CompareTag("Interactable") && gameObject.GetComponent<Interactable>())
             //   return;
        }
    }

    private void RemoveInteractable(GameObject newObject)
    {
        if (photonView.IsMine)
        {
            // remove cube yg tgh pegang
            Interactable exisitngInteractable = newObject.GetComponent<Interactable>();
            //buang dalam list contact interactable
            contactInteractables.Remove(exisitngInteractable);
        }
    }

    public void TryInteraction()
    {
        if (NearestInteraction())
            return;

        HeldInteraction();
    }

    private bool NearestInteraction()
    {
        Interactable nearestObject = Utility.GetNearestInteractable(transform.position, contactInteractables);

        if (nearestObject)
            nearestObject.StartInteraction(this);

        return nearestObject;
    }
    
    private void HeldInteraction()
    {
        Moveable heldObject = socket.GetStoredObject();

        if (heldObject) 
        heldObject.Interaction(this);
    }
    
    public void StopInteraction()
    {
        Pickup();
        //  Moveable heldObject = socket.GetStoredObject();
        //if (heldObject)
        //  heldObject.EndInteraction(this);
    }

    public Socket GetSocket()
    {
        return socket;
    }

    public SteamVR_Behaviour_Pose GetPose()
    {
        return pose;
    }

   
}
