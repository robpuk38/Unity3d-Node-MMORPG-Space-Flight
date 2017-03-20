using UnityEngine;
using System.Collections;

public class GeroBeam : MonoBehaviour {
	
	private ShotParticleEmitter SHP_Emitter;

	private float NowLength;
	public float MaxLength = 16.0f;
	public float AddLength = 0.1f;
	public float Width = 10.0f;
	private LineRenderer LR;
	private Vector3[] F_Vec;
	private int LRSize;

	private float RateA;

	public float NowLengthGlobal;
	private BeamParam BP;
 
    private GameObject Flash;
    private float FlashSize;
    // Use this for initialization
    void Start () {
		BP = GetComponent<BeamParam>();
		LRSize = 16;
		NowLength = 0.0f;
		LR = this.GetComponent<LineRenderer>();
	
        SHP_Emitter = this.transform.FindChild("ShotParticle_Emitter").GetComponent<ShotParticleEmitter>();
        Flash = this.transform.FindChild("BeamFlash").gameObject;
        F_Vec = new Vector3[LRSize+1];
        FlashSize = Flash.transform.localScale.x;
        for (int i=0;i < LRSize+1;i++)
		{
			F_Vec[i] = transform.forward;
		}
	}
	
	
	void Update () {
       
		SHP_Emitter.ShotPower = 1.0f;
		

		NowLength = Mathf.Min(1.0f,NowLength+AddLength);
		
		Vector3 NowPos = Vector3.zero;
       
		LR.startWidth = Width*BP.Scale;
        LR.startColor= BP.BeamColor;
        MaxLength = BP.MaxLength;
        for (int i=LRSize-1;i > 0;i--)
		{
			F_Vec[i] = F_Vec[i-1];
		}
		F_Vec[0] = transform.forward;
		F_Vec[LRSize] = F_Vec[LRSize-1];
		float BlockLen = MaxLength/LRSize;

		for(int i=0;i < LRSize;i++)
		{
			NowPos = transform.position;
			for(int j=0;j<i;j++)
			{
				NowPos+=F_Vec[j]*BlockLen;
			}
			LR.SetPosition(i,NowPos);
		}

		
        float ShotFlashScale = FlashSize * Width * 5.0f;
        Flash.GetComponent<ScaleWiggle>().DefScale = new Vector3(ShotFlashScale, ShotFlashScale, ShotFlashScale);
     

		this.gameObject.GetComponent<Renderer>().material.SetFloat("_AddTex",Time.frameCount*-0.05f*BP.AnimationSpd*10);
		this.gameObject.GetComponent<Renderer>().material.SetFloat("_BeamLength",NowLength);
        Flash.GetComponent<Renderer>().material.SetColor("_Color", BP.BeamColor*2);
        SHP_Emitter.col = BP.BeamColor*2;
    
    }
}
