using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;

namespace WeaponShufflePlugin;

public class WeaponShufflePlugin : BasePlugin
{
    public override string ModuleName => "Weapon Shuffle Plugin";

    public override string ModuleVersion => "0.0.1";

    static Random rand = new Random();

    bool primaryWeapon;
    string primaryWeaponString = "";
    string secondaryWeaponString = "";

    List<string> primaryWeaponNames = new List<string>()
        {
            "weapon_ak47",
            "weapon_aug",
            "weapon_awp",
            "weapon_bizon",
            "weapon_famas",
            "weapon_g3sg1",
            "weapon_galilar",
            "0",
            "weapon_m249",
            "weapon_m4a1_silencer",
            "weapon_m4a1",
            "weapon_mac10",
            "weapon_mag7",
            "weapon_mp5sd",
            "weapon_mp7",
            "weapon_mp9",
            "weapon_negev",
            "weapon_nova",
            "weapon_p90",
            "weapon_sawedoff",
            "weapon_scar20",
            "weapon_sg556",
            "weapon_ssg08",
            "weapon_ump45",
            "weapon_xm1014",
        };

    List<string> secondaryWeaponNames = new List<string>()
        {
            "weapon_cz75a",
            "weapon_deagle",
            "weapon_elite",
            "weapon_usp_silencer",
            "weapon_tec9",
            "weapon_revolver",
            "weapon_p250",
            "weapon_glock",
            "weapon_fiveseven",
            "weapon_hkp2000",
        };

    public override void Load(bool hotReload)
    {
        Console.WriteLine("Initialized WeaponShuffle!");

        Server.ExecuteCommand("mp_ct_default_secondary 0");
        Server.ExecuteCommand("mp_t_default_secondary 0");
        Server.ExecuteCommand("mp_buy_allow_guns 0");
    }

    [GameEventHandler]
    public HookResult OnRoundPoststart(EventRoundPoststart @event, GameEventInfo info)
    {
        PickRandomWeapons();
        List<CCSPlayerController> playerList = Utilities.GetPlayers();

        foreach (var player in playerList)
        {
            GiveWeapons(player);

            Logger.LogInformation(player.PlayerName);
        }

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        CCSPlayerController? player = @event.Userid;
        if (player == null || !player.IsValid) return HookResult.Stop;
        player.RemoveAllItemsOnNextRoundReset = true;

        return HookResult.Continue;
    }

    void PickRandomWeapons()
    {
        primaryWeapon = rand.NextDouble() > 0.5;
        primaryWeaponString = primaryWeaponNames[rand.Next(primaryWeaponNames.Count)];
        secondaryWeaponString = secondaryWeaponNames[rand.Next(secondaryWeaponNames.Count)];
    }

    void GiveWeapons(CCSPlayerController player)
    {
        if (primaryWeapon)
        {
            if (primaryWeaponString == "0")
            {
                Logger.LogInformation("New weapon is: KNIFE!!!");
                return;
            }
            player.GiveNamedItem(primaryWeaponString);

            Logger.LogInformation("New weapon is: {Weapon}", primaryWeaponString);
        }
        else
        {
            player.GiveNamedItem(secondaryWeaponString);

            Logger.LogInformation("New weapon is: {Weapon}", secondaryWeaponString);
        }
    }
}
