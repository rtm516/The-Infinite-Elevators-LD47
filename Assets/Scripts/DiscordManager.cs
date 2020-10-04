using UnityEngine;
using Discord;
using System;

public class DiscordManager : MonoBehaviour
{
    [SerializeField]
    private long clientId;

    private Discord.Discord discord;
    private ActivityManager activityManager;
    private Activity activity = new Activity();

    void OnEnable()
    {
        discord = new Discord.Discord(clientId, (ulong)CreateFlags.NoRequireDiscord);
        discord.SetLogHook(LogLevel.Debug, DiscordLog);
        activityManager = discord.GetActivityManager();
    }

	void OnDisable()
    {
        discord.Dispose();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        UpdatePresence();
        discord.RunCallbacks();
    }

	private void UpdatePresence()
	{
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                activity.Details = "At the main menu";
                activity.State = "";
                break;
            case "Main":
                activity.Details = "In singleplayer\n";

                activity.State = "Floors: " + GameManager.Instance.floors.ToString().PadLeft(2, '0');
                break;
            default:
                activity.Details = "";
                activity.State = "";
                break;
        }

        activity.Assets.LargeImage = "logo";

        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res != Result.Ok)
            {
                Debug.LogError("Failed to update discord status");
            }
        });
    }

    private void DiscordLog(LogLevel level, string message)
    {
        Debug.Log(String.Format("Discord:{0} - {1}", level, message));
    }
}
