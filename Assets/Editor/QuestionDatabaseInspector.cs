using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(QuestionDatabase))]
public class QuestionDatabaseInspector : Editor
{
    private const int MinScoreAllowed = 500;
    private const int MaxScoreAllowed = 4000;
    private static int _index = 0;
    private bool mConfirmDelete = false;

    /// <summary>
    /// Helper function that sets the index to the index of the specified question.
    /// </summary>
    public static void SelectIndex(QuestionDatabase db, BaseQuestion question)
    {
        _index = 0;
        foreach (BaseQuestion q in db.questions)
        {
            if (q == question) break;
            ++_index;
        }
    }

    public override void OnInspectorGUI()
    {
        QuestionDatabase db = target as QuestionDatabase;
        EditorGUILayout.Separator();

        BaseQuestion question = null;

        if (db.questions == null || db.questions.Count == 0)
        {
            _index = 0;
        }
        else
        {
            _index = Mathf.Clamp(_index, 0, db.questions.Count - 1);
            question = db.questions[_index];
        }

        if (mConfirmDelete)
        {
            GUILayout.Label("Deseja realmente excluir a questão?");
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
                    db.questions.RemoveAt(_index);
                    mConfirmDelete = false;
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            float countDown = EditorGUILayout.FloatField("pre-contagem", db.timeBeforeCountdown);
            if (countDown != db.timeBeforeCountdown)
            {
                db.timeBeforeCountdown = countDown;
            }
            // "New" Button
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Nova Questão"))
            {
                BaseQuestion bq = new BaseQuestion();
                bq.id16 = (db.questions.Count > 0) ? db.questions[db.questions.Count - 1].id16 + 1 : 0;
                db.questions.Add(bq);
                _index = db.questions.Count - 1;

                bq.question = "Nova questão";
                question = bq;
            }
            GUI.backgroundColor = Color.white;

            if (question != null)
            {
                EditorGUILayout.Separator();

                // Navigation Section
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
                    GUILayout.Label("/ " + db.questions.Count, GUILayout.Width(40f));
                    if (_index + 1 == db.questions.Count) GUI.color = Color.grey;
                    if (GUILayout.Button(">>"))
                    {
                        mConfirmDelete = false;
                        ++_index;
                    }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                // Question name and delete question button
                GUILayout.BeginHorizontal();
                {
                    string questionName = EditorGUILayout.TextField("Questão (ID " + _index + ")", question.question);
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Deletar", GUILayout.Width(55f)))
                    {
                        mConfirmDelete = true;
                    }
                    GUI.backgroundColor = Color.white;

                    if (!questionName.Equals(question.question))
                    {
                        question.question = questionName;
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                for (int i = 0; i < question.alternative.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Alternativa " + i);
                        string alternativeText = EditorGUILayout.TextArea(question.alternative[i], GUILayout.Width(150f));
                        //string alternativeText = GUILayout.TextArea(question.Alternative[i], GUILayout.Width(150f));
                        if (alternativeText != question.alternative[i])
                        {
                            question.alternative[i] = alternativeText;
                        }
                        bool myToggle = i == question.correctAlternative ? true : false;
                        bool toggle = EditorGUILayout.Toggle(myToggle);
                        if (toggle != myToggle)
                        {
                            question.correctAlternative = i;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("LEVEL: q" + question.id16.ToString());
                bool isUnlocked = EditorGUILayout.Toggle("Destravado", question.isUnlocked);
                if (isUnlocked != question.isUnlocked)
                {
                    question.isUnlocked = isUnlocked;
                }

                bool failed = EditorGUILayout.Toggle("Falhou", question.failed);
                if (failed != question.failed)
                {
                    question.failed = failed;
                }
                int minScore = EditorGUILayout.IntSlider("Pontuação Mínima", question.minScore, MinScoreAllowed, MaxScoreAllowed);
                if (minScore != question.minScore)
                {
                    question.minScore = minScore;
                }
                int maxScore = EditorGUILayout.IntSlider("Pontuação Máxima", question.maxScore, MinScoreAllowed, MaxScoreAllowed);
                if (maxScore < minScore)
                {
                    maxScore = minScore;
                }
                if (maxScore != question.maxScore)
                {
                    question.maxScore = maxScore;
                }
                EditorGUILayout.Separator();
                float time = EditorGUILayout.FloatField("Tempo de resposta", question.time);
                if (question.time != time)
                {
                    question.time = time;
                }
            }
        }
    }
}
