using System.Collections;
using UnityEngine;
using TMPro;

public class AnimalCounter : MonoBehaviour
{
    public TextMeshProUGUI eggCounterText;
    public TextMeshProUGUI chickCounterText;
    public TextMeshProUGUI roosterCounterText;
    public TextMeshProUGUI henCounterText;

    public GameObject eggPrefab;
    public GameObject chickPrefab;
    public GameObject henPrefab;
    public GameObject roosterPrefab;

    private int eggCount = 0;
    private int chickCount = 0;
    private int roosterCount = 0;
    private int henCount = 0;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        UpdateCounters();
        StartCoroutine(GameLoop());
    }

    void UpdateCounters()
    {
        eggCounterText.text = eggCount.ToString();
        chickCounterText.text = chickCount.ToString();
        roosterCounterText.text = roosterCount.ToString();
        henCounterText.text = henCount.ToString();
    }

    IEnumerator GameLoop()
    {
        eggCount++;
        GameObject egg = Instantiate(eggPrefab, GetRandomPositionWithinCamera(), Quaternion.identity);
        egg.tag = "Egg";
        UpdateCounters();
        yield return new WaitForSeconds(10); 

        eggCount--;
        if (egg != null)
        {
            Destroy(egg);
        }
        chickCount++;
        GameObject chick = Instantiate(chickPrefab, egg.transform.position, Quaternion.identity);
        UpdateCounters();
        yield return new WaitForSeconds(10); 

        chickCount--;
        Destroy(chick);
        henCount++;
        GameObject hen = Instantiate(henPrefab, chick.transform.position, Quaternion.identity);
        UpdateCounters();

        yield return StartCoroutine(HenLifecycle(hen));
    }

    IEnumerator ProcessEgg(GameObject egg)
    {
        yield return new WaitForSeconds(10); 

        eggCount--;
        if (egg != null)
        {
            Destroy(egg);
        }
        chickCount++;
        GameObject chick = Instantiate(chickPrefab, egg.transform.position, Quaternion.identity);
        UpdateCounters();
        yield return new WaitForSeconds(10); 

        chickCount--;
        Destroy(chick);
        if (Random.value < 0.5f)
        {
            henCount++;
            GameObject hen = Instantiate(henPrefab, chick.transform.position, Quaternion.identity);
            UpdateCounters();
            StartCoroutine(HenLifecycle(hen));
        }
        else
        {
            roosterCount++;
            GameObject rooster = Instantiate(roosterPrefab, chick.transform.position, Quaternion.identity);
            UpdateCounters();
            StartCoroutine(RoosterLifecycle(rooster));
        }
    }

    IEnumerator HenLifecycle(GameObject hen)
    {
        yield return new WaitForSeconds(40); 

        int eggsLaid = Random.Range(2, 10);
        eggCount += eggsLaid;
        for (int i = 0; i < eggsLaid; i++)
        {
            GameObject newEgg = Instantiate(eggPrefab, hen.transform.position, Quaternion.identity);
            newEgg.tag = "Egg"; 
            StartCoroutine(ProcessEgg(newEgg));
        }
        UpdateCounters();

        yield return new WaitForSeconds(10);
        henCount--;
        Destroy(hen);
        UpdateCounters();
    }

    IEnumerator RoosterLifecycle(GameObject rooster)
    {
        yield return new WaitForSeconds(40);
        roosterCount--;
        Destroy(rooster);
        UpdateCounters();
    }

    Vector3 GetRandomPositionWithinCamera()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float x = Random.Range(mainCamera.transform.position.x - cameraWidth / 2, mainCamera.transform.position.x + cameraWidth / 2);
        float y = Random.Range(mainCamera.transform.position.y - cameraHeight / 2, mainCamera.transform.position.y + cameraHeight / 2);

        return new Vector3(x, y, 0f);
    }
}
