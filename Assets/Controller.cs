using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

	public GameObject SwingPrefab;
	public Button btn;
	public InputField S1P, S2P, S1G, S2G, Num;
	public Toggle S1B, S1D, S1H, S2B, S2D, S2H;
	public Text FrameRate, S1M, S2M;
	GameObject swing1, swing2;
	Swing S1, S2;

	void Awake()
	{
		Application.targetFrameRate = 60;
	}

	private float accum = 0; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft = 0.5f; // Left time for current interval

	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		// Interval ended - update GUI text and start new interval
		if (timeleft <= 0.0)
		{
			// display two fractional digits (f2 format)
			float fps = accum / frames;
			FrameRate.text = string.Format("{0:F2}", fps);
			
			timeleft = 0.5f;
			accum = 0.0F;
			frames = 0;
		}

		if (S1 == null) S1M.text = "최대높이 : ";
		else S1M.text = "최대높이 : " + S1.getMaxHeight();

		if (S2 == null) S2M.text = "최대높이 : ";
		else S2M.text = "최대높이 : " + S2.getMaxHeight();

	}

	public void ButtonClicked()
	{
		StartCoroutine(controll());
	}

	IEnumerator controll()
	{
		int t;
		btn.enabled = false;

		if (swing1 != null)
		{
			Destroy(swing1);
			swing1 = null;
			S1 = null;
		}
		if (swing2 != null)
		{
			Destroy(swing2);
			swing2 = null;
			S2 = null;
		}

		swing1 = Instantiate(SwingPrefab) as GameObject;
		swing1.name = "swing1";
		swing1.transform.localPosition = new Vector3(-10, 0, 5);
		S1 = (swing1.GetComponent("Swing") as Swing);
        float p1 = System.Convert.ToSingle(S1P.text);
		if (S1B.isOn) t = 2; else if (S1D.isOn) t = 10; else t = 16;
		uint g1 = System.Convert.ToUInt32(S1G.text, t);

		swing2 = Instantiate(SwingPrefab) as GameObject;
		swing2.name = "swing2";
		swing2.transform.localPosition = new Vector3(-6, 0, 5);
		S2 = (swing2.GetComponent("Swing") as Swing);
        float p2 = System.Convert.ToSingle(S2P.text);
		if (S2B.isOn) t = 2; else if (S2D.isOn) t = 10; else t = 16;
		uint g2 = System.Convert.ToUInt32(S2G.text, t);

		yield return new WaitForSeconds(1.5f);

		int cnt = int.Parse(Num.text);
		((S1.Human.GetComponent("HumanSwing")) as HumanSwing).StartPlayGPC(g1, p1, cnt);
		((S2.Human.GetComponent("HumanSwing")) as HumanSwing).StartPlayGPC(g2, p2, cnt);
		yield return new WaitForSeconds(2.0f + (p1>p2?p1:p2) * cnt);
		btn.enabled = true;
	}
}
