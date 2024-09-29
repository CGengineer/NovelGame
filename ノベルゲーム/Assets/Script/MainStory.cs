using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Serialization;
using UnityEngine.Scripting;

public class MainStory : MonoBehaviour
{
    public static bool OneChoiseClickFlag = false;
    public static bool TwoChoiseClickFlag = false;
    public bool LoopSetupFlag;

    /// <summary>現在のループリストのインデックス</summary>
    private int _ChildLoopSetupListIndex = 0;
    /// <summary>現在のループリストのインデックス</summary>
    private int _LoopSetupListIndex = 0;
    /// <summary>現在の文章リストのインデックス</summary>
    private int _ChildMessageListIndex = 0;
    /// <summary>現在の文章リストのインデックス</summary>
    private int _MessageListIndex = 0;
    /// <summary>画面がフェードイン中かフェードアウト中かのフラグ</summary>
    private bool _AllFadeInOutFlag = true;
    /// <summary>キャラクターのフェードアウト初回フラグ</summary>
    private bool _FirstCharacterFadeOutFlag = true;
    /// <summary>文章表示中のクリック初回フラグ</summary>
    private bool _FirstClickFlag = true;
    /// <summary>画像フェードアウト終了フラグ</summary>
    private bool _FadeFinishFlag = true;
    /// <summary>画像フェードアウト終了フラグ</summary>
    private bool _TextDisplayEnd = true;
    #region リスト設定

    [System.Serializable]
    public class LoopSetup
    {
        /// <summary>文章を画面に出てくる文章リスト</summary>
        public List<string> MessageList;
        /// <summary>画面に出てくるテキストボックスの表示非表示フラグ</summary>
        public bool TextBoxDisplayFlag;
        /// <summary>選択肢を出すか出さないかのフラグ</summary>
        public bool TextChoiceFlag;
        /// <summary>選択肢1のテキスト</summary>
        public string ChoiceTextOne;
        /// <summary>選択肢2のテキスト</summary>
        public string ChoiceTextTwo;
        public List<LoopSetup> OneChoiseChildLoopSetup;
        public List<LoopSetup> TwoChoiseChildLoopSetup;
        /// <summary>キャラクターイメージクラス</summary>
        //フィールドの古い名前を指定することで、以前のデータが新しいフィールド名にマッピングされる
        [FormerlySerializedAs("ImageClass")]

        public ImageClass ImageClass;

    }

    [System.Serializable]
    public class ImageClass
    {
        /// <summary>背景画像</summary>
        public Sprite BackBroundImage;
        ///<summary>全体フェードフラグ</summary>
        public bool AllFadeFlag;
        /// <summary>フェードインフェードアウトの速度</summary>
        public float FadeSpeed;
        /// <summary>キャラクター右画像</summary>
        public Sprite RightCharacterImage;
        /// <summary>キャラクター右画像フェードフラグ</summary>
        public bool RightCharacterFadeFlag;
        /// <summary>キャラクター中央画像</summary>
        public Sprite CenterCharacterImage;
        /// <summary>キャラクター中央画像フェードフラグ</summary>
        public bool CenterCharacterFadeFlag;
        /// <summary>キャラクター左画像</summary>
        public Sprite LeftCharacterImage;
        /// <summary>キャラクター左画像フェードフラグ</summary>
        public bool LeftCharacterFadeFlag;
    }

    // フィールドの古い名前を指定することで、以前のデータが新しいフィールド名にマッピングされる
    [FormerlySerializedAs("_LoopSetupList")]
    public List<LoopSetup> _LoopSetupList;
    #endregion

    #region 初期設定

    /// <summary>背景画像のコンポーネント </summary>
    public Image _ImageComponent_Background;
    /// <summary>キャラクター右画像のコンポーネント</summary>
    public Image _ImageComponent_RightCharacter;
    /// <summary>キャラクター右画像のコンポーネント</summary>
    public Image _ImageComponent_CenterCharacter;
    /// <summary>キャラクター左画像のコンポーネント</summary>
    public Image _ImageComponent_LeftCharacter;
    /// <summary>フェードインフェードアウト</summary>
    public Image FadeInOut;
    /// <summary>ボタンオブジェクト</summary>
    public Button _btnMessage;
    /// <summary>ボタンオブジェクト</summary>
    public Button _btnChooseOne;
    /// <summary>ボタンオブジェクト</summary>
    public Button _btnChooseTwo;
    /// <summary>選択肢1オブジェクト</summary>
    public TextMeshProUGUI _txtChooseOne;
    /// <summary>ボタンオブジェクト</summary>
    public TextMeshProUGUI _txtChooseTwo;
    /// <summary>画面に出てくるテキストボックス</summary>
    public TextMeshProUGUI Text;
    /// <summary>画面に出てくる文字の出現速度</summary>
    public float _NovelSpeed;
    /// <summary>デバッグしたい場合の画像数値</summary>
    public int _DebugCount;
    #endregion

    private void Start()
    {
        _LoopSetupListIndex = _DebugCount;
        _ChildLoopSetupListIndex = _DebugCount;

        SetObject();
    }

