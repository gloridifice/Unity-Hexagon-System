using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexSystem.Utils
{
    public class HexMeshUtils
    {
        public static int[] DeleteFaces(int[] tris, Vector3Int[] faces)
        {
            List<int> trisList = new List<int>(tris);
            List<int> indexes = new List<int>();
            //获取面序号
            for (int i = 0; i < tris.Length/3; i++)
            {
                foreach (var face in faces)
                {
                    bool flag = tris[i * 3] == face.x && tris[i * 3 + 1] == face.y && tris[i * 3 + 2] == face.z;
                    if (flag)
                    {
                        indexes.Add(i);
                        break;
                    }
                }
            }
            //倒序排序
            indexes.Sort(((x, y) => x > y ? -1 : 1));
            foreach (int index in indexes)
            {
                trisList.RemoveAt(index * 3 + 2);
                trisList.RemoveAt(index * 3 + 1);
                trisList.RemoveAt(index * 3);
            }

            return trisList.ToArray();
        }

        public static int[] AddFaces(int[] tris, Vector3Int[] faces)
        {
            //检查面是否已经存在
            for (int i = 0; i < tris.Length/3; i++)
            {
                foreach (var face in faces)
                {
                    bool flag = tris[i * 3] == face.x && tris[i * 3 + 1] == face.y && tris[i * 3 + 2] == face.z;
                    if (flag)
                    {
                        Debug.LogError("Add Face Fail: face already existed - "+ face);
                        return null;
                    }
                }
            }
            //添加面
            List<int> list = new List<int>(tris);
            foreach (var face in faces)
            {
                list.Add(face.x);
                list.Add(face.y);
                list.Add(face.z);
            }

            return list.ToArray();
        }
    }
}