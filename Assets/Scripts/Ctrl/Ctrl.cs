using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ctrl : MonoBehaviour
{
    [HideInInspector]
    public View view;

    private FSMSystem fsm;

    private CameraManager cameraMgr;
    public CameraManager CameraMgr { get { return cameraMgr; } }
    private GameManager gameMgr;
    public GameManager GameMgr { get => gameMgr;}
    private AudioManager audioMgr;
    public AudioManager AudioMgr { get => audioMgr;}
    private Model model;
    public Model Model { get => model; }  

    private void Awake()
    {
        model = GameObject.FindGameObjectWithTag("Model").GetComponent<Model>();
        view = GameObject.FindGameObjectWithTag("View").GetComponent<View>();

        cameraMgr = GetComponent<CameraManager>();
        gameMgr = GetComponent<GameManager>();
        audioMgr = GetComponent<AudioManager>();
    }

    private void Start()
    {
        MakeFSM();
    }

    private void MakeFSM()
    {
        fsm = new FSMSystem();
        FSMState[] states = GetComponentsInChildren<FSMState>();
        foreach (FSMState state in states)
        {
            fsm.AddState(state, this);
        }

        GameMenuState menuState = GetComponentInChildren<GameMenuState>();
        fsm.SetCurState(menuState);
    }
}
