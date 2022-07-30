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
            description = "�_���_���[�h���N�����C�v���C���[���}�b�v������R�Ɉړ�����@�\��񋟂��܂��D";
            detail = "'observer start'�Ő_���_���[�h���N�����C'observer end'�Ő_���_���[�h���N�������ꏊ�܂Ŗ߂�܂��D" +
                "�܂��C�_���_���[�h���ɃW�����v���͂��s���� 'observer land' �����s����ƁC�C�ӂ̏ꏊ�ɒ��n�ł��܂��D\n" +
                "�������C���n����ƒ��Ԓn�_�܂Ŗ߂����悤�ȏꏊ�ł́C�����ɒ��Ԓn�_�܂Ŗ߂����̂Œ��ӂ��܂��傤�D" +
                "�܂��C�I�u�W�F�N�g�̓����Ȃǂł͒��n�Ɏ��s����ꍇ������܂��D\n" +
                "���p�͂��Ȃ��悤�ɂ��܂��傤�D";
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
                    ChatMessageManager.SendChatMessage(TxtUtil.C("�����Ȓ��n�n�_�ł��D", Clr.red));
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
                tracer.AddMessage("���݁C���n�_�𒲍����̂��߃R�}���h�����s�ł��܂���", Tracer.Level.error);
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
                        tracer.AddMessage("���łɐ_���_���[�h���N�����Ă��܂��D", Tracer.Level.error);
                    }

                    else
                    {
                        originalPosition = Player.Myself.transform.position;
                        originalEulerAngle = PM_Camera.EulerAngle();

                        Active = true;
                        SetPlayerPhysics(false);

                        tracer.AddMessage("�_���_���[�h���N�����܂����D", Tracer.Level.normal);
                    }
                }

                else if (value == "end")
                {
                    if (Active)
                    {
                        Land(originalPosition, originalEulerAngle);
                        tracer.AddMessage("�_���_���[�h���I�����܂����D", Tracer.Level.normal);
                    }

                    else
                    {
                        tracer.AddMessage("�_���_���[�h�͋N�����Ă��܂���D", Tracer.Level.error);
                    }
                }

                else
                {
                    tracer.AddMessage("�_���_���[�h���N������ɂ�'observer start'���C�_���_���[�h���I������ɂ�'observer end'�����s���Ă��������D", Tracer.Level.error);
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

