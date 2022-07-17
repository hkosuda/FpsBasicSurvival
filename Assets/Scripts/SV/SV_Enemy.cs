using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Enemy : HostComponent
    {
        static Dictionary<EnemyType, float> spawnRate;
        static Dictionary<EnemyType, GameObject> enemyPrefabList;
        static int spawnCounter;

        public override void Initialize()
        {
            enemyPrefabList = new Dictionary<EnemyType, GameObject>
            {
                { EnemyType.mine, Resources.Load<GameObject>("Enemy/Mine") },
                { EnemyType.turret, Resources.Load<GameObject>("Enemy/Turret") }
            };

            spawnCounter = 0;
            SetEvent(1);
        }

        public override void Shutdown()
        {
            SetEvent(-1);
            enemyPrefabList = null;
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                EnemyMain.EnemyDestroyed += Respawn;
            }

            else
            {
                EnemyMain.EnemyDestroyed -= Respawn;
            }
        }

        public override void Begin()
        {
            if (GameHost.World  == null) { return; }

            spawnCounter = 0;

            SV_Seed.Init(SV_Round.RoundNumber);
            var randPoints = SvUtil.GetRandomBlankPointList(new List<int[]>() { SV_GoalStart.StartPoint, SV_GoalStart.GoalPoint });

            var randomCandidatePointList = GetCandidateListFromRandomBlankList(randPoints);
            int maxEnemy = SV_Round.NumberOfEnemies;

            spawnRate = new Dictionary<EnemyType, float>()
            {
                { EnemyType.mine, Params.mine_spawn_rate },
                { EnemyType.turret, 1.0f - Params.mine_spawn_rate },
            };

            var objectList = SvUtil.RandomSpawn(randomCandidatePointList, enemyPrefabList, spawnRate, EnemyType.mine, maxEnemy);

            for (int n = 0; n < objectList.Count; n++)
            {
                spawnCounter++;

                var bomb = objectList[n].GetComponent<MineBrain>();
                if (bomb != null) { bomb.ID = 10 * spawnCounter; continue; }

                var turret = objectList[n].GetComponent<TurretBrain>();
                if (turret != null) { turret.ID = 10 * spawnCounter; continue; }
            }

            //
            // function
            static List<int[]> GetCandidateListFromRandomBlankList(List<int[]> randomPointList)
            {
                var wallSize = Vecf.Magnitude(new float[2] { SV_Map.wall_depth, SV_Map.wall_width });

                var startPosition = ShareSystem.Point2Position(SV_GoalStart.StartPoint, 0.0f);
                var goalPosition = ShareSystem.Point2Position(SV_GoalStart.GoalPoint, 0.0f);

                for (int n = (randomPointList.Count - 1); n > -1; n--)
                {
                    var position = ShareSystem.Point2Position(randomPointList[n], 0.0f);

                    var dispFromStart = Vecf.Magnitude(new float[2] { startPosition.z - position.z, startPosition.x - position.x });
                    if (dispFromStart < wallSize * 3.0f) { randomPointList.RemoveAt(n); continue; }

                    var dispFromGoal = Vecf.Magnitude(new float[2] { goalPosition.z - position.z, startPosition.x - position.x });
                    if (dispFromGoal < wallSize * 3.0f) { randomPointList.RemoveAt(n); continue; }
                }

                return randomPointList;
            }
        }

        void Respawn(object obj, EnemyMain enemyMain)
        {
            if (GameHost.World == null) { return; }

            spawnCounter++;
            var randomAdd = Mathf.RoundToInt(Mathf.Pow(spawnCounter, 2)) + SV_Round.RoundNumber;

            SV_Seed.Init(randomAdd);
            if (!ProbabilityCheck())
            {
                if (!EnemiesCheck())
                {
                    return;
                }
            }

            var playerPosition = Player.Myself.transform.position;
            var playerPoint = ShareSystem.Position2Point(playerPosition);

            var noObject = ShareSystem.Passable;

            var row = noObject.GetLength(0);
            var col = noObject.GetLength(1);

            var candidatePoints = new List<int[]>();

            for (int c = 0; c < col; c++)
            {
                if (Mathf.Abs(playerPoint[1] - c) <= 2) { continue; }

                for (int r = 0; r < row; r++)
                {
                    if (Mathf.Abs(playerPoint[0] - r) <= 1) { continue; }
                    if (!noObject[r, c]) { continue; }

                    candidatePoints.Add(new int[2] { r, c });
                }
            }

            if (candidatePoints.Count == 0) { return; }

            var randomCandidatePoints = SvUtil.RandomSort(candidatePoints);
            var spawnPosition = ShareSystem.Point2Position(randomCandidatePoints[0], 0.0f);

            var _enemy = GetEnemy();

            var _enemyPrefab = enemyPrefabList[_enemy];
            var enemy = GameObject.Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.transform.SetParent(GameHost.World.transform);

            var turret = enemy.GetComponent<TurretBrain>();
            var mine = enemy.GetComponent<MineBrain>();

            if (turret != null)
            {
                turret.ID = 10 * spawnCounter;
            }

            else
            {
                mine.ID = 10 * spawnCounter;
            }

            // function
            static bool EnemiesCheck()
            {
                var enemies = GameObject.FindGameObjectsWithTag("Enemy");
                var min_enemies = Mathf.RoundToInt(SV_Round.NumberOfEnemies * Params.sv_min_enemies_rate);

                if (enemies.Count() < min_enemies)
                {
                    return true;
                }

                return false;
            }

            // function
            static bool ProbabilityCheck()
            {
                var value = UnityEngine.Random.Range(0.0f, 1.0f);
                var probability = Params.sv_enemy_respawn_probability;

                if (probability == 0.0f)
                {
                    return false;
                }

                if (value <= probability)
                {
                    return true;
                }

                return false;
            }

            // function
            static EnemyType GetEnemy()
            {
                SV_Seed.Init(spawnCounter + SV_Round.RoundNumber);

                var value = UnityEngine.Random.Range(0.0f, 1.0f);
                var normalizedSpawnRate = GetNormalizedSpawnRatioList();

                var min = 0.0f;

                foreach (var pair in normalizedSpawnRate)
                {
                    var max = min + pair.Value;

                    if (min <= value && value <= max)
                    {
                        return pair.Key;
                    }

                    min = max;
                }

                return EnemyType.mine;

                // function
                static Dictionary<EnemyType, float> GetNormalizedSpawnRatioList()
                {
                    var sum = 0.0f;

                    foreach (var pair in spawnRate)
                    {
                        sum += pair.Value;
                    }

                    var normalizedSpawnRatioList = new Dictionary<EnemyType, float>();

                    if (sum == 0.0f)
                    {
                        foreach (var pair in spawnRate)
                        {
                            normalizedSpawnRatioList.Add(pair.Key, 0.0f);
                        }
                    }

                    else
                    {
                        foreach (var pair in spawnRate)
                        {
                            normalizedSpawnRatioList.Add(pair.Key, pair.Value / sum);
                        }
                    }

                    return normalizedSpawnRatioList;
                }
            }
        }
    }
}

