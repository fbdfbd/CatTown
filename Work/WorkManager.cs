using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager : MonoBehaviour
{
    public static WorkManager Instance { get; private set; }

    [SerializeField]
    private List<Work> woodWork;
    [SerializeField]
    private List<Work> stoneWork;

    public List<Work> WoodWork => woodWork;
    public List<Work> StoneWork => stoneWork;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject WorkMoveCat()
    {
        Work foundWork = WorkSearch();
        if (foundWork == null)
        {
            return WorkMoveCat();
        }
        else
        {
            return foundWork.gameObject;
        }
    }

    private Work WorkSearch()
    {
        List<Work> selectList;
        int attempts = 0;
        const int maxAttempts = 10;

        while (attempts < maxAttempts)
        {
            int firstRan = Random.Range(0, 2);
            selectList = (firstRan == 0) ? woodWork : stoneWork;

            if (selectList.Count == 0)
                return null;

            List<Work> possibleWorks = selectList.FindAll(work => work.WorkPossible);

            if (possibleWorks.Count > 0)
            {
                Work selectedWork = possibleWorks[Random.Range(0, possibleWorks.Count)];
                selectedWork.WorkPossibleChange();
                return selectedWork;
            }

            attempts++;
        }
        return null;
    }


    public void WorkingComplete(GameObject workObject)
    {
        Work work = workObject.GetComponent<Work>();
        StartCoroutine(OneSeconds());
        work.WorkCompleteGetProduction();
        work.gameObject.SetActive(false);
        StartCoroutine(work.Regenerative());
    } 

    public IEnumerator OneSeconds()
    {
        yield return new WaitForSeconds(1f);
    }

    public void InitializeWorks()
    {
        foreach (Work work in woodWork)
        {
            work.gameObject.SetActive(false);
        }

        foreach (Work work in stoneWork)
        {
            work.gameObject.SetActive(false);
        }

        foreach (Work work in woodWork)
        {
            work.gameObject.SetActive(true);
        }

        foreach (Work work in stoneWork)
        {
            work.gameObject.SetActive(true);
        }
    }
}
