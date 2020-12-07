using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private List<ChipData> m_ChipTypes;//List of all chip data that can be used for betting
    private Dictionary<string,List<GameObject>> m_BetPoolVisuals = new Dictionary<string, List<GameObject>>();//Dictionary of the bet visuals that are shown in the bet pool
    private Dictionary<string, StoredChips> m_CurrentBets = new Dictionary<string, StoredChips>();//Dictionary of the data for the current bet data 
    private Dictionary<string, Transform> m_PlayerChipLocations = new Dictionary<string, Transform>();//Dictionary of the Player chip location for quick access to the position transforms
    private Dictionary<string, Transform> m_BetPoolChipLocations = new Dictionary<string, Transform>();//Dictionary for the bet pool chip locations for quick access to the position transforms
    private Dictionary<string, List<GameObject>> m_PlayerChipVisuals = new Dictionary<string, List<GameObject>>();//Dictionary for the Player chip Visuals 
    [SerializeField]
    private PlayerHandler m_PlayerHandler;//Reference to the player handler 
    [SerializeField]
    private List<Color> m_Colours =  new List<Color>();//List of Colours that can be won used to display the winning colour
    private bool m_ShouldCheckChips;//Bool that is used when the player handler has flagged that their chips should be checked for having no chips
    [SerializeField]
    public int CurrentBetCount = 0;//current amount of chips that have been bet 
    private int m_SelectedColour = 0;//int representing which Colour button has been clicked
    [SerializeField]
    private float m_ChipHeight = 1;//the Height offset chips will be placed on their stacks at 
    [SerializeField]
    private GameObject m_ChipPrefab;//Prefab for the chips

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeGame());
    }

    /// <summary>
    /// Method to give players there inital chips and then show the UI
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitializeGame()
    {
        yield return null; // Wait a frame for Chip Locations to hook themselves in
        //Add Initial Chips to player
        foreach (ChipData chip in m_ChipTypes)
        {
            AddChipVisual(chip, 10, true);
            m_PlayerHandler.AdjustChips(chip, 10);
        }

        UIHandler.Instance.ShowUI();
    }

    /// <summary>
    /// Main Game Coroutine that chooses the winning colour, and gives out rewards
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChooseColour()
    {
        UIHandler.Instance.HideUI();
        yield return new WaitForSeconds(0.1f);
        int Colour = Random.Range(0, 3);

        UIHandler.Instance.ShowWinner(m_Colours[Colour]);
        yield return new WaitForSeconds(0.5f);

        if (m_SelectedColour == Colour)
        {
            foreach (KeyValuePair<string, StoredChips> keyValuePair in m_CurrentBets)//Go through all bets 
            {
                RemoveChipVisual(keyValuePair.Value.chipData, keyValuePair.Value.Amount * -1, false);//Remove bet visuals from bet pool
                AddChipVisual(keyValuePair.Value.chipData, keyValuePair.Value.Amount * 2);//add chip visuals to the player for their gained chips
                m_PlayerHandler.AdjustChips(keyValuePair.Value.chipData, keyValuePair.Value.Amount * 2);//add chips to the player
                m_CurrentBets[keyValuePair.Key].Amount = 0;//Remove chips from betting pooldata
            }
        }
        else
        {
            foreach (KeyValuePair<string, StoredChips> keyValuePair in m_CurrentBets)//Go through all bets 
            {
                RemoveChipVisual(keyValuePair.Value.chipData, keyValuePair.Value.Amount * -1, false);//Remove bet visuals from bet pool
                m_CurrentBets[keyValuePair.Key].Amount = 0;//Remove chips from betting pooldata
            }

            m_ShouldCheckChips = true;
        }

        if (m_ShouldCheckChips)
        {
            if (!m_PlayerHandler.VerifyChipCounts())//Check if player has run out of chips
            {
                //Give player back all chips
                foreach (ChipData chip in m_ChipTypes)
                {
                    AddChipVisual(chip, 10, true);
                    m_PlayerHandler.AdjustChips(chip, 10);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        CurrentBetCount = 0;
        UIHandler.Instance.LockConfirmation();
        //UIHandler.Instance.UnlockBetting();
        UIHandler.Instance.ChangeMade();
        UIHandler.Instance.ShowUI();
        UIHandler.Instance.HideWinner();
    }

    /// <summary>
    /// Sets the selected colour and starts of the Main Game Coroutine
    /// </summary>
    /// <param name="ColourID"></param>
    public void SelectColour(int ColourID)
    {
        m_SelectedColour = ColourID;
        StartCoroutine(ChooseColour());
    }

    /// <summary>
    /// Method to add in the given chiplocation to the Location dictionaries 
    /// </summary>
    /// <param name="chipID"></param>
    /// <param name="betPool"></param>
    /// <param name="ChipLocation"></param>
    public void AddChipLocations(string chipID, bool betPool, Transform ChipLocation)
    {
        if (betPool)
        {
            if (!m_BetPoolChipLocations.ContainsKey(chipID))
            {
                m_BetPoolChipLocations.Add(chipID, ChipLocation);
            }
        }
        else
        {
            if (!m_PlayerChipLocations.ContainsKey(chipID))
            {
                m_PlayerChipLocations.Add(chipID, ChipLocation);
            }
        }
    }

    /// <summary>
    /// Method that adds a chip visual to either the player or the betting pool
    /// </summary>
    /// <param name="chip"></param>
    /// <param name="amount"></param>
    /// <param name="Player"></param>
    public void AddChipVisual(ChipData chip, int amount = 1,bool Player = true)
    {
        GameObject newChip;
        string chipID = chip.ChipID; // Cache to local variable as it is used alot in this method
        for (int i = 0; i < amount; i++)
        {
            newChip = (GameObject)Instantiate(m_ChipPrefab);// Instantiate prefab
            if (Player)//this is a player chip being added
            {
                newChip.transform.SetParent(m_PlayerChipLocations[chipID]);//Attach to one of the player chip locations
                newChip.transform.localPosition = new Vector3(0, (m_PlayerHandler.GetChipCount(chipID) + i) * m_ChipHeight, 0);//set it to the proper location
                newChip.GetComponent<Renderer>().material.color = chip.ChipColour;//set the materials colour to the chip colour
                if (m_PlayerChipVisuals.ContainsKey(chipID))//check if the player chip visuals already contains this type of chip in it
                {
                    m_PlayerChipVisuals[chipID].Add(newChip);//add it to the list in the visuals
                }
                else
                {
                    m_PlayerChipVisuals.Add(chipID, new List<GameObject>());//add a new list
                    m_PlayerChipVisuals[chipID].Add(newChip);//add the chip to the list
                }
            }
            else
            {
                if (m_BetPoolVisuals.ContainsKey(chipID))//Check if the bet pool visuals contains this key
                {
                    m_BetPoolVisuals[chipID].Add(newChip);//Add chip
                }
                else
                {
                    m_BetPoolVisuals.Add(chipID, new List<GameObject>());//Create new list 
                    m_BetPoolVisuals[chipID].Add(newChip);//add chip
                }
                newChip.transform.SetParent(m_BetPoolChipLocations[chipID]);//Attach to the bet pool chip location
                newChip.transform.localPosition = new Vector3(0, m_BetPoolVisuals[chipID].Count * m_ChipHeight, 0);//set position
                newChip.GetComponent<Renderer>().material.color = chip.ChipColour;//Set colour for the chip
            }
        }
    }

    /// <summary>
    /// This method removes a chip visual 
    /// </summary>
    /// <param name="chip"></param>
    /// <param name="amount"></param>
    /// <param name="Player"></param>
    public void RemoveChipVisual(ChipData chip, int amount = 1,bool Player = true)
    {
        for (int i = 0; i > amount; i--)
        {
            if (Player)//Visual to remove is for the player
            {

                if (m_PlayerChipVisuals.ContainsKey(chip.ChipID))//Chip type exists in the player visuals
                {
                    if (m_PlayerChipVisuals[chip.ChipID].Count > 0)//there is a visual to remove
                    {
                        Destroy(m_PlayerChipVisuals[chip.ChipID][m_PlayerChipVisuals[chip.ChipID].Count - 1]);//destroy the visual
                        m_PlayerChipVisuals[chip.ChipID].RemoveAt(m_PlayerChipVisuals[chip.ChipID].Count - 1);//remove the visuals position in the list
                    }
                }
            }
            else
            {
                if (m_BetPoolVisuals.ContainsKey(chip.ChipID))//Chip type exists in the player visuals
                {
                    if (m_BetPoolVisuals[chip.ChipID].Count > 0)//there is a visual to remove
                    {
                        Destroy(m_BetPoolVisuals[chip.ChipID][m_BetPoolVisuals[chip.ChipID].Count - 1]);//destroy the visual
                        m_BetPoolVisuals[chip.ChipID].RemoveAt(m_BetPoolVisuals[chip.ChipID].Count - 1);//remove the visuals position in the list
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method adjusts the players bet 
    /// </summary>
    /// <param name="chip"></param>
    /// <param name="amount"></param>
    public void AdjustBet(ChipData chip, int amount)
    {

        if ((amount*-1) > 0)
        {
            AddChipVisual(chip, (amount * -1), true);//Add a chip to the players visual
        }
        else if ((amount * -1) < 0)
        {
            RemoveChipVisual(chip, (amount * -1), true);//Remove a chip from the players visual
        }

        m_PlayerHandler.AdjustChips(chip, (amount * -1));//Adjust the player chips based on bet

        AdjustBetPool(chip,amount);//Move Chips needed to/from Bet Pool

        if (CurrentBetCount <= 0)
        {
            UIHandler.Instance.LockConfirmation();
        }
        else
        {
            UIHandler.Instance.UnlockConfirmation();
        }

        UIHandler.Instance.ChangeMade();//Notify UI that a change has been made
    }

    /// <summary>
    /// This Method does all adjustments to the betting pool
    /// </summary>
    /// <param name="chip"></param>
    /// <param name="amount"></param>
    public void AdjustBetPool(ChipData chip, int amount)
    {
        //Setup Visual for Bet Pool
        if (amount > 0)
        {
            AddChipVisual(chip, amount, false);
        }
        else if (amount < 0)
        {
            RemoveChipVisual(chip, amount, false);
        }

        //Adjust Current Bets 
        if (m_CurrentBets.ContainsKey(chip.ChipID))
        {
            m_CurrentBets[chip.ChipID].Amount += amount;
        }
        else
        {
            m_CurrentBets.Add(chip.ChipID, new StoredChips(chip));
            m_CurrentBets[chip.ChipID].Amount += amount;
        }
        //Adjust total number of bets 
        CurrentBetCount += amount;
        
    }

    /// <summary>
    /// Check if the given chip can be decreased.  there needs to be this chip in the current bets dictionary
    /// </summary>
    /// <param name="chip"></param>
    /// <returns></returns>
    public bool CanChipDecrease(ChipData chip)
    {
        if(m_CurrentBets.ContainsKey(chip.ChipID))
            return m_CurrentBets[chip.ChipID].Amount > 0;
        return false;
    }
    
    /// <summary>
    /// Check if the given chip can be decreased.  there needs to be this chip in the current bets dictionary
    /// </summary>
    /// <param name="chip"></param>
    /// <returns></returns>
    public bool CanChipIncrease(ChipData chip)
    {
            return m_PlayerHandler.GetChipCount(chip.ChipID) >= 1 && CurrentBetCount < 10;
    }
}
