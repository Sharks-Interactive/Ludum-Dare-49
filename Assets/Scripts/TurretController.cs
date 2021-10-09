using Chrio.Entities;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using static SharkUtils.ExtraFunctions;
using UnityEngine.UI;
using SharkUtils;

namespace Chrio.World
{
    public class TurretController : SharksBehaviour
    {
        private IBaseEntity AttachedEntity;
        public IBaseEntity Target;
        public float TurnSpeed = 10;
        public WeaponInfo Info;

        [Tooltip("Continous/Continous Pulsed")]
        public LineRenderer Laser;
        
        private Color[] LaserColor = new Color[2];
        private Color2 _clearColor = new Color2(Color.clear, Color.clear);
        private DamageInfo _weaponDmg;
        public Image imageRenderer;
        private AudioSource _audio;

        private Vector3 _offset;

        // Start is called before the first frame update
        public override void OnLoad(Game_State.State _gameState, Loading.ILoadableObject.CallBack _callback)
        {
            imageRenderer = GetComponent<Image>();
            AttachedEntity = transform.parent.GetComponent<IBaseEntity>();
            Spaceship ship = AttachedEntity as Spaceship;
            _audio = GetComponent<AudioSource>();
            imageRenderer.sprite = ship.Data.TurretSprite;

            Laser = GetComponent<LineRenderer>();
            _offset = RandomPointInsideCircle(Vector3.zero, 0.5f);
            _offset.y = Random.Range(-1.0f, 1.0f);

            // Setup beam color based on teamcolor from weaponinfo
            LaserColor[0] = Info.Colors[AttachedEntity.GetOwnerID()];
            LaserColor[1] = Info.Colors[AttachedEntity.GetOwnerID()];
            LaserColor[1].a = 35;

            _weaponDmg = new DamageInfo(
                    Info.Damage,
                    Info.DmgType,
                    AttachedEntity,
                    Target
                );

            switch (Info.Type)
            {
                case WeaponInfo.WeaponType.ContinousPulse:
                    StartCoroutine(FireContinousPulseWeapon());
                    break;
                case WeaponInfo.WeaponType.Pulse:
                    StartCoroutine(FirePulseWeapon());
                    break;
            }

            base.OnLoad(_gameState, _callback);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Turn to facce target
            if (Target == null) { Laser.enabled = false; GlobalState.Game.Entities.Visible.RemoveIfContains(AttachedEntity); return; }
            if (!Target.GetGameObject().activeInHierarchy) { Target = null; GlobalState.Game.Entities.Visible.RemoveIfContains(AttachedEntity);  return; }
            _weaponDmg = new DamageInfo(
                    Info.Damage,
                    Info.DmgType,
                    AttachedEntity,
                    Target
                );
            transform.up += ((Target.GetGameObject().transform.position - transform.position) / TurnSpeed) * Time.deltaTime * 50;

            if (Info.Type == WeaponInfo.WeaponType.ContinousPulse) UpdateLaserPos(Target.GetGameObject().transform.position + _offset); // Randomize laser position
            if (Info.Type != WeaponInfo.WeaponType.Continous) return;

            // Continous damage
            UpdateLaserPos(Target.GetGameObject().transform.position);
            if (imageRenderer.rectTransform.IsFullyVisibleFrom(GlobalState.Game.MainCamera))
            {
                GlobalState.Game.Shake.Shake(0.1f, 0.05f - ((70 - GlobalState.Game.MainCamera.orthographicSize) / 700.0f));
                _audio.pitch = RandomFromRange(new Vector2(0.5f, 1.5f));
                if (Time.time % 30 == 0) _audio.PlayOneShot(Resources.Load<AudioClip>("Sound/LaserContinous"));
                GlobalState.Game.Entities.Visible.AddIfNotContains(AttachedEntity);
            }
            else try { GlobalState.Game.Entities.Visible.RemoveIfContains(AttachedEntity); } catch { };
            Laser.startColor = LaserColor[0];
            Laser.endColor = LaserColor[1];
            
            _weaponDmg.Amount = (_weaponDmg.Amount / 60);
            Target.OnDamaged(_weaponDmg);
        }

        private IEnumerator FireContinousPulseWeapon ()
        {
            if (Target == null)
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(FireContinousPulseWeapon());
                yield break;
            }
            yield return new WaitForSeconds(Info.Cooldown);
            if (Target == null) // EDGE CASE! Target is nullified in the cooldown time
            {
                yield return new WaitForSeconds(1);
                StartCoroutine(FireContinousPulseWeapon());
                yield break;
            }

            // Show the laser
            Laser.startColor = LaserColor[0];
            Laser.endColor = LaserColor[1];


            if (imageRenderer.rectTransform.IsFullyVisibleFrom(GlobalState.Game.MainCamera))
            {
                _audio.pitch = RandomFromRange(new Vector2(1, 1.5f));
                _audio.PlayOneShot(Resources.Load<AudioClip>("Sound/Laser"));
                GlobalState.Game.Shake.Shake(0.1f, 0.1f - ((70 - GlobalState.Game.MainCamera.orthographicSize) / 700.0f));
                GlobalState.Game.Entities.Visible.AddIfNotContains(AttachedEntity);
            }
            else try { GlobalState.Game.Entities.Visible.RemoveIfContains(AttachedEntity); } catch { };

            // Fade it out as part of the "pulse"
            Laser.DOColor(LaserColor.GetColor2(), _clearColor, Info.Cooldown / 2);
            Target.OnDamaged(_weaponDmg); // Damage the Target

            StartCoroutine(FireContinousPulseWeapon());
        }

        private IEnumerator FirePulseWeapon ()
        {
            yield return null;
        }

        private void UpdateLaserPos(Vector3 _target)
        {
            Laser.enabled = true;
            Laser.SetPosition(0, transform.position);
            Laser.SetPosition(1, _target);
        }
    }
}
