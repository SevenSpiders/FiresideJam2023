using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MA_Follower_with_Event : MonoBehaviour
{
    public int value = 1;
    public float collectRadius = 1;
    public KeyCode interactionKey = KeyCode.F;
    public KeyCode exitKey = KeyCode.Escape;

    private static readonly string[] AncientGreekNames = {
    "Agathokles", "Alethea", "Alkibiades", "Amara", "Anaximander",
    "Antigone", "Apolonios", "Archimedes", "Aristides", "Artemisia",
    "Astyanax", "Caliope", "Chryses", "Cimon", "Cleobulus",
    "Clytemnestra", "Damon", "Diogenes", "Drakon", "Eirene",
    "Epaminondas", "Eudoxia", "Euphemia", "Gorgias", "Hecuba",
    "Helen", "Heracleitus", "Hermia", "Hiparchus", "Iphigenia",
    "Isocrates", "Kleon", "Laodice", "Leontius", "Lysander",
    "Makeda", "Meleager", "Melitta", "Myron", "Neoptolemus",
    "Nikias", "Oinone", "Pallas", "Parmenides", "Pelagia",
    "Pericles", "Philostratus", "Phocion", "Polyxena", "Proxenos",
    "Pyrrhus", "Sappho", "Seleucus", "Simaetha", "Solon",
    "Talthybius", "Theano", "Thucydides", "Timothea", "Xenophon",
    "Adrastos", "Aglaea", "Alcibiades", "Alecto", "Althea",
    "Amalthea", "Anaxagoras", "Andromache", "Antiope", "Aphrodite",
    "Aratus", "Archelaus", "Aristophanes", "Astypalaea", "Calchas",
    "Charis", "Chloris", "Cleon", "Crates", "Damaris",
    "Democritus", "Dionysia", "Elektra", "Eos", "Eratosthenes",
    "Euboea", "Euphrosyne", "Glaucus", "Harmonia", "Hector",
    "Hermocrates", "Hiponax", "Iason", "Iole", "Kalisto",
    "Leander", "Lycoris", "Melantho", "Metis", "Nausicaa"
    };

    private TextMeshProUGUI inpName, curName;
    private SphereCollider col;

    private void Awake()
    {
        col = AddSphereCollider();
        AddCanvas(Color.black, Color.red);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }

    private SphereCollider AddSphereCollider()
    {
        // Get existing collider or create one and apply values
        SphereCollider col = GetComponent<SphereCollider>() != null ? GetComponent<SphereCollider>() : this.gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = collectRadius;

        return col;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MA_PlayerAttributes>() == null)
            return;

        StartCoroutine(StartMiniGame());
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MA_PlayerAttributes>() == null)
            return;

        exitEvent();
    }


    private void collectEvent() {
        Destroy(this.gameObject);
        MA_PlayerAttributes.souls += value;
        MA_BoatController.movementEnabled = true;

    }

    private void startEvent() {
        Debug.Log("Event Started");
        MA_BoatController.movementEnabled = false;
    }

    private void exitEvent() {
            StopAllCoroutines();
            curName.text = "";
            inpName.text = "";
            MA_BoatController.movementEnabled = true;
    }

    private IEnumerator StartMiniGame(int typeCount = 1)
    {
        startEvent();
        
        for (int i = 0; i < typeCount; i++)
        {
            string randomName = AncientGreekNames[(int)Random.Range(0, 99)];
            curName.text = randomName;
            string curInput = "";
            char lastPressedChar = '\0';

            foreach (char c in randomName)
            {
                while (lastPressedChar != c)
                {
                    string keyPressed = Input.inputString;

                    if (!string.IsNullOrEmpty(keyPressed))
                        lastPressedChar = keyPressed[keyPressed.Length - 1];

                    if (Input.GetKeyDown(exitKey))
                        exitEvent() ;
               
                    yield return null;
                }

                curInput += c;
                inpName.text = curInput;
            }
            curName.text = "";
            inpName.text = "";
        }
        collectEvent();
    }


    private void AddCanvas(Color inputCol, Color nameCol)
    {
        GameObject CanvasGO = new("Canvas");
        CanvasGO.transform.parent = this.transform;
        Canvas canvas = CanvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = CanvasGO.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        GameObject nameGO = new("Current Name");
        nameGO.transform.parent = CanvasGO.transform;

        curName = nameGO.AddComponent<TextMeshProUGUI>();
        curName.color = nameCol;
        curName.enableWordWrapping = false;

        Vector2 centerPos = new(Screen.width / 2, -Screen.height / 2);

        RectTransform curNameRect = curName.GetComponent<RectTransform>();
        curNameRect.pivot = new(1f, 1f);
        curNameRect.anchorMin = new(0f, 1f);
        curNameRect.anchorMax = new(0f, 1f);
        curNameRect.anchoredPosition = centerPos;
        curNameRect.sizeDelta = new(0, 0);

        GameObject inputGO = new("Input Name");
        inputGO.transform.parent = CanvasGO.transform;

        inpName = inputGO.AddComponent<TextMeshProUGUI>();
        inpName.color = inputCol;
        inpName.enableWordWrapping = false;

        RectTransform inpNameRect = inpName.GetComponent<RectTransform>();
        inpNameRect.pivot = new(1f, 1f);
        inpNameRect.anchorMin = new(0f, 0f);
        inpNameRect.anchorMax = new(0f, 1f);
        inpNameRect.anchoredPosition = centerPos;
        inpNameRect.sizeDelta = new(0, 0);
    }


}
