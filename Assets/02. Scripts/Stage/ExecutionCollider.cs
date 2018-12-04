using UnityEngine;

public class ExecutionCollider : MonoBehaviour {

    private DayNightController dnc;
    public GameObject colliders;
	void Start () {
        dnc = FindObjectOfType<DayNightController>();
	}
    private void Update()
    {
        if (dnc.currentTimeOfDay > 2f / 3f && dnc.currentTimeOfDay < 0.75f)
            colliders.SetActive(true);
        else
            colliders.SetActive(false);
    }
}