    void Update()
    {
        ChangeObject();

        if (_FadeFinishFlag && _TextDisplayEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Novel());
            }
        }
    }

    private void ChangeObject()
    {
        if (!LoopSetupFlag)
        {
            if (_LoopSetupList[_LoopSetupListIndex].ImageClass.AllFadeFlag)
            {
                var currentColor = FadeInOut.color;

                if (_AllFadeInOutFlag && currentColor.a < 1f)
                {
                    currentColor.a += _LoopSetupList[_LoopSetupListIndex].ImageClass.FadeSpeed;

                    _FadeFinishFlag = false;
                }
                else
                {
                    _AllFadeInOutFlag = false;

                    SetObject();

                    if (!_AllFadeInOutFlag && currentColor.a > 0f)
                    {
                        currentColor.a -= _LoopSetupList[_LoopSetupListIndex].ImageClass.FadeSpeed;
                    }
                    else
                    {
                        _FadeFinishFlag = true;
                    }
                }
                FadeInOut.color = currentColor;
            }
            else
            {
                SetObject();
            }
        }
        else
        {
            if (_LoopSetupList[_ChildLoopSetupListIndex].ImageClass.AllFadeFlag)
            {
                var currentColor = FadeInOut.color;

                if (_AllFadeInOutFlag && currentColor.a < 1f)
                {
                    currentColor.a += _LoopSetupList[_ChildLoopSetupListIndex].ImageClass.FadeSpeed;

                    _FadeFinishFlag = false;
                }
                else
                {
                    _AllFadeInOutFlag = false;

                    SetObject();

                    if (!_AllFadeInOutFlag && currentColor.a > 0f)
                    {
                        currentColor.a -= _LoopSetupList[_ChildLoopSetupListIndex].ImageClass.FadeSpeed;
                    }
                    else
                    {
                        _FadeFinishFlag = true;
                    }
                }
                FadeInOut.color = currentColor;
            }
            else
            {
                SetObject();
            }
        }

    }

    private List<LoopSetup> GetLoopSetupList(List<LoopSetup> loopSetupsList)
    {

        foreach (var loopSetup in loopSetupsList)
        {
            if (loopSetup.TextChoiceFlag)
            {
                if (OneChoiseClickFlag)
                {
                    LoopSetupFlag = true;
                    _ChildLoopSetupListIndex = 0;
                    _ChildMessageListIndex = 0;
                    OneChoiseClickFlag = false;
                    return GetLoopSetupList(loopSetup.OneChoiseChildLoopSetup);
                }

                if (TwoChoiseClickFlag)
                {
                    LoopSetupFlag = true;
                    _ChildLoopSetupListIndex = 0;
                    _ChildMessageListIndex = 0;
                    TwoChoiseClickFlag = false;
                    return GetLoopSetupList(loopSetup.TwoChoiseChildLoopSetup);
                }

            }
        }
        return loopSetupsList;
    }

    private IEnumerator Novel()
    {
        _LoopSetupList = GetLoopSetupList(_LoopSetupList);

        if (LoopSetupFlag)
        {
            if (_LoopSetupList[_ChildLoopSetupListIndex].MessageList.Count > 0)
            {
                var messageCount = 0;
                var currentMessage = _LoopSetupList[_ChildLoopSetupListIndex].MessageList[_ChildMessageListIndex];
                Text.text = "";

                if (_FirstClickFlag)
                {
                    _FirstClickFlag = false;

                    while (currentMessage.Length > Text.text.Length)
                    {
                        Text.text += currentMessage[messageCount];
                        messageCount++;
                        yield return new WaitForSeconds(_NovelSpeed);
                    }
                    //ループしている最中に、クリックして新しい処理を起こすとダブルで処理が走る。これは覚えておこう。
                    ChildSetRestart(_LoopSetupList);
                }
                else
                {
                    Text.text = currentMessage;
                    _TextDisplayEnd = false;
                    yield return new WaitForSeconds(0.25f);
                    _TextDisplayEnd = true;
                }


            }
            else
            {
                _ChildLoopSetupListIndex++;
                _AllFadeInOutFlag = true;
            }
        }
        else
        {
            if (_LoopSetupList[_LoopSetupListIndex].MessageList.Count > 0)
            {
                var messageCount = 0;
                var currentMessage = _LoopSetupList[_LoopSetupListIndex].MessageList[_MessageListIndex];
                Text.text = "";

                if (_FirstClickFlag)
                {
                    _FirstClickFlag = false;

                    while (currentMessage.Length > Text.text.Length)
                    {
                        Text.text += currentMessage[messageCount];
                        messageCount++;
                        yield return new WaitForSeconds(_NovelSpeed);
                    }
                    //ループしている最中に、クリックして新しい処理を起こすとダブルで処理が走る。これは覚えておこう。
                    SetRestart();
                }
                else
                {
                    Text.text = currentMessage;
                    _TextDisplayEnd = false;
                    yield return new WaitForSeconds(0.25f);
                    _TextDisplayEnd = true;
                }
            }
            else
            {
                _LoopSetupListIndex++;
                _AllFadeInOutFlag = true;
            }
        }
    }

    private void SetRestart()
    {
        _FirstClickFlag = true;

        _MessageListIndex++;

        if (_LoopSetupList[_LoopSetupListIndex].MessageList.Count == _MessageListIndex)
        {
            _LoopSetupListIndex++;
            _MessageListIndex = 0;
            _AllFadeInOutFlag = true;
            Text.text = "";
        }
    }

    private void ChildSetRestart(List<LoopSetup> loopSetupsList)
    {
        _FirstClickFlag = true;

        _ChildMessageListIndex++;

        if (_LoopSetupList[_ChildLoopSetupListIndex].MessageList.Count == _ChildMessageListIndex)
        {
            _ChildLoopSetupListIndex++;
            _ChildMessageListIndex = 0;
            _AllFadeInOutFlag = true;
            Text.text = "";
        }
    }

    private void SetObject()
    {
        if (!LoopSetupFlag)
        {
            if (_LoopSetupList[_LoopSetupListIndex].TextBoxDisplayFlag)
            {
                _btnMessage.gameObject.SetActive(true);
            }
            else
            {
                _btnMessage.gameObject.SetActive(false);
            }

            if (_LoopSetupList[_LoopSetupListIndex].TextChoiceFlag)
            {
                _btnChooseOne.gameObject.SetActive(true);
                _txtChooseOne.text = _LoopSetupList[_LoopSetupListIndex].ChoiceTextOne;
                _btnChooseTwo.gameObject.SetActive(true);
                _txtChooseTwo.text = _LoopSetupList[_LoopSetupListIndex].ChoiceTextTwo;
            }
            else
            {
                _btnChooseOne.gameObject.SetActive(false);
                _txtChooseOne.text = "";
                _btnChooseTwo.gameObject.SetActive(false);
                _txtChooseTwo.text = "";
            }

            var imageClass = _LoopSetupList[_LoopSetupListIndex].ImageClass;

            _ImageComponent_Background.sprite = imageClass.BackBroundImage;

            _ImageComponent_RightCharacter.sprite = imageClass.RightCharacterImage;
            CharacterImageFadeOut(_ImageComponent_RightCharacter, imageClass.RightCharacterFadeFlag);

            _ImageComponent_CenterCharacter.sprite = imageClass.CenterCharacterImage;
            CharacterImageFadeOut(_ImageComponent_CenterCharacter, imageClass.CenterCharacterFadeFlag);

            _ImageComponent_LeftCharacter.sprite = imageClass.LeftCharacterImage;
            CharacterImageFadeOut(_ImageComponent_LeftCharacter, imageClass.LeftCharacterFadeFlag);
        }
        else
        {
            if (_LoopSetupList[_ChildLoopSetupListIndex].TextBoxDisplayFlag)
            {
                _btnMessage.gameObject.SetActive(true);
            }
            else
            {
                _btnMessage.gameObject.SetActive(false);
            }

            if (_LoopSetupList[_ChildLoopSetupListIndex].TextChoiceFlag)
            {
                _btnChooseOne.gameObject.SetActive(true);
                _txtChooseOne.text = _LoopSetupList[_ChildLoopSetupListIndex].ChoiceTextOne;
                _btnChooseTwo.gameObject.SetActive(true);
                _txtChooseTwo.text = _LoopSetupList[_ChildLoopSetupListIndex].ChoiceTextTwo;
            }
            else
            {
                _btnChooseOne.gameObject.SetActive(false);
                _txtChooseOne.text = "";
                _btnChooseTwo.gameObject.SetActive(false);
                _txtChooseTwo.text = "";
            }

            var imageClass = _LoopSetupList[_ChildLoopSetupListIndex].ImageClass;

            _ImageComponent_Background.sprite = imageClass.BackBroundImage;

            _ImageComponent_RightCharacter.sprite = imageClass.RightCharacterImage;
            CharacterImageFadeOut(_ImageComponent_RightCharacter, imageClass.RightCharacterFadeFlag);

            _ImageComponent_CenterCharacter.sprite = imageClass.CenterCharacterImage;
            CharacterImageFadeOut(_ImageComponent_CenterCharacter, imageClass.CenterCharacterFadeFlag);

            _ImageComponent_LeftCharacter.sprite = imageClass.LeftCharacterImage;
            CharacterImageFadeOut(_ImageComponent_LeftCharacter, imageClass.LeftCharacterFadeFlag);
        }

    }
    private void CharacterImageFadeOut(Image image,bool fadeFlag)
    {
        if (fadeFlag)
        {
            if (_FirstCharacterFadeOutFlag)
            {
                ResetAlpha(image);
                _FirstCharacterFadeOutFlag = false;
            }

            var currentColor = image.color;

            if (_AllFadeInOutFlag && currentColor.a < 1f)
            {
                currentColor.a += 0.01f;
                _FadeFinishFlag = false;
            }
            else
            {
                _FadeFinishFlag = true;
            }
            image.color = currentColor;
        }
    }

    private void ResetAlpha(Image image)
    {
        var currentColor = image.color;
        currentColor.a = 0f;
        image.color = currentColor;
    }
}