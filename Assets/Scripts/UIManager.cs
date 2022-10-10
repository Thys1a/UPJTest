using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /* �ֶ� */
    private static UIManager instance = null;

    private Transform _CanvasTransfrom = null;
    private Transform _NormalTransfrom = null;
    private Transform _UIScriptsTransfrom = null;
    private Transform _UIMaskTransform = null;

    //UI����Ԥ��·��(����1������Ԥ�����ƣ�2����ʾ����Ԥ��·��)
    private Dictionary<string, string> _DicFormsPaths;
    //��������UI����
    private Dictionary<string, BaseUIForm> _DicAllUIForms;
    //��ǰ��ʾ��UI����
    private Dictionary<string, BaseUIForm> _DicCurrentUIForms;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static UIManager Instance()
    {

        if (instance == null)
        {
            instance = new GameObject("_UIManager").AddComponent<UIManager>();
            Debug.Log("UIManager Loaded");
        }
        return instance;
    }

    private void Awake()
    {


        _DicAllUIForms = new Dictionary<string, BaseUIForm>();
        _DicCurrentUIForms = new Dictionary<string, BaseUIForm>();
        _DicFormsPaths = new Dictionary<string, string>();

        InitRootCanvas();


        if (_DicFormsPaths != null)//����Ԥ��
        {
            _DicFormsPaths.Add("ClueSelectedPanel", @"UIPrefab\clueSelectedPanel");
            _DicFormsPaths.Add("StaffPanel", @"UIPrefab\StaffPanel");

        }
    }

    /// <summary>
    /// ͨ��������ʾui
    /// </summary>
    /// <param name="uiFormName"></param>
    public void ShowUIForm(string uiFormName)
    {
        BaseUIForm baseUIForm = null;//UI�������

        if (string.IsNullOrEmpty(uiFormName)) return;
        //����UI��������ƣ����ص�������UI���塱���漯����
        baseUIForm = LoadFormsToAllUIFormsCache(uiFormName);
        if (baseUIForm == null) return;

        LoadUIToCurrentCache(uiFormName);
        Debug.Log(1);
    }

    /// <summary>
    /// �رյ�ǰUI����
    /// </summary>
    public void CloseUIForms(string strUIFormName)
    {
        BaseUIForm baseUIForm = null;                   //UI�������

        /* ������� */
        if (string.IsNullOrEmpty(strUIFormName)) return;
        //������UI���建�桱���û�м�¼����ֱ�ӷ��ء�
        _DicAllUIForms.TryGetValue(strUIFormName, out baseUIForm);
        if (baseUIForm == null) return;
        ExitUIFormsCache(strUIFormName); 
        
    }
    public void SetUIMask(GameObject ui)
    {
        Color newColor = _UIMaskTransform.GetComponent<Image>().color;
        
        _UIMaskTransform.GetComponent<Image>().color = new Color(255 / 255F, 255 / 255F, 255 / 255F, 0F / 255F);
        _UIMaskTransform.gameObject.SetActive(true);

        _UIMaskTransform.SetAsLastSibling();
        ui.transform.SetAsLastSibling();

    }
    public void CancelUIMask(GameObject ui)
    {
        //���㴰������
        ui.transform.SetAsFirstSibling();
        //�������ִ���
        if (_UIMaskTransform.gameObject.activeInHierarchy)
        {
            //����
            _UIMaskTransform.gameObject.SetActive(false);
        }
    }


    private void InitRootCanvas()
    {
        //ResourcesMgr.GetInstance().LoadAsset(SysDefine.SYS_PATH_CANVAS, false);
        GameObject obj = Instantiate((GameObject)Resources.Load(SysDefine.SYS_PATH_CANVAS));
        _CanvasTransfrom = obj.transform;

        _NormalTransfrom = _CanvasTransfrom.Find("UI");
        _UIScriptsTransfrom = _CanvasTransfrom.Find("Script");
        _UIMaskTransform = _NormalTransfrom.Find("MaskPanel");
        _UIMaskTransform.gameObject.SetActive(false);

        this.gameObject.transform.SetParent(_UIScriptsTransfrom, false);

        DontDestroyOnLoad(_CanvasTransfrom);
    }

    private BaseUIForm LoadFormsToAllUIFormsCache(string uiFormName)
    {
        /// ����UI��������ƣ����ص�������UI���塱���漯����
        /// ���ܣ� ��顰����UI���塱�����У��Ƿ��Ѿ����ع�������ż��ء�
        BaseUIForm baseUIResult = null;                 //���صķ���UI�������

        _DicAllUIForms.TryGetValue(uiFormName, out baseUIResult);
        if (baseUIResult == null)
        {
            //����ָ�����Ƶġ�UI���塱
            baseUIResult = LoadUIForm(uiFormName);
        }
        return baseUIResult;
    }
    private BaseUIForm LoadUIForm(string uiFormName)
    {
        //����UI���嵽���漯����
        string UIpath = null;                   //UI����·��
        GameObject UIfab = null;             //������UI��¡��Ԥ��
        BaseUIForm baseUIForm = null;                     //�������


        //����UI�������ƣ��õ���Ӧ�ļ���·��
        _DicFormsPaths.TryGetValue(uiFormName, out UIpath);
        //���ݡ�UI�������ơ������ء�Ԥ���¡�塱
        if (!string.IsNullOrEmpty(UIpath))
        {
            UIfab = Instantiate((GameObject)Resources.Load(UIpath));
        }
        //���á�UI��¡�塱�ĸ��ڵ㣨���ݿ�¡���д��Ľű��в�ͬ�ġ�λ����Ϣ����
        if (_CanvasTransfrom != null && UIfab != null)
        {
            baseUIForm = UIfab.GetComponent<BaseUIForm>();
            if (baseUIForm == null)
            {
                Debug.Log("baseUiForm==null! ,����ȷ�ϴ���Ԥ��������Ƿ������baseUIForm������ű��� ���� uiFormName=" + uiFormName);
                return null;
            }
            UIfab.transform.SetParent(_NormalTransfrom, false); 
            

            //��������
            UIfab.SetActive(false);
            //�ѿ�¡�壬���뵽������UI���塱�����棩�����С�
            _DicAllUIForms.Add(uiFormName, baseUIForm);
            return baseUIForm;
        }
        else
        {
            Debug.Log("_TraCanvasTransfrom==null Or goCloneUIPrefabs==null!! ,Plese Check!, ����uiFormName=" + uiFormName);
        }

        Debug.Log("���ֲ�����Ԥ���Ĵ������飬���� uiFormName=" + uiFormName);
        return null;
    }

    private void LoadUIToCurrentCache(string uiFormName)
    {
        BaseUIForm baseUIForm;                          //UI�������
        BaseUIForm baseUIFormFromAllCache;              //�ӡ����д��弯�ϡ��еõ��Ĵ���

        //�����������ʾ���ļ����У���������UI���壬��ֱ�ӷ���
        _DicCurrentUIForms.TryGetValue(uiFormName, out baseUIForm);
        if (baseUIForm != null) return;

        //�ѵ�ǰ���壬���ص���������ʾ��������
        _DicAllUIForms.TryGetValue(uiFormName, out baseUIFormFromAllCache);
        if (baseUIFormFromAllCache != null)
        {
            _DicCurrentUIForms.Add(uiFormName, baseUIFormFromAllCache);
            baseUIFormFromAllCache.Display();           //��ʾ��ǰ����
        }
    }

    /// <summary>
    /// ж��UI����ӡ���ǰ��ʾ���弯�ϡ������С�
    /// </summary>
    /// <param name="strUIFormName"></param>
    private void ExitUIFormsCache(string strUIFormName)
    {
        BaseUIForm baseUIForm;                        //UI�������

        //��������ʾUI���建�桱����û�м�¼����ֱ�ӷ��ء�
        _DicCurrentUIForms.TryGetValue(strUIFormName, out baseUIForm);
        if (baseUIForm == null) return;

        //ָ��UI���壬��������״̬���Ҵӡ�������ʾUI���建�桱�������Ƴ���
        baseUIForm.Hiding();
        _DicCurrentUIForms.Remove(strUIFormName);
    }

}
