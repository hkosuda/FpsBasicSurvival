using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class Shop_InfoButton : MonoBehaviour
    {
        static readonly string description = "" +
            "�@�}�l�[���g�p���āC�񕜂�X�e�[�^�X�̋������s�����Ƃ��ł��܂��D" +
            "�G�̓��E���h���ɋ�������Ă������߁C�ϋɓI�ɃX�e�[�^�X�̋������s���܂��傤�D\n" +
            "�@�w�����I�������̃��E���h�ɐi�ނɂ́C�E���́uNext Round�v�������Ă��������D\n" +
            "�yTips�z\n" +
            "�E�u�̗́v�Ɓu�A�[�}�[�v�̏������i�́C���E���h���ɉ����ď㏸���܂��D" +
            "����ȊO�̃A�C�e���͂���܂ł̍w�����ɉ����ď㏸���܂��D\n" +
            "�E�w���𑱂��Ă����ƁC�ЂƂw������ɂ���ʂ̃}�l�[���K�v�ɂȂ��Ă��܂��D" +
            "���̂��߁C�u�l���}�l�[�{�������v���w�����ă��E���h���ɓ�����}�l�[�𑝂₷���Ƃ��������܂��傤�D\n" +
            "�E�c�����}�l�[��" + (1.0f + SvParams.Get(SvParam.money_increase_after_round)).ToString() + 
            "�{�ɂ��Ď��̃��E���h�Ɏ����������Ƃ��ł��܂��D";

        private void Start()
        {
            var button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(ShowDescription);

            ShowDescription();
        }

        static public void ShowDescription()
        {
            Shop_Description.ShowDescription("�V���b�v�ɂ���", description);
        }
    }
}

