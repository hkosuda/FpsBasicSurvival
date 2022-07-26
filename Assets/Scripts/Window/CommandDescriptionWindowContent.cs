using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class CommandDescriptionWindowContent : MonoBehaviour
    {
        static readonly int titileFontSize = 14;
        static readonly int descriptionFontSize = 12;

        static readonly int titlePadding = 2;
        static readonly int descriptionPadding = 14;
        static readonly int detailPadding = 26;

        static GameObject _content;
        static GameObject myself;

        private void Awake()
        {
            if (_content == null) { _content = Resources.Load<GameObject>("UiComponent/CommandDescriptionContent"); }

            myself = gameObject;
        }

        void Start()
        {
            BeginDeploy();
        }

        static void BeginDeploy()
        {
            foreach(var command in CommandReceiver.CommandList)
            {
                var content1 = GameObject.Instantiate(_content);
                content1.transform.SetParent(myself.transform);

                var text1 = content1.GetComponent<TextMeshProUGUI>();
                text1.margin = new Vector4(titlePadding, 0, 0, 0);
                text1.text = TxtUtil.C("Åy" + command.commandName + "Åz", Clr.lime);
                text1.fontSize = titileFontSize;

                var content2 = GameObject.Instantiate(_content);
                content2.transform.SetParent(myself.transform);

                var text2 = content2.GetComponent<TextMeshProUGUI>();
                text2.margin = new Vector4(descriptionPadding, 0, 0, 0);
                text2.fontSize = descriptionFontSize;

                var description = "ÅyäTóvÅz\n";
                description += command.description;

                text2.text = description;

                var content3 = GameObject.Instantiate(_content);
                content3.transform.SetParent(myself.transform);

                var text3 = content3.GetComponent<TextMeshProUGUI>();
                text3.margin = new Vector4(detailPadding, 0, 0, 0);
                text3.fontSize = descriptionFontSize;

                var detail = "Åyè⁄ç◊Åz\n";
                detail += command.detail;

                text3.text = detail;
            }
        }
    }
}

