using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class Shop_InfoButton : MonoBehaviour
    {
        static readonly string description = "" +
            "　マネーを使用して，回復やステータスの強化を行うことができます．" +
            "敵はラウンド毎に強化されていくため，積極的にステータスの強化を行いましょう．\n" +
            "　購入を終了し次のラウンドに進むには，右下の「Next Round」を押してください．\n" +
            "【Tips】\n" +
            "・「体力」と「アーマー」の初期価格は，ラウンド数に応じて上昇します．" +
            "それ以外のアイテムはこれまでの購入数に応じて上昇します．\n" +
            "・購入を続けていくと，ひとつ購入するにも大量のマネーが必要になってきます．" +
            "そのため，「獲得マネー倍率増加」を購入してラウンド中に得られるマネーを増やすことも検討しましょう．\n" +
            "・残したマネーは" + (1.0f + SvParams.Get(SvParam.money_increase_after_round)).ToString() + 
            "倍にして次のラウンドに持ちこすことができます．";

        private void Start()
        {
            var button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(ShowDescription);

            ShowDescription();
        }

        static public void ShowDescription()
        {
            Shop_Description.ShowDescription("ショップについて", description);
        }
    }
}

