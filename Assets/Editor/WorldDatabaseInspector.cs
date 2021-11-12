using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WorldDatabase))]
public class WorldDatabaseInspector : Editor
{
    private static int _index = 0;
    private bool mConfirmDelete = false;

    public static void SelectIndex(WorldDatabase db, BaseWorld world)
    {
        _index = 0;
        foreach (BaseWorld w in db.worlds)
        {
            if (w == world) break;
            ++_index;
        }
    }

    public override void OnInspectorGUI()
    {
        WorldDatabase db = target as WorldDatabase;
        EditorGUILayout.Separator();

        BaseWorld world = null;

        if (db.worlds == null || db.worlds.Count == 0)
        {
            _index = 0;
        }
        else
        {
            _index = Mathf.Clamp(_index, 0, db.worlds.Count - 1);
            world = db.worlds[_index];
        }

        if (mConfirmDelete)
        {
            GUILayout.Label("Deseja realmente excluir esse mundo?");
            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Cancelar"))
                {
                    mConfirmDelete = false;
                }
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Deletar"))
                {
                    db.worlds.RemoveAt(_index);
                    mConfirmDelete = false;
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Novo Mundo"))
            {
                BaseWorld bw = new BaseWorld();
                bw.id16 = (db.worlds.Count > 0) ? db.worlds[db.worlds.Count - 1].id16 + 1 : 0;
                db.worlds.Add(bw);
                _index = db.worlds.Count - 1;

                bw.name = "Novo mundo";
                bw.levelsContent = new string[1];
                world = bw;
            }
            GUI.backgroundColor = Color.white;

            if (world != null)
            {
                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();
                {
                    if (_index == 0) GUI.color = Color.grey;
                    if (GUILayout.Button("<<"))
                    {
                        mConfirmDelete = false;
                        --_index;
                    }
                    GUI.color = Color.white;
                    _index = EditorGUILayout.IntField(_index + 1, GUILayout.Width(40f)) - 1;
                    GUILayout.Label("/ " + db.worlds.Count, GUILayout.Width(40f));
                    if (_index + 1 == db.worlds.Count) GUI.color = Color.grey;
                    if (GUILayout.Button(">>"))
                    {
                        mConfirmDelete = false;
                        ++_index;
                    }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();
                {
                    string worldName = EditorGUILayout.TextField("Mundo (ID " + _index + ")", world.name);
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Deletar", GUILayout.Width(55f)))
                    {
                        mConfirmDelete = true;
                    }
                    GUI.backgroundColor = Color.white;

                    if (!worldName.Equals(world.name))
                    {
                        world.name = worldName;
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                var worldSprite = EditorGUILayout.ObjectField("World Sprite", world.worldSprite, typeof(Sprite), false);
                if (worldSprite != world.worldSprite)
                {
                    world.worldSprite = worldSprite as Sprite;
                }
                var notDone = EditorGUILayout.ObjectField("Not Done", world.levelNotDone, typeof(Sprite), false);
                if (notDone != world.levelNotDone)
                {
                    world.levelNotDone = notDone as Sprite;
                }
                var done = EditorGUILayout.ObjectField("Done", world.levelDone, typeof(Sprite), false);
                if (done != world.levelDone)
                {
                    world.levelDone = done as Sprite;
                }

                var cover = EditorGUILayout.ObjectField("Cover", world.worldCover, typeof (Sprite), false);
                if (cover != world.worldCover)
                {
                    world.worldCover = cover as Sprite;
                }

                var color = EditorGUILayout.ColorField("Cor", world.worldColor);
                if (color != world.worldColor)
                {
                    world.worldColor = color;
                }


                bool isUnlocked = EditorGUILayout.Toggle("Destravado", world.isUnlocked);
                if (isUnlocked != world.isUnlocked)
                {
                    world.isUnlocked = isUnlocked;
                }

                int levelsContentCount = EditorGUILayout.IntField("level Count", world.levelsContent.Length);
                if (levelsContentCount != world.levelsContent.Length)
                {
                    world.levelsContent = new string[levelsContentCount];
                    world.levelsDone = new bool[levelsContentCount];
                    world.levelsUnlocked = new bool[levelsContentCount];
                }
                EditorGUILayout.LabelField("levelType / unlocked / completed");
                for (int i = 0; i < world.levelsContent.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    string levelContent = EditorGUILayout.TextField("Level " + i, world.levelsContent[i]);
                    if (levelContent != world.levelsContent[i])
                    {
                        world.levelsContent[i] = levelContent;
                    }
                    bool unlocked = EditorGUILayout.Toggle(world.levelsUnlocked[i]);
                    if (unlocked != world.levelsUnlocked[i])
                    {
                        world.levelsUnlocked[i] = unlocked;
                    }
                    bool levelDone = EditorGUILayout.Toggle(world.levelsDone[i]);
                    if (levelDone != world.levelsDone[i])
                    {
                        world.levelsDone[i] = levelDone;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                
            }
        }
    }
}
