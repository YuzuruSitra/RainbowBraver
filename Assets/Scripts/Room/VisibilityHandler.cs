using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Linq;

// 部屋の可視性変更
public class VisibilityHandler
{
    // シングルトン
    private static VisibilityHandler instance;
    public static VisibilityHandler Instance => instance ?? (instance = new VisibilityHandler());
    private RoomBunker _roomBunker;
    private bool _isVisibleOneRoom = false;
    public bool IsVisibleOneRoom => _isVisibleOneRoom;
    const float DURATION = 1f;

    private VisibilityHandler()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }

    public void ChangeAllRoom()
    {
        RoomDetails[] rooms = _roomBunker.RoomDetails;
        for (int i = 0; i < rooms.Length; i++)
            for (int u = 0; u < rooms[i].FrontMesh.Length; u++)
                rooms[i].FrontMesh[i].enabled = !rooms[i].FrontMesh[i].enabled;
    }

    public void ChangeTargetRoom(bool state, int roomNum)
    {
        RoomDetails[] rooms = _roomBunker.RoomDetails;
        List<Material> mats = new List<Material>();
        for (int i = 0; i < rooms[roomNum].FrontMesh.Length; i++)
            mats.Add(rooms[roomNum].FrontMesh[i].material);
        FadingMaterial(state, mats);
        _isVisibleOneRoom = state;
    }

    // マテリアルのフェード処理
    static async void FadingMaterial(bool state, List<Material> targetMats)
    {
        float[] startAlphas = targetMats.Select(mat => mat.color.a).ToArray();
        float[] targetAlphas = Enumerable.Repeat(state ? 1.0f : 0.0f, targetMats.Count).ToArray();

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        await Task.WhenAll(targetMats.Select(async _ =>
        {
            float currentTime = 0f;
            while (currentTime < DURATION)
            {
                currentTime += DURATION * 0.01f;
                await Task.Delay((int)DURATION * 10);
                Color color = _.color;
                for (int i = 0; i < targetMats.Count; i++)
                {
                    color.a = Mathf.Lerp(startAlphas[i], targetAlphas[i], currentTime / DURATION);
                    targetMats[i].color = color;
                }
            }
        }));

        sw.Stop();
        Debug.Log(sw.ElapsedMilliseconds + "ms");

        for (int i = 0; i < targetMats.Count; i++)
        {
            Color color = targetMats[i].color;
            color.a = targetAlphas[i];
            targetMats[i].color = color;
        }
    }
}

