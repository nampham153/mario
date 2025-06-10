using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
   
    public static GameManager Instance { get; private set; }
    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;
    public int lives { get; private set; } = 3;
    public int coins { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject); 
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        NewGame();
    }

    /// Bắt đầu trò chơi mới, đặt lại số mạng và số xu, tải level đầu tiên.
    public void NewGame()
    {
        lives = 3;
        coins = 0;

        LoadLevel(1, 1); // Tải màn chơi đầu tiên
    }

    /// Khi người chơi hết mạng, trò chơi kết thúc và bắt đầu lại từ đầu.
    public void GameOver()
    {
        NewGame();
    }


    /// Tải một màn chơi cụ thể dựa trên thông số world và màn chơi.

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"Mario"); 
    }


    // Tải màn chơi tiếp theo trong cùng một world

    public void NextLevel()
    {
        LoadLevel(world, stage + 1);
    }

    /// Reset lại màn chơi sau một khoảng thời gian trì hoãn
    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevel)); 
        Invoke(nameof(ResetLevel), delay); 
    }


    /// Reset lại màn chơi ngay lập tức, giảm số mạng.
    /// Nếu hết mạng, kết thúc trò chơi.

    public void ResetLevel()
    {
        lives--;

        if (lives > 0)
        {
            LoadLevel(world, stage); // Nếu còn mạng, load lại màn chơi hiện tại
        }
        else
        {
            GameOver(); // Nếu hết mạng, bắt đầu lại trò chơi
        }
    }


    /// Khi thu thập một đồng xu, tăng số lượng xu.
    /// Nếu đạt 100 xu, thưởng thêm một mạng và đặt lại số xu về 0.

    public void AddCoin()
    {
        coins++;

        if (coins == 100)
        {
            coins = 0;
            AddLife(); // Thêm một mạng khi đạt 100 xu
        }
    }

    /// Thêm một mạng cho người chơi.
    public void AddLife()
    {
        lives++;
    }
}
