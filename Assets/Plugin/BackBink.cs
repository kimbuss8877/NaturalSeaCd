using System.Xml;
using System.IO;
using System.Collections;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using Bink;

public class BackBink : MonoBehaviour
{
    public string filename;
    public bool Transparent = false;
    public bool noSound = true;
    public bool loop = true;
    //   public float Posi_Z = 30;
    //  public bool playOnAwake = false;
    //  public bool turnSubsOff = false;
    //  public GUISkin subsSkin;

    [Range(0, 255)]
    public byte Volume = 255;

    BinkVideo.TBink binkS;
    Texture2D tex;

    IntPtr hBink, Pointer;
  

    int size, n;
    float timer = 0.0f;
    byte[] bits;
    bool isStopped = true;
    //    bool hasSubs = false;
    uint pgoto = 0;
    private bool isgoto = false;
    // private bool Bg_Active;
    public const string DrawTexture_Str1 = "DrawTexture";
 //   private MainObj main_Obj;
    private void Start()
    {
    //    main_Obj = GameObject.FindWithTag("Main").GetComponent<MainObj>();
        Vector3 vectormovie;
        vectormovie.x = -7.68f;
        vectormovie.y = 12.8f;
        vectormovie.z = 1f;

        transform.localScale = vectormovie;

        vectormovie.x = 0f;
        //   vectormovie.y = 2.86f;
        vectormovie.y = 0f;
        vectormovie.z = 10f;
        transform.localPosition = vectormovie;
        transform.Rotate(0, 0, 180);
      
       
        Rand = GetComponent<MeshRenderer>();
        Rand.enabled = false;
        gameObject.SetActive(false);
    }
   
    MeshRenderer Rand;
    void OnEnable()
    {
        if (Rand != null)
        {
            Rand.enabled = true;
            loop = true;
            BinkPlay();
        }
    }
    
    private void OnDisable()
    {
        if (Rand != null)
        {
            Rand.enabled = false;
            loop = false;
            binkS.CurrentFrame = binkS.Frames;  // 종료 시점
        }    
    }

    public void BinkStop()
    {
        CancelInvoke(DrawTexture_Str1);
        binkS.CurrentFrame = 0;
        BinkVideo.BinkClose(hBink);
        isStopped = true;
        Marshal.FreeHGlobal(Pointer);
        Texture2D.DestroyImmediate(tex);
        tex = null;
        timer = 0.0f;   
    }

    public bool isBinkStopped()
    {
        if (isStopped)
            return true;

        return false;
    }


    public void BinkPlay()
    {
        BinkVideo.BinkSetSoundSystem(BinkVideo.BinkOpenDirectSound(), IntPtr.Zero);
        //hBink = BinkVideo.BinkOpen(Application.dataPath + BinkVideo.folder + filename, BinkVideo.BinkOpenEnum.BINK_OPEN_STREAM);
     //   hBink = BinkVideo.BinkOpen(Application.dataPath + BinkVideo.folder + filename, 0x00100000);
           hBink = BinkVideo.BinkOpen(/*Application.dataPath + BinkVideo.folder +*/ filename, 0x00100000);
        if (noSound)
            BinkVideo.BinkSetVolume(hBink, 0, 0);
        else BinkVideo.BinkSetVolume(hBink, 0, Volume * (uint)655.36);

        binkS = (BinkVideo.TBink)Marshal.PtrToStructure(hBink, typeof(BinkVideo.TBink));
        if (Transparent)
            n = 4;
        else n = 3;
        size = (int)binkS.Width * n * (int)binkS.Height;
        bits = new byte[size];
        isStopped = false;
        Pointer = Marshal.AllocHGlobal(size);
        if (Transparent)
            //tex = new Texture2D((int)binkS.Width, (int)binkS.Height, TextureFormat.ARGB32, false);
            tex = new Texture2D((int)binkS.Width, (int)binkS.Height, TextureFormat.RGBA32, false);
        else tex = new Texture2D((int)binkS.Width, (int)binkS.Height, TextureFormat.RGB24, false);

        /*        if (File.Exists(Path.ChangeExtension(Application.dataPath + BinkVideo.folder + filename, "xml")))
        {
            if (subsSkin != null)
            {
                xDoc.Load(Path.ChangeExtension(Application.dataPath + BinkVideo.folder + filename, "xml"));
                XmlElement xRoot = xDoc.DocumentElement;
                subs = new BinkVideo.SubStr[xRoot.ChildNodes.Count];
                int i = 0;
 //               hasSubs = true;
                foreach (XmlNode xnode in xRoot)
                {
                    if (xnode.Name == "subtitle")
                    {
                        subs[i].Start = Convert.ToInt32(xnode.Attributes.GetNamedItem("start").Value);
                        subs[i].End = Convert.ToInt32(xnode.Attributes.GetNamedItem("end").Value);
                        subs[i].Text = xnode.ChildNodes[0].InnerText;
                        i++;
                    }
                }
            }
            else Debug.Log("GUI Skin is not assigned");
        }
        */
        InvokeRepeating(DrawTexture_Str1, 0.0f, (1.0f / ((float)binkS.FrameRate / (float)binkS.FrameRate2)));
    }

