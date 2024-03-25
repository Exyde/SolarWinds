using UnityEngine;
using UnityEditor;
using Core.GameEvents;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(FactCondition))]
public class FactConditionPropertyDrawer : PropertyDrawer{

    SerializedProperty _blackboardName;
    SerializedProperty _factName;
    SerializedProperty _comparaison;
    SerializedProperty _value;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){

        //Getting back all fields
        _blackboardName = property.FindPropertyRelative("_blackboardName");
        _factName = property.FindPropertyRelative("_factName");
        _comparaison = property.FindPropertyRelative("_comparaison");
        _value = property.FindPropertyRelative("_value");

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        float yOffset = position.height * 0.4f;
        var displayBlackboardRect = new Rect(position.x, position.y, position.width * 0.39f, position.height * 0.4f);
        var displayFactRect = new Rect(position.x + position.width * 0.4f , position.y, position.width * 0.39f, position.height * 0.4f);
        var displayCompRect = new Rect(position.x + position.width * 0.8f, position.y, position.width * 0.12f, position.height * 0.4f);
        var displayValueRect = new Rect(position.x  + position.width * 0.93f, position.y, position.width * 0.06f, position.height * 0.4f);
        
        var blackboardRect = new Rect(position.x, position.y + yOffset, position.width * 0.39f, position.height * 0.5f);
        var factRect = new Rect(position.x + position.width * 0.4f , position.y + yOffset, position.width * 0.39f, position.height * 0.5f);
        var compRect = new Rect(position.x + position.width * 0.8f, position.y + yOffset, position.width * 0.12f, position.height * 0.5f);
        var valueRect = new Rect(position.x  + position.width * 0.93f, position.y + yOffset, position.width * 0.06f, position.height * 0.5f);
 
        //Draws Label
        EditorGUI.LabelField(displayBlackboardRect, "Blackboard Name" );
        EditorGUI.LabelField(displayFactRect, "Fact Name"); 
        EditorGUI.LabelField(displayCompRect, "Condition");
        EditorGUI.LabelField(displayValueRect, "Value" );

        //Draws Values Fields
        EditorGUI.PropertyField(blackboardRect, _blackboardName, GUIContent.none);
        EditorGUI.PropertyField(factRect, _factName, GUIContent.none);
        EditorGUI.PropertyField(compRect, _comparaison, GUIContent.none);
        EditorGUI.PropertyField(valueRect, _value , GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalLines = 2f;

        return totalLines * EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing *(totalLines - 1);
    }
}
