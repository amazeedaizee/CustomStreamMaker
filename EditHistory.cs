namespace CustomStreamMaker
{
    internal enum EditType
    {
        Add, Delete, Edit
    }
    internal class EditHistory
    {
        public EditType EditType;
        public int index;
        public PlayingObject playingObject;
        public int editedIndex = -1;
        public PlayingObject editedObject = null;

        public EditHistory(EditType editType, int index, PlayingObject playingObject, int editedIndex = -1, PlayingObject editedObject = null)
        {
            EditType = editType;
            this.index = index;
            this.playingObject = PlayingObject.DupePlayingObj(playingObject);
            if (EditType == EditType.Edit && editedObject != null)
            {
                this.editedIndex = editedIndex;
                this.editedObject = PlayingObject.DupePlayingObj(editedObject);
            }
        }

        public (int index, PlayingObject playObj) ReturnMainObject()
        {
            return (index, PlayingObject.DupePlayingObj(playingObject));
        }

        public (int editedIndex, PlayingObject editedObj) ReturnEditedObject()
        {
            return (editedIndex, PlayingObject.DupePlayingObj(editedObject));
        }
    }
}
