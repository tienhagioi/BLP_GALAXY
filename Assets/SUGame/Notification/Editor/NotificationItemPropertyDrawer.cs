using UnityEngine;
using System.Collections;
using UnityEditor;

#pragma warning disable
//[CustomPropertyDrawer(typeof(NotificationItem))]
public class NotificationItemPropertyDrawer : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty (position, label, property);

		SerializedProperty id = property.FindPropertyRelative ("id");
		SerializedProperty title = property.FindPropertyRelative ("title");
		SerializedProperty content = property.FindPropertyRelative ("content");
		SerializedProperty icon = property.FindPropertyRelative ("icon");
		SerializedProperty hasSound = property.FindPropertyRelative ("hasSound");
		SerializedProperty hasVibrate = property.FindPropertyRelative ("hasVibrate");

		SerializedProperty notifyType = property.FindPropertyRelative ("notificationType");
		SerializedProperty notifyTimeType = property.FindPropertyRelative ("notificationTimeType");

		SerializedProperty delayTime = property.FindPropertyRelative ("delayTime");
		SerializedProperty delayDate = property.FindPropertyRelative ("delayDate");

		SerializedProperty dateInWeek = property.FindPropertyRelative ("dateInWeek");

		SerializedProperty exactHour = property.FindPropertyRelative ("exactHour");
		SerializedProperty exactMinute = property.FindPropertyRelative ("exactMinute");
		SerializedProperty exactSecond = property.FindPropertyRelative ("exactSecond");

		SerializedProperty exactDate = property.FindPropertyRelative ("exactDate");

		SerializedProperty repeatTimes = property.FindPropertyRelative ("repeatTimes");

		Rect contentPosition = EditorGUI.PrefixLabel (position, label);
		EditorGUI.PropertyField (contentPosition, id);
		EditorGUI.PropertyField (contentPosition, title);
		//EditorGUILayout.PropertyField(title);
		//EditorGUILayout.PropertyField(content);
		//EditorGUILayout.PropertyField(icon);
		//EditorGUILayout.PropertyField(hasSound);
		//EditorGUILayout.PropertyField(hasVibrate);

		EditorGUI.EndProperty ();

		//if(notifyType)

		//base.OnGUI(position, property, label);
	}
}
