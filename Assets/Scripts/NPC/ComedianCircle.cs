using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComedianCircle : MonoBehaviour
{

    private List<AudienceMember> audienceMembers;
    private Comedian _comedian;

    private Vector3 _comedianFocus;
    private Vector3 _audienceFocus;
    
    // Start is called before the first frame update
    void Start()
    {
        audienceMembers = GetComponentsInChildren<AudienceMember>().ToList();
        _comedian = GetComponentInChildren<Comedian>();

        var audienceCentrePosition = Vector3.zero;
        
        foreach (var member in audienceMembers)
        {
            var memBillboard = member.GetComponentInChildren<BillboardSprite>();
            memBillboard.SetFocusAngle(_comedian.transform.position);
            audienceCentrePosition += member.transform.position;
        }
        
        audienceCentrePosition /= 4.0f;
        var comBillboard = _comedian.GetComponentInChildren<BillboardSprite>();
        comBillboard.SetFocusAngle(audienceCentrePosition);
    }
    
}
