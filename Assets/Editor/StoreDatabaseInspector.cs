using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(StoreDatabase))]
public class StoreDatabaseInspector : Editor
{
    private static int _index = 0;
    private bool mConfirmDelete = false;

    public override void OnInspectorGUI()
    {
        StoreDatabase db = target as StoreDatabase;
        EditorGUILayout.Separator();

        BaseProduct product = null;

        if (db.products == null || db.products.Count == 0)
        {
            _index = 0;
        }
        else
        {
            _index = Mathf.Clamp(_index, 0, db.products.Count - 1);
            product = db.products[_index];
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
                    db.products.RemoveAt(_index);
                    mConfirmDelete = false;
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.EndHorizontal();
        }
        else
        {

            // "New" Button
            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Novo Produto"))
            {
                BaseProduct bp = new BaseProduct();
                bp.id16 = (db.products.Count > 0) ? db.products[db.products.Count - 1].id16 + 1 : 0;
                db.products.Add(bp);
                _index = db.products.Count - 1;

                bp.name = "Novo Produto";
                product = bp;
            }
            GUI.backgroundColor = Color.white;

            if (product != null)
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
                    GUILayout.Label("/ " + db.products.Count, GUILayout.Width(40f));
                    if (_index + 1 == db.products.Count) GUI.color = Color.grey;
                    if (GUILayout.Button(">>"))
                    {
                        mConfirmDelete = false;
                        ++_index;
                    }
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();

                // product name and delete product button
                GUILayout.BeginHorizontal();
                {
                    string productName = EditorGUILayout.TextField("Produto (ID " + _index + ")", product.name);
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Deletar", GUILayout.Width(55f)))
                    {
                        mConfirmDelete = true;
                    }
                    GUI.backgroundColor = Color.white;

                    if (!productName.Equals(product.name))
                    {
                        product.name = productName;
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                int productPrice = EditorGUILayout.IntField("Preco", product.price);
                if (productPrice != product.price)
                {
                    product.price = productPrice;
                }
                var image = EditorGUILayout.ObjectField("Imagem", product.image, typeof (Sprite), false);
                if (image != product.image)
                {
                    product.image = image as Sprite;
                }

                string description = EditorGUILayout.TextField("Descricao", product.description);
                if (description != product.description)
                {
                    product.description = description;
                }
            }
        }
    }
}
