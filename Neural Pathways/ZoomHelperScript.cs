using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomHelperScript : MonoBehaviour {
    [SerializeField] GameObject CoyoteMesh;
    [SerializeField] GameObject CoyoteBrainMesh;
    [SerializeField] GameObject HumanMesh;
    [SerializeField] GameObject HumanBrainMesh;
    [SerializeField] GameObject DolphinMesh;
    [SerializeField] GameObject DolphinBrainMesh;
    [SerializeField] Animator cameraAnimator;

    MeshRenderer[] CoyoteRenderer;
    MeshRenderer[] CoyoteBrainRenderer;
    MeshRenderer[] HumanRenderer;
    MeshRenderer[] HumanBrainRenderer;
    MeshRenderer[] DolphinRenderer;
    MeshRenderer[] DolphinBrainRenderer;

    Color dim, fade, restore;

    string zoomActive = "";

    float r, g, b, a;

    // Use this for initialization
    void Start () {
        CoyoteRenderer = CoyoteMesh.GetComponentsInChildren<MeshRenderer>();
        CoyoteBrainRenderer = CoyoteBrainMesh.GetComponentsInChildren<MeshRenderer>();
        HumanRenderer = HumanMesh.GetComponentsInChildren<MeshRenderer>();
        HumanBrainRenderer = HumanBrainMesh.GetComponentsInChildren<MeshRenderer>();
        DolphinRenderer = DolphinMesh.GetComponentsInChildren<MeshRenderer>();
        DolphinBrainRenderer = DolphinBrainMesh.GetComponentsInChildren<MeshRenderer>();

        r = CoyoteRenderer[0].material.color.r;
        g = CoyoteRenderer[0].material.color.g;
        b = CoyoteRenderer[0].material.color.b;
        a = CoyoteRenderer[0].material.color.a;

        dim = new Color(r, g, b, 0.005f);
        fade = new Color(r, g, b, 0);
        restore = new Color(r, g, b, a);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("a"))
        {
            ZoomCoyote();
        }

        if (Input.GetKey("s"))
        {
            ZoomHuman();
        }

        if (Input.GetKey("d"))
        {
            ZoomDolphin();
        }

        if (Input.GetKey("f"))
        {
            ZoomOut();
        }

    }

    void ZoomCoyote()
    {
        cameraAnimator.ResetTrigger("human");
        cameraAnimator.ResetTrigger("dolphin");
        cameraAnimator.SetTrigger("coyote");

        RestoreHuman();
        RestoreHumanBrain();
        RestoreDolphin();
        RestoreDolphinBrain();
        FadeCoyote();
        DimCoyoteBrain();

        zoomActive = "coyote";
    }

    void ZoomHuman()
    {
        cameraAnimator.ResetTrigger("coyote");
        cameraAnimator.ResetTrigger("dolphin");
        cameraAnimator.SetTrigger("human");

        RestoreCoyote();
        RestoreCoyoteBrain();
        RestoreDolphin();
        RestoreDolphinBrain();
        FadeHuman();
        DimHumanBrain();

        zoomActive = "human";
    }

    void ZoomDolphin()
    {
        cameraAnimator.ResetTrigger("human");
        cameraAnimator.ResetTrigger("coyote");
        cameraAnimator.SetTrigger("dolphin");

        RestoreHuman();
        RestoreHumanBrain();
        RestoreCoyote();
        RestoreCoyoteBrain();
        FadeDolphin();
        DimDolphinBrain();

        zoomActive = "dolphin";
    }

    void ZoomOut()
    {
        cameraAnimator.ResetTrigger("human");
        cameraAnimator.ResetTrigger("coyote");
        cameraAnimator.ResetTrigger("dolphin");
        RestoreCoyote();
        RestoreCoyoteBrain();
        RestoreHuman();
        RestoreHumanBrain();
        RestoreDolphin();
        RestoreDolphinBrain();
        
        if (zoomActive.Equals("coyote"))
        {
            cameraAnimator.SetTrigger("zoom out from coyote");
            cameraAnimator.SetTrigger("zoom out");
            zoomActive = "";
        }
        else if (zoomActive.Equals("human"))
        {
            cameraAnimator.SetTrigger("zoom out from human");
            cameraAnimator.SetTrigger("zoom out");
            zoomActive = "";
        }
        else if (zoomActive.Equals("dolphin"))
        {
            cameraAnimator.SetTrigger("zoom out from dolphin");
            cameraAnimator.SetTrigger("zoom out");
            zoomActive = "";
        }
    }

    void FadeCoyote()
    {
        for (int i = 0; i < CoyoteRenderer.Length; i++)
        {
            CoyoteRenderer[i].material.color = fade;
        }
    }

    void FadeHuman()
    {
        for (int i = 0; i < HumanRenderer.Length; i++)
        {
            HumanRenderer[i].material.color = fade;
        }
    }

    void FadeDolphin()
    {
        for (int i = 0; i < DolphinRenderer.Length; i++)
        {
            DolphinRenderer[i].material.color = fade;
        }
    }

    void DimCoyoteBrain()
    {
        for (int i = 0; i < CoyoteBrainRenderer.Length; i++)
        {
            CoyoteBrainRenderer[i].material.color = dim;
        }
    }

    void DimHumanBrain()
    {
        for (int i = 0; i < HumanBrainRenderer.Length; i++)
        {
            HumanBrainRenderer[i].material.color = dim;
        }
    }

    void DimDolphinBrain()
    {
        for (int i = 0; i < DolphinBrainRenderer.Length; i++)
        {
            DolphinBrainRenderer[i].material.color = dim;
        }
    }

    void RestoreCoyote()
    {
        for (int i = 0; i < CoyoteRenderer.Length; i++)
        {
            CoyoteRenderer[i].material.color = restore;
        }
    }

    void RestoreHuman()
    {
        for (int i = 0; i < HumanRenderer.Length; i++)
        {
            HumanRenderer[i].material.color = restore;
        }
    }

    void RestoreDolphin()
    {
        for (int i = 0; i < DolphinRenderer.Length; i++)
        {
            DolphinRenderer[i].material.color = restore;
        }
    }

    void RestoreCoyoteBrain()
    {
        for (int i = 0; i < CoyoteBrainRenderer.Length; i++)
        {
            CoyoteBrainRenderer[i].material.color = restore;
        }
    }

    void RestoreHumanBrain()
    {
        for (int i = 0; i < HumanBrainRenderer.Length; i++)
        {
            HumanBrainRenderer[i].material.color = restore;
        }
    }

    void RestoreDolphinBrain()
    {
        for (int i = 0; i < DolphinBrainRenderer.Length; i++)
        {
            DolphinBrainRenderer[i].material.color = restore;
        }
    }
}
