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
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class MainStory : MonoBehaviour
{
    public static bool OneChoiseClickFlag = false;
    public static bool TwoChoiseClickFlag = false;
    private bool ChoiseClickNotProcessFlag = false;

    public AudioSource VoiceAudioSource;// AudioSourceコンポーネント
    public AudioSource BGMAudioSource;// AudioSourceコンポーネント

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
        public string Memo;
        /// <summary>文章を画面に出てくる文章リスト</summary>
        public List<string> MessageList;
        public List<AudioClip> VoiceClips;         // セリフごとのAudioClip配列
        /// <summary>画面に出てくるテキストボックスの表示非表示フラグ</summary>
        /// <summary>選択肢1を押した後に移動する要素</summary>
        public int DefaultSkipElementIndex;
        public bool TextBoxDisplayFlag;
        /// <summary>選択肢を出すか出さないかのsフラグ</summary>
        public bool TextChoiceFlag;
        /// <summary>選択肢1のテキスト</summary>
        public string ChoiceTextOne;
        /// <summary>選択肢2のテキスト</summary>
        public string ChoiceTextTwo;
        /// <summary>選択肢1を押した後に移動する要素</summary>
        public int ChoiceOneSkipElementIndex;
        /// <summary>選択肢1を押した後に移動する要素</summary>
        public int ChoiceTwoSkipElementIndex;
        public AudioClip BGMClips;         // セリフごとのAudioClip配列
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

    private int _SceneNameNumber;


    private void Start()
    {
        _LoopSetupListIndex = _DebugCount;
        _ChildLoopSetupListIndex = _DebugCount;

        SetObject();
        // 現在のシーンを取得
        _SceneNameNumber = int.Parse(SceneManager.GetActiveScene().name[0].ToString());
        BGMClip(_LoopSetupListIndex);
    }

    void Update()
    {
        if (_FadeFinishFlag && _TextDisplayEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {

                switch (_SceneNameNumber)
                {
                    case 2:
                        if (_LoopSetupList.Count == _LoopSetupListIndex + 1 && _LoopSetupList[_LoopSetupListIndex].MessageList.Count == _MessageListIndex + 1)
                        {
                            FadeManager.Instance.LoadScene("3_HomeScene", 1f);
                            return;
                        }
                        break;
                    case 3:
                        if (_LoopSetupList.Count == _LoopSetupListIndex + 1 && _LoopSetupList[_LoopSetupListIndex].MessageList.Count == _MessageListIndex + 1)
                        {

                            FadeManager.Instance.LoadScene("4_SpiritWorld", 1f);
                            return;
                        }
                        break;
                    default:
                        break;
                }

                StartCoroutine(Novel());
            }
            else
            {
                if (OneChoiseClickFlag || TwoChoiseClickFlag)
                {
                    StartCoroutine(Novel());
                }
            }
        }
        if (!ChoiseClickNotProcessFlag)
        {
            ChangeObject();
        }
        else
        {
            SetObject();
        }

    }

    private void ChangeObject()
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

    private IEnumerator Novel()
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
                PlayVoice(_MessageListIndex);
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
            Set_LoopSetupListIndex();
            _AllFadeInOutFlag = true;
        }

    }

    private void SetRestart()
    {
        _FirstClickFlag = true;

        _MessageListIndex++;

        if (_LoopSetupList[_LoopSetupListIndex].MessageList.Count == _MessageListIndex)
        {
            Set_LoopSetupListIndex();
            _MessageListIndex = 0;
            _AllFadeInOutFlag = true;
            Text.text = "";
        }
    }


    public void Set_LoopSetupListIndex()
    {
        if (_LoopSetupList[_LoopSetupListIndex].ChoiceOneSkipElementIndex >= 1 || _LoopSetupList[_LoopSetupListIndex].ChoiceTwoSkipElementIndex >= 1)
        {
            if (OneChoiseClickFlag)
            {
                _LoopSetupListIndex = (int)_LoopSetupList[_LoopSetupListIndex].ChoiceOneSkipElementIndex;
                OneChoiseClickFlag = false;
                ChoiseClickNotProcessFlag = false;
                BGMClip(_LoopSetupListIndex);
            }
            else if (TwoChoiseClickFlag)
            {
                _LoopSetupListIndex = (int)_LoopSetupList[_LoopSetupListIndex].ChoiceTwoSkipElementIndex;
                TwoChoiseClickFlag = false;
                ChoiseClickNotProcessFlag = false;
                BGMClip(_LoopSetupListIndex);
            }
            else if (_LoopSetupList[_LoopSetupListIndex].DefaultSkipElementIndex >= 1)
            {
                _LoopSetupListIndex = (int)_LoopSetupList[_LoopSetupListIndex].DefaultSkipElementIndex;
                BGMClip(_LoopSetupListIndex);
            }
            else
            {
                ChoiseClickNotProcessFlag = true;
            }
        }
        else
        {
            if (_LoopSetupList[_LoopSetupListIndex].DefaultSkipElementIndex >= 1)
            {
                _LoopSetupListIndex = (int)_LoopSetupList[_LoopSetupListIndex].DefaultSkipElementIndex;
            }
            else
            {
                _LoopSetupListIndex++;
            }
            BGMClip(_LoopSetupListIndex);
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
    private void CharacterImageFadeOut(Image image, bool fadeFlag)
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

    public void PlayVoice(int lineIndex)
    {
        if (lineIndex < _LoopSetupList[_LoopSetupListIndex].VoiceClips.Count && _LoopSetupList[_LoopSetupListIndex].VoiceClips[_MessageListIndex] != null)
        {
            VoiceAudioSource.clip = _LoopSetupList[_LoopSetupListIndex].VoiceClips[_MessageListIndex];
            VoiceAudioSource.Play();
        }
    }

    private void BGMClip(int lineIndex)
    {
        if (_LoopSetupList[_LoopSetupListIndex].BGMClips != null)
        {
            if (BGMAudioSource.clip == null)
            {
                BGMAudioSource.clip = _LoopSetupList[_LoopSetupListIndex].BGMClips;
                BGMAudioSource.Play();
            }
            else if (_LoopSetupListIndex >= 1)
            {
                if (_LoopSetupList[_LoopSetupListIndex - 1].BGMClips != _LoopSetupList[_LoopSetupListIndex].BGMClips)
                {
                    BGMAudioSource.clip = _LoopSetupList[_LoopSetupListIndex].BGMClips;
                    BGMAudioSource.Play();
                }
            }



        }
    }

    private void ResetAlpha(Image image)
    {
        var currentColor = image.color;
        currentColor.a = 0f;
        image.color = currentColor;
    }
}