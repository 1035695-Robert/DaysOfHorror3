using UnityEngine;
using UnityEditor;
public class VectorLabelsAttribute : PropertyAttribute
{
    public string xLabel;
    public string yLabel;

    public VectorLabelsAttribute(string xLabel, string yLabel)
    {
        this.xLabel = xLabel;
        this.yLabel = yLabel;
    }
}

[CustomPropertyDrawer(typeof(VectorLabelsAttribute))]
public class VectorLabelsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        VectorLabelsAttribute attr = (VectorLabelsAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.Vector2)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw the foldout or original property label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Calculate widths
            float labelWidth = 30f;
            float fieldWidth = (position.width - labelWidth * 2) / 2;

            // Store original labels
            var originalX = new GUIContent("X");
            var originalY = new GUIContent("Y");

            // Draw X
            EditorGUI.PrefixLabel(new Rect(position.x, position.y, labelWidth, position.height), new GUIContent(attr.xLabel));
            property.vector2Value = new Vector2(
                EditorGUI.FloatField(new Rect(position.x + labelWidth, position.y, fieldWidth, position.height), property.vector2Value.x),
                property.vector2Value.y
            );

            // Draw Y
            EditorGUI.PrefixLabel(new Rect(position.x + labelWidth + fieldWidth, position.y, labelWidth, position.height), new GUIContent(attr.yLabel));
            property.vector2Value = new Vector2(
                property.vector2Value.x,
                EditorGUI.FloatField(new Rect(position.x + labelWidth * 2 + fieldWidth, position.y, fieldWidth, position.height), property.vector2Value.y)
            );

            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
