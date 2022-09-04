using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class HistoryCommand : Command
    {
        public HistoryCommand(string commandName) : base(commandName)
        {
            description = "バージョンの更新履歴を確認することができます．";
            detail = "";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values.Count == 1)
            {
                AddHistoryInfo(tracer);
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }

        static void AddHistoryInfo(Tracer tracer)
        {
            tracer.AddMessage("ver 01.00", Tracer.Level.emphasis);
            tracer.AddMessage(v0100, Tracer.Level.normal, 2);

            tracer.AddMessage("ver 01.01", Tracer.Level.emphasis);
            tracer.AddMessage(v0101, Tracer.Level.normal, 2);

            tracer.AddMessage("ver 01.02", Tracer.Level.emphasis);
            tracer.AddMessage(v0102, Tracer.Level.normal, 2);

            tracer.AddMessage("ver 01.03", Tracer.Level.emphasis);
            tracer.AddMessage(v0103, Tracer.Level.normal, 2);

            tracer.AddMessage("ver 01.04", Tracer.Level.emphasis);
            tracer.AddMessage(v0104, Tracer.Level.normal, 2);

            tracer.AddMessage("ver 01.05", Tracer.Level.emphasis);
            tracer.AddMessage(v0105, Tracer.Level.normal, 2);

            tracer.AddMessage("ver 01.06 (latest)", Tracer.Level.emphasis);
            tracer.AddMessage(v0106, Tracer.Level.normal, 2);
        }

        static readonly string v0100 = "" +
            "(2022/7/28)\n" +
            "'FPS_Basic サバイバル'をリリース";
        static readonly string v0101 = "" +
            "(2022/8/1)\n" +
            "・Historyコマンドを追加．\n" +
            "・Clearコマンドを追加\n" +
            "・HPもしくはアーマーの上限を引き上げるアップグレードを複数選択したうえで，HPもしくはアーマーの回復を複数選択したのち，先述の上限を引き上げるアップグレードの個数を減らしたとき，回復の個数が余分となっても更新されないバグを修正．\n" +
            "・アップグレード購入ウィンドウのレイアウトを変更．\n" +
            "・アップグレードを購入すると，該当するアップグレードのテキストの色が変わるように変更．\n" +
            "・視点移動のメカニズムを修正．\n" +
            "・ヘルプを表示し，キーバインドを確認できるように．\n" +
            "・サバイバルモードの，最初のラウンドのデザインを一部変更．\n" +
            "・コンソールの説明欄のフォントをより小さいサイズに変更．\n" +
            "・サバイバルモードの壁の色を変更．";
        static readonly string v0102 = "" +
            "(2022/8/2)\n" +
            "・サバイバルモードの最初のラウンドのデザインを大幅に変更し，surfマップやbhopマップにアクセスしやすくしました．\n" +
            "・「遊び方」についての説明を修正しました．";
        static readonly string v0103 = "" +
            "(2022/8/3)\n" +
            "・前進，後退，右に移動，左に移動のキーを変更できない問題を修正．\n" +
            "・プレイヤーが射撃したときに表示される弾の弾速を上昇．";
        static readonly string v0104 = "" +
            "(2022/8/5)\n" +
            "・ナイフのカラーを追加（追加されたカラー：ホワイト，ブラックの2色）．\n" +
            "・サバイバルモードの最初のラウンドのデザインを修正．\n" +
            "・surfマップやbhopマップで使用できる，「pm_max_speed_in_air」コマンドを追加．ストレイフ時の加速を任意に調整できるように．\n" +
            "・アップグレードの説明を追加．" +
            "・ラグドールの追加．";
        static readonly string v0105 = "" +
            "(2022/8/6)\n" +
            "・historyに更新時の日付の表記を追加．\n" +
            "・ヘルプの表示，非表示を切り替えるキーを\"Ctrl+H\"から\"Alt+H\"に変更\n" +
            "（\"Ctrl+H\"はChromeの場合，履歴を開くショートカットに指定されていました．確認不足で申し訳ございません）．" +
            "・デフォルトで使えるコマンドから，Demoコマンドを削除しました．";
        static readonly string v0106 = "" +
            "(2022/9/4)\n" +
            "・chainコマンドを追加．比較的長めのマップでも途中セーブとロードが可能になります．\n" +
            "・ez_towerにデモ再生用のボタンを配置．\n" +
            "・bhop_settingsコマンドを追加．bhop用のおすすめ設定を即座に適用することができます．";
    }
}

