using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SvUtil : MonoBehaviour
    {
        static public int[,] CrossPoints { get; private set; } = new int[4, 2]
        {
            { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }
        };

        static public int[,] BoxPoints = new int[8, 2]
        {
            { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, 1 }, { 0, -1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }
        };

        static public int[,] PanelPoints = new int[9, 2]
        {
            { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, 1 }, { 0, 0 }, { 0, -1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }
        };

        static public List<int[]> GetRandomBlankPointList(List<int[]> exept = null)
        {
            var noObject = ShareSystem.Passable;

            var mazeRow = noObject.GetLength(0);
            var mazeCol = noObject.GetLength(1);

            var pointList = new List<int[]>();

            for (int c = 0; c < mazeCol; c++)
            {
                for (int r = 0; r < mazeRow; r++)
                {
                    if (!noObject[r, c]) { continue; }
                    if (exept != null && Contain(exept, new int[2] { r, c })) { continue; }

                    pointList.Add(new int[2] { r, c });
                }
            }

            var randomList = new List<int[]>();

            while (true)
            {
                if (pointList.Count == 0) { break; }

                var idx = UnityEngine.Random.Range(0, pointList.Count);

                randomList.Add(new int[2] { pointList[idx][0], pointList[idx][1] });
                pointList.RemoveAt(idx);
            }

            return randomList;

            // function
            static bool Contain(List<int[]> list, int[] item)
            {
                foreach (var l in list)
                {
                    if (l[0] == item[0] && l[1] == item[1]) { return true; }
                }

                return false;
            }
        }

        static public List<GameObject> RandomSpawn<T>(List<int[]> randomCandidatePoints, Dictionary<T, GameObject> prefabList,
            Dictionary<T, float> spawnRateList, T defaultValue, int maxSpawns)
        {
            var randomDropList = GetRandomObjectList(spawnRateList, randomCandidatePoints, maxSpawns, defaultValue);

            var objectList = new List<GameObject>();

            // randomDropList.Count <= maxDrops
            for (int n = 0; n < randomDropList.Count; n++)
            {
                var point = randomCandidatePoints[n];
                var _item = randomDropList[n];

                var position = ShareSystem.Point2Position(point, 0.0f);
                var itemPrefab = prefabList[_item];

                var item = GameObject.Instantiate(itemPrefab, position, Quaternion.identity);
                item.transform.SetParent(GameHost.World.transform);

                objectList.Add(item);
            }

            return objectList;

            // - inner function
            static List<T> GetRandomObjectList(Dictionary<T, float> spawnRateList, List<int[]> candidatePointList, int maxDrops, T defaultValue)
            {
                var _spawnRateList = new Dictionary<T, float>(spawnRateList);

                var normalizedDropRateList = GetNormalizedRateList(_spawnRateList);
                var maxDropList = GetMaxDropList(normalizedDropRateList, maxDrops);

                var n_dropList = new Dictionary<T, int>();
                var randomDropList = new List<T>();

                var counter = 0;
                while (true)
                {
                    counter++;
                    if (counter > maxDrops) { break; }
                    if (counter > candidatePointList.Count) { break; }

                    if (normalizedDropRateList.Count == 0) { break; }

                    var item = GetRandomObjectBasedOnDropRate(normalizedDropRateList, defaultValue);

                    if (n_dropList.ContainsKey(item))
                    {
                        n_dropList[item]++;
                    }

                    else
                    {
                        n_dropList.Add(item, 1);
                    }

                    randomDropList.Add(item);

                    if (n_dropList[item] >= maxDropList[item])
                    {
                        normalizedDropRateList.Remove(item);
                        normalizedDropRateList = GetNormalizedRateList(normalizedDropRateList);
                    }
                }

                return randomDropList;

                // function
                static Dictionary<T, float> GetNormalizedRateList<T>(Dictionary<T, float> dropRateList)
                {
                    var sum = 0.0f;
                    foreach (var rate in dropRateList.Values)
                    {
                        sum += rate;
                    }

                    var normalizedDropRate = new Dictionary<T, float>();

                    foreach (var pair in dropRateList)
                    {
                        if (sum == 0.0f)
                        {
                            normalizedDropRate.Add(pair.Key, 0.0f);
                        }

                        else
                        {
                            normalizedDropRate.Add(pair.Key, pair.Value / sum);
                        }
                    }

                    return normalizedDropRate;
                }

                // function
                static Dictionary<T, int> GetMaxDropList<T>(Dictionary<T, float> normalizedDropRate, int maxDrop)
                {
                    var maxDropList = new Dictionary<T, int>();

                    foreach (var pair in normalizedDropRate)
                    {
                        var rate = pair.Value;

                        if (rate > 0.0f)
                        {
                            var drop = Mathf.RoundToInt(rate * maxDrop);

                            if (drop == 0)
                            {
                                maxDropList.Add(pair.Key, 1);
                            }

                            else
                            {
                                maxDropList.Add(pair.Key, drop);
                            }
                        }

                        else
                        {
                            maxDropList.Add(pair.Key, 0);
                        }
                    }

                    return maxDropList;
                }

                static T GetRandomObjectBasedOnDropRate(Dictionary<T, float> normalizedDropRate, T defaultValue)
                {
                    var randomValue = UnityEngine.Random.Range(0.0f, 1.0f);

                    var min = 0.0f;

                    foreach (var pair in normalizedDropRate)
                    {
                        var max = min + pair.Value;

                        if (min <= randomValue && randomValue <= max)
                        {
                            return pair.Key;
                        }

                        min = max;
                    }

                    return defaultValue;
                }
            }
        }

        static public List<T> RandomSort<T>(List<T> list)
        {
            var _list = new List<T>(list);
            var randomList = new List<T>();

            while (true)
            {
                if (_list.Count == 0) { break; }

                var index = UnityEngine.Random.Range(0, _list.Count);
                randomList.Add(_list[index]);

                _list.RemoveAt(index);
            }

            return randomList;
        }

        static public int[] Time2MinSecMSec(float time)
        {
            var min = Mathf.FloorToInt(time / 60.0f);
            var sec = (int)time % 60;
            var msec = Mathf.RoundToInt((time - min * 60 - sec) * 100.0f);

            return new int[3] { min, sec, msec };
        }

        static public string GetTimeText(float time, bool onlyMinSec = false)
        {
            var msms = Time2MinSecMSec(time);

            if (onlyMinSec)
            {
                return PaddingZero(msms[0]) + " : " + PaddingZero(msms[1]);
            }

            else
            {
                return PaddingZero(msms[0]) + " : " + PaddingZero(msms[1]) + " : " + PaddingZero(msms[2]);
            }
        }

        static public string PaddingZero(int value)
        {
            if (value < 10)
            {
                return "0" + value.ToString();
            }

            else
            {
                return value.ToString();
            }
        }

        static public string GetDividedNumberText(int number)
        {
            var digitList = new List<int>();

            while (true)
            {
                var rem = number % 10;
                digitList.Add(rem);

                var quo = Mathf.FloorToInt(number / 10);
                if (quo == 0) { break; }

                number = quo;
            }

            var text = "";

            for (var n = 0; n < digitList.Count; n++)
            {
                if (n > 0 && n % 3 == 0)
                {
                    text = "," + text;
                }

                text = digitList[n].ToString() + text;
            }

            return text;
        }

        static public Vector3 GetViewVector(float magnitude, float q = 0.0f, float r = 0.0f)
        {
            var radrotX = -Player.Camera.transform.eulerAngles.x * Mathf.Deg2Rad;
            var radrotY = Player.Camera.transform.eulerAngles.y * Mathf.Deg2Rad;

            var pqr_vector = new float[3] { 1.0f, q, r };
            pqr_vector = CorrectPQR(pqr_vector, radrotX);

            var zxy_vector = PQR2ZXY(pqr_vector, radrotX, radrotY);
            var direction = new Vector3(zxy_vector[1], zxy_vector[2], zxy_vector[0]).normalized;

            return direction * magnitude;

            // - inner function
            static float[] CorrectPQR(float[] pqr, float radrotX)
            {
                var p = pqr[0];
                var r = pqr[2];

                if (r * Mathf.Sin(radrotX) > p * Mathf.Cos(radrotX))
                {
                    r = Calcf.SafetyDiv(p * Mathf.Cos(radrotX), Mathf.Sin(radrotX), 0.0f);
                }

                return new float[3] { p, pqr[1], r };
            }

            // - inner function
            static float[] PQR2ZXY(float[] pqr_vector, float radrotX, float radrotY)
            {
                var p = pqr_vector[0];
                var q = pqr_vector[1];
                var r = pqr_vector[2];

                var sX = Mathf.Sin(radrotX);
                var cX = Mathf.Cos(radrotX);
                var sY = Mathf.Sin(radrotY);
                var cY = Mathf.Cos(radrotY);

                var z = p * cX * cY - q * sY - r * sX * cY;
                var x = p * cX * sY + q * cY - r * sX * sY;
                var y = p * sX + r * cX;

                return new float[3] { z, x, y };
            }


        }

        static public int[] GetAmmoInMagBag(int currentAmmoInMag, int currentAmmoInBag, int maxAmmoInMag, int maxAmmoInBag, int additionalAmmo)
        {
            if (additionalAmmo < 0)
            {
                return new int[2] { maxAmmoInMag, maxAmmoInBag };
            }

            var totalAmmo = currentAmmoInMag + currentAmmoInBag + additionalAmmo;

            if (totalAmmo < maxAmmoInMag)
            {
                return new int[2] { totalAmmo, 0 };
            }

            var ammoOutOfMag = totalAmmo - maxAmmoInMag;

            if (ammoOutOfMag > maxAmmoInBag)
            {
                return new int[2] { maxAmmoInMag, maxAmmoInBag };
            }

            return new int[2] { maxAmmoInMag, ammoOutOfMag };
        }
    }
}

