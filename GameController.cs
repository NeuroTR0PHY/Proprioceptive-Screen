using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;


public class GameController : MonoBehaviour
{
    [Header("Scene Objects")]
    public NB.Cursor cursor;
    public Cylinder start;
    public Cylinder target;
    public bool cursorCenter = true;
    public TMP_InputField pID;
    [Header("Settings")]
    public float margin;
    public float mouseIntensity;
    public int currentTrial = 1;
    public int currentBlock = 1;
    public bool practiceBlock = false;
    [Header("UI Objects")]
    public TextMeshProUGUI DebugText;
    public TextMeshProUGUI marginText;
    public Slider MarginSlider;
    public TextMeshProUGUI sensText;
    public Slider SensSlider;
    public TextMeshProUGUI canvasText;
    public Slider CanvasSlider;
    public TextMeshProUGUI cursorText;
    public Slider trialsInBlock;
    public TextMeshProUGUI trialsText;
    public Slider blocksInTask;
    public TextMeshProUGUI blocksText;
    public TextMeshProUGUI practiceText;



    private StreamWriter writer;
    private bool initiated = false;
    private bool started;


    public GameObject inGameMenu;

    private int cycle = 0;
    private bool trial = false;
    private bool count;
    private float counter;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        CanvasScale();
        SensUpdate();
        MarginUpdate();
        CursorUpdate();
        NumTrialUpdate();
        NumBlockUpdate();
        PracticeUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (count)
        {
            counter += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AdvanceCycle();
            if (currentTrial == 1 && currentBlock == 1 && started == false)
            {
                InitGame();
            }

        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && cycle == 3)
        {
            AdvanceCycle();
        }
        if (cycle == 1 && trial == true)
        {
            if (cursorCenter)
            {
                cursor.Reset(cursorCenter, start.gameObject);
            }
            cursor.Appear();
            start.Vanish();
            target.Vanish();
            trial = false;
            DebugText.text = "Debug:";
            Invoke("AdvanceCycle", 2);
        }
        if (cycle == 2)
        {
            if (trial == true)
            {
                start.Appear();
                start.RandomStart(margin);
                if (!cursorCenter)
                {
                    cursor.Reset(cursorCenter, start.gameObject);
                }
                trial = false;
            }
            if (start.GetComponent<StartZone>().count >= 2)
            {
                AdvanceCycle();
            }
        }
        if (cycle == 3 && trial == true)
        {
            if (currentBlock != 0)
            {
                cursor.Vanish();
            }
            target.Appear();
            target.transform.position = start.transform.position;
            while (Vector3.Distance(start.transform.position, target.transform.position) < start.transform.localScale.x + 1)
            {
                target.RandomStart(margin);
            }
            trial = false;
            count = true;
        }
        if (cycle == 4 && trial == true)
        {
            // cursor.Appear();
            trial = false;
            Log(counter);
            cycle = 1;
            if (currentTrial == trialsInBlock.value)
            {
                if (currentBlock == blocksInTask.value)
                {
                    EndGame();
                    Debug.Log("Finished");
                }
                currentTrial = 1;
                currentBlock++;
            }
            else
            {
                currentTrial++;
            }
            trial = true;
        }


        if (Input.GetKeyUp(KeyCode.Escape))
        {
            inGameMenu.SetActive(!inGameMenu.activeSelf);
            if (inGameMenu.activeSelf)
            {
                Cursor.visible = true;
                cursor.track = false;
            }
            else
            {
                Cursor.visible = false;
                cursor.track = true;
            }
        }
    }

    public void Log(float time)
    {
        count = false;
        var error = Vector3.Distance(cursor.transform.position, target.transform.position);
        var travelDistanceOptimal = Vector3.Distance(start.transform.position, target.transform.position);

        //Debug.Log("error:" + error + ", distance:" + travelDistanceOptimal + ", time:" + counter*1000);
        DebugText.text = ("Debug: Trail = " + currentTrial + " / Block = " + currentBlock + " / Error = " + error + " / AB Distance = " + travelDistanceOptimal + " / Time(ms) = " + counter * 1000);
        WriteLine(currentTrial, currentBlock, error, travelDistanceOptimal, counter);
        counter = 0;
    }

    public void ToggleDebugText(int forceChange = 0)
    {
        DebugText.enabled = !DebugText.enabled;
        if (forceChange == 1)
        {
            DebugText.enabled = true;
        }
        if (forceChange == 2)
        {
            DebugText.enabled = false;
        }
    }

    public void MarginUpdate()
    {
        marginText.text = "Margin (Percentage of Screen): " + MarginSlider.value;
        margin = MarginSlider.value;
    }

    public void SensUpdate()
    {
        sensText.text = "Mouse Sensitivity: " + SensSlider.value;
        mouseIntensity = SensSlider.value;
    }

    public void CanvasScale()
    {
        Camera.main.orthographicSize = CanvasSlider.value;
        canvasText.text = "Canvas Scale: " + CanvasSlider.value;
    }

    public void CursorUpdate()
    {
        cursorCenter = !cursorCenter;
        if (cursorCenter)
        {
            cursorText.text = "Relocate Cursor: Center";
        }
        else
        {
            cursorText.text = "Relocate Cursor: Start Object";
        }
    }

    public void InitGame()
    {
        //var pID = pIDInput.text;
        writer = new StreamWriter(Application.persistentDataPath + "/ULPD_Screen_" + pID.text + ".txt", true);
        writer.WriteLine("Trial" + ";" + "Block" + ";" + "Error" + " ; " + "OptimalDistance" + ";" + "Time");
        if (practiceBlock == true)
        {
            currentBlock = 0;
            started = true;
        }
    }
    public void EndGame()
    {
        writer.Close();
        Application.Quit();
    }
    public void WriteLine(int trial, int block, float err, float optDist, float t)
    {
        var timeMS = t * 1000;
        writer.WriteLine(trial.ToString() + ";" + block.ToString() + ";" + err.ToString() + " ; " + optDist.ToString() + ";" + timeMS.ToString());
    }

    public void NumTrialUpdate()
    {
        trialsText.text = "Trials in Block: " + trialsInBlock.value.ToString();
    }

    public void NumBlockUpdate()
    {
        blocksText.text = "Blocks in Task: " + blocksInTask.value.ToString();
    }

    public void PracticeUpdate()
    {
        practiceBlock = !practiceBlock;
        if (practiceBlock)
        {
            practiceText.text = "Practice Blocks: True";
        }
        else
        {
            practiceText.text = "Practice Blocks: False";
        }
    }

    public void AdvanceCycle()
    {
        cycle += 1;
        trial = true;
    }
}
