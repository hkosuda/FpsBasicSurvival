using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ObserverCommand : Command
    {
        static readonly List<string> availableValues = new List<string>()
    {
        "start", "end", "land"
    };

        static public bool Active { get; private set; }

        static GameObject _virtualPlayer;

        static Vector3 originalPosition;
        static Vector3 originalEulerAngle;

        public ObserverCommand(string commandName) : base(commandName)
        {
            description = "神視点モードを起動し，プレイヤーがマップ上を自由に移動する機能を提供します．";
            detail = "'observer start'で神視点モードを起動し，'observer end'で神視点モードを起動した場所まで戻ります．" +
                "また，神視点モード中にジャンプ入力を行うか 'observer land' を実行すると，任意の場所に着地できます．\n" +
                "ただし，着地すると中間地点まで戻されるような場所では，すぐに中間地点まで戻されるので注意しましょう．" +
                "また，オブジェクトの内側などでは着地に失敗する場合があります．\n" +
                "悪用はしないようにしましょう．";
        }

        public override void Update(float dt)
        {
            if (!Active) { return; }
            if (VirtualPlayer.CheckLandingNow) { return; }

            if (InputSystem.CheckInput(Keyconfig.KeybindList[KeyAction.jump], true))
            {
                var hit = SphereCastCheck();

                if (hit.collider == null)
                {
                    ChatMessageManager.SendChatMessage(TxtUtil.C("無効な着地地点です．", Clr.red));
                    return;
                }

                var landingPoint = hit.point + new Vector3(0.0f, Player.centerY + 0.1f, 0.0f);
                var virtualPlayer = GameObject.Instantiate(_virtualPlayer, landingPoint, Quaternion.identity);

                virtualPlayer.GetComponent<VirtualPlayer>().Initialize(landingPoint);
                virtualPlayer.transform.SetParent(GameHost.World.transform);
            }
        }

        public override void Initialize()
        {
            _virtualPlayer = Resources.Load<GameObject>("Object/VirtualPlayer");
        }

        public override void Shutdown()
        {
            Inactivate();
        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values == null || values.Count == 0) { return new List<string>(); }

            else if (values.Count < 3)
            {
                return availableValues;
            }

            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            if (VirtualPlayer.CheckLandingNow)
            {
                tracer.AddMessage("現在，着地点を調査中のためコマンドを実行できません", Tracer.Level.error);
            }

            else if (values.Count == 1)
            {
                ERROR_AvailableOnly(tracer, availableValues);
            }

            else if (values.Count == 2)
            {
                var value = values[1];

                if (value == "start")
                {
                    if (Active)
                    {
                        tracer.AddMessage("すでに神視点モードが起動しています．", Tracer.Level.error);
                    }

                    else
                    {
                        originalPosition = Player.Myself.transform.position;
                        originalEulerAngle = PM_Camera.EulerAngle();

                        Active = true;
                        SetPlayerPhysics(false);

                        tracer.AddMessage("神視点モードを起動しました．", Tracer.Level.normal);
                    }
                }

                else if (value == "end")
                {
                    if (Active)
                    {
                        Land(originalPosition, originalEulerAngle);
                        tracer.AddMessage("神視点モードを終了しました．", Tracer.Level.normal);
                    }

                    else
                    {
                        tracer.AddMessage("神視点モードは起動していません．", Tracer.Level.error);
                    }
                }

                else
                {
                    tracer.AddMessage("神視点モードを起動するには'observer start'を，神視点モードを終了するには'observer end'を実行してください．", Tracer.Level.error);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }

        static RaycastHit SphereCastCheck()
        {
            var origin = Player.Myself.transform.position;

            Physics.SphereCast(origin, Player.playerRadius, Vector3.down, out RaycastHit hitinfo);

            return hitinfo;
        }

        static void Inactivate()
        {
            Active = false;
            SetPlayerPhysics(true);
        }

        static public void Land(Vector3 position, Vector3 eulerAngle)
        {
            Active = false;
            Player.SetPosition(position, eulerAngle);
            SetPlayerPhysics(true);
        }

        static void SetPlayerPhysics(bool status)
        {
            if (Player.Rb != null) { Player.Rb.useGravity = status; }
            if (Player.Collider != null) { Player.Collider.enabled = status; }
        }
    }
}

