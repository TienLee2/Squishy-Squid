using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Level : MonoBehaviour
{
    //Size of the camera
    private const float Camera_Size = 50f;

    //Pipe setting
    private const float Pipe_Width = 7.8f;
    private const float Pipe_Head_Width = 3.75f;
    private const float Pipe_Move_Speed = 30f;
    private const float Pipe_Destroy_X_Position = -100f;
    private const float Pipe_Spawn_X_Position = +100f;
    private const float Bird_X_Position = 0f;


    //Pipe List and level
    private List<Pipe> pipeList;
    private static Level instance;
    public static Level GetInstance()
    {
        return instance;
    }

    public int levelNum;

    private int pipeSpawned;
    private int pipePassedCount;
    private float pipeSpawnTime;
    private float pipeSpawnTimeMax;
    private float pipeSpawnTimeMax2;
    private float gapSize;
    private State state;

    public Animator SquidAnimator;


    public enum Difficulty
    {
        Easy,
        Easy1,
        Medium,
        Medium1,
        Hard,
        Hard1,
        Impossible,
        Impossible1,
    }

    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead,
    }

    private void Awake()
    {
        instance = this;
        pipeList = new List<Pipe>();
        pipeSpawnTimeMax = 1f;
        pipeSpawnTimeMax2 = 0.5f;
        levelNum = 1;
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start()
    {
        Squid.GetInstance().onDied += Level_onDied;
        Squid.GetInstance().onStartPlaying += Level_onStartPlaying;
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            HandlePipeMovement();
            HandlePipeSpawning();
        }
        if (pipeSpawned == 20 || pipeSpawned == 90)
        {
            levelNum = 2;
        }
        else if (pipeSpawned == 60 || pipeSpawned == 120)
        {
            levelNum = 1;
        }
    }

    private void Level_onDied(object sender, System.EventArgs e)
    {
        state = State.BirdDead;
        SquidAnimator.SetBool("Die", true);

    }

    private void Level_onStartPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void HandlePipeSpawning()
    {
        pipeSpawnTime -= Time.deltaTime;
        if (pipeSpawnTime < 0)
        {
            if (levelNum == 1)
            {
                //Time to spawn another pipes
                pipeSpawnTime += pipeSpawnTimeMax;

                float heightEdgeLimit = 10f;
                float minHeight = gapSize * .5f + heightEdgeLimit;
                float totalHeight = Camera_Size * 2f;
                float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

                float height = Random.Range(minHeight, maxHeight);
                CreateGapPipe(height, gapSize, Pipe_Spawn_X_Position);
            }
            else if (levelNum == 2)
            {
                //Time to spawn another pipes
                pipeSpawnTime += pipeSpawnTimeMax2;
                float height = Random.Range(45f, 55f);
                CreateGapPipe(height, gapSize, Pipe_Spawn_X_Position);
            }
        }
    }

    private void HandlePipeMovement()
    {
        for (int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool isToTheRightOfTheBird = pipe.GetXPosition() > Bird_X_Position;
            pipe.Move();

            if (isToTheRightOfTheBird && pipe.GetXPosition() <= Bird_X_Position && pipe.IsBottom())
            {
                //Pipe passed bird
                pipePassedCount++;
                SoundManager.PlaySound(SoundManager.Sound.Score);
            }

            if (pipe.GetXPosition() < Pipe_Destroy_X_Position)
            {
                //Remove the pipe
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 45f;
                pipeSpawnTimeMax = 1.3f;
                break;
            case Difficulty.Easy1:
                gapSize = 40f;
                pipeSpawnTimeMax = 1.3f;
                break;
            case Difficulty.Medium:
                gapSize = 37f;
                pipeSpawnTimeMax = 1.25f;
                break;
            case Difficulty.Medium1:
                gapSize = 32f;
                pipeSpawnTimeMax = 1.25f;
                break;
            case Difficulty.Hard:
                gapSize = 30f;
                pipeSpawnTimeMax = 1.2f;
                break;
            case Difficulty.Hard1:
                gapSize = 26f;
                pipeSpawnTimeMax = 1.2f;
                break;
            case Difficulty.Impossible:
                gapSize = 20f;
                pipeSpawnTimeMax = 1.15f;
                break;
            case Difficulty.Impossible1:
                gapSize = 20f;
                pipeSpawnTimeMax = 1f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipeSpawned >= 150) return Difficulty.Impossible1;
        if (pipeSpawned >= 130) return Difficulty.Impossible;
        if (pipeSpawned >= 105) return Difficulty.Hard1;
        if (pipeSpawned >= 75) return Difficulty.Hard;
        if (pipeSpawned >= 45) return Difficulty.Medium1;
        if (pipeSpawned >= 20) return Difficulty.Medium;
        if (pipeSpawned >= 15) return Difficulty.Easy1;
        return Difficulty.Easy;
    }

    private void CreateGapPipe(float gapY, float gapSize, float xPosition)
    {
        //Create above pipe
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        //create below pipe
        CreatePipe(Camera_Size * 2f - gapY - gapSize * .5f, xPosition, false);
        pipeSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition, bool createBottom)
    {
        //set up pipe head
        Transform pipeHead = Instantiate(GameAsset.GetInstance().pfPipeHead);
        float pipeHeadYPosition;
        if (createBottom)
        {
            pipeHeadYPosition = -Camera_Size + height - Pipe_Head_Width * .5f - 1.4f;

        }
        else
        {
            pipeHeadYPosition = +Camera_Size - height + Pipe_Head_Width * .5f + 1.4f;
            pipeHead.eulerAngles = new Vector3(0, 0, 180f);
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);


        //Set up pipe body
        Transform pipeBody = Instantiate(GameAsset.GetInstance().pfPipeBody);
        float pipeBodyYPosition;
        if (createBottom)
        {
            pipeBodyYPosition = -Camera_Size;
        }
        else
        {
            pipeBodyYPosition = +Camera_Size;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);

        SpriteRenderer pipeBodyRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodyRenderer.size = new Vector2(Pipe_Width, height);

        //Set up box collider
        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(Pipe_Width, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    public int GetPipeSpawn()
    {
        return pipeSpawned;
    }
    public int GetPipePassedCount()
    {
        return pipePassedCount;
    }

    //Represent a single pipe
    private class Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * Pipe_Move_Speed * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * Pipe_Move_Speed * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeHeadTransform.position.x;
        }

        public bool IsBottom()
        {
            return isBottom;
        }

        public void DestroySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }
    }

}
