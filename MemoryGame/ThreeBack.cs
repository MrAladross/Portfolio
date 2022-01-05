using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ThreeBack : MonoBehaviour
{

    public int[] recentNums;
    public Sprite[] submitButtonImages;
    public Image[] lifeImages;
    public int numIndex;
    public bool readyForInput;
    public InputField numInputField;
    public Slider remainingTimeSlider;
    private bool isSliderTime;
    private float remainingTimer;
    private float maximumTime;
    
    public Button submitButton;
    public Image num1Image;
    public Image num2Image;
    public Image score1Image;
    public Image score2Image;
    public Sprite[] numberSprites;

    public Text instructionsText;
    private int score;
    private float numWaitSpeed = 1.0f;

    private ColorBlock CB;
    private Image subButtonImage;
    private bool isCorrect;

    private float round = 0;
    private float accuracy = 0;
    private int correctAnswers = 0;
    private int incorrectAnswers = 0;
    public int gamesPlayed;
    public int highScore;
    public Profile myProfile;

    public Canvas gameCanvas;
    public Canvas menuCanvas;
    public Button menuButton;

    public Button[] difficultyButtons;
    private int difficultyUnlocked;

    void FreshStats()
    {
        recentNums = new int[5];
        score = 0;
        ScoreImagesSet(score);
        correctAnswers = 0;
        incorrectAnswers = 0;
        round = 0;
        accuracy = 0;
        gamesPlayed += 1;

        numInputField.interactable = false;
        submitButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
        readyForInput = false;
        submitButton.interactable = false;
        ToggleMenu();
    }
    #region tempo

    void Start()
    {
        #region freshStart
        recentNums = new int[5];
        score = 0;
        correctAnswers = 0;
        incorrectAnswers = 0;
        round = 0;
        accuracy = 0;
        gamesPlayed += 1;

        numInputField.interactable = false;
        submitButton.onClick.RemoveAllListeners();
        StopAllCoroutines();
        readyForInput = false;
        submitButton.interactable = false;
        #endregion freshStart

        CB = submitButton.colors;
        subButtonImage = submitButton.GetComponent<Image>();
        myProfile = LoadProfile();
        RefreshButtons();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            submitButton.onClick.Invoke();

            numInputField.ActivateInputField();
        }

        if (isSliderTime)
        {
            if (remainingTimer > 0)
            {
                remainingTimer -= Time.deltaTime;
            }
            else
            {
                submitButton.onClick.Invoke();
            }
            remainingTimeSlider.value = remainingTimer / maximumTime;
        }
    }
    public void ToggleMenu()
    {
        if (menuCanvas.enabled==true)
        {
            menuCanvas.enabled = false;
        }
        else
        {
            menuCanvas.enabled = true;
        }
    }
    public void ScoreImagesSet(int number)
    {

        if (number < 10)
        {
            score1Image.enabled = false;
            switch (number)
            {
                case 0:
                    score2Image.sprite = numberSprites[0];
                    break;
                case 1:
                    score2Image.sprite = numberSprites[1];
                    break;
                case 2:
                    score2Image.sprite = numberSprites[2];
                    break;
                case 3:
                    score2Image.sprite = numberSprites[3];
                    break;
                case 4:
                    score2Image.sprite = numberSprites[4];
                    break;
                case 5:
                    score2Image.sprite = numberSprites[5];
                    break;
                case 6:
                    score2Image.sprite = numberSprites[6];
                    break;
                case 7:
                    score2Image.sprite = numberSprites[7];
                    break;
                case 8:
                    score2Image.sprite = numberSprites[8];
                    break;
                case 9:
                    score2Image.sprite = numberSprites[9];
                    break;
            }
        }
        else
        {
            score1Image.enabled = true;
            switch (number / 10)
            {
                case 0:
                    score1Image.sprite = numberSprites[0];
                    break;
                case 1:
                    score1Image.sprite = numberSprites[1];
                    break;
                case 2:
                    score1Image.sprite = numberSprites[2];
                    break;
                case 3:
                    score1Image.sprite = numberSprites[3];
                    break;
                case 4:
                    score1Image.sprite = numberSprites[4];
                    break;
                case 5:
                    score1Image.sprite = numberSprites[5];
                    break;
                case 6:
                    score1Image.sprite = numberSprites[6];
                    break;
                case 7:
                    score1Image.sprite = numberSprites[7];
                    break;
                case 8:
                    score1Image.sprite = numberSprites[8];
                    break;
                case 9:
                    score1Image.sprite = numberSprites[9];
                    break;
            }
            switch (number % 10)
            {
                case 0:
                    score2Image.sprite = numberSprites[0];
                    break;
                case 1:
                    score2Image.sprite = numberSprites[1];
                    break;
                case 2:
                    score2Image.sprite = numberSprites[2];
                    break;
                case 3:
                    score2Image.sprite = numberSprites[3];
                    break;
                case 4:
                    score2Image.sprite = numberSprites[4];
                    break;
                case 5:
                    score2Image.sprite = numberSprites[5];
                    break;
                case 6:
                    score2Image.sprite = numberSprites[6];
                    break;
                case 7:
                    score2Image.sprite = numberSprites[7];
                    break;
                case 8:
                    score2Image.sprite = numberSprites[8];
                    break;
                case 9:
                    score2Image.sprite = numberSprites[9];
                    break;
            }
        }

    }
    public void NumImagesSet(int number)
    {
        if (number < 10)
        {
            num1Image.enabled = false;
            switch (number)
            {
                case 0:
                    num2Image.sprite = numberSprites[0];
                    break;
                case 1:
                    num2Image.sprite = numberSprites[1];
                    break;
                case 2:
                    num2Image.sprite = numberSprites[2];
                    break;
                case 3:
                    num2Image.sprite = numberSprites[3];
                    break;
                case 4:
                    num2Image.sprite = numberSprites[4];
                    break;
                case 5:
                    num2Image.sprite = numberSprites[5];
                    break;
                case 6:
                    num2Image.sprite = numberSprites[6];
                    break;
                case 7:
                    num2Image.sprite = numberSprites[7];
                    break;
                case 8:
                    num2Image.sprite = numberSprites[8];
                    break;
                case 9:
                    num2Image.sprite = numberSprites[9];
                    break;
            }
        }
        else
        {
            num1Image.enabled = true;
            switch (number / 10)
            {
                case 0:
                    num1Image.sprite = numberSprites[0];
                    break;
                case 1:
                    num1Image.sprite = numberSprites[1];
                    break;
                case 2:
                    num1Image.sprite = numberSprites[2];
                    break;
                case 3:
                    num1Image.sprite = numberSprites[3];
                    break;
                case 4:
                    num1Image.sprite = numberSprites[4];
                    break;
                case 5:
                    num1Image.sprite = numberSprites[5];
                    break;
                case 6:
                    num1Image.sprite = numberSprites[6];
                    break;
                case 7:
                    num1Image.sprite = numberSprites[7];
                    break;
                case 8:
                    num1Image.sprite = numberSprites[8];
                    break;
                case 9:
                    num1Image.sprite = numberSprites[9];
                    break;
            }
            switch (number % 10)
            {
                case 0:
                    num2Image.sprite = numberSprites[0];
                    break;
                case 1:
                    num2Image.sprite = numberSprites[1];
                    break;
                case 2:
                    num2Image.sprite = numberSprites[2];
                    break;
                case 3:
                    num2Image.sprite = numberSprites[3];
                    break;
                case 4:
                    num2Image.sprite = numberSprites[4];
                    break;
                case 5:
                    num2Image.sprite = numberSprites[5];
                    break;
                case 6:
                    num2Image.sprite = numberSprites[6];
                    break;
                case 7:
                    num2Image.sprite = numberSprites[7];
                    break;
                case 8:
                    num2Image.sprite = numberSprites[8];
                    break;
                case 9:
                    num2Image.sprite = numberSprites[9];
                    break;
            }
        }
    }
    public void NextNum()
    {
        if (incorrectAnswers < 4)
        {
            recentNums[0] = recentNums[1];
            recentNums[1] = recentNums[2];
            recentNums[2] = recentNums[3];
            recentNums[3] = recentNums[4];
            recentNums[4] = UnityEngine.Random.Range(0,99);
            NumImagesSet(recentNums[4]);
        }
        else
        {
            NumImagesSet(0);
        }
        StartCoroutine(NumAnimation());
    }
    public IEnumerator ButtonAnimation()
    {
        isSliderTime = false;
        remainingTimer = maximumTime;
        if (isCorrect == true)
        {
            subButtonImage.sprite = submitButtonImages[1];
        }
        else
        {
            subButtonImage.sprite = submitButtonImages[0];
        }
        yield return new WaitForSeconds(0.8f);
        subButtonImage.sprite = submitButtonImages[2];
        submitButton.colors = CB;
        isCorrect = false;
        yield return null;

        //do the remaining stuff here
        StartCoroutine(ScoreAnimation());
        if (readyForInput)
            NextNum();
    }
    public IEnumerator ButtonAnimationRotating()
    {
        for (int i = 0; i < 9; ++i)
        {
            yield return new WaitForSeconds(0.2f);
            subButtonImage.sprite = submitButtonImages[i % submitButtonImages.Length];
        }

        if (isCorrect == true)
        {
            subButtonImage.sprite = submitButtonImages[1];
        }
        else
        {
            subButtonImage.sprite = submitButtonImages[0];
        }
        yield return new WaitForSeconds(0.7f);
        subButtonImage.sprite = submitButtonImages[2];
        submitButton.colors = CB;
        isCorrect = false;
        yield return null;
    }
    public IEnumerator ScoreAnimation()
    {
        ScoreImagesSet(score);
        yield return null;
    }

    public void Evaluate4()
    {
        lifeImages[2].gameObject.SetActive(false);
        if (readyForInput)
        {
            int result = -1;
            if (int.TryParse(numInputField.text, out result))
            {
                if (result == recentNums[0])
                {
                    score += 6;
                    correctAnswers++;
                    isCorrect = true;
                }
                else
                {
                    incorrectAnswers++;
                    for (int i=0;i<lifeImages.Length-1;++i)
                    {
                        if (i<incorrectAnswers)
                        {
                            lifeImages[lifeImages.Length - i - 2].sprite = submitButtonImages[0];
                        }
                    }
                    if (incorrectAnswers > 2)
                        YouLose();
                }
                ScoreImagesSet(score);
            }
            //button and score animations before determining next number
            //disable submit button until input field is updated
            SubmitButtonDisable();

            ++round;

            if (incorrectAnswers>=2)
            {
                YouLose();
                readyForInput = false;
            }
        }
    }
    public void Evaluate3Legendary()
    {
        lifeImages[2].gameObject.SetActive(false);
        if (readyForInput)
        {
            int result = -1;
            if (int.TryParse(numInputField.text, out result))
            {
                if (result == recentNums[1])
                {
                    score += 5;
                    correctAnswers++;
                    isCorrect = true;
                }
                else
                {
                    incorrectAnswers++;
                    for (int i = 0; i < lifeImages.Length-1; ++i)
                    {
                        if (i < incorrectAnswers)
                        {
                            lifeImages[lifeImages.Length - i - 2].sprite = submitButtonImages[0];
                        }
                    }
                    if (incorrectAnswers > 2)
                        YouLose();
                }
                ScoreImagesSet(score);
            }
            SubmitButtonDisable();
            //button and score animations before determining next number
            //disable submit button until input field is updated

            //check for accuracy
            ++round;
            accuracy = (float) (correctAnswers / round);
            if (round<15&&!readyForInput)
            {
                YouLose();
            }
            if (round >= 15)
            {
                if (incorrectAnswers < 2)
                    YouWin();
                else YouLose();
                readyForInput = false;
            }
        }
    }
    public void Evaluate3Insane()
    {
        lifeImages[2].gameObject.SetActive(true);
        if (readyForInput)
        {
            int result = -1;
            if (int.TryParse(numInputField.text, out result))
            {
                if (result == recentNums[1])
                {
                    score += 4;
                    correctAnswers++;
                    isCorrect = true;
                }
                else
                {
                    incorrectAnswers++;
                    for (int i = 0; i < lifeImages.Length; ++i)
                    {
                        if (i < incorrectAnswers)
                        {
                            lifeImages[lifeImages.Length - i - 1].sprite = submitButtonImages[0];
                        }
                    }
                    if (incorrectAnswers > 3)
                        YouLose();
                }
                ScoreImagesSet(score);
            }
            SubmitButtonDisable();
            //button and score animations before determining next number
            //disable submit button until input field is updated

            //check for accuracy
            ++round;
            accuracy = (float)(correctAnswers / round);
            if (round < 12 && !readyForInput)
            {
                YouLose();
            }
            if (round >= 12)
            {
                CheckAccuracyTarget(0.8f);
                readyForInput = false;
            }
        }
    }
    public void Evaluate3Hard()
    {
        lifeImages[2].gameObject.SetActive(true);
        if (readyForInput)
        {
            int result = -1;
            if (int.TryParse(numInputField.text, out result))
            {
                if (result == recentNums[1])
                {
                    score += 3;
                    correctAnswers++;
                    isCorrect = true;
                }
                else
                {
                    incorrectAnswers++;
                    for (int i = 0; i < lifeImages.Length; ++i)
                    {
                        if (i < incorrectAnswers)
                        {
                            lifeImages[lifeImages.Length - i - 1].sprite = submitButtonImages[0];
                        }
                    }
                    if (incorrectAnswers > 3)
                        YouLose();
                }
                ScoreImagesSet(score);
            }
            SubmitButtonDisable();
            //button and score animations before determining next number
            //disable submit button until input field is updated

            //check for accuracy
            ++round;
            accuracy = (float)(correctAnswers / round);
            if (round < 10 && !readyForInput)
            {
                YouLose();
            }
            if (round >= 10)
            {
                CheckAccuracyTarget(0.7f);
                readyForInput = false;
            }
        }
    }

    public void Evaluate2()
    {
        lifeImages[2].gameObject.SetActive(true);
        if (readyForInput)
        {
            int result = -1;
            if (int.TryParse(numInputField.text, out result))
            {
                if (result == recentNums[2])
                {
                    score += 2;
                    correctAnswers++;
                    isCorrect = true;
                }
                else
                {
                    incorrectAnswers++;
                    for (int i = 0; i < lifeImages.Length; ++i)
                    {
                        if (i < incorrectAnswers)
                        {
                            lifeImages[lifeImages.Length - i - 1].sprite = submitButtonImages[0];
                        }
                    }
                    if (incorrectAnswers > 3)
                        YouLose();
                }
                ScoreImagesSet(score);
            }
            SubmitButtonDisable();
            //button and score animations before determining next number
            //disable submit button until input field is updated
            
            //check for accuracy
            ++round;
            accuracy = (float)(correctAnswers / round);
            if (round < 10 && !readyForInput)
            {
                YouLose();
            }
            if (round >= 10)
            {
                CheckAccuracyTarget(0.8f);
                readyForInput = false;
            }
        }
    }
    public void Evaluate1()
    {
        lifeImages[2].gameObject.SetActive(true);
        if (readyForInput)
        {
            int result = -1;
            if (int.TryParse(numInputField.text, out result))
            {
                if (result == recentNums[3])
                {
                    score++;
                    correctAnswers++;
                    isCorrect = true;
                }
                else
                {
                    incorrectAnswers++;
                    for (int i = 0; i < lifeImages.Length; ++i)
                    {
                        if (i < incorrectAnswers)
                        {
                            lifeImages[lifeImages.Length - i - 1].sprite = submitButtonImages[0];
                        }
                    }
                    if (incorrectAnswers > 3)
                        YouLose();
                }
                ScoreImagesSet(score);
            }
            SubmitButtonDisable();
            //button and score animations before determining next number

            //disable submit button until input field is updated



            //check for accuracy
            ++round;
            accuracy = (float)(correctAnswers / round);
            if (round < 10 && !readyForInput)
            {
                YouLose();
            }
            if (round >= 10)
            {
                Debug.Log(accuracy);
                CheckAccuracyTarget(0.9f);
                readyForInput = false;
            }


        }
    }
    #endregion tempo

    public void EnableEasy()
    {
        lifeImages[2].gameObject.SetActive(true);
        for (int i = 0; i < lifeImages.Length; ++i)
        {
            if (lifeImages[i].IsActive())
            {
                lifeImages[i].sprite = submitButtonImages[3];
            }
        }
        FreshStats();
        submitButton.onClick.AddListener(() => Evaluate1());
        StartCoroutine(FirstNums());
        instructionsText.text = "One Back";
        difficultyUnlocked = 1;
        maximumTime = 7f;
        remainingTimer = maximumTime;
    }
    public void EnableMedium()
    {
        lifeImages[2].gameObject.SetActive(true);
        for (int i = 0; i < lifeImages.Length; ++i)
        {
            if (lifeImages[i].IsActive())
            {
                lifeImages[i].sprite = submitButtonImages[3];
            }
        }
        FreshStats();
        submitButton.onClick.AddListener(() => Evaluate2());
        StartCoroutine(FirstNums());
        instructionsText.text = "Two Back";
        difficultyUnlocked = 2;
        maximumTime = 7f;
        remainingTimer = maximumTime;
    }
    public void EnableHard()
    {
        lifeImages[2].gameObject.SetActive(true);
        for (int i = 0; i < lifeImages.Length; ++i)
        {
            if (lifeImages[i].IsActive())
            {
                lifeImages[i].sprite = submitButtonImages[3];
            }
        }
        FreshStats();
        submitButton.onClick.AddListener(() => Evaluate3Hard());
        StartCoroutine(FirstNums());
        instructionsText.text = "Three Back";
        difficultyUnlocked = 3;
        maximumTime = 7f;
        remainingTimer = maximumTime;
    }
    public void EnableInsane()
    {
        lifeImages[2].gameObject.SetActive(true);
        for (int i = 0; i < lifeImages.Length; ++i)
        {
            if (lifeImages[i].IsActive())
            {
                lifeImages[i].sprite = submitButtonImages[3];
            }
        }
        FreshStats();
        submitButton.onClick.AddListener(() => Evaluate3Insane());
        StartCoroutine(FirstNums());
        instructionsText.text = "Three Back";
        difficultyUnlocked = 4;
        maximumTime = 5f;
        remainingTimer = maximumTime;
    }
    public void EnableLegendary()
    {
        lifeImages[2].gameObject.SetActive(false);
        for (int i = 0; i < lifeImages.Length; ++i)
        {
            if (lifeImages[i].IsActive())
            {
                lifeImages[i].sprite = submitButtonImages[3];
            }
        }
        FreshStats();
        submitButton.onClick.AddListener(() => Evaluate3Legendary());
        StartCoroutine(FirstNums());
        instructionsText.text = "Three Back";
        difficultyUnlocked = 5;
        maximumTime = 5f;
        remainingTimer = maximumTime;
    }
    public void EnableMyth()
    {
        lifeImages[2].gameObject.SetActive(false);
        for (int i=0;i<lifeImages.Length;++i)
        {
            if (lifeImages[i].IsActive())
            {
                lifeImages[i].sprite = submitButtonImages[3];
            }
        }
        FreshStats();
        submitButton.onClick.AddListener(() => Evaluate4());
        StartCoroutine(FirstNums());
        instructionsText.text = "Four Back";
        difficultyUnlocked = 6;
        maximumTime = 3f;
        remainingTimer = maximumTime;
    }
    public IEnumerator SaveProfile()
    {
        Profile data = LoadProfile();
        yield return new WaitForSeconds(0.14f);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/profile.dat", FileMode.Create);
        if (difficultyUnlocked>data.tbDifficultyUnlocked)
            data.tbDifficultyUnlocked = difficultyUnlocked;
        if (gamesPlayed > data.tbGamesPlayed)
            data.tbGamesPlayed = gamesPlayed;
        if (highScore > data.tbHighScore)
            data.tbHighScore = highScore;
        myProfile = data;
        bf.Serialize(file, data);
        file.Close();

        RefreshButtons();
    }
    public Profile LoadProfile()
    {
        if (File.Exists(Application.persistentDataPath + "/profile.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/profile.dat", FileMode.Open);
            Profile p = (Profile)bf.Deserialize(file);
            file.Close();
            return p;
        }
        else
        {
            return new Profile();
        }
    }
    private void RefreshButtons()
    {
        for (int i=0;i<= myProfile.tbDifficultyUnlocked;++i)
        {
            difficultyButtons[i].interactable = true;
        }
    }
    private void YouWin()
    {
        //unlock next level
        StartCoroutine(SaveProfile());

        //toggle to return to menu?
        StartCoroutine(WinAnimation());

        instructionsText.text = "Accuracy: " + Mathf.RoundToInt(accuracy*100) + "% \r\nYou Advance!";
        numInputField.interactable = false;
    }
    public GameObject coinPrefab;
    private IEnumerator WinAnimation()
    {
        for(int i=0;i<200;++i)
        {
            if (i%5==0)
            {
                float xOffset = UnityEngine.Random.Range(0f,(float)Screen.width);
                float yOffset = UnityEngine.Random.Range(0f, 2* (float)Screen.height);
                GameObject go = Instantiate(coinPrefab, gameCanvas.transform);
                Vector2 offset = new Vector2(xOffset, yOffset);
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.up;
                rt.anchorMax = Vector2.up;
                rt.pivot = Vector2.up;
                rt.position = new Vector3(xOffset, yOffset, 0);
            }
            yield return null;
        }
        StartCoroutine(MenuButtonAttention());
    }
    bool menuGrowing = true;
    private IEnumerator MenuButtonAttention()
    {
        while (true)
        {
            if (menuGrowing)
            {
                menuButton.transform.localScale *= 1.01f;
                if (menuButton.transform.localScale.x > 1.2f)
                    menuGrowing = false;
            }
            else
            {
                menuButton.transform.localScale *= .99f;
                if (menuButton.transform.localScale.x < 1f)
                    menuGrowing = true;
            }
            yield return null;
        }
    }
    private void YouLose()
    {
        instructionsText.text = "Accuracy: " + Mathf.RoundToInt(accuracy*100) + "% \r\nTry Again";
        numInputField.interactable = false;
        readyForInput = false;
    }
    public void CheckAccuracyTarget(float minimum)//TODO: pass recent unlock as parameter
    {
        if (accuracy >= minimum)
        {
            YouWin();
        }
        else
        {
            YouLose();
        }
    }
    public void EnableButton()
    {
        submitButton.interactable = true;
    }
    public IEnumerator NumAnimation()
    {
            num1Image.rectTransform.localScale = Vector3.one * 1.3f;
            num2Image.rectTransform.localScale = Vector3.one * 1.3f;
            while (num1Image.rectTransform.localScale.y > 1.0f)
            {
                num1Image.rectTransform.localScale *= .98f;
                num2Image.rectTransform.localScale *= .98f;

                yield return null;
            }
            if (readyForInput)
                isSliderTime = true;
    }
    public IEnumerator FirstNums()
    {
        NextNum();
        yield return new WaitForSeconds(numWaitSpeed);
        NextNum();
        yield return new WaitForSeconds(numWaitSpeed);
        NextNum();
        yield return new WaitForSeconds(numWaitSpeed);
        NextNum();
        yield return new WaitForSeconds(numWaitSpeed);
        NextNum();
        readyForInput = true;
        numInputField.interactable = true;

    }
    public void SubmitButtonDisable()
    {
        ColorBlock cb = submitButton.colors;
        cb.disabledColor = Color.white;
        submitButton.colors = cb;
        submitButton.interactable = false;

        StartCoroutine(ButtonAnimation());

        numInputField.Clear();
        submitButton.interactable = false;
    }
}
