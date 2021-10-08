using Chrio.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chrio.UI
{
    public class ControlDisplay : UIBehaviour
    {
        [Header("Components")]
        public Slider ControlBar;
        public TextMeshProUGUI Label;

        // Update is called once per frame
        void FixedUpdate()
        {
            int _playerOwned = 0;
            int _enemyOwned = 0;

            foreach(IBaseEntity ent in GlobalState.Game.Entities.WorldEntities.Values)
                if (ent.GetOwnerID() == 0) _playerOwned++; else if (ent.GetOwnerID() == 1) _enemyOwned++;

            ControlBar.maxValue = _playerOwned + _enemyOwned;
            ControlBar.value = _enemyOwned;

            Label.text = $"Control\nMoney: {GlobalState.Game.Money[0]}";
        }
    }
}
