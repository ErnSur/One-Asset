using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuickEye.EventSystem.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GameEventBase), true)]
    public class GameEventEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            var wasInvokedField = root.Q<PropertyField>($"PropertyField:wasInvoked");
            wasInvokedField.SetEnabled(false);

            var lastPayloadField = root.Q<PropertyField>($"PropertyField:_lastPayload");

            var invokeButton = new Button(OnInvoke) { name = "invoke-button", text = "Invoke" };
            invokeButton.SetEnabled(EditorApplication.isPlaying);

            var eventPropsContainer = new VisualElement
                { name = "eventProps" };
            eventPropsContainer.style.flexDirection = FlexDirection.Row;
            eventPropsContainer.style.alignItems = Align.Center;
            lastPayloadField.style.flexGrow = 1;
            wasInvokedField.label = "";
            eventPropsContainer.Add(invokeButton);
            eventPropsContainer.Add(wasInvokedField);
            eventPropsContainer.Add(lastPayloadField);
            root.Add(eventPropsContainer);
            return root;
        }

        private void OnInvoke()
        {
            foreach (var invokable in targets.OfType<IInvokable>())
            {
                invokable.RepeatLastInvoke();
            }
        }
    }
}