using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "EnemyProfile")]
public class EnemyProfile : ScriptableObject
{
    [SerializeField]
    private float walkSpeed = 1;
    [SerializeField]
    private float runSpeed = 3.5f;
    [SerializeField]
    private float moveWaitTime = 1;
    [SerializeField]
    private float iconRange = 12;
    [SerializeField]
    private float sensingRange = 8;
    [SerializeField]
    private float sensingRunRange = 6;
    //rayを飛ばす長さ(視力)
    [SerializeField]
    private float rayDistance = 5;
    //rayを飛ばす大きさ(視野の広さ)
    [SerializeField]
    private float rayWide = 1;


    public float WalkSpeed
    {
        get
        {
            return walkSpeed;
        }
    }

    public float RunSpeed
    {
        get
        {
            return runSpeed;
        }
    }

    public float MoveWaitTime
    {
        get
        {
            return moveWaitTime;
        }
    }

    public float IconRange {
        get
        {
            return iconRange;
        }
    }

    public float SensingRange
    {
        get
        {
            return sensingRange;
        }
    }

    public float SensingRunRange
    {
        get
        {
            return sensingRunRange;
        }
    }

    public float RayDistance
    {
        get
        {
            return rayDistance;
        }
    }

    public float RayWide
    {
        get
        {
            return rayWide;
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(EnemyProfile))]
[CanEditMultipleObjects]
public class EnemyProfileEditor : Editor
{
    SerializedProperty walkSpeed;
    SerializedProperty runSpeed;
    SerializedProperty moveWaitTime;
    SerializedProperty iconRange;
    SerializedProperty sensingRunRange;
    SerializedProperty rayDistance;
    SerializedProperty rayWide;

    bool isDefProfile;

    private void OnEnable()
    {
        SetProperties();

        isDefProfile = IsDefaultFile();
    }

    void SetProperties()
    {
        walkSpeed = serializedObject.FindProperty("walkSpeed");
        runSpeed = serializedObject.FindProperty("runSpeed");
        iconRange = serializedObject.FindProperty("iconRange");
        moveWaitTime = serializedObject.FindProperty("moveWaitTime");
        sensingRunRange = serializedObject.FindProperty("sensingRunRange");
        rayDistance = serializedObject.FindProperty("rayDistance");
        rayWide = serializedObject.FindProperty("rayWide");
    }

    bool IsDefaultFile()
    {
        var guids = AssetDatabase.FindAssets("EnemyProfile");
        // EnemyProfileファイルが存在しない場合自動生成
        if (guids.Length == 0)
        {
            return false;
        }

        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var obj = AssetDatabase.LoadAssetAtPath<EnemyProfile>(path);

        return serializedObject.targetObject == obj;
    }

    public override void OnInspectorGUI()
    {
        // Inspectorの値とファイルに保存されている値を同じにする
        serializedObject.Update();

        // 既定ファイルを選択中のみ表示
        if (isDefProfile)
        {
            EditorGUILayout.HelpBox("このファイルは消さないでください。", MessageType.Warning);
        }

        // それぞれのプロパティ
        EditorGUILayout.PropertyField(walkSpeed, new GUIContent("歩く速度"));
        EditorGUILayout.PropertyField(runSpeed, new GUIContent("走る速度"));
        EditorGUILayout.PropertyField(moveWaitTime, new GUIContent("移動終了後の待ち時間"));
        EditorGUILayout.PropertyField(iconRange, new GUIContent("聞き耳(アイコン表示)の有効範囲"));
        EditorGUILayout.PropertyField(sensingRunRange, new GUIContent("シンデレラの走行を感知する範囲"));
        EditorGUILayout.PropertyField(rayDistance, new GUIContent("視力"));
        EditorGUILayout.PropertyField(rayWide, new GUIContent("視野の範囲"));

        // Inspectorの変更を適用
        serializedObject.ApplyModifiedProperties();
    }

    
}

#endif