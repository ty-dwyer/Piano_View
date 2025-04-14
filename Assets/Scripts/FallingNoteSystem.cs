using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingNoteSystem : MonoBehaviour
{
    [System.Serializable]
    public class NoteEvent
    {
        public int midiNoteNumber; // MIDI note number (e.g. 60 = Middle C)
        public float startTime;    // Time in seconds from the start
        public float duration;     // Duration in seconds
    }

    private HashSet<int> blackKeys;
    private HashSet<int> whiteKeys;

    public GameObject fallingNotePrefabWhite;
    public GameObject fallingNotePrefabBlack;
    public Transform[] keyPositions; // Array of key positions (assign in Inspector)
    public List<NoteEvent> noteEvents = new List<NoteEvent>();
    public float noteSpeed = 0.5f; // Fall speed in meters/second
    public Transform pianoRoot;

    private float songStartTime;

    void Start()
    {
        blackKeys = new HashSet<int>()
        {
            1,
            4, 6,
            9, 11, 13,
            16, 18,
            21, 23, 25,
            28, 30,
            33, 35, 37,
            40, 42,
            45, 47, 49,
            52, 54,
            57, 59, 61,
            64, 66,
            69, 71, 73,
            76, 78,
            81, 83, 85,
        };

        whiteKeys = new HashSet<int>()
        {
            0,  2,  3,  5,  7,
            8,  10, 12, 14, 15,
            17, 19, 20, 22, 24,
            26, 27, 29, 31, 32,
            34, 36, 38, 39, 41,
            43, 44, 46, 48, 50,
            51, 53, 55, 56, 58,
            60, 62, 63, 65, 67,
            68, 70, 72, 74, 75,
            77, 79, 80, 82, 84,
            86, 87
        };

        // Add some test notes manually
        for (int i = 0; i < 60; i++)
        {
            noteEvents.Add(new NoteEvent { midiNoteNumber = i, startTime = (float)i + 1, duration = 10f });
        }
        //noteEvents.Add(new NoteEvent { midiNoteNumber = 0, startTime = 1f, duration = 10f });
        //noteEvents.Add(new NoteEvent { midiNoteNumber = 2, startTime = 2f, duration = 10f });
        //noteEvents.Add(new NoteEvent { midiNoteNumber = 4, startTime = 3f, duration = 10f });
        //noteEvents.Add(new NoteEvent { midiNoteNumber = 0, startTime = 1f, duration = 10f });
        //noteEvents.Add(new NoteEvent { midiNoteNumber = 2, startTime = 2f, duration = 10f });
        //noteEvents.Add(new NoteEvent { midiNoteNumber = 4, startTime = 3f, duration = 10f });
        //noteEvents.Add(new NoteEvent { midiNoteNumber = 0, startTime = 1f, duration = 10f });
        //noteEvents.Add(new NoteEvent { midiNoteNumber = 2, startTime = 2f, duration = 10f });
        //noteEvents.Add(new NoteEvent { midiNoteNumber = 4, startTime = 3f, duration = 10f });

        songStartTime = Time.time;
        StartCoroutine(SpawnNotesCoroutine());
    }

    IEnumerator SpawnNotesCoroutine()
    {
        foreach (NoteEvent note in noteEvents)
        {
            float delay = note.startTime - (Time.time - songStartTime);
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            SpawnNote(note);
        }
    }

    void SpawnNote(NoteEvent note)
    {
        if (note.midiNoteNumber < 0 || note.midiNoteNumber >= keyPositions.Length)
        {
            Debug.LogWarning("Note out of key range!");
            return;
        }

        Transform key = keyPositions[note.midiNoteNumber];
        Vector3 spawnPos = key.position + new Vector3(0, 2.0f, 0); // Spawn above key

        if (whiteKeys.Contains(note.midiNoteNumber))
        {
            GameObject fallingNote = Instantiate(fallingNotePrefabWhite, spawnPos, Quaternion.identity, pianoRoot);
            fallingNote.AddComponent<FallingNoteMover>().Init(note.duration, noteSpeed);
        }
        else
        {
            GameObject fallingNote = Instantiate(fallingNotePrefabBlack, spawnPos, Quaternion.identity, pianoRoot);
            fallingNote.AddComponent<FallingNoteMover>().Init(note.duration, noteSpeed);
        }
    }

    // Internal class to move falling note
    public class FallingNoteMover : MonoBehaviour
    {
        private float lifetime;
        private float speed;

        public void Init(float duration, float fallSpeed)
        {
            lifetime = duration;
            speed = fallSpeed;
        }

        void Update()
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
                Destroy(gameObject);
        }
    }
}