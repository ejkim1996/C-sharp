using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Helper script containing functions that zooms the camera to the appropriate
 * neural pathways. Call using ZoomCoyote(), ZoomHuman(), ZoomDolphin, and ZoomOut().
 * Fades out the head mesh and dims the brain mesh of selected model when zoomed in.
 * Alpha restored to default when zoomed to other models or zoomed out.
 * Automatically zooms out after set time.
 */

public class ZoomHelperScript : MonoBehaviour {
    //drag and drop GameObjects containing appropriate mesh renderers
    [SerializeField] GameObject CoyoteMesh;
    [SerializeField] GameObject CoyoteBrainMesh;
    [SerializeField] GameObject HumanMesh;
    [SerializeField] GameObject HumanBrainMesh;
    [SerializeField] GameObject DolphinMesh;
    [SerializeField] GameObject DolphinBrainMesh;

    //drag and drop main camera
    [SerializeField] Animator cameraAnimator;

    //MeshRenderer arrays to hold mesh renderers from above game objects
    MeshRenderer[] CoyoteRenderer;
    MeshRenderer[] CoyoteBrainRenderer;
    MeshRenderer[] HumanRenderer;
    MeshRenderer[] HumanBrainRenderer;
    MeshRenderer[] DolphinRenderer;
    MeshRenderer[] DolphinBrainRenderer;

    //Color objects to change alpha when zooming in or out
    Color dim, fade, restore;

    //string to check where to zoom out from
    string zoomActive = "";

    //variables for auto zoom out, time in seconds
    public float timeTillZoomOut = 10.0f;
    float zoomOutTimer;
    bool zoomedIn = false;

    //floats to hold r, g, b, and alpha values of mesh renderers
    float r, g, b, a;

    // Use this for initialization
    void Start () {
        //fill MeshRenderer[] arrays with renderers from appropriate GameObjects
        CoyoteRenderer = CoyoteMesh.GetComponentsInChildren<MeshRenderer>();
        CoyoteBrainRenderer = CoyoteBrainMesh.GetComponentsInChildren<MeshRenderer>();
        HumanRenderer = HumanMesh.GetComponentsInChildren<MeshRenderer>();
        HumanBrainRenderer = HumanBrainMesh.GetComponentsInChildren<MeshRenderer>();
        DolphinRenderer = DolphinMesh.GetComponentsInChildren<MeshRenderer>();
        DolphinBrainRenderer = DolphinBrainMesh.GetComponentsInChildren<MeshRenderer>();

        //save default r, g, b, alpha values.
        //will require more variables if default values are not identical across meshes
        r = CoyoteRenderer[0].material.color.r;
        g = CoyoteRenderer[0].material.color.g;
        b = CoyoteRenderer[0].material.color.b;
        a = CoyoteRenderer[0].material.color.a;

        //set dim, fade, and restore colors.
        dim = new Color(r, g, b, 0.005f);
        fade = new Color(r, g, b, 0);
        restore = new Color(r, g, b, a);
    }
	
	// Update is called once per frame
	void Update () {
        //keys used for testing
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

        //auto zoom out function
        if (zoomedIn == true)
        {
            zoomOutTimer += Time.deltaTime;
            if (zoomOutTimer > timeTillZoomOut)
            {
                ZoomOut();
                zoomedIn = false;
            }
        }
    }

    //public zoom out functions
    public void ZoomCoyote()
    {
        zoomedIn = true;
        zoomOutTimer = 0.0f;

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

    public void ZoomHuman()
    {
        zoomedIn = true;
        zoomOutTimer = 0.0f;

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

    public void ZoomDolphin()
    {
        zoomedIn = true;
        zoomOutTimer = 0.0f;

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

    public void ZoomOut()
    {
        zoomedIn = false;

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
            zoomActive = "";
        }
        else if (zoomActive.Equals("human"))
        {
            cameraAnimator.SetTrigger("zoom out from human");
            zoomActive = "";
        }
        else if (zoomActive.Equals("dolphin"))
        {
            cameraAnimator.SetTrigger("zoom out from dolphin");
            zoomActive = "";
        }

        //change animator state to default state so that zoom in functions work properly.
        cameraAnimator.SetTrigger("zoom out");
    }

    //functions that loop through the appropriate MeshRenderer[] arrays to change the alpha
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
