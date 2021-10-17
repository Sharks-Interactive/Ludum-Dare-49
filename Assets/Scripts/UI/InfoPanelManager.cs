using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Chrio.Interaction;
using Chrio.Entities;
using static SharkUtils.ExtraFunctions;

namespace Chrio.UI
{
    public class InfoPanelManager : UIBehaviour
    {
        [Header("Components")]
        public Slider HealthBar;
        [Tooltip("In this order: Name, Desc, Stats")]
        public TextMeshProUGUI[] TextBoxes = new TextMeshProUGUI[3];
        public GameObject[] Construction = new GameObject[2];
        public CanvasGroup Group;

        // Update is called once per frame
        void Update()
        {
            Construction.SetArrayActive(false);
            if (GlobalState.Game.Entities.Selected.Count > 0)
            {
                ShowScreen();
                if (GlobalState.Game.Entities.Selected.Count > 1) // Multiple selected
                {
                    TextBoxes[0].text = $"Multi Selection ({GlobalState.Game.Entities.Selected.Count})";
                    TextBoxes[1].text = $"Multi Selection ({GlobalState.Game.Entities.Selected.Count})";

                    float TotalHealth = 0;
                    float TotalMaxHealth = 0;

                    float TotalDmg = 0;
                    foreach (IBaseEntity ent in GlobalState.Game.Entities.Selected)
                    {
                        Drydock dock = GlobalState.Game.Entities.Selected[0].GetEntity() as Drydock;
                        TotalHealth += ent.Health;
                        if (dock != null)
                            TotalMaxHealth += ent.GetData().Health * (dock.GetOwnerID() == 2 ? 1 : 5);
                        else
                            TotalMaxHealth += ent.GetData().Health;

                        Spaceship ship = ent.GetEntity() as Spaceship;
                        
                        if (ship != null)
                            TotalDmg += ship.Turret.Info.Damage;
                    }

                    HealthBar.maxValue = TotalMaxHealth;
                    HealthBar.value = TotalHealth;

                    if (AllEntsOfType("Asteroid"))
                    {
                        int TotalWorth = 0;
                        foreach (IBaseEntity ent in GlobalState.Game.Entities.Selected)
                        {
                            Asteroid roid = ent.GetEntity() as Asteroid;
                            if (roid != null)
                                TotalWorth += roid.AsteroidWorth;
                        }

                        TextBoxes[2].text =
                        $"Total Health: {TotalHealth}/{TotalMaxHealth}\nWorth: {TotalWorth}\n";
                        return;
                    }
                    TextBoxes[2].text =
                        $"Total Health: {TotalHealth}/{TotalMaxHealth}\nAttacking: Multiple\nTotal Weapon Damage: {TotalDmg} per pulse\n";
                }
                else
                {
                    Asteroid roid = GlobalState.Game.Entities.Selected[0].GetEntity() as Asteroid;
                    if (roid == null)
                        TextBoxes[0].text = (GlobalState.Game.Entities.Selected[0].GetOwnerID() == 1 ? "Enemy " : "") + (GlobalState.Game.Entities.Selected[0].GetOwnerID() == 2 ? "Neutral " : "") + GlobalState.Game.Entities.Selected[0].GetData().DisplayName;
                    else
                        TextBoxes[0].text = GlobalState.Game.Entities.Selected[0].GetData().DisplayName;

                    TextBoxes[1].text = GlobalState.Game.Entities.Selected[0].GetData().ShortDescription;

                    Spaceship ship = GlobalState.Game.Entities.Selected[0].GetEntity() as Spaceship;
                    Drydock dock = GlobalState.Game.Entities.Selected[0].GetEntity() as Drydock;
                    if (ship != null)
                    {
                        TextBoxes[2].text = $"Health: {GlobalState.Game.Entities.Selected[0].Health}/{GlobalState.Game.Entities.Selected[0].GetData().Health}\n" +
                            $"Attacking: {(ship.Turret.Target != null ? (ship.Turret.Target.GetOwnerID() == 1 ? "Enemy " : "") + ship.Turret.Target.GetData().DisplayName : "Nothing")}\n" +
                            $"Weapon Damage: {ship.Turret.Info.Damage} per {(ship.Turret.Info.Type == World.WeaponInfo.WeaponType.Pulse || ship.Turret.Info.Type == World.WeaponInfo.WeaponType.ContinousPulse ? "pulse" : "second")}\n" +
                            $"Weapon Cooldown: {ship.Turret.Info.Cooldown} seconds\n" +
                            $"Speed: {ship.Data.MaxSpeed} m/s\n" +
                            $"Current speed: {ship.Velocity.AbsSumOfVector2()} m/s\n" +
                            $"Name: N/A";

                        HealthBar.maxValue = GlobalState.Game.Entities.Selected[0].GetData().Health;
                        HealthBar.value = GlobalState.Game.Entities.Selected[0].Health;
                    }
                    else if (roid != null)
                    {
                        HealthBar.maxValue = GlobalState.Game.Entities.Selected[0].GetData().Health;
                        HealthBar.value = GlobalState.Game.Entities.Selected[0].Health;
                        TextBoxes[2].text = $"Health: {GlobalState.Game.Entities.Selected[0].Health}/{GlobalState.Game.Entities.Selected[0].GetData().Health}\nWorth: {roid.AsteroidWorth}\n";
                    }
                    else if (dock != null)
                    {
                        if (dock.GetOwnerID() == 0) Construction.SetArrayActive(true);
                        HealthBar.maxValue = GlobalState.Game.Entities.Selected[0].GetData().Health * (dock.GetOwnerID() == 2 ? 1 : 5);
                        HealthBar.value = GlobalState.Game.Entities.Selected[0].Health;
                        TextBoxes[2].text = $"Health: {GlobalState.Game.Entities.Selected[0].Health}/{GlobalState.Game.Entities.Selected[0].GetData().Health * (dock.GetOwnerID() == 2 ? 1 : 5)}\nBuilding: {(dock.Constructing != null ? dock.Constructing.DisplayName : "Nothing")}\nV Construction V";
                    }
                    else
                    {
                        TextBoxes[2].text = $"Health: {GlobalState.Game.Entities.Selected[0].Health}/{GlobalState.Game.Entities.Selected[0].GetData().Health}\n";
                        HealthBar.maxValue = GlobalState.Game.Entities.Selected[0].GetData().Health;
                        HealthBar.value = GlobalState.Game.Entities.Selected[0].Health;
                    }
                }
            }
            else HideScreen();
        }

        public void ExecuteCommand(SpaceshipData SpaceData)
        {
            Drydock dock = GlobalState.Game.Entities.Selected[0].GetEntity() as Drydock;

            if (dock == null) return;
            dock.BuildShip(SpaceData, 0);
        }

        public void ShowScreen() => Group.alpha = 1.0f;

        public void HideScreen() => Group.alpha = 0.0f;

        public bool AllEntsOfType(string EntType)
        {
            foreach (IBaseEntity ent in GlobalState.Game.Entities.Selected)
                if (!ent.CompareEntityType(EntType)) return false;

            return true;
        }
    }
}
