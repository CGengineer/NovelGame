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

    public AudioSource VoiceAudioSource;// AudioSource�R���|�[�l���g
    public AudioSource BGMAudioSource;// AudioSource�R���|�[�l���g

    /// <summary>���݂̃��[�v���X�g�̃C���f�b�N�X</summary>
    private int _ChildLoopSetupListIndex = 0;
    /// <summary>���݂̃��[�v���X�g�̃C���f�b�N�X</summary>
    private int _LoopSetupListIndex = 0;
    /// <summary>���݂̕��̓��X�g�̃C���f�b�N�X</summary>
    private int _ChildMessageListIndex = 0;
    /// <summary>���݂̕��̓��X�g�̃C���f�b�N�X</summary>
    private int _MessageListIndex = 0;
    /// <summary>��ʂ��t�F�[�h�C�������t�F�[�h�A�E�g�����̃t���O</summary>
    private bool _AllFadeInOutFlag = true;
    /// <summary>�L�����N�^�[�̃t�F�[�h�A�E�g����t���O</summary>
    private bool _FirstCharacterFadeOutFlag = true;
    /// <summary>���͕\�����̃N���b�N����t���O</summary>
    private bool _FirstClickFlag = true;
    /// <summary>�摜�t�F�[�h�A�E�g�I���t���O</summary>
    private bool _FadeFinishFlag = true;
    /// <summary>�摜�t�F�[�h�A�E�g�I���t���O</summary>
    private bool _TextDisplayEnd = true;
    #region ���X�g�ݒ�

    [System.Serializable]
    public class LoopSetup
    {
        public string Memo;
        /// <summary>���͂���ʂɏo�Ă��镶�̓��X�g</summary>
        public List<string> MessageList;
        public List<AudioClip> VoiceClips;         // �Z���t���Ƃ�AudioClip�z��
        /// <summary>��ʂɏo�Ă���e�L�X�g�{�b�N�X�̕\����\���t���O</summary>
        /// <summary>�I����1����������Ɉړ�����v�f</summary>
        public int DefaultSkipElementIndex;
        public bool TextBoxDisplayFlag;
        /// <summary>�I�������o�����o���Ȃ�����s�t���O</summary>
        public bool TextChoiceFlag;
        /// <summary>�I����1�̃e�L�X�g</summary>
        public string ChoiceTextOne;
        /// <summary>�I����2�̃e�L�X�g</summary>
        public string ChoiceTextTwo;
        /// <summary>�I����1����������Ɉړ�����v�f</summary>
        public int ChoiceOneSkipElementIndex;
        /// <summary>�I����1����������Ɉړ�����v�f</summary>
        public int ChoiceTwoSkipElementIndex;
        public AudioClip BGMClips;         // �Z���t���Ƃ�AudioClip�z��
        /// <summary>�L�����N�^�[�C���[�W�N���X</summary>
        //�t�B�[���h�̌Â����O���w�肷�邱�ƂŁA�ȑO�̃f�[�^���V�����t�B�[���h���Ƀ}�b�s���O�����
        [FormerlySerializedAs("ImageClass")]
        public ImageClass ImageClass;

    }

    [System.Serializable]
    public class ImageClass
    {
        /// <summary>�w�i�摜</summary>
        public Sprite BackBroundImage;
        ///<summary>�S�̃t�F�[�h�t���O</summary>
        public bool AllFadeFlag;
        /// <summary>�t�F�[�h�C���t�F�[�h�A�E�g�̑��x</summary>
        public float FadeSpeed;
        /// <summary>�L�����N�^�[�E�摜</summary>
        public Sprite RightCharacterImage;
        /// <summary>�L�����N�^�[�E�摜�t�F�[�h�t���O</summary>
        public bool RightCharacterFadeFlag;
        /// <summary>�L�����N�^�[�����摜</summary>
        public Sprite CenterCharacterImage;
        /// <summary>�L�����N�^�[�����摜�t�F�[�h�t���O</summary>
        public bool CenterCharacterFadeFlag;
        /// <summary>�L�����N�^�[���摜</summary>
        public Sprite LeftCharacterImage;
        /// <summary>�L�����N�^�[���摜�t�F�[�h�t���O</summary>
        public bool LeftCharacterFadeFlag;
    }

    // �t�B�[���h�̌Â����O���w�肷�邱�ƂŁA�ȑO�̃f�[�^���V�����t�B�[���h���Ƀ}�b�s���O�����
    [FormerlySerializedAs("_LoopSetupList")]
    public List<LoopSetup> _LoopSetupList;
    #endregion

    #region �����ݒ�

    /// <summary>�w�i�摜�̃R���|�[�l���g </summary>
    public Image _ImageComponent_Background;
    /// <summary>�L�����N�^�[�E�摜�̃R���|�[�l���g</summary>
    public Image _ImageComponent_RightCharacter;
    /// <summary>�L�����N�^�[�E�摜�̃R���|�[�l���g</summary>
    public Image _ImageComponent_CenterCharacter;
    /// <summary>�L�����N�^�[���摜�̃R���|�[�l���g</summary>
    public Image _ImageComponent_LeftCharacter;
    /// <summary>�t�F�[�h�C���t�F�[�h�A�E�g</summary>
    public Image FadeInOut;
    /// <summary>�{�^���I�u�W�F�N�g</summary>
    public Button _btnMessage;
    /// <summary>�{�^���I�u�W�F�N�g</summary>
    public Button _btnChooseOne;
    /// <summary>�{�^���I�u�W�F�N�g</summary>
    public Button _btnChooseTwo;
    /// <summary>�I����1�I�u�W�F�N�g</summary>
    public TextMeshProUGUI _txtChooseOne;
    /// <summary>�{�^���I�u�W�F�N�g</summary>
    public TextMeshProUGUI _txtChooseTwo;
    /// <summary>��ʂɏo�Ă���e�L�X�g�{�b�N�X</summary>
    public TextMeshProUGUI Text;
    /// <summary>��ʂɏo�Ă��镶���̏o�����x</summary>
    public float _NovelSpeed;
    /// <summary>�f�o�b�O�������ꍇ�̉摜���l</summary>
    public int _DebugCount;
    #endregion

    private int _SceneNameNumber;


    private void Start()
    {
        _LoopSetupListIndex = _DebugCount;
        _ChildLoopSetupListIndex = _DebugCount;

        SetObject();
        // ���݂̃V�[�����擾
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
                //���[�v���Ă���Œ��ɁA�N���b�N���ĐV�����������N�����ƃ_�u���ŏ���������B����͊o���Ă������B
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