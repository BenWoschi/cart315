using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RhythmGameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject notePrefab;
    public Transform noteContainer;

    [Header("Settings")]
    public float approachTime = 1.2f;
    public float okayWindow = 0.3f;
    public int minNotes = 8;
    public int maxNotes = 12;
    public float minSpacing = 120f;
    public Vector2 spawnRangeX = new Vector2(-300, 300);
    public Vector2 spawnRangeY = new Vector2(-200, 200);
    public float timeBetweenNotes = 0.2f; // extra delay between notes fading in

    [HideInInspector] public bool isActive = false; // flag for minigame active

    private List<RhythmNote> sequence = new List<RhythmNote>();
    private int currentIndex = 0;
    private int mistakes = 0;
    private int maxMistakes = 3;

    KeyCode[] possibleKeys = {
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
        KeyCode.J, KeyCode.K, KeyCode.L
    };

    void Start()
    {
        Debug.Log("Rhythm Minigame Starting!");
        StartMinigame();
    }

    public void StartMinigame()
    {
        isActive = true;
        sequence.Clear();
        currentIndex = 0;
        mistakes = 0;

        GenerateSequence();
        StartCoroutine(SpawnNotesSequentially());
    }

    void EndGame(bool success)
    {
        isActive = false;
        Debug.Log(success ? "SUCCESS" : "FAIL");
    }

    void Update()
    {
        if (!isActive) return;
        if (currentIndex >= sequence.Count) return;

        // 🔴 AUTO MISS (player too late)
        RhythmNote note = sequence[currentIndex];

        if (note.ui != null && !note.ui.isDestroyed)
        {
            if (Time.time > note.hitTime + okayWindow)
            {
                Debug.Log("Auto MISS (too late)");
                Miss(note);
                return;
            }
        }

        // 🟢 INPUT HANDLING (no more Input.anyKeyDown)
        KeyCode pressed = GetPressedKey();

        if (pressed != KeyCode.None)
        {
            Debug.Log("Handling input: " + pressed);
            HandleInput(pressed);
        }
    }

    KeyCode GetPressedKey()
    {
        foreach (KeyCode key in possibleKeys)
        {
            if (Input.GetKeyDown(key))
            {
                Debug.Log("Detected key: " + key);
                return key;
            }
        }
        return KeyCode.None;
    }

    void HandleInput(KeyCode key)
    {
        if (currentIndex >= sequence.Count) return;
        RhythmNote note = sequence[currentIndex];

        if (note.ui == null || note.ui.isDestroyed)
        {
            currentIndex++;
            return;
        }

        float error = Mathf.Abs(Time.time - note.hitTime);
        float rawDiff = Time.time - note.hitTime;
        Debug.Log(
    $"KEY: {key} | EXPECTED: {note.key} | " +
    $"rawDiff: {rawDiff:F3} | error: {error:F3} | " +
    $"window: {okayWindow}"
);
        Debug.Log("Key match? " + (key == note.key));

        if (key == note.key && error <= okayWindow)
            Hit(note);
        else
            Miss(note);
    }

    void Hit(RhythmNote note)
    {
        if (note.ui != null && !note.ui.isDestroyed)
        {
            note.ui.ShowHit();
            Destroy(note.ui.gameObject, 0.05f);
        }

        currentIndex++;

        if (currentIndex >= sequence.Count)
            EndGame(true);
    }

    void Miss(RhythmNote note)
    {
        mistakes++;

        if (note.ui != null && !note.ui.isDestroyed)
            note.ui.ShowMiss();

        ScreenShake.Instance?.Shake(0.15f, 0.2f);

        if (mistakes >= maxMistakes)
        {
            Debug.Log("Game OVER due to mistakes: " + mistakes);
            EndGame(false);      // sets isActive = false
            return;              // stops further processing
        }

        currentIndex++;
    }

    void GenerateSequence()
    {
        int count = Random.Range(minNotes, maxNotes + 1);
        KeyCode lastKey = KeyCode.None;

        for (int i = 0; i < count; i++)
        {
            KeyCode key = (Random.value < 0.3f && lastKey != KeyCode.None) ?
                lastKey : possibleKeys[Random.Range(0, possibleKeys.Length)];

            RhythmNote note = new RhythmNote
            {
                key = key,
                hitTime = 0f // placeholder, will set when spawning
            };

            sequence.Add(note);
            lastKey = key;
        }
    }

    // --- New sequential spawning ---
    IEnumerator SpawnNotesSequentially()
    {
        List<Vector2> usedPositions = new List<Vector2>();

        foreach (var note in sequence)
        {
            if (!isActive) yield break;  // stop spawning if game ended

            GameObject obj = Instantiate(notePrefab, noteContainer);
            RhythmNoteUI ui = obj.GetComponent<RhythmNoteUI>();

            float visualDelay = ui.fadeInDuration + timeBetweenNotes;
            note.hitTime = Time.time + approachTime + visualDelay;
            ui.Init(note.key, note.hitTime, approachTime);

            RectTransform rect = obj.GetComponent<RectTransform>();
            Vector2 pos = GetValidPosition(usedPositions);
            rect.anchoredPosition = pos;
            usedPositions.Add(pos);

            note.ui = ui;

            yield return new WaitForSeconds(ui.fadeInDuration + timeBetweenNotes);
        }
    }

    Vector2 GetValidPosition(List<Vector2> existingPositions)
    {
        Vector2 pos;
        int attempts = 0;

        do
        {
            pos = new Vector2(
                Random.Range(spawnRangeX.x, spawnRangeX.y),
                Random.Range(spawnRangeY.x, spawnRangeY.y)
            );

            bool tooClose = false;
            foreach (var p in existingPositions)
            {
                if (Vector2.Distance(pos, p) < minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return pos;

            attempts++;
        } while (attempts < 20);

        return pos; // fallback
    }
}

// Rhythm note data class
public class RhythmNote
{
    public KeyCode key;
    public float hitTime;
    public RhythmNoteUI ui;
}

