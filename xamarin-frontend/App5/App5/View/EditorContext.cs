using System;
using App5.Entities;
using App5.PageElements;

namespace App5
{
    public class EditorContext
    {
        public TextElementView currentItem { get; set; }
        // public bool isFocused { get; set; }
        public EditorState State { get; set; }

        public EventHandler<EventArgs> SelectTypeChanged;
        private SelectedType _selectedType;

        public Note currentNote { get; set; }

        public SelectedType SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                SelectTypeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public enum SelectedType
    {
        NONE, TEXT, IMAGE
    }

    public enum EditorState
    {
        NONE, SELECTED, EDITING
    }
}