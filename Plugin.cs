using BepInEx;
using UnityEngine;
using Misoge.EventValues.Implements;
using EggRaiders.Player.Movement;
using EggRaiders.Player.Status;
using HarmonyLib;

namespace CheatMod
{
    [BepInPlugin("yukikaco.CheatMod", "Cheat Mod By Yuki.kaco", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private bool menu = false;
        private bool stamina = false;
        private bool oxygen = false;
        private bool health = false;
        private bool speed = false;

        private void Awake()
        {
            Debug.Log("Cheat Mod By Yuki.kaco");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ToggleCheatMenu();
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                // Toggle ultimate abilities
                UltimateHealth();
                UltimateOxygen();
                UltimateStamina();
                ModifySpeed();
            }
        }

        private void ModifySpeed()
        {
            GameObject playerMovementObj = GameObject.Find("MovementSystem/PlayerMovement");
            if (playerMovementObj != null)
            {
                var playerMovement = playerMovementObj.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    // Toggle speed between normal and boosted values
                    playerMovement.BaseSpeed = speed ? new Vector3(2f, 2f, 2f) : new Vector3(20f, 15f, 20f);
                    speed = !speed;

                    Debug.Log(speed ? "Speed boosted." : "Speed set to normal.");
                }
                else
                {
                    Debug.LogWarning("PlayerMovement component not found on the object.");
                }
            }
            else
            {
                Debug.LogWarning("PlayerMovement object not found in the scene.");
            }
        }

        private void UltimateHealth()
        {
            GameObject oxygenHealthConsumer = GameObject.Find("PlayerStatus/Health/OxygenHealthConsumer");
            if (oxygenHealthConsumer != null)
            {
                oxygenHealthConsumer.SetActive(!health);
                health = !health;
                Debug.Log(health ? "Ultimate Health enabled." : "Ultimate Health disabled.");
            }
            else
            {
                Debug.LogWarning("OxygenHealthConsumer object not found in the scene.");
            }
        }

        private void UltimateOxygen()
        {
            GameObject oxygenConsumer = GameObject.Find("PlayerStatus/Oxygen/OxygenConsumer");
            if (oxygenConsumer != null)
            {
                oxygenConsumer.SetActive(!oxygen);
                oxygen = !oxygen;
                Debug.Log(oxygen ? "Ultimate Oxygen enabled." : "Ultimate Oxygen disabled.");
            }
            else
            {
                Debug.LogWarning("OxygenConsumer object not found in the scene.");
            }
        }

        private void UltimateStamina()
        {
            // Attempt to find the PlayerStaminaController directly from the Player GameObject
            GameObject playerObj = GameObject.Find("Player"); // Adjust this line to the correct player object name
            if (playerObj != null)
            {
                PlayerStaminaController playerStaminaController = playerObj.GetComponent<PlayerStaminaController>();
                if (playerStaminaController != null)
                {
                    // Toggle stamina consumption values
                    if (!stamina)
                    {
                        Traverse.Create(playerStaminaController).Property("JumpConsumption").SetValue(0f);
                        Traverse.Create(playerStaminaController).Property("SprintConsumption").SetValue(0f);
                        Traverse.Create(playerStaminaController).Property("ConsumptionFactor").SetValue(0f);
                        Traverse.Create(playerStaminaController).Property("ConsumptionDefaultFactor").SetValue(0f);
                    }
                    else
                    {
                        Traverse.Create(playerStaminaController).Property("JumpConsumption").SetValue(5f);
                        Traverse.Create(playerStaminaController).Property("SprintConsumption").SetValue(10f);
                        Traverse.Create(playerStaminaController).Property("ConsumptionFactor").SetValue(1f);
                        Traverse.Create(playerStaminaController).Property("ConsumptionDefaultFactor").SetValue(1f);
                    }

                    stamina = !stamina;
                    Debug.Log(stamina ? "Ultimate Stamina enabled." : "Ultimate Stamina disabled.");
                }
                else
                {
                    Debug.LogWarning("PlayerStaminaController component not found on the player object.");
                }
            }
            else
            {
                Debug.LogWarning("Player object not found in the scene.");
            }
        }

        private void ToggleCheatMenu()
        {
            GameObject toggleObj = GameObject.Find("CheatCanvas/Toggle_Type01");
            if (toggleObj != null)
            {
                var booleanEvent = toggleObj.GetComponent<BooleanEvent>();
                if (booleanEvent != null)
                {
                    booleanEvent.Toggle();
                    menu = !menu;

                    // Show or hide the cursor based on the cheat menu state
                    Cursor.visible = menu;
                    Cursor.lockState = menu ? CursorLockMode.None : CursorLockMode.Locked;

                    Debug.Log(menu ? "Cheat menu enabled." : "Cheat menu disabled.");
                }
                else
                {
                    Debug.LogWarning("BooleanEvent component not found on the toggle object.");
                }
            }
            else
            {
                Debug.LogWarning("Toggle object not found in the scene.");
            }
        }
    }
}