    public void BinkGoToFrame(uint a)
    {
        isgoto = true;
        pgoto = a;
        BinkVideo.BinkGoto(hBink, pgoto - 100, 1);
        BinkVideo.BinkGoto(hBink, pgoto, 0);
        //    timer = 1.0f / ((float)binkS.FrameRate / (float)binkS.FrameRate2) * (float)a;
    }

    /*    void OnGUI()
    {
        if ((hasSubs) && (!turnSubsOff))
        {
            for (int i = 0; i < subs.Length; i++)
            {
                if ((timer >= (float)subs[i].Start / 1000.0) && (timer <= (float)subs[i].End / 1000.0))
                {
                    subsSkin.GetStyle("Label").fontSize = Screen.height / 32;
                    subsSkin.GetStyle("Label").normal.textColor = Color.black;
                    GUI.Label(new Rect(Screen.width / 6 + 1, 1, Screen.width - Screen.width / 6 * 2 + 1, Screen.height + 1), subs[i].Text, subsSkin.GetStyle("Label"));
                    subsSkin.GetStyle("Label").normal.textColor = Color.white;
                    GUI.Label(new Rect(Screen.width / 6, 0, Screen.width - Screen.width / 6 * 2, Screen.height), subs[i].Text, subsSkin.GetStyle("Label"));
                }
            }
        }
    }
*/
   
    void DrawTexture()
    {
        if ((binkS.CurrentFrame < binkS.Frames) || loop)
        {
            if (!isgoto)
            {
                BinkVideo.BinkDoFrame(hBink);
                if (Transparent)
                    BinkVideo.BinkCopyToBuffer(hBink, Pointer, binkS.Width * (uint)n, binkS.Height, 0, 0, BinkVideo.BinkSurface.BINKSURFACE32AR);
                else BinkVideo.BinkCopyToBuffer(hBink, Pointer, binkS.Width * (uint)n, binkS.Height, 0, 0, BinkVideo.BinkSurface.BINKSURFACE24R);

                Marshal.Copy(Pointer, bits, 0, size);
                tex.LoadRawTextureData(bits);
                tex.Apply();
                BinkVideo.BinkSetVolume(hBink, 0, (uint)(Volume * 255));
                Rand.material.mainTexture = tex;

                timer += 1.0f / ((float)binkS.FrameRate / (float)binkS.FrameRate2);
                BinkVideo.BinkNextFrame(hBink);
                binkS.CurrentFrame++;               
            }
            else
            {
                BinkGoToFrame(pgoto);
                isgoto = false;
            }
        }
        else
        {
            Rand.enabled = false;
            Rand.material.mainTexture = null;
            CancelInvoke(DrawTexture_Str1);
            binkS.CurrentFrame = 0;
            BinkVideo.BinkClose(hBink);
            isStopped = true;
            Marshal.FreeHGlobal(Pointer);
            Texture2D.DestroyImmediate(tex);
            tex = null;
            timer = 0.0f;
            gameObject.SetActive(false);
        }
    }
   
}